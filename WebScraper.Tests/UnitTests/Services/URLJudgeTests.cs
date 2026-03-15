using Microsoft.Extensions.Options;
using WebScraper.Model.Settings;
using WebScraper.Services.URLJudge;

namespace WebScraper.Tests.UnitTests.Services;

public class URLJudgeTests
{
  [Fact]
  public async Task IsValidURL_ShouldReturnTrue_ForValidURL()
  {
    // Arrange
    var crawlerSettings = new CrawlerSettings
    {
      MaxDepth = 3
    };

    var crawlerSettingsOptions = Options.Create(crawlerSettings);

    var urlJudge = new URLJudge(crawlerSettingsOptions);

    // Act
    var result = await urlJudge.ShouldScrape( new Uri("https://www.example.com"), 1, CancellationToken.None );

    // Assert
    Assert.True( result );
  }

  [Fact]
  public async Task ShouldReturnFalse_ForInvalidScheme()
  {
    // Arrange
    var crawlerSettings = new CrawlerSettings
    {
      MaxDepth = 3
    };
    var crawlerSettingsOptions = Options.Create(crawlerSettings);
    var urlJudge = new URLJudge(crawlerSettingsOptions);

    // Act
    var result = await urlJudge.ShouldScrape( new Uri("ftp://www.example.com"), 1, CancellationToken.None );

    // Assert
    Assert.False( result );
  }

  [Fact]
  public async Task ShouldReturnFalse_ForExcessiveDepth()
  {
    // Arrange
    var crawlerSettings = new CrawlerSettings
    {
      MaxDepth = 2
    };
    var crawlerSettingsOptions = Options.Create(crawlerSettings);
    var urlJudge = new URLJudge(crawlerSettingsOptions);
    
    // Act
    var result = await urlJudge.ShouldScrape( new Uri("https://www.example.com"), 3, CancellationToken.None );
    
    // Assert
    Assert.False( result );
  }

  [Theory]
  [InlineData("https://www.example.com/login")]
  [InlineData("https://www.example.com/signup")]
  [InlineData("https://www.example.com/register")]
  [InlineData("https://www.example.com/account")]
  [InlineData("https://www.example.com/search")]
  public async Task ShouldReturnFalse_ForLoginAndSimilarURLs(string url)
  {
    // Arrange
    var crawlerSettings = new CrawlerSettings
    {
      MaxDepth = 3
    };
    var crawlerSettingsOptions = Options.Create(crawlerSettings);
    var urlJudge = new URLJudge(crawlerSettingsOptions);
    
    // Act
    var result = await urlJudge.ShouldScrape( new Uri(url), 1, CancellationToken.None );
    
    // Assert
    Assert.False( result );
  }

  [Fact]
  public async Task ShouldReturnFalse_ForLongQueryString()
  {
    // Arrange
    var crawlerSettings = new CrawlerSettings
    {
      MaxDepth = 3
    };
    var crawlerSettingsOptions = Options.Create(crawlerSettings);
    var urlJudge = new URLJudge(crawlerSettingsOptions);
    var longQuery = new string('a', 201);
    var url = $"https://www.example.com/search?query={longQuery}";

    // Act
    var result = await urlJudge.ShouldScrape( new Uri(url), 1, CancellationToken.None );

    // Assert
    Assert.False( result );
  }

  [Fact]
  public async Task ShouldReturnFalse_ForCalendarLikeQuery()
  {
    // Arrange
    var crawlerSettings = new CrawlerSettings
    {
      MaxDepth = 3
    };
    var crawlerSettingsOptions = Options.Create(crawlerSettings);
    var urlJudge = new URLJudge(crawlerSettingsOptions);
    var url = "https://www.example.com/events?date=2024-12-31";

    // Act
    var result = await urlJudge.ShouldScrape( new Uri(url), 1, CancellationToken.None );

    // Assert
    Assert.False( result );
  }
}
