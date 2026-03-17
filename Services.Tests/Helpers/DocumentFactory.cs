using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace WebScraper.Tests.Helpers;

internal static class DocumentFactory
{
  public static IDocument CreateDocumentFromString(string html)
  {
    var parser = new HtmlParser();
    return parser.ParseDocument(html);
  }
}
