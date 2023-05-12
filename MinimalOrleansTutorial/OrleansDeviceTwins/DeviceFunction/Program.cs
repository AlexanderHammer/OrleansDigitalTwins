using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

internal class Program
{
  private static void Main(string[] args)
  {
    var host = new HostBuilder()
        .ConfigureFunctionsWorkerDefaults()
        .UseOrleansClient(client =>
        {
          client.UseLocalhostClustering();
          client.Configure<ClusterOptions>(options =>
          {
            options.ClusterId = "ClusterGroupName";
            options.ServiceId = "ClusterServiceId";
          });
        })
        .Build();

    host.Run();
    Console.WriteLine("Client successfully connected to silo");
  }
}