using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Model.Entry;

namespace WebScraper.Model.Snapshots.InternalLogger;

public class OnLogAddedSnapshot
{
  public InternalLog? Log { get; set; } = null;
  public DateTime SnapshotAt { get; set; } = DateTime.UtcNow;
}
