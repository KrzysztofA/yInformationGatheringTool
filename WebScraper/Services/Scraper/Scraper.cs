using AngleSharp;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Threading;
using WebScraper.Model.Context;
using WebScraper.Model.Entry;
using WebScraper.Model.Settings;

namespace WebScraper.Services.Scraper;

public class Scraper(IHttpClientFactory httpClientFactory, IOptions<ScraperSettings> settings) : IScraper
{
  private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
  private readonly IOptions<ScraperSettings> _settings = settings;

  // Responsible only for fetching page HTML. If the static HTML appears to have little
  // or no content, a fallback attempt is made using AngleSharp's default loader which
  // can load additional resources. The fallback has a short timeout.
  public async Task<ScrapedPage> Scrap(IndexedWebsite website, CancellationToken cancellationToken)
  {
    try
    {
      var client = _httpClientFactory.CreateClient();
      if (!string.IsNullOrWhiteSpace( _settings.Value.DefaultUserAgent ))
      {
        client.DefaultRequestHeaders.UserAgent.ParseAdd( _settings.Value.DefaultUserAgent );
      }

      var response = await client.GetAsync( website.URL, cancellationToken );
      if (!response.IsSuccessStatusCode)
      {
        Console.WriteLine( $"Failed to retrieve content from {website.URL}. Status code: {response.StatusCode}" );
        return new ScrapedPage();
      }

      var content = await response.Content.ReadAsStringAsync( cancellationToken );
      return new()
      {
        Content = content,
        Success = true,
        Url = new Uri( website.URL ),
        ContentType = response.Content.Headers.ContentType?.MediaType ?? string.Empty
      };
    }
    catch (Exception ex)
    {
      Console.WriteLine( $"Error scraping {website.URL}: {ex.Message}" );
      return new ScrapedPage();
    }
  }
}
