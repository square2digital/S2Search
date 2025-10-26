using S2Search.Backend.Domain.AzureSearch.Indexes;
using TinyCsvParser.Mapping;

namespace S2Search.Backend.Services.AzureFunctions.FeedServices.Mappers.TinyCsvParser
{
    public class CsvMapVehicleIndex : CsvMapping<VehicleIndex>
    {
        public CsvMapVehicleIndex() : base()
        {
            MapProperty(0, x => x.VehicleID);
            MapProperty(1, x => x.Make);
            MapProperty(2, x => x.Model);
            MapProperty(3, x => x.Variant);
            MapProperty(4, x => x.Location);
            MapProperty(6, x => x.Price);
            MapProperty(7, x => x.MonthlyPrice);
            MapProperty(8, x => x.Mileage);
            MapProperty(9, x => x.FuelType);
            MapProperty(10, x => x.Transmission);
            MapProperty(11, x => x.Doors);
            MapProperty(12, x => x.EngineSize);
            MapProperty(13, x => x.BodyStyle);
            MapProperty(14, x => x.ManufactureColour);
            MapProperty(15, x => x.Colour);
            MapProperty(16, x => x.VRM);
            MapProperty(17, x => x.Year);
            MapProperty(18, x => x.ImageURL);
            MapProperty(19, x => x.PageUrl);
        }
    }
}
