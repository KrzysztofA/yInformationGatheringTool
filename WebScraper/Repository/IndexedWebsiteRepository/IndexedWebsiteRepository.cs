using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Database;
using WebScraper.Model.Entry;
using WebScraper.Services.InternalLogger;

namespace WebScraper.Repository.IndexedWebsiteRepository;

public class IndexedWebsiteRepository(IDatabase database) : IIndexedWebsiteRepository
{
  private readonly IDatabase _database = database;

  public async Task<bool> Create( IndexedWebsite website )
  {
    try
    {
      var result = await ( await _database.Connection() ).InsertAsync( website );
      return result > 0;
    }
    catch
    {
      return false;
    }
  }

  public async Task Delete( IndexedWebsite website )
  {
    await ( await _database.Connection() ).DeleteAsync( website );
  }

  public async Task<IEnumerable<IndexedWebsite>> GetAll()
  {
    return await( await _database.Connection() ).Table<IndexedWebsite>().ToListAsync();
  }

  public async Task<IEnumerable<IndexedWebsite>> GetByDomain( string domain )
  {
    return await ( await _database.Connection() ).Table<IndexedWebsite>().Where( x => x.Domain == domain ).ToListAsync();
  }

  public async Task<IndexedWebsite> GetByGuid( Guid guid )
  {
    return await( await _database.Connection() ).Table<IndexedWebsite>().Where( x => x.Guid == guid ).FirstOrDefaultAsync();
  }

  public async Task<IndexedWebsite> GetByID( long id )
  {
    return await(await _database.Connection()).Table<IndexedWebsite>().Where(x => x.ID == id).FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<IndexedWebsite>> GetByParent( string parent )
  {
    return await( await _database.Connection() ).Table<IndexedWebsite>().Where( x => x.ParentURL == parent ).ToListAsync();
  }

  public async Task<IEnumerable<IndexedWebsite>> GetByParentID( long parentID )
  {
    return await( await _database.Connection() ).Table<IndexedWebsite>().Where( x => x.ParentID == parentID ).ToListAsync();
  }

  public async Task<IndexedWebsite> GetByURL( string url )
  {
    return await ( await _database.Connection() ).Table<IndexedWebsite>().Where( x => x.URL == url ).FirstOrDefaultAsync();
  }

  public async Task<IndexedWebsite?> GetNextForScraping()
  {
    var db = await _database.Connection();

    var result = await db.QueryAsync<IndexedWebsite>(
        @"UPDATE IndexedWebsite
          SET Status = ?
          WHERE ID = (
              SELECT ID
              FROM IndexedWebsite
              WHERE Status = ?
              LIMIT 1
          )
          RETURNING *;",
        IndexedWebsiteStatus.Pending,
        IndexedWebsiteStatus.Indexed
    );

    return result.FirstOrDefault();
  }

  public async Task UpdateStatus( IndexedWebsite website, IndexedWebsiteStatus status )
  {
    website.Status = status;
    website.UpdatedAt = DateTime.UtcNow;
    await ( await _database.Connection() ).UpdateAsync( website );
  }

  public async Task Update( IndexedWebsite website )
  {
    website.UpdatedAt = DateTime.UtcNow;
    await ( await _database.Connection() ).UpdateAsync( website );
  }
}
