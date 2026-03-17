using SQLite;

namespace Infrastructure.Database;

public interface IDatabase
{
  public Task<SQLiteAsyncConnection> Connection();
}
