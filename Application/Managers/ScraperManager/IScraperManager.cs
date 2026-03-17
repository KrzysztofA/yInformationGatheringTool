using Core.Utilities.Concurrency;

namespace Application.Managers.ScraperManager;

public interface IScraperManager
{
  public ManagedServiceControlFlowOrchestrator Control { get; }
  public ConcurrencyManager Concurrency { get; }
  public Task StartAsync();
}
