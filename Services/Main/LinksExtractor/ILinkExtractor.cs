using Services.Contracts;

namespace Services.Main.LinksExtractor;

public interface ILinkExtractor
{
  public Task<IEnumerable<string>> ExtractLinks( ParsedPage page );
}
