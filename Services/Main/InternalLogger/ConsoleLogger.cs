using Core.Enumerations;
using Core.Interfaces.Logging;
using Core.Model;
using Core.Snapshots.Logging;

namespace Services.Main.InternalLogger;

public class ConsoleLogger : IInternalLogger
{
  public event EventHandler<OnLogAddedSnapshot>? OnLogAdded;

  public async Task Log( InternalLog log, Exception? ex = null )
  {
    switch (log.Level)
    {
      case LogLevel.Info:
        Console.WriteLine( $"[INFO] {log.Message}" );
        break;
      case LogLevel.Warning:
        Console.WriteLine( $"[WARNING] {log.Message}" );
        break;
      case LogLevel.Error:
        Console.WriteLine( $"[ERROR] {log.Message}" );
        if (ex != null)
        {
          Console.WriteLine( "=====================EXCEPTION=============================" );

          Console.WriteLine( ex.ToString() );

          Console.WriteLine( "===========================================================" );
        }
        break;
      case LogLevel.All:
        break;
      case LogLevel.None:
        Console.WriteLine( $"{log.Message}" );
        break;
      default:
        Console.WriteLine( $"[UNKNOWN] {log.Message}" );
        break;
    }

    OnLogAdded?.Invoke( this, new OnLogAddedSnapshot
    {
      Log = log,
    } );
  }
}
