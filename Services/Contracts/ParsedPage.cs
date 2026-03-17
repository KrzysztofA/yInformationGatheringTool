using AngleSharp.Dom;

namespace Services.Contracts;

public class ParsedPage
{
  public bool Success { get; set; } = false;
  public Uri Url { get; set; } = new Uri("http://example.com");
  public IDocument? Document { get; set; } = null;
}
