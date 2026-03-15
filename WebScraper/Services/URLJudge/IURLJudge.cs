using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Model.Context;

namespace WebScraper.Services.URLJudge;

public interface IURLJudge
{
  public Task<bool> ShouldScrape( Uri uri, long depth, CancellationToken cancellationToken);
}
