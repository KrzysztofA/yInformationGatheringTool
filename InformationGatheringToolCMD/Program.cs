using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Infrastructure.Settings;
using Services.Settings;
using Infrastructure.Repository.IndexedWebsiteRepository;
using Infrastructure.Database;
using Application.Managers.ScraperManager;
using Core.Interfaces.Repositories;
using Services.Main.Scraper;
using Services.Main.LinksExtractor;
using Services.Main.URLJudge;
using Services.Main.InternalLogger;
using Core.Interfaces.Logging;
using Services.Main.URLResolver;
using Services.Main.HTMLParser;
using Services.Main.Indexer;
using Infrastructure.FileIO.ContentCompressor;
using Mapster;

namespace InformationGatheringToolCMD;

internal class Program
{
  static async Task Main( string[] args )
  {
    var builder = Host.CreateApplicationBuilder(args);

    builder.Configuration.AddJsonFile( "appsettings.json" );

    builder.Services.Configure<DatabaseSettings>( builder.Configuration.GetSection( "DatabaseSettings" ) );
    builder.Services.Configure<ScraperSettings>( builder.Configuration.GetSection( "ScraperSettings" ) );
    builder.Services.Configure<CrawlerSettings>( builder.Configuration.GetSection( "CrawlerSettings" ) );

    builder.Services.AddMapster();

    builder.Services.AddSingleton<IDatabase, Database>();
    builder.Services.AddSingleton<IScraperManager, ScraperManager>();
    builder.Services.AddScoped<IIndexedWebsiteRepository, IndexedWebsiteRepository>();
    builder.Services.AddScoped<IScraper, Scraper>();
    builder.Services.AddScoped<ILinkExtractor, LinkExtractor>();
    builder.Services.AddScoped<IURLJudge, URLJudge>();
    builder.Services.AddScoped<IURLResolver, URLResolver>();
    builder.Services.AddScoped<IInternalLogger, ConsoleLogger>();
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
