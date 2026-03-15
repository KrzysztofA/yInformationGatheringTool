using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraper.Services.ContentCompressor;

public interface IContentCompressor
{
  public Task CompressToFile( string content, string filePath );
  public Task<string> DecompressFromFile( string filePath );
}
