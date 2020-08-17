using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalTask
{
    public interface IProcedureExecutable<T>
    {
        Task<List<T>> ExecuteStoreProcedure(DataTable tvp);
    }
}
