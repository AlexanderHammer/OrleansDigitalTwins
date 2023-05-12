namespace Interfaces.Models;

[GenerateSerializer]
public sealed record Device(bool Connected, int Rssi, List<Tier> Tiers)
{
  public bool Equals(Device other)
  {
    if (other == null || Connected != other.Connected || Rssi != other.Rssi) return false;

    foreach (var tier in Tiers)
    {
      var otherTier = other.Tiers
          .FirstOrDefault(x => x.TierNumber == tier.TierNumber);
      if (otherTier == null)
        return false;
      if (tier.Counter != otherTier.Counter || tier.State != otherTier.State)
        return false;
    }
    return true;
  }
}