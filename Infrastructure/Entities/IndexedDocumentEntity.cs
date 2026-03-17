using SQLite;

namespace Infrastructure.Entities;

public class IndexedDocumentEntity
{
  [PrimaryKey, AutoIncrement] public long ID { get; set; }
  [Indexed] public Guid Guid { get; set; } = Guid.NewGuid();
  [Indexed] public long WebsiteID { get; set; }
  public string LocalPath { get; set; } = string.Empty;
  public string ExtractedTextPath { get; set; } = string.Empty;
  public bool Extracted { get; set; }
  public bool Summarized { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
