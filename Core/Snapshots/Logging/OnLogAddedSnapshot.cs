using Core.Model;
namespace Core.Snapshots.Logging;

public class OnLogAddedSnapshot
{
  public InternalLog? Log { get; set; } = null;
  public DateTime SnapshotAt { get; set; } = DateTime.UtcNow;
}
