using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraper.Model.Context;

public class CrawlContext
{
  public int Depth { get; init; } = 0;
  public string ParentUrl { get; init; } = string.Empty;
}
