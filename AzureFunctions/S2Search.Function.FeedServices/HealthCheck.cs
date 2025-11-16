using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace S2Search.Function.FeedServices;

public class HealthCheck
{
    public HealthCheck()
    {
    }

    [Function("HealthCheck")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        
        response.WriteString($"{{\"status\":\"healthy\",\"timestamp\": \"{DateTime.UtcNow:O}\",\"service\":\"FeedServices\"}}");

        return response;
    }
}