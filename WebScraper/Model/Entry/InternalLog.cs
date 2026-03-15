using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraper.Model.Entry;

public enum LogLevel
{
  None,
  Info,
  Warning,
  Error
}

public enum ErrorCode
{
  None,
  NetworkError,
  TaskFaulted,
  ServiceAlreadyRunning,

}

public class InternalLog
{
  [AutoIncrement, PrimaryKey] public long ID { get; set; }
  [Indexed] public Guid Guid { get; set; } = Guid.NewGuid();
  public LogLevel Level { get; set; } = LogLevel.None;
  public ErrorCode ErrorCode { get; set; } = ErrorCode.None;
  public string Message { get; set; } = string.Empty;
  public string Exception { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
