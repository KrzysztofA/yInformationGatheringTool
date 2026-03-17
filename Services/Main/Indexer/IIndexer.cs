namespace Services.Main.Indexer;

public interface IIndexer
{
  public Task<bool> TryIndex(string url, string baseURL, long depth, CancellationToken cancellationToken);
}
