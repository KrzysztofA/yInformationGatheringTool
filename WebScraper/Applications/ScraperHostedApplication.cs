using Microsoft.Extensions.Hosting;
using WebScraper.Managers.ScraperManager;

namespace WebScraper.Applications;

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
