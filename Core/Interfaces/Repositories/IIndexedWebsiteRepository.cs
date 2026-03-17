using Core.Enumerations;
using Core.Model;

namespace Core.Interfaces.Repositories;

public interface IIndexedWebsiteRepository
{
  public Task<IndexedWebsite> GetByID( long id );
  public Task<IndexedWebsite> GetByURL( string url );
  public Task<IEnumerable<IndexedWebsite>> GetAll();
  public Task<IndexedWebsite> GetByGuid( Guid guid );
  public Task<IndexedWebsite?> GetNextForScraping();
  public Task<IEnumerable<IndexedWebsite>> GetByDomain( string domain );
  public Task<IEnumerable<IndexedWebsite>> GetByParent( string parent );
  public Task<IEnumerable<IndexedWebsite>> GetByParentID( long parentID );
  public Task Delete( IndexedWebsite website );
  public Task<bool> Create( IndexedWebsite website );
  public Task UpdateStatus( IndexedWebsite website, IndexedWebsiteStatus status );
  public Task Update( IndexedWebsite website );
}
