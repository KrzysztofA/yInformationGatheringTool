using Core.Model;
using Core.Snapshots.Logging;

namespace Core.Interfaces.Logging;

public interface IInternalLogger
{
  public event EventHandler<OnLogAddedSnapshot>? OnLogAdded;
  public Task Log(InternalLog log, Exception? ex = null );
}
