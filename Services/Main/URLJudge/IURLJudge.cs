namespace Services.Main.URLJudge;

public interface IURLJudge
{
  public Task<bool> ShouldScrape( Uri uri, long depth, CancellationToken cancellationToken);
}
