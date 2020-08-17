using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalTask
{
    public interface IDataProvider<T>
    {
        List<T> GetEntity(Func<T, bool> whereClause);
    }
}
