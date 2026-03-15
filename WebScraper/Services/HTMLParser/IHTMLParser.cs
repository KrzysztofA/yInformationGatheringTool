using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Model.Context;

namespace WebScraper.Services.HTMLParser;

public interface IHTMLParser
{
  public Task<ParsedPage> Parse(ScrapedPage scrapedPage, CancellationToken cancellationToken);
}
