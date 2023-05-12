using System.Net;
using System.Text.Json;
using DeviceFunction.HttpFunction.Models;
using Interfaces;
using Interfaces.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DeviceFunction.HttpFunction;

public class DeviceFunctions
{
  private readonly ILogger _logger;
  private readonly IGrainFactory _factory;

  public DeviceFunctions(IGrainFactory factory, ILoggerFactory loggerFactory)
  {
    _logger = loggerFactory.CreateLogger<DeviceFunctions>();
    _factory = factory ?? throw new ArgumentNullException(nameof(factory));
  }

  [Function(nameof(UpdateDevice))]
  public async Task<HttpResponseData> UpdateDevice([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req)
  {

    var data = JsonSerializer.Deserialize<UpdateDeviceRequest>(req.Body);
    var newDeviceState = new Device(data.Connected, data.Rssi, data.Tiers.ToList());
    var grain = _factory.GetGrain<IDeviceGrain>($"device@{data.DeviceId}");
    await grain.UpdateDevice(newDeviceState);
    return req.CreateResponse(HttpStatusCode.OK);
  }
}
