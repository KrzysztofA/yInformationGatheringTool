using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraper.Model.Context;

public class ScrapedPage
{
  public bool Success { get; set; } = false;
  public Uri Url { get; set; } = new Uri("http://example.com");
  public string ContentType { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;
}
