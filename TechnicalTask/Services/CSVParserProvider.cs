using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTask
{
    public class CSVParserProvider : IFileParserProvider
    {
        public IEnumerable<string[]> ParseFile(string pathToFile)
        {
            string[] seps = { "\",", ",\"", "," };
            char[] quotes = { '"', '”' };


            var lines = System.IO.File.ReadLines(pathToFile);

            foreach (var line in lines)
            {
                var fields = line.Split(',', StringSplitOptions.None)
                                .Select(s => s.Replace("\"", "").Trim())
                                .ToArray();
                fields[1] = Utils.Utils.CapitalizeFirstLetterAndConcat(fields[1]);
                yield return fields;
            }
        }
    }
}
