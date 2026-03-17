using Application.Managers.ScraperManager;
using Microsoft.Extensions.Hosting;

namespace InformationGatheringToolCMD.Applications;

public class ScraperHostedApplication( IScraperManager scraperManager ) : IHostedService
{
  private readonly IScraperManager _scraperManager = scraperManager;

  public async Task StartAsync( CancellationToken cancellationToken )
  {
    await _scraperManager.StartAsync();
  }

  public Task StopAsync( CancellationToken cancellationToken )
  {
    return Task.CompletedTask;
  }
}
