using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTDataAccessLibrary;
using TTDataAccessLibrary.Models;

namespace TechnicalTask
{
    public class FlagDataProvider : IDataProvider<Flag>
    {
        IServiceProvider services;
        public FlagDataProvider(IServiceProvider services)
        {
            this.services = services;
        }
        public List<Flag> GetEntity(Func<Flag, bool> whereClause = null)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TechnicalTaskContext>();
                return dbContext.Flags.Where(whereClause).ToList();
            }
        }
    }
}
