using Interfaces;
using Interfaces.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Orleans;

public partial class DeviceGrain(ILogger<DeviceGrain> logger) : Grain, IDeviceGrain
{
  private Device? _device;
  private DateTime _lastReceived = DateTime.MinValue;
  private IGrainTimer? _timer;

  public override async Task OnActivateAsync(CancellationToken cancellationToken)
  {
    LogIdentityStringActivated(IdentityString);
    RegisterCheckStateTimer();
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

    _lastReceived = DateTime.UtcNow;
    _device = device;

    if (_timer is null) RegisterCheckStateTimer();
    LogSerialize(JsonSerializer.Serialize(_device));
    return Task.FromResult(_device);
  }

  private Task CheckConnectionStatus(CancellationToken token)
  {
    if (_lastReceived != DateTime.MinValue &&
        DateTime.UtcNow - _lastReceived >= TimeSpan.FromMinutes(2) &&
        _device!.Connected)
    {
      LogDeviceIdentityStringDisconnected(IdentityString);
      _device = _device with { Connected = false };
      _timer?.Dispose();
      _timer = null;
    }

    LogCheckConnectionStatusCompletedForIdentity(IdentityString, JsonSerializer.Serialize(_device));
    return Task.CompletedTask;
  }

  private void RegisterCheckStateTimer()
  {
    _timer = this.RegisterGrainTimer(CheckConnectionStatus,
      new() { KeepAlive = true, DueTime = TimeSpan.FromMinutes(1), Period = TimeSpan.FromMinutes(1), });
  }
}