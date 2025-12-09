using Interfaces;
using Interfaces.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Orleans;

public class DeviceGrain(ILogger<DeviceGrain> logger) : Grain, IDeviceGrain
{
  private Device? _device;
  private DateTime _lastReceived = DateTime.MinValue;

  private readonly ILogger _logger = logger;

  public override async Task OnActivateAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("{IdentityString} activated\n", IdentityString);
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
    _logger.LogInformation("{IdentityString} deactivated. Reason: {Serialize}", IdentityString, JsonSerializer.Serialize(reason));
    await base.OnDeactivateAsync(reason, cancellationToken);
  }

  public Task UpdateDevice(Device? device)
  {
    if (device is null)
    {
      _logger.LogInformation("Device created {id}", IdentityString);
    }
    
    if (_device is not null)
    {
      if (_device.Equals(device))
      {
        _lastReceived = DateTime.Now;
        _logger.LogInformation("Device state unchanged: {id}", IdentityString);
        return Task.FromResult(_device);
      }
      _logger.LogInformation("Device changed: {id}", IdentityString);
    }
    
    
    _lastReceived = DateTime.Now;
    _device = device;

    _logger.LogInformation("{Serialize}", JsonSerializer.Serialize(_device));
    return Task.FromResult(_device);
  }
  private void CheckConnectionStatus()
  {
    if (_device != null && DateTime.Now - _lastReceived >= TimeSpan.FromSeconds(30) && _device.Connected)
    {
      _logger.LogInformation("Device {IdentityString} disconnected", IdentityString);
      _device = _device with { Connected = false };
    }
    _logger.LogInformation(@"CheckConnectionStatus completed for {IdentityString}:\n{Serialize}\n", IdentityString, JsonSerializer.Serialize(_device));
  }
}
