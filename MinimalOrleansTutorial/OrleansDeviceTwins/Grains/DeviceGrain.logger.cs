using Microsoft.Extensions.Logging;

namespace Orleans
{
  public partial class DeviceGrain
  {
    [LoggerMessage(LogLevel.Information, "{identityString} activated\n")]
    partial void LogIdentityStringActivated(string identityString);

    [LoggerMessage(LogLevel.Information, "{identityString} deactivated. Reason: {serialize}")]
    partial void LogIdentityStringDeactivatedReasonSerialize(string identityString, string serialize);
    
    [LoggerMessage(LogLevel.Information, "Device created {id}")]
    partial void LogDeviceCreatedId(string id);

    [LoggerMessage(LogLevel.Information, "Device changed: {id}")]
    partial void LogDeviceChangedId(string id);

    [LoggerMessage(LogLevel.Information, "Device state: {serialize}")]
    partial void LogSerialize(string serialize);

    [LoggerMessage(LogLevel.Information, "Device {identityString} disconnected")]
    partial void LogDeviceIdentityStringDisconnected(string identityString);

    [LoggerMessage(LogLevel.Information, @"CheckConnectionStatus completed for {identityString}:\n{serialize}\n")]
    partial void LogCheckConnectionStatusCompletedForIdentity(string identityString, string serialize);

    [LoggerMessage(LogLevel.Information, "Device state unchanged: {id}")]
    partial void LogDeviceStateUnchangedId(string id);
  }
}