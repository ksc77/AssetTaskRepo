using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechnicalTask.Models.Request;
using TTDataAccessLibrary;
using TTDataAccessLibrary.Models;

namespace TechnicalTask.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        int batch = 1000;
        IFileParserProvider fileParserProvider;
        IProcedureExecutable<Asset> procedureExecProvider;
        IConfiguration configuration;
        IDataProvider<Asset> dataAssetProvider;
        IDataProvider<Flag> dataFlagProvider;
        Dictionary<string, Type> columns;

        public AssetController(
            IFileParserProvider fileParserProvider,
            IProcedureExecutable<Asset> procedureExecProvider,
            IConfiguration configuration,
            IDataProvider<Asset> dataAssetProvider,
            IDataProvider<Flag> dataFlagProvider)
        {
            this.fileParserProvider = fileParserProvider;
            this.procedureExecProvider = procedureExecProvider;
            this.configuration = configuration;
            this.dataAssetProvider = dataAssetProvider;
            this.dataFlagProvider = dataFlagProvider;

            this.batch = Int32.Parse(configuration["AssetProcessing:BatchSize"]);

            // Columns definition to configuration
            columns = new Dictionary<string, Type>() { { "AssetId", typeof(String) }, { "Property", typeof(String) }, { "PropertyValue", typeof(bool) }, { "TimeStamp", typeof(DateTime) } };

        }
        [HttpGet("ProcessCSV")]
        public async Task<IActionResult> ProcessCSV()
        {
            bool doWork = true;
            int offset = 0;

            List<Asset> notMatchedInDb = new List<Asset>();
            List<Task> workers = new List<Task>();
            var rows = this.fileParserProvider.ParseFile(configuration["AssetProcessing:CSVPath"]);

            while (doWork)
            {
                var rangeRows = rows.Skip(batch * offset).Take(batch).ToList();
                if (rangeRows.Count < 1) break;

                var tvp = Utils.Utils.ParseEnumarableToTable<string[]>(rangeRows, columns);

                var worker = procedureExecProvider.ExecuteStoreProcedure(tvp);
                workers.Add(worker);

                var dbNotMatched = await worker;
                notMatchedInDb.AddRange(dbNotMatched.ToList());
                offset++;
            }

            Task.WaitAll(workers.ToArray());
            return Ok(notMatchedInDb);
        }

        [HttpPost("GetAssetIds")]
        public IActionResult GetAssetIds([FromBody]AssetIdRequestModel request)
        {
            string upperProp = Utils.Utils.CapitalizeFirstLetterAndConcat(request.Property);
            var flag = dataFlagProvider.GetEntity((flag) =>
            {
                return flag.Name == upperProp;
            }).FirstOrDefault();

            if (flag == null)
                return Ok();

            var AssetIds = dataAssetProvider.GetEntity((Asset) =>
            {
                if (request.Value.ToLower() == "true")
                    return (Asset.TypeBitField & flag.BitFlag) == flag.BitFlag;
                else
                    return (Asset.TypeBitField & flag.BitFlag) != flag.BitFlag;
            }).Select(s => s.AssetId);

            return Ok(AssetIds);
        }

        [HttpPost("SetPropertyAsset")]
        public async Task<IActionResult> SetPropertyAsset([FromBody]AssetPropertyRequestModel request)
        {
            request.Property = Utils.Utils.CapitalizeFirstLetterAndConcat(request.Property);
            var tvp = Utils.Utils.ParseEnumarableToTable(new List<AssetPropertyRequestModel> { request }, columns);
            var worker = procedureExecProvider.ExecuteStoreProcedure(tvp);
            var notMatched = await worker;

            worker.Wait();
            return Ok(notMatched.ToList());
        }
    }
}