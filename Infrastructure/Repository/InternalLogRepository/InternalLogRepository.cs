using Core.Interfaces.Repositories;
using Core.Model;
using Infrastructure.Database;
using Infrastructure.Entities;
using MapsterMapper;

namespace Infrastructure.Repository.InternalLogRepository;

public class InternalLogRepository(IDatabase database, IMapper mapper) : IInternalLogRepository
{
  private readonly IDatabase _database = database;
  private readonly IMapper _mapper = mapper;

  public async Task<bool> Create( InternalLog log )
  {
    try
    {
      var enitity = _mapper.Map<InternalLogEntity>( log );
      await ( await _database.Connection() ).InsertAsync( enitity );
      return true;
    }
    catch
    {
      return false;
    }
  }

  public async Task<IEnumerable<InternalLog>> GetAll()
  {
    var entities = await ( await _database.Connection() ).Table<InternalLogEntity>().ToListAsync();
    return _mapper.Map<IEnumerable<InternalLog>>( entities );
  }
}
