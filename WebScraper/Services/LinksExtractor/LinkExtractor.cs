using WebScraper.Model.Context;
using WebScraper.Services.URLResolver;

namespace WebScraper.Services.LinksExtractor;

public partial class LinkExtractor : ILinkExtractor
{
  public async Task<IEnumerable<string>> ExtractLinks(ParsedPage page)
  {
    var links = new List<string>();

    page.Document?.QuerySelectorAll("a").ToList().ForEach(a =>
    {
      var href = a.GetAttribute("href");
      if (!string.IsNullOrEmpty(href))
      {
        links.Add(href);
      }
    });

    return links;
  }
}
