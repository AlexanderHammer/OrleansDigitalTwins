using System.Net;
using System.Text.Json;
using DeviceFunction.HttpFunction.Models;
using Interfaces;
using Interfaces.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DeviceFunction.HttpFunction;

public class DeviceFunctions(IGrainFactory factory)
{
  private readonly IGrainFactory _factory = factory ?? throw new ArgumentNullException(nameof(factory));

  [Function(nameof(UpdateDevice))]
  public async Task<HttpResponseData> UpdateDevice([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req)
  {
    UpdateDeviceRequest? data = JsonSerializer.Deserialize<UpdateDeviceRequest>(req.Body);
    if(data == null) return req.CreateResponse(HttpStatusCode.BadRequest);
    
    Device newDeviceState = new (data.Connected, data.Rssi, data.Tiers.ToList());
    IDeviceGrain grain = _factory.GetGrain<IDeviceGrain>($"device@{data.DeviceId}");
    await grain.UpdateDevice(newDeviceState);
    return req.CreateResponse(HttpStatusCode.OK);
  }
}
