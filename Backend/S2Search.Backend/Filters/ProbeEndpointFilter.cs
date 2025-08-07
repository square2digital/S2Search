using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace S2Search.Backend.Filters;

public class ProbeEndpointFilter : ITelemetryProcessor
{
    private ITelemetryProcessor _next { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string ExcludedEndpoint = "/api/status";

    // next will point to the next TelemetryProcessor in the chain.
    public ProbeEndpointFilter(ITelemetryProcessor next,
                               IHttpContextAccessor httpContextAccessor)
    {
        _next = next;
        _httpContextAccessor = httpContextAccessor;
    }

    public void Process(ITelemetry item)
    {
        // To filter out an item, return without calling the next processor.
        if (!OkToSend(item)) { return; }

        _next.Process(item);
    }

    private bool OkToSend(ITelemetry item)
    {
        if (!string.IsNullOrWhiteSpace(item.Context.Operation.SyntheticSource))
        {
            return false;
        }

        var isProbeEndpoint = _httpContextAccessor.HttpContext != null
                           && _httpContextAccessor.HttpContext.Request.Path.HasValue
                           && _httpContextAccessor.HttpContext.Request.Path.Value.ToLower().Contains(ExcludedEndpoint);

        if (!(item is RequestTelemetry) || isProbeEndpoint)
        {
            return false;
        }

        return true;
    }
}