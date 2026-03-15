using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraper.Database;

public interface IDatabase
{
  public Task<SQLiteAsyncConnection> Connection();
}
