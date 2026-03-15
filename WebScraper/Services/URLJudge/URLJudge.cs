using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Model.Context;
using WebScraper.Model.Settings;

namespace WebScraper.Services.URLJudge;

public partial class URLJudge(IOptions<CrawlerSettings> settings) : IURLJudge
{
  private readonly IOptions<CrawlerSettings> _settings = settings;

  public async Task<bool> ShouldScrape( Uri uri, long depth, CancellationToken cancellationToken )
  {
    // Depth control
    if (_settings.Value.MaxDepth == -1 && depth > _settings.Value.MaxDepth)
      return false;

    // Only HTTP/HTTPS and file for PDFs
    if (uri.Scheme != Uri.UriSchemeHttp &&
        uri.Scheme != Uri.UriSchemeHttps &&
        uri.Scheme != Uri.UriSchemeFile)
      return false;

    var path = uri.AbsolutePath.ToLowerInvariant();

    // Avoid obvious infinite traps
    if (path.Contains( "/login" ) ||
        path.Contains( "/signup" ) ||
        path.Contains( "/register" ) ||
        path.Contains( "/search" ) ||
        path.Contains( "/account" ) 
        ) 
        
      return false;

    // Block extremely long query strings
    if (uri.Query.Length > 200)
      return false;

    // Block calendar-like patterns
    if (CalendardLikeURLRegex().IsMatch( uri.Query ))
      return false;

    return true;
  }

  [System.Text.RegularExpressions.GeneratedRegex( @"\d{4}-\d{2}-\d{2}" )]
  private static partial System.Text.RegularExpressions.Regex CalendardLikeURLRegex();
}
