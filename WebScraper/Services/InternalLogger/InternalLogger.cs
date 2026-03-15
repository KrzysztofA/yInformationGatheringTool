using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Model.Entry;
using WebScraper.Model.Snapshots.InternalLogger;
using WebScraper.Repository.InternalLogRepository;

namespace WebScraper.Services.InternalLogger;

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
