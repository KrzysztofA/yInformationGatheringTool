using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace WebScraper.Services.ContentCompressor;

public class ContentCompressor : IContentCompressor
{
  public async Task CompressToFile( string content, string filePath )
  {
    
    await using var file = File.Create(filePath);
    await using var brotli = new BrotliStream(file, CompressionLevel.Optimal);
    await using var writer = new StreamWriter(brotli);

    await writer.WriteAsync( content );
  }

  public async Task<string> DecompressFromFile( string filePath )
  {
    await using var file = File.OpenRead(filePath);
    await using var brotli = new BrotliStream(file, CompressionMode.Decompress);

    using var reader = new StreamReader(brotli, Encoding.UTF8);
    return await reader.ReadToEndAsync();
  }
}
