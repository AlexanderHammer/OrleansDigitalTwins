using Microsoft.Extensions.Hosting;

namespace DeviceFunction.HttpFunction;

internal class Program
{
  private static void Main(string[] args)
  {
    var host = new HostBuilder()
      .ConfigureFunctionsWorkerDefaults()
      .UseOrleansClient(client =>
      {
        client.UseLocalhostClustering();
      })
      .Build();

    host.Run();
    Console.WriteLine("Client successfully connected to silo");
  }
}