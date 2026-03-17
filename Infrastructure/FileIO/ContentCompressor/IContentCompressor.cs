namespace Infrastructure.FileIO.ContentCompressor;

public interface IContentCompressor
{
  public Task CompressToFile( string content, string filePath );
  public Task<string> DecompressFromFile( string filePath );
}
