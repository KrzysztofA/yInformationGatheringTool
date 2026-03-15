using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Library;

namespace WebScraper.Managers.ScraperManager;

public interface IScraperManager
{
  public ManagedServiceControlFlowOrchestrator Control { get; }
  public ConcurrencyManager Concurrency { get; }
  public Task StartAsync();
}
