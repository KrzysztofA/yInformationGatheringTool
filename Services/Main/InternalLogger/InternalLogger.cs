using Core.Interfaces.Logging;
using Core.Interfaces.Repositories;
using Core.Model;
using Core.Snapshots.Logging;

namespace Services.Main.InternalLogger;

internal class InternalLogger(IInternalLogRepository internalLogRepository) : IInternalLogger
{
  private readonly IInternalLogRepository _internalLoggerRepository = internalLogRepository;

  public event EventHandler<OnLogAddedSnapshot>? OnLogAdded;

  public async Task Log( InternalLog log, Exception? ex = null )
  {
    if (ex != null) 
    {
      log.Exception = ex.ToString();
    }

    await _internalLoggerRepository.Create( log );
    OnLogAdded?.Invoke( this, new OnLogAddedSnapshot() { Log = log } );
  }
}
