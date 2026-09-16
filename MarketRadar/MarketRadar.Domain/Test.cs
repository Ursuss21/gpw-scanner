var price = new Requirement
{   
    Name = "Price",
    Instrument = "ASB",
    ObservationDate = new DateTime(2026, 4, 23),
    Requirements = []
};

var dailyChange = new Requirement
{
    Name = "DailyChange",
    Instrument = "ASB",
    ObservationDate = new DateTime(2026, 4, 23),
    Requirements = [
        new Requirement
        {   
            Name = "Price",
            Instrument = "ASB",
            ObservationDate = new DateTime(2026, 4, 23),
            Requirements = []
        },
        new Requirement
        {   
            Name = "Price",
            Instrument = "ASB",
            ObservationDate = new DateTime(2026, 4, 22),
            Requirements = []
        }
    ]
};

var relativeMomentum12M = new Requirement
{
    Name = "RelativeMomentum12M",
    Instrument =  "ASB",
    ObservationDate = new DateTime(2026, 4, 23),
    Requirements = [
        new Requirement
        {   
            Name = "Price",
            Instrument = "ASB",
            ObservationDate = new DateTime(2026, 4, 23),
            Requirements = []
        },
        new Requirement
        {   
            Name = "Price",
            Instrument = "ASB",
            ObservationDate = new DateTime(2025, 4, 23),
            Requirements = []
        },
        new Requirement
        {   
            Name = "Price",
            Instrument = "WIG",
            ObservationDate = new DateTime(2026, 4, 23),
            Requirements = []
        },
        new Requirement
        {   
            Name = "Price",
            Instrument = "WIG",
            ObservationDate = new DateTime(2025, 4, 23),
            Requirements = []
        }
    ]
};