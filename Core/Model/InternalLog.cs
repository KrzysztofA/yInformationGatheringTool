using Core.Enumerations;

namespace Core.Model;

public class InternalLog
{
  public Guid Guid { get; set; } = Guid.NewGuid();
  public LogLevel Level { get; set; } = LogLevel.None;
  public ErrorCode ErrorCode { get; set; } = ErrorCode.None;
  public string Message { get; set; } = string.Empty;
  public string Exception { get; set; } = string.Empty;
}
