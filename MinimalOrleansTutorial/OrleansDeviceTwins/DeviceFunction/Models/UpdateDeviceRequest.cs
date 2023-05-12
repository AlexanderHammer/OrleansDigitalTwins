using System.Text.Json.Serialization;
using Interfaces.Models;

namespace DeviceFunction.HttpFunction.Models;

public class UpdateDeviceRequest
{
  [JsonPropertyName("id")]
  public Guid DeviceId { get; set; }

  [JsonPropertyName("connected")]
  public bool Connected { get; set; }

  [JsonPropertyName("rssi")]
  public int Rssi { get; set; }

  [JsonPropertyName("tiers")]
  public Tier[] Tiers { get; set; }
}