using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;

try
{
  using var host = await StartSiloAsync();
  Console.WriteLine("\n\n Press Enter to terminate...\n\n");
  Console.ReadLine();

  await host.StopAsync();

  return 0;
}
catch (Exception ex)
{
  Console.WriteLine(ex);
  return 1;
}

static async Task<IHost> StartSiloAsync()
{
  var builder = new HostBuilder()
    .UseOrleans(builder =>
    {
      builder.UseLocalhostClustering().ConfigureLogging(logger => logger.AddConsole());
    });

  var host = builder.Build();
  await host.StartAsync();

  return host;
}