using SQLite;

using Core.Enumerations;

namespace Infrastructure.Entities;

public class IndexedWebsiteEntity
{
  [AutoIncrement, PrimaryKey] public long ID { get; set; }
  [Indexed] public Guid Guid { get; set; } = Guid.NewGuid();
  [Indexed( Unique = true )] public string URL { get; set; } = string.Empty;
  public string BaseURL { get; set; } = string.Empty;
  public string ContentPath { get; set; } = string.Empty; 
  public long ParentID { get; set; } = 0;
  public string ParentURL { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string Domain { get; set; } = string.Empty;
  public string ContentType { get; set; } = string.Empty;
  public string ContentHash { get; set; } = string.Empty;
  public long Depth { get; set; } = 0;
  public IndexedWebsiteStatus Status { get; set; } = IndexedWebsiteStatus.Indexed;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
