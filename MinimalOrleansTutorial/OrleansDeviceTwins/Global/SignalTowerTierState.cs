namespace Interfaces.Models;

[Serializable]
public enum SignalTowerTierState
{
    Off = 0,
    On = 1,
    BlinkSlow = 2,
    BlinkFast = 3,
    Disconnected = 4,
    Ignored = 99
}