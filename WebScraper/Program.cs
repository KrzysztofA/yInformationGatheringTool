using AngleSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UglyToad.PdfPig;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using WebScraper.Database;
using WebScraper.Repository.IndexedWebsiteRepository;
using WebScraper.Services.Scraper;
using WebScraper.Services.InternalLogger;
using WebScraper.Managers.ScraperManager;
using WebScraper.Repository.InternalLogRepository;
using WebScraper.Services.LinksExtractor;
using WebScraper.Services.HTMLParser;
using WebScraper.Services.Indexer;
using WebScraper.Services.ContentCompressor;

namespace WebScraper;

internal class Program
{
  static async Task Main( string[] args )
  {
    var builder = Host.CreateApplicationBuilder(args);

    builder.Configuration.AddJsonFile( "appsettings.json" );

    builder.Services.Configure<Model.Settings.DatabaseSettings>( builder.Configuration.GetSection( "DatabaseSettings" ) );
    builder.Services.Configure<Model.Settings.ScraperSettings>( builder.Configuration.GetSection( "ScraperSettings" ) );
    builder.Services.Configure<Model.Settings.CrawlerSettings>( builder.Configuration.GetSection( "CrawlerSettings" ) );

    builder.Services.AddSingleton<IDatabase, Database.Database>();
    builder.Services.AddSingleton<IScraperManager, ScraperManager>();
    builder.Services.AddScoped<IInternalLogRepository, InternalLogRepository>();
    builder.Services.AddScoped<IIndexedWebsiteRepository, IndexedWebsiteRepository>();
    builder.Services.AddScoped<IScraper, Scraper>();
    builder.Services.AddScoped<ILinkExtractor, LinkExtractor>();
    builder.Services.AddScoped<Services.URLJudge.IURLJudge, Services.URLJudge.URLJudge>();
    builder.Services.AddScoped<Services.URLResolver.IURLResolver, Services.URLResolver.URLResolver>();
    builder.Services.AddScoped<IInternalLogger, InternalLogger>();
    builder.Services.AddScoped<IHTMLParser, HTMLParser>();
    builder.Services.AddScoped<IIndexer, Indexer>();
    builder.Services.AddScoped<IContentCompressor, ContentCompressor>();
    builder.Services.AddHttpClient();
    builder.Services.AddHostedService<Applications.ScraperHostedApplication>();


    builder.Logging.AddConsole();

    var host = builder.Build();
    await host.RunAsync();
  }
}
