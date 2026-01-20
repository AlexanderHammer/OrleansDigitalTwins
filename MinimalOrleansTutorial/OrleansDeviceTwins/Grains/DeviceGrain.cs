using Interfaces;
using Interfaces.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Orleans;

public partial class DeviceGrain(ILogger<DeviceGrain> logger) : Grain, IDeviceGrain
{
  private Device? _device;
  private DateTime _lastReceived = DateTime.MinValue;

  public override async Task OnActivateAsync(CancellationToken cancellationToken)
  {
    LogIdentityStringActivated(IdentityString);
    this.RegisterGrainTimer(CheckConnectionStatus, new()
      {
        DueTime = TimeSpan.FromSeconds(15), 
        Period = TimeSpan.FromSeconds(15),
      });
    await base.OnActivateAsync(cancellationToken);
  }

  public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
  {
    LogIdentityStringDeactivatedReasonSerialize(IdentityString, JsonSerializer.Serialize(reason));
    await base.OnDeactivateAsync(reason, cancellationToken);
  }

  public Task UpdateDevice(Device? device)
  {
    if (_lastReceived == DateTime.MinValue)
    {
      LogDeviceCreatedId(IdentityString);
    }
    
    if (_device is not null && _device.Equals(device))
    {
      LogDeviceStateUnchangedId(IdentityString);
    }
    else
    {
      LogDeviceChangedId(IdentityString);
    }
    
    _lastReceived = DateTime.Now;
    _device = device;

    LogSerialize(JsonSerializer.Serialize(_device));
    return Task.FromResult(_device);
  }
  private Task CheckConnectionStatus(CancellationToken token)
  {
    if (_lastReceived != DateTime.MinValue && DateTime.Now - _lastReceived >= TimeSpan.FromSeconds(30) && _device!.Connected)
    {
      LogDeviceIdentityStringDisconnected(IdentityString);
      _device = _device with { Connected = false };
    }
    LogCheckConnectionStatusCompletedForIdentity(IdentityString, JsonSerializer.Serialize(_device));
    return Task.CompletedTask;
  }
}
