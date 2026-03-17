using Services.Contracts;

namespace Services.Main.HTMLParser;

public interface IHTMLParser
{
  public Task<ParsedPage> Parse(ScrapedPage scrapedPage, CancellationToken cancellationToken);
}
