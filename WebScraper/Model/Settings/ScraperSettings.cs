using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraper.Model.Settings;

public class ScraperSettings
{
  public string StartURL { get; set; } = string.Empty;
  public string StartBaseURL { get; set; } = string.Empty;
  public string FilePath { get; set; } = string.Empty;
  public string DefaultUserAgent { get; set; } = string.Empty;
}
