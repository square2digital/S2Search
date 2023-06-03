using Domain.AzureSearch.Index;
using TinyCsvParser.Mapping;

namespace Services.Mappers
{
    public class CsvMapDMS14 : CsvMapping<DMS14>
    {
        public CsvMapDMS14() : base()
        {
            MapProperty(0, x => x.Vehicle_ID);
            MapProperty(1, x => x.FullRegistration);
            MapProperty(2, x => x.Make);
            MapProperty(3, x => x.Model);
            MapProperty(4, x => x.Variant);
            MapProperty(5, x => x.Site);
            MapProperty(6, x => x.Price);
            MapProperty(7, x => x.MonthlyPrice);
            MapProperty(8, x => x.Mileage);
            MapProperty(9, x => x.FuelType);
            MapProperty(10, x => x.Transmission);
            MapProperty(11, x => x.Doors);
            MapProperty(12, x => x.EngineSize);
            MapProperty(13, x => x.Bodytype);
            MapProperty(14, x => x.Colour);
            MapProperty(15, x => x.Year);
            MapProperty(16, x => x.PictureRefs);
            MapProperty(17, x => x.PageUrl);
        }
    }
}
