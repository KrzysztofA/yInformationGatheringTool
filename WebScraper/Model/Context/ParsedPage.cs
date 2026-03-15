using AngleSharp.Dom;

namespace WebScraper.Model.Context;

public class ParsedPage
{
  public bool Success { get; set; } = false;
  public Uri Url { get; set; } = new Uri("http://example.com");
  public IDocument? Document { get; set; } = null;
}
