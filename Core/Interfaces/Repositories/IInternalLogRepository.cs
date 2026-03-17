using Core.Model;

namespace Core.Interfaces.Repositories;

public interface IInternalLogRepository
{
  public Task<bool> Create( InternalLog log );
  public Task<IEnumerable<InternalLog>> GetAll();
}
