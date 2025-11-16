using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace S2Search.Functions.SearchInsights;

public class HealthCheck
{
    [Function("HealthCheck")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);

        var payload = new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow.ToString("O"),
            service = "FeedServices"
        };

        await response.WriteAsJsonAsync(payload);

        return response;
    }
}