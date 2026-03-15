using Microsoft.Extensions.Options;
using System.Runtime;
using WebScraper.Library;
using WebScraper.Model.Settings;
using WebScraper.Repository.IndexedWebsiteRepository;
using WebScraper.Services.ContentCompressor;
using WebScraper.Services.HTMLParser;
using WebScraper.Services.Indexer;
using WebScraper.Services.InternalLogger;
using WebScraper.Services.LinksExtractor;
using WebScraper.Services.Scraper;
using WebScraper.Services.URLResolver;

namespace WebScraper.Managers.ScraperManager;

public class ScraperManager(IInternalLogger internalLogger, IScraper scraper, IOptions<ScraperSettings> options, IIndexedWebsiteRepository indexedWebsiteRepository, ILinkExtractor linksExtractor, IHTMLParser htmlParser, IIndexer indexer, IContentCompressor contentCompressor, IURLResolver urlResolver ) : IScraperManager
{
  private readonly ManagedServiceControlFlowOrchestrator _flow = new();
  private readonly IScraper _scraper = scraper;
  private readonly IOptions<ScraperSettings> _options = options;
  private readonly IIndexedWebsiteRepository _indexedWebsiteRepository = indexedWebsiteRepository;
  private readonly ILinkExtractor _linkExtractor = linksExtractor;
  private readonly IHTMLParser _htmlParser = htmlParser;
  private readonly IIndexer _indexer = indexer;
  private readonly IContentCompressor _contentCompressor = contentCompressor;
  private readonly IURLResolver _urlResolver = urlResolver;

  public ConcurrencyManager Concurrency { get; private set; } = new( internalLogger, 3 );

  public async Task ExecuteAsync()
  {
    if (!await _flow.TryStartAsync()) return;

    // Mark a new generation of work
    Concurrency.NextGeneration();

    try
    {
      while (!_flow.Token.IsCancellationRequested)
      {
        // 1. Check if we are paused
        await _flow.WaitForPermissionAsync();

        // 2. Offload work to the concurrency manager
        // This will naturally "throttle" the loop because of the internal semaphore
        await Concurrency.RunAsync( ProcessNextWebsite, _flow.Token );

        // 3. Small yield to keep the UI responsive and prevent tight-looping
        await Task.Yield();
      }
    }
    catch (OperationCanceledException) { /* Clean exit */ }
    finally
    {
      // 4. Force state back to Stopped on exit
      _flow.Stop();
      await Concurrency.WaitForAllCurrentTasksAsync();
    }
  }

  private async Task ProcessNextWebsite( CancellationToken token )
  {
    // Fetch the next website to scrape from a database or create a new one from the start URL
    var uri = new Uri(_options.Value.StartURL);

    var website = await _indexedWebsiteRepository.GetNextForScraping();
    if (website == null)
    {
      website = new Model.Entry.IndexedWebsite()
      {
        URL = _options.Value.StartURL,
        BaseURL = _options.Value.StartBaseURL,
        Domain = uri.Host,
        Depth = 0,
      };

      var success = await _indexedWebsiteRepository.Create( website );
      if(!success)
      {
        // If we failed to create the initial entry, log and exit
        Console.WriteLine($"Failed to create initial website entry for {_options.Value.StartURL}");
        return;
      }
    }

    // Scrap the website content
    var content = await _scraper.Scrap( website, token );

    // If content is null or empty, mark as failed and return
    if (!content.Success)
    {
      await _indexedWebsiteRepository.UpdateStatus( website, Model.Entry.IndexedWebsiteStatus.Failed );
      return;
    }

    // Otherwise, we have content to process. Mark as processing and parse the content to the document object model (DOM)
    await _indexedWebsiteRepository.UpdateStatus( website, Model.Entry.IndexedWebsiteStatus.Processing );
    var document = await _htmlParser.Parse(content, token);

    // Extract links
    var links = await _linkExtractor.ExtractLinks( document );
    Console.WriteLine($"Extracted {links.Count()} links from {website.URL}");
    
    // Resolve links to absolute URLs
    var resolvedLinks = new List<string>();
    foreach ( var link in links )
    {
      var resolvedLink = await _urlResolver.ResolveURL( website.BaseURL, link );
      if (!string.IsNullOrWhiteSpace(resolvedLink))
      {
        resolvedLinks.Add(resolvedLink);
      }
    }

    // Try to index links one by one, checking for duplicates and respecting the max depth
    var successfullyIndexedLinks = 0;
    foreach (var link in resolvedLinks)
    {
      if(await _indexer.TryIndex( link, website.BaseURL, website.Depth + 1, token ))
      {
        successfullyIndexedLinks++;
      }
    }

    Console.WriteLine($"Scraped {website.URL} - Extracted {resolvedLinks.Count} links, Indexed {successfullyIndexedLinks} new links." );

    // Compress and store the scraped content in the file system
    var compressedFilePath = Path.Combine(_options.Value.FilePath, $"{website.Guid}.br");

    Console.WriteLine( $"Compressing and saving the content to: {compressedFilePath}" );
    await _contentCompressor.CompressToFile(content.Content, compressedFilePath);
    Console.WriteLine($"Content compressed and saved to: {compressedFilePath}");
    
    // Update the website entry in the database with the new information and mark it as scraped
    website.Status = Model.Entry.IndexedWebsiteStatus.Parsed;
    website.ContentPath = compressedFilePath;
    await _indexedWebsiteRepository.Update( website );

    Console.WriteLine($"Finished working on website: {website.URL}" );
  }

  public async Task StartAsync()
  {
    _ = ExecuteAsync();
  }

  // Expose control to the ViewModel
  public ManagedServiceControlFlowOrchestrator Control => _flow;
}