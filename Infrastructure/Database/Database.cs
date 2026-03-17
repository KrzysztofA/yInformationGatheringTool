using Infrastructure.Entities;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using SQLite;

namespace Infrastructure.Database;

public class Database( IOptions<DatabaseSettings> settings ) : IDatabase
{
  private readonly IOptions<DatabaseSettings> _settings = settings;

  private SQLiteAsyncConnection? _connection = null;
  public async Task<SQLiteAsyncConnection> Connection()
  {
    if(_connection == null)
    {
      if(!Path.Exists(_settings.Value.Path ))
      {
        var directory = Path.GetDirectoryName(_settings.Value.Path);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
          Directory.CreateDirectory(directory);
        }
      }

      _connection = new SQLiteAsyncConnection( _settings.Value.Path );

      await _connection.CreateTableAsync<IndexedWebsiteEntity>();
      await _connection.CreateTableAsync<IndexedDocumentEntity>();
      await _connection.CreateTableAsync<InternalLogEntity>();
    }
    return _connection;
  }
}
