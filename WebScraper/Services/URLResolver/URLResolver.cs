using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraper.Services.URLResolver;

public class URLResolver : IURLResolver
{
  public async Task<string> ResolveURL( string baseURL, string fullURL )
  {
    if (fullURL.StartsWith( "http://" ) || fullURL.StartsWith( "https://" ))
    {
      return fullURL;
    }
    else
    {
      try
      {
        var baseUri = new Uri(baseURL);
        var resolvedUri = new Uri(baseUri, fullURL);
        return resolvedUri.AbsoluteUri;
      }
      catch
      {
        return string.Empty;
      }
    }
  }
}
