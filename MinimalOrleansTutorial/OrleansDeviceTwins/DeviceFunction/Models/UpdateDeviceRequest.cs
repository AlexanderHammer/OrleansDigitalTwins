using System.Text.Json.Serialization;
using Interfaces.Models;

namespace DeviceFunction.HttpFunction.Models;

public class UpdateDeviceRequest
{
  [JsonPropertyName("id")]
  public required Guid DeviceId { get; init; }

  [JsonPropertyName("connected")]
  public required bool Connected { get; init; }

  [JsonPropertyName("rssi")]
  public required int Rssi { get; init; }

  [JsonPropertyName("tiers")]
  public required Tier[] Tiers { get; init; }
}