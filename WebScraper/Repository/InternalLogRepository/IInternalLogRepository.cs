using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Model.Entry;

namespace WebScraper.Repository.InternalLogRepository;

public interface IInternalLogRepository
{
  public Task<bool> Create( InternalLog log );
  public Task<IEnumerable<InternalLog>> GetAll();
}
