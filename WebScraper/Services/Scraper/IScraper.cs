using WebScraper.Model.Context;
using WebScraper.Model.Entry;

namespace WebScraper.Services.Scraper;

public interface IScraper
{
  public Task<ScrapedPage> Scrap( IndexedWebsite website, CancellationToken cancellationToken );
}
