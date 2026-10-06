# MarketRadar — Codex Project Context

## Project

MarketRadar is a personal .NET project for building a GPW stock scanner. The project is intentionally being built as a real application rather than as a tutorial-only exercise.

Repository:
- `Ursuss21/gpw-scanner`
- working directory on Windows: `E:\repos\gpw-scanner`

The user is a software developer with strong frontend experience and is using this project to learn .NET, EF Core and PostgreSQL through implementation.

## Working style

- Work incrementally, one meaningful step at a time.
- Prefer discussing the architectural reason before introducing abstractions.
- Do not blindly agree with the user; challenge decisions when there is a concrete technical reason.
- Avoid overengineering.
- Do not introduce patterns merely because they are fashionable.
- Preserve existing architectural decisions unless there is a strong reason to revisit them.
- When implementing something, first inspect the current repository state instead of assuming file contents.
- Prefer small, focused changes that compile.
- After a meaningful change, build/test the solution.
- The user prefers deterministic C# formatting.
- The user writes the project code themselves as a learning exercise. Explain trade-offs, ask guiding questions, and review or suggest code, but do not edit application code unless the user explicitly asks for implementation.
- Updating `AGENTS.md` to preserve project context and collaboration preferences is allowed.

## Solution architecture

Projects:

- `MarketRadar.Domain`
- `MarketRadar.Application`
- `MarketRadar.Infrastructure`

Dependency direction:

```text
Domain
  ↑
Application
  ↑
Infrastructure
```

More explicitly:

```text
MarketRadar.Domain
    ← MarketRadar.Application
    ← MarketRadar.Infrastructure

MarketRadar.Application
    ← MarketRadar.Infrastructure
```

The domain must not depend on infrastructure.

### Responsibilities

#### Domain

The domain defines business concepts and calculations.

A `Requirement` is a component of the company-classification algorithm. It represents a concrete determinant for a concrete instrument based on information available at a specific point in time.

The domain decides WHAT information is needed.

Base requirements represent raw values obtained from infrastructure.

Derived requirements calculate values from base requirements in the domain. Derived calculations should not be persisted as raw database fields unless there is a deliberate performance/data-model reason to do so.

#### Application

Application contains communication contracts between domain/application logic and infrastructure.

It owns:
- request/response contracts,
- ports/interfaces.

Current generic port:

```csharp
public interface IRequirementReader<TRequest, TResponse>
{
    TResponse Get(TRequest request);
}
```

This generic reader is intentional. The reading mechanism is common: infrastructure receives a request and deterministically returns a response. The concrete request/response types preserve type safety.

Requests and responses are not domain entities.

#### Infrastructure

Infrastructure implements application ports and knows how data is actually obtained.

A reader should not need to understand the domain's internal concepts beyond the request/response contract it implements.

Example:

```csharp
public sealed class PriceReader : IRequirementReader<PriceRequest, PriceResponse>
{
    // reads MarketData through MarketRadarDbContext
}
```

## Current application contracts

Current files:

```text
MarketRadar.Application/
└── Requirements/
    ├── IRequirementReader.cs
    └── Base/
        ├── PriceRequest.cs
        └── PriceResponse.cs
```

Current contracts:

```csharp
public interface IRequirementReader<TRequest, TResponse>
{
    TResponse Get(TRequest request);
}
```

```csharp
public record PriceRequest(string Instrument, DateTime At);
```

```csharp
public record PriceResponse(decimal Value);
```

Current infrastructure reader:

```text
MarketRadar.Infrastructure/
└── Requirements/
    └── Base/
        └── PriceReader.cs
```

`PriceReader` is currently a stub and is the next intended implementation target.

## Data availability semantics

This is an important domain rule.

`AvailableAt` means:

> the point in time at which the observation/report was available to the algorithm/market.

It does NOT mean when the application happened to save the row.

Do not silently replace this with an ingestion timestamp.

For a request:

```text
PriceRequest(Instrument, At)
```

the reader must only use observations satisfying:

```text
AvailableAt <= At
```

Among those observations, select the latest one:

```text
OrderByDescending(AvailableAt)
```

This prevents look-ahead bias.

Conceptually:

```text
PriceRequest
    |
    | Instrument + At
    v
MarketData
    |
    | Instrument matches
    | AvailableAt <= At
    | latest AvailableAt
    v
Price
    |
    v
PriceResponse
```

Example:

```text
ASB | 2026-04-21 17:00 | 82.10
ASB | 2026-04-22 17:00 | 83.40
ASB | 2026-04-24 17:00 | 84.20
```

For:

```text
PriceRequest("ASB", 2026-04-23)
```

the result must be:

```text
83.40
```

The 2026-04-24 observation is unavailable at the requested time and must not be used.

## Persistence

Database:

- PostgreSQL
- EF Core
- Npgsql

Database name:

```text
market_radar
```

Local PostgreSQL runs in Docker.

Do not hardcode database passwords into source code.

The design-time EF Core factory reads:

```text
MARKET_RADAR_CONNECTION_STRING
```

from the environment.

## Current persistence model

Current infrastructure structure:

```text
MarketRadar.Infrastructure/
└── Persistence/
    ├── MarketRadarDbContext.cs
    ├── MarketRadarDbContextFactory.cs
    ├── Configurations/
    │   ├── InstrumentConfiguration.cs
    │   └── MarketDataConfiguration.cs
    └── Entities/
        ├── Instrument.cs
        └── MarketData.cs
```

### Instrument

```csharp
public sealed class Instrument
{
    public long Id { get; set; }
    public string Symbol { get; set; } = null!;
}
```

Database concept:

```text
Instrument
---------
Id       bigint PK
Symbol   varchar NOT NULL
```

`Symbol` has a unique index.

Instruments are intentionally represented by database strings, not enums.

### MarketData

```csharp
public sealed class MarketData
{
    public long Id { get; set; }
    public long InstrumentId { get; set; }
    public DateTime AvailableAt { get; set; }
    public decimal Price { get; set; }
    public decimal SessionTurnover { get; set; }
    public Instrument Instrument { get; set; } = null!;
}
```

Database concept:

```text
MarketData
----------
Id              bigint PK
InstrumentId    bigint FK -> Instrument.Id
AvailableAt     timestamp NOT NULL
Price           decimal NOT NULL
SessionTurnover decimal NOT NULL
```

Unique constraint/index:

```text
(InstrumentId, AvailableAt)
```

Foreign key deletion behavior:

```text
ON DELETE RESTRICT
```

Simple surrogate numeric IDs are preferred over composite primary keys.

## Current DbContext

Conceptually:

```csharp
public sealed class MarketRadarDbContext(
    DbContextOptions<MarketRadarDbContext> options) : DbContext(options)
{
    public DbSet<Instrument> Instruments => Set<Instrument>();
    public DbSet<MarketData> MarketData => Set<MarketData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MarketRadarDbContext).Assembly);
    }
}
```

EF configurations are separate classes implementing `IEntityTypeConfiguration<T>`.

## EF Core state

An initial migration exists:

```text
20260930215734_InitialCreate
```

It has already been applied to the local PostgreSQL database.

Current database tables include:

```text
Instrument
MarketData
__EFMigrationsHistory
```

The schema has been inspected and confirmed.

Do not create a new initial migration or reset the database unless explicitly requested.

When changing persistence models:
1. change the entity/configuration,
2. build,
3. create a new migration,
4. inspect the migration,
5. apply it with `dotnet ef database update`.

## Planned data model

The intended core separation is:

```text
Instrument
MarketData
FundamentalData
```

Fundamental data is not implemented yet.

Planned `FundamentalData` fields:

```text
Id
InstrumentId
AvailableAt
PeriodEnd
EBIT
SharesInYear
NetIncome
Equity
TotalAssets
EV
Amortization
SharesOutstanding
SalesRevenue
TotalLiabilities
```

`PeriodEnd` describes the reporting period; `AvailableAt` describes when the information became available.

## Derived domain concepts

Some metrics discussed for the scanner:

- PriceChange
- RelativeMomentum
- WIG-relative momentum
- AverageTurnover
- MarketCapitalization
- TotalDebt
- AverageOperatingProfitPerShare

Examples:

```text
MarketCapitalization = Price * SharesOutstanding
```

```text
WIG-relative change =
    instrument percentage change
    - WIG percentage change
```

```text
TotalDebt = TotalLiabilities / TotalAssets
```

`AverageTurnover` is period-based and should not be hardcoded to a fixed number such as 20 sessions.

`RelativeMomentum` may represent 12M/6M/3M variants by changing the historical price dependency.

Derived values belong in domain/application logic rather than being treated as base persistence fields by default.

## Current next task

The immediate next implementation step is the first real infrastructure read:

```text
PriceRequest
    ↓
IRequirementReader<PriceRequest, PriceResponse>
    ↓
PriceReader
    ↓
MarketRadarDbContext
    ↓
MarketData
    ↓
PriceResponse
```

`PriceReader` should:

1. receive `PriceRequest`,
2. resolve the requested instrument by symbol,
3. find `MarketData` for that instrument where:
   ```text
   AvailableAt <= request.At
   ```
4. select the record with the greatest `AvailableAt`,
5. return:
   ```csharp
   new PriceResponse(marketData.Price)
   ```

Error behavior for missing instrument / missing historical data should be decided deliberately before implementation rather than invented silently.

Do not introduce a separate `Requirement -> Request` mapping layer yet. This was discussed previously and intentionally left open. Avoid adding that abstraction until an actual use case demonstrates its value.

## Coding preferences

Use modern C# syntax where it improves clarity.

Existing project style includes:
- file-scoped namespaces,
- primary constructors,
- `sealed` classes where appropriate,
- records for request/response DTOs.

Prefer explicit, readable LINQ over clever abstractions.

Keep EF queries inside infrastructure.

Do not leak EF entities into domain contracts.

Do not make the domain depend on EF Core or PostgreSQL.

## Testing direction

Tests have not yet been established as a major project layer.

When introducing tests, prefer testing meaningful behavior:
- availability cutoff,
- latest available observation,
- no look-ahead,
- correct instrument selection,
- derived metric calculations.

Do not create large test scaffolding before there is behavior worth testing.

## Important architectural decisions already made

1. `Requirement` is a domain concept, not a database observation abstraction.
2. Infrastructure receives concrete request/response contracts rather than domain `Requirement` objects.
3. `IRequirementReader<TRequest,TResponse>` is the generic application port.
4. `PriceRequest` contains `Instrument` and `At`.
5. `PriceResponse` contains the returned decimal `Value`.
6. `AvailableAt` is information availability time, not technical save time.
7. Historical reads must never use data with `AvailableAt > requested At`.
8. Instrument is represented as a string symbol, not an enum.
9. Persistence uses separate `Instrument`, `MarketData`, and planned `FundamentalData`.
10. Persistence uses simple surrogate `long` IDs.
11. Derived metrics are calculated rather than persisted by default.
12. PostgreSQL + EF Core is the persistence stack.
13. Domain → Application → Infrastructure dependency direction is intentional.
14. Avoid adding abstractions until a concrete use case justifies them.

## Git / repository hygiene

The repository has previously had generated `obj` files accidentally tracked; those were removed from Git tracking.

`.gitignore` already covers typical .NET generated files and `.env`.

Never commit:
- database passwords,
- `.env`,
- generated `bin/` or `obj/`,
- local secrets.

The old PostgreSQL password that was once accidentally committed should be considered compromised; do not reuse it.

## How to proceed

Before modifying code:

1. Inspect the current relevant files.
2. State the smallest useful change.
3. Make the change.
4. Build the affected project/solution.
5. If persistence changed, inspect migration output before applying it.
6. Avoid unrelated refactors.

When uncertain about an architectural decision, stop and discuss the trade-off rather than silently choosing a large abstraction.
