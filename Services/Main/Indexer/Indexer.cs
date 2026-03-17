using Core.Enumerations;
using Core.Interfaces.Repositories;
using Core.Model;
using Services.Main.URLJudge;

namespace Services.Main.Indexer;

public class Indexer(IURLJudge urlJudge, IIndexedWebsiteRepository indexedWebsiteRepository) : IIndexer
{
  private readonly IURLJudge _urlJudge = urlJudge;
  private readonly IIndexedWebsiteRepository _indexedWebsiteRepository = indexedWebsiteRepository;

  public async Task<bool> TryIndex(string url, string baseURL, long depth, CancellationToken cancellationToken)
  {
    // Check if the URL should be scraped based on various criteria
    if (!await _urlJudge.ShouldScrape(new Uri(url), depth, cancellationToken)) return false;

    // Create a new IndexedWebsite entry in the database with status "Indexed" for this URL, if it doesn't already exist
    var indexedWebsite = new IndexedWebsite()
    {
      URL = url,
      BaseURL = baseURL,
      Depth = depth,
      Status = IndexedWebsiteStatus.Indexed
    };

    if (!await _indexedWebsiteRepository.Create( indexedWebsite )) return false;

    // If the entry was successfully created, return true to indicate that the URL has been indexed
    return true;
  }
}