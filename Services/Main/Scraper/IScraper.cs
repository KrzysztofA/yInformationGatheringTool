using Core.Model;
using Services.Contracts;

namespace Services.Main.Scraper;

public interface IScraper
{
  public Task<ScrapedPage> Scrap( IndexedWebsite website, CancellationToken cancellationToken );
}
