using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using WebScraper.Services.InternalLogger;

namespace WebScraper.Library;

public sealed class ConcurrencyManager( IInternalLogger logger, int limit ) : IDisposable
{
  private readonly IInternalLogger _logger = logger;
  private readonly SemaphoreSlim _semaphore = new(limit, limit);
  private readonly ConcurrentDictionary<Task, long> _tasks = new();
  private long _currentGeneration = 0;

  public long CurrentGenerationTasksCount => _tasks.LongCount( predicate => predicate.Value == _currentGeneration );
  public long TasksCount => _tasks.Count;
  public long CurrentGeneration => _currentGeneration;

  public async Task RunAsync( Func<CancellationToken, Task> work, CancellationToken token )
  {
    // Capture the generation at the MOMENT the task is queued
    long taskGeneration = _currentGeneration;

    await _semaphore.WaitAsync( token );

    // Pass the token to the work itself
    var task = work(token);
    _tasks.TryAdd( task, taskGeneration );

    // Optimized cleanup
    _ = task.ContinueWith( t =>
    {
      try
      {
        if (t.IsFaulted)
        {
          _logger.Log( new()
          {
            Level = Model.Entry.LogLevel.Error,
            ErrorCode = Model.Entry.ErrorCode.TaskFaulted,
            Message = $"Generation {taskGeneration} task faulted."
          }, t.Exception );
        }
      }
      finally
      {
        _tasks.TryRemove( t, out _ );
        _semaphore.Release();
      }
    }, CancellationToken.None );
  }

  public void NextGeneration() => Interlocked.Increment( ref _currentGeneration );

  public async Task WaitForAllCurrentTasksAsync()
  {
    while (!_tasks.IsEmpty)
    {
      await Task.WhenAll( [.. _tasks.Keys] );
    }
  }

  /// <summary>
  /// Waits for all tasks belonging to a specific generation to finish.
  /// Useful for ensuring a "Clean Slate" before starting new logic.
  /// </summary>
  public async Task WaitForGenerationAsync( long generation )
  {
    while (true)
    {
      var generationTasks = _tasks
                .Where(kvp => kvp.Value == generation)
                .Select(kvp => kvp.Key)
                .ToArray();

      if (generationTasks.Length == 0) break;

      // Wait for the current batch of this generation to finish
      await Task.WhenAll( generationTasks );

      // We loop once more just in case a task was added 
      // during the 'await' (unlikely with NextGeneration() logic, but safe)
    }
  }

  public void Dispose()
  {
    _semaphore.Dispose();
    _tasks.Clear();
  }
}