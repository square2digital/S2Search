namespace Services.Interfaces.Processors
{
    public interface IPurgeCacheProcessor
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}
