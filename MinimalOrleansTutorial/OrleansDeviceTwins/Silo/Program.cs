using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

try
{
  using IHost host = await StartSiloAsync();
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
  IHostBuilder? builder = new HostBuilder()
    .UseOrleans(builder =>
    {
      builder.UseLocalhostClustering().ConfigureLogging(logger => logger.AddConsole());
    });

  IHost host = builder.Build();
  await host.StartAsync();

  return host;
}