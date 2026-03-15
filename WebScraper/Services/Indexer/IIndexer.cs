using WebScraper.Model.Entry;

namespace WebScraper.Services.Indexer;

public interface IIndexer
{
  public Task<bool> TryIndex(string url, string baseURL, long depth, CancellationToken cancellationToken);
}
