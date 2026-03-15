namespace WebScraper.Library;

public enum ManagedServiceState
{
  Stopped = 0,
  Running = 1,
  Paused = 2,
  Failed = 3,
}

public sealed class ManagedServiceControlFlowOrchestrator : IDisposable
{
  public event EventHandler<ManagedServiceState>? OnStateChanged = null;

  private readonly SemaphoreSlim _syncLock = new(1, 1);
  private CancellationTokenSource _cts = new();
  private TaskCompletionSource<bool> _pauseTcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

  // Tracks the current state
  private ManagedServiceState _state = ManagedServiceState.Stopped;
  public ManagedServiceState State => _state;

  // Helper properties for UI Binding
  public bool IsRunning => _state == ManagedServiceState.Running;
  public bool IsPaused => _state == ManagedServiceState.Paused;
  public bool IsStopped => _state == ManagedServiceState.Stopped;

  public ManagedServiceControlFlowOrchestrator()
  {
    _pauseTcs.SetResult( true );
  }

  public async Task<bool> TryStartAsync()
  {
    if(!await _syncLock.WaitAsync( 0 )) return false;
    try
    {
      if (_state != ManagedServiceState.Stopped)
      {
        return false;
      }
      _cts = new CancellationTokenSource();
      _state = ManagedServiceState.Running;
      OnStateChanged?.Invoke( this, _state );
      return true;
    }
    finally
    {
      _syncLock.Release();
    }
  }

  public void Pause()
  {
    if (_state != ManagedServiceState.Running) return;

    _state = ManagedServiceState.Paused;
    OnStateChanged?.Invoke( this, _state );
    _pauseTcs = new TaskCompletionSource<bool>( TaskCreationOptions.RunContinuationsAsynchronously );
  }

  public void Resume()
  {
    if (_state != ManagedServiceState.Paused) return;

    _state = ManagedServiceState.Running;
    OnStateChanged?.Invoke( this, _state );
    _pauseTcs.TrySetResult( true );
  }

  public void Stop()
  {
    if (_state == ManagedServiceState.Stopped) return;

    _state = ManagedServiceState.Stopped;
    OnStateChanged?.Invoke( this, _state );
    _cts.Cancel();
    _pauseTcs.TrySetResult( true ); // Release loop if it was paused
  }

  public async Task WaitForPermissionAsync()
  {
    _cts.Token.ThrowIfCancellationRequested();

    // This will await only if the state is Paused
    await _pauseTcs.Task;

    // Double check cancellation after waking up
    _cts.Token.ThrowIfCancellationRequested();
  }

  public CancellationToken Token => _cts.Token;

  public void Dispose()
  {
    _cts?.Cancel();
    _cts?.Dispose();
    _syncLock.Dispose();
  }
}

