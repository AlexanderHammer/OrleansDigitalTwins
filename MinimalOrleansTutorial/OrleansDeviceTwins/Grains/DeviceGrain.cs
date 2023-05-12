using Interfaces;
using Interfaces.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Orleans;

public class DeviceGrain : Grain, IDeviceGrain
{
  private Device _device;
  private DateTime _lastReceived = DateTime.Now;

  private readonly ILogger _logger;

  public DeviceGrain(ILogger<DeviceGrain> logger)
  {
    _logger = logger;
  }
  public override async Task OnActivateAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("{IdentityString} activated\\n", IdentityString);
    RegisterTimer(
        _ =>
        {
          CheckConnectionStatus();
          return Task.CompletedTask;
        },
        null,
        TimeSpan.FromSeconds(15),
        TimeSpan.FromSeconds(15));
    await base.OnActivateAsync(cancellationToken);
  }

  public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
  {
    _logger.LogInformation("{IdentityString} deactivated.\\nReason: {Serialize}", IdentityString, JsonSerializer.Serialize(reason));
    await base.OnDeactivateAsync(reason, cancellationToken);
  }

  public Task UpdateDevice(Device device)
  {
    if (_device == null)
      _logger.LogInformation("Device created\n");

    if (_device != null && !device.Equals(_device))
      _logger.LogInformation("Device changed\n");

    _lastReceived = DateTime.Now;
    _device = device;

    _logger.LogInformation("{Serialize}\\n", JsonSerializer.Serialize(_device));
    return Task.FromResult(_device);
  }
  private void CheckConnectionStatus()
  {
    if (DateTime.Now - _lastReceived >= TimeSpan.FromSeconds(30) && _device.Connected)
    {
      _logger.LogInformation("Device {IdentityString} disconnected", IdentityString);
      _device = new Device(false, _device.Rssi, _device.Tiers);
    }
    _logger.LogInformation("CheckConnectionStatus completed for {IdentityString}:\\n{Serialize}\\n", IdentityString, JsonSerializer.Serialize(_device));
  }
}
