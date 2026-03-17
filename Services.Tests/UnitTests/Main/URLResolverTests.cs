using WebScraper.Services.URLResolver;

namespace WebScraper.Tests.UnitTests.Services;

public class URLResolverTests
{
  [Fact]
  public async Task Resolve_ShouldReturnFullURL_WhenFullURLIsProvided()
  {
    // Arrange
    var urlResolver = new URLResolver();
    string baseURL = "http://example.com";
    string fullURL = "http://example.com/page";

    // Act
    var result = await urlResolver.ResolveURL(baseURL, fullURL);

    // Assert
    Assert.Equal(fullURL, result);
  }

  [Fact]
  public async Task ResolveURL_ShouldReturnResolvedURL_WhenRelativeURLIsProvided()
  {
    // Arrange
    var urlResolver = new URLResolver();
    string baseURL = "http://example.com";
    string relativeURL = "/page";
    string expectedURL = "http://example.com/page";

    // Act
    var result = await urlResolver.ResolveURL(baseURL, relativeURL);

    // Assert
    Assert.Equal(expectedURL, result);
  }

  [Fact]
  public async Task ResolveURL_ShouldReturnEmptyString_WhenInvalidURLIsProvided()
  {
    // Arrange
    var urlResolver = new URLResolver();
    string baseURL = "http://example.com";
    string invalidURL = "http://";

    // Act
    var result = await urlResolver.ResolveURL(baseURL, invalidURL);

    // Assert
    Assert.Equal(string.Empty, result);
  }
}
