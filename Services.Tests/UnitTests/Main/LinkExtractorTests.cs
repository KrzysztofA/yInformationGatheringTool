using WebScraper.Model.Context;
using WebScraper.Services.LinksExtractor;
using WebScraper.Tests.Helpers;

namespace WebScraper.Tests.UnitTests.Services;

public class LinkExtractorTests
{
  [Fact]
  public async Task LinkExtractor_ShouldExtractLinks_Correctly()
  {
    // Arrange
    var linkExtractor = new LinkExtractor();

    var html = """
        <a href="/a">A</a>
    """;

    var document = DocumentFactory.CreateDocumentFromString(html);

    var page = new ParsedPage
    {
      Document = document,
      Url = new Uri("http://example.com"),
      Success = true
    };

    // Act
    var links = await linkExtractor.ExtractLinks( page );

    // Assert

    // Should not be null
    Assert.NotNull( links );

    // Should be 1 link
    Assert.Single( links );

    // Should be the correct link
    Assert.Contains( "/a", links );
  }

  [Fact]
  public async Task LinkExtractor_ShouldReturnAnEmptyList_WhenNoLinks()
  {
    // Arrange
    var linkExtractor = new LinkExtractor();
    var html = """
        <p>No links here</p>
    """;
    var document = DocumentFactory.CreateDocumentFromString(html);
    var page = new ParsedPage
    {
      Document = document,
      Url = new Uri("http://example.com"),
      Success = true
    };

    // Act
    var links = await linkExtractor.ExtractLinks( page );

    // Assert

    // Should not be null
    Assert.NotNull( links );

    // Should be empty
    Assert.Empty( links );
  }

  [Fact]
  public async Task LinkExtractor_ShouldIgnoreEmptyLinks()
  {
    // Arrange
    var linkExtractor = new LinkExtractor();
    var html = """
        <a href="">Empty</a>
        <a>No href</a>
    """;
    var document = DocumentFactory.CreateDocumentFromString(html);
    var page = new ParsedPage
    {
      Document = document,
      Url = new Uri("http://example.com"),
      Success = true
    };

    // Act
    var links = await linkExtractor.ExtractLinks( page );

    // Assert

    // Should not be null
    Assert.NotNull( links );

    // Should be empty
    Assert.Empty( links );
  }

  [Fact]
  public async Task LinkExtractor_ShouldExtractMultipleLinks()
  {
    // Arrange
    var linkExtractor = new LinkExtractor();
    var html = """
        <a href="/a">A</a>
        <a href="/b">B</a>
        <a href="/c">C</a>
    """;
    var document = DocumentFactory.CreateDocumentFromString(html);
    var page = new ParsedPage
    {
      Document = document,
      Url = new Uri("http://example.com"),
      Success = true
    };

    // Act
    var links = await linkExtractor.ExtractLinks( page );

    // Assert

    // Should not be null
    Assert.NotNull( links );

    // Should be 3 links
    Assert.Equal( 3, links.Count() );

    // Should contain the correct links
    Assert.Contains( "/a", links );
    Assert.Contains( "/b", links );
    Assert.Contains( "/c", links );
  }
}
