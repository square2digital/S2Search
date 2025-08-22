// See https://aka.ms/new-console-template for more information
using CacheManager.Extensions;
using Microsoft.Extensions.Hosting;
using Services.Interfaces.Processors;

Console.WriteLine("Cache Manager Starting up...");

var host = HostBuilderExtensions.CreateHostBuilder(args).Build();
var cancelTokenSource = new CancellationTokenSource();

using (host)
{
    try
    {
        await host.Instance<IPurgeCacheProcessor>().RunAsync(cancelTokenSource.Token);
    }
    catch
    {
        Console.WriteLine("Exception caught");
    }
    finally
    {
        //pause here to allow the logs
        cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
        await host.WaitForShutdownAsync(cancelTokenSource.Token);
    }
}
    
Console.WriteLine("Cache Manager Shutting Down...");