using MarketRadar.Infrastructure.Persistence;

public sealed class PriceReader(MarketRadarDbContext dbContext) : IRequirementReader<PriceRequest, PriceResponse>
{

    public PriceResponse Get(PriceRequest request)
    {
        var price = dbContext.MarketData
            .Where(x => x.Instrument.Symbol == request.Instrument && x.AvailableAt <= request.At)
            .OrderByDescending(x => x.AvailableAt)
            .Select(x => (decimal?)x.Price);

        return new PriceResponse(price.FirstOrDefault());
    }
}