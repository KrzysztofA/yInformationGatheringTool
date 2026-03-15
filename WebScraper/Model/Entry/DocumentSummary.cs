using SQLite;

namespace WebScraper.Model.Entry;

public class DocumentSummary
{
  [PrimaryKey, AutoIncrement] public long ID { get; set; }
  [Indexed] public Guid Guid { get; set; } = Guid.NewGuid();
  public string SummaryText { get; set; } = string.Empty;
  public string ModelUsed { get; set; } = string.Empty;
  public long IndexedWebsiteID { get; set; } = 0;
  public long IndexedDocumentID { get; set; } = 0;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
