using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraper.Model.Settings;

public record DatabaseSettings
{
  public string Path { get; init; } = string.Empty;
}
