using Interfaces.Models;

namespace Interfaces;

public interface IDeviceGrain : IGrainWithStringKey
{
  Task UpdateDevice(Device device);
}