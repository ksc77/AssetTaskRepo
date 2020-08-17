using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TTDataAccessLibrary;
using TTDataAccessLibrary.Models;

namespace TechnicalTask
{
    public class AssetDataProvider : IDataProvider<Asset>, IProcedureExecutable<Asset>
    {
        IServiceProvider services;
        IConfiguration configuration;
        string spName;
        string spParamName;
        string spTVPName;
        public AssetDataProvider(IServiceProvider services, IConfiguration configuration)
        {
            this.services = services;
            this.configuration = configuration;
            spName = configuration["AssetProcessing:SPName"];
            spParamName = configuration["AssetProcessing:SPParamName"];
            spTVPName = configuration["AssetProcessing:SPTVPName"];
        }
        public Task<List<Asset>> ExecuteStoreProcedure(DataTable tvp)
        {
            var param = new SqlParameter(spParamName, tvp) { TypeName = spTVPName, SqlDbType = SqlDbType.Structured };
            return Task.Run(() =>
            {
                using (var scope = services.CreateScope())
                {
                    string sql = "EXEC " + spName + " " + spParamName + ";";
                    var dbContext = scope.ServiceProvider.GetRequiredService<TechnicalTaskContext>();
                    return dbContext.Assets.FromSqlRaw(sql, param).ToList();
                }
            });

        }

        public List<Asset> GetEntity(Func<Asset, bool> whereClause)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TechnicalTaskContext>();
                return dbContext.Assets.Where(whereClause).ToList();
            }
        }
    }
}
