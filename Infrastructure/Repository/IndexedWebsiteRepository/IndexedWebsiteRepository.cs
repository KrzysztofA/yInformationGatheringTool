using Core.Interfaces.Repositories;
using Infrastructure.Entities;
using MapsterMapper;
using Core.Model;
using Core.Enumerations;
using Infrastructure.Database;

namespace Infrastructure.Repository.IndexedWebsiteRepository;

public class IndexedWebsiteRepository(IDatabase database, IMapper mapper) : IIndexedWebsiteRepository
{
  private readonly IDatabase _database = database;
  private readonly IMapper _mapper = mapper;

  public async Task<bool> Create( IndexedWebsite website )
  {
    try
    {
      var entity = _mapper.Map<IndexedWebsiteEntity>( website );
      var result = await ( await _database.Connection() ).InsertAsync( entity );
      return result > 0;
    }
    catch
    {
      return false;
    }
  }

  public async Task Delete( IndexedWebsite website )
  {
    var entity = _mapper.Map<IndexedWebsiteEntity>( website );
    await ( await _database.Connection() ).DeleteAsync( entity );
  }

  public async Task<IEnumerable<IndexedWebsite>> GetAll()
  {
    var entities = await( await _database.Connection() ).Table<IndexedWebsiteEntity>().ToListAsync();
    return _mapper.Map<IEnumerable<IndexedWebsite>>( entities );
  }

  public async Task<IEnumerable<IndexedWebsite>> GetByDomain( string domain )
  {
    var entities = await ( await _database.Connection() ).Table<IndexedWebsiteEntity>().Where( x => x.Domain == domain ).ToListAsync();
    return _mapper.Map<IEnumerable<IndexedWebsite>>( entities );
  }

  public async Task<IndexedWebsite> GetByGuid( Guid guid )
  {
    var entity = await( await _database.Connection() ).Table<IndexedWebsiteEntity>().Where( x => x.Guid == guid ).FirstOrDefaultAsync();
    return _mapper.Map<IndexedWebsite>( entity );
  }

  public async Task<IndexedWebsite> GetByID( long id )
  {
    var entity = await(await _database.Connection()).Table<IndexedWebsiteEntity>().Where(x => x.ID == id).FirstOrDefaultAsync();
    return _mapper.Map<IndexedWebsite>( entity );
  }

  public async Task<IEnumerable<IndexedWebsite>> GetByParent( string parent )
  {
    var entities = await( await _database.Connection() ).Table<IndexedWebsiteEntity>().Where( x => x.ParentURL == parent ).ToListAsync();
    return _mapper.Map<IEnumerable<IndexedWebsite>>( entities );
  }

  public async Task<IEnumerable<IndexedWebsite>> GetByParentID( long parentID )
  {
    var entities = await( await _database.Connection() ).Table<IndexedWebsiteEntity>().Where( x => x.ParentID == parentID ).ToListAsync();
    return _mapper.Map<IEnumerable<IndexedWebsite>>( entities );
  }

  public async Task<IndexedWebsite> GetByURL( string url )
  {
    var entity = await ( await _database.Connection() ).Table<IndexedWebsiteEntity>().Where( x => x.URL == url ).FirstOrDefaultAsync();
    return _mapper.Map<IndexedWebsite>( entity );
  }

  public async Task<IndexedWebsite?> GetNextForScraping()
  {
    var db = await _database.Connection();

    var result = await db.QueryAsync<IndexedWebsiteEntity>(
        @"UPDATE IndexedWebsiteEntity
          SET Status = ?
          WHERE ID = (
              SELECT ID
              FROM IndexedWebsiteEntity
              WHERE Status = ?
              LIMIT 1
          )
          RETURNING *;",
        IndexedWebsiteStatus.Pending,
        IndexedWebsiteStatus.Indexed
    );
    var entity = result.FirstOrDefault();
    if (entity == null) return null;

    var website = _mapper.Map<IndexedWebsite>( entity );
    return website;
  }

  public async Task UpdateStatus( IndexedWebsite website, IndexedWebsiteStatus status )
  {
    var entity = _mapper.Map<IndexedWebsiteEntity>( website );
    entity.Status = status;
    entity.UpdatedAt = DateTime.UtcNow;
    await ( await _database.Connection() ).UpdateAsync( entity );
  }

  public async Task Update( IndexedWebsite website )
  {
    var entity = _mapper.Map<IndexedWebsiteEntity>( website );
    entity.UpdatedAt = DateTime.UtcNow;
    await ( await _database.Connection() ).UpdateAsync( entity );
  }
}
