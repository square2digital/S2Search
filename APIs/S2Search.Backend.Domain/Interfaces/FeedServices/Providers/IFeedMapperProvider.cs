using Services.Interfaces.Mappers;

namespace Services.Interfaces.Providers
{
    public interface IFeedMapperProvider
    {
        IFeedMapper GetMapper(string feedDataFormat);
    }
}