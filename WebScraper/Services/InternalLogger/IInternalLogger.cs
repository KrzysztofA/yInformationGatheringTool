using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Model.Entry;
using WebScraper.Model.Snapshots.InternalLogger;

namespace WebScraper.Services.InternalLogger;

public interface IInternalLogger
{
  public event EventHandler<OnLogAddedSnapshot>? OnLogAdded;
  public Task Log(InternalLog log, Exception? ex = null );
}
