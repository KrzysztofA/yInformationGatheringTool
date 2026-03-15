using WebScraper.Model.Context;

namespace WebScraper.Services.LinksExtractor;

public interface ILinkExtractor
{
  public Task<IEnumerable<string>> ExtractLinks( ParsedPage page );
}
