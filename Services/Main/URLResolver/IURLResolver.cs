namespace Services.Main.URLResolver;

public interface IURLResolver
{
  public Task<string> ResolveURL( string baseURL, string fullURL );
}
