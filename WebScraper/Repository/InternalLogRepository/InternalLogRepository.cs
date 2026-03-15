using UglyToad.PdfPig.Logging;
using WebScraper.Database;
using WebScraper.Model.Entry;

namespace WebScraper.Repository.InternalLogRepository;

internal class InternalLogRepository(IDatabase database) : IInternalLogRepository
{
  private readonly IDatabase _database = database;
  public async Task<bool> Create( InternalLog log )
  {
    try
    {
      await ( await _database.Connection() ).InsertAsync( log );
      return true;
    }
    catch
    {
      return false;
    }
  }

  public async Task<IEnumerable<InternalLog>> GetAll()
  {
    return await ( await _database.Connection() ).Table<InternalLog>().ToListAsync();
  }
}
