using System.Text.Json.Serialization;

namespace Interfaces.Models;

[GenerateSerializer]
public class Tier
{
  [JsonPropertyName("tierNumber")]
  [Id(0)] public int TierNumber { get; set; }

  [JsonPropertyName("counter")]
  [Id(1)] public int Counter { get; set; }

  [JsonPropertyName("state")]
  [Id(2)] public SignalTowerTierState State { get; set; }
}