using WebScraper.Services.ContentCompressor;

namespace WebScraper.Tests.UnitTests.Services;

public class ContentCompressorTests
{
  [Fact]
  public async Task CompressAndDecompress_ShouldReturnOriginalContent()
  {
    // Arrange
    var compressor = new ContentCompressor();
    var originalContent = "This is a test string to be compressed and decompressed.";

    // Act
    var compressedFilePath = "/compressed.br";
    await compressor.CompressToFile( originalContent, compressedFilePath );
    var decompressedContent = await compressor.DecompressFromFile( compressedFilePath );

    // Assert

    // Verify that the compressed file was created and the decompressed content matches the original
    Assert.True( File.Exists( compressedFilePath ) );
    Assert.Equal( originalContent, decompressedContent );

    // Clean up 
    if ( File.Exists( compressedFilePath ) )
    {
      File.Delete( compressedFilePath );
    }
  }
}
