using Core.Enumerations;

namespace Core.Model;

public class IndexedWebsite
{
  public long ID { get; set; } = 0;
  public Guid Guid { get; set; } = Guid.NewGuid();
  public string URL { get; set; } = string.Empty;
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
}
