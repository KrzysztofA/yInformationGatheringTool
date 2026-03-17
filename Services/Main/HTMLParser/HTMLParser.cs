using AngleSharp;
using Services.Contracts;

namespace Services.Main.HTMLParser;

public class HTMLParser : IHTMLParser
{
  public async Task<ParsedPage> Parse( ScrapedPage scrapedPage, CancellationToken cancellationToken )
  {
    var context = BrowsingContext.New(Configuration.Default);

    var document = await context.OpenAsync( req => req.Content(scrapedPage.Content).Address(scrapedPage.Url.ToString()), cancel: cancellationToken );

    return new ParsedPage
    {
      Success = true,
      Url = scrapedPage.Url,
      Document = document
    };
  }
}
