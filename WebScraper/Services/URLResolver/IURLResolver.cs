using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraper.Services.URLResolver;

public interface IURLResolver
{
  public Task<string> ResolveURL( string baseURL, string fullURL );
}
