using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTask.Utils
{
    public static class Utils
    {
        public static DataTable ParseEnumarableToTable<T>(IEnumerable<T> rows, Dictionary<string, Type> columns)
        {
            DataTable tvp = new DataTable();
            foreach (var column in columns)
            {
                tvp.Columns.Add(column.Key, column.Value);
            }
            rows.ToList().ForEach(x =>
            {
                var array = x as string[];
                if (array != null)
                {
                    tvp.Rows.Add(array);
                }
                else
                {
                    DataRow dataRow = tvp.NewRow();
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        Console.WriteLine(prop.Name);
                        dataRow[prop.Name] = prop.GetValue(x);
                    }
                    tvp.Rows.Add(dataRow);
                }
            });

            return tvp;
        }

        public static string CapitalizeFirstLetterAndConcat(string str)
        {
            var tempStr = str.Trim().Split(' ');
            StringBuilder concatStr = new StringBuilder("");
            foreach (var word in tempStr)
            {
                concatStr.Append(word.First().ToString().ToUpper());
                concatStr.Append(word.Substring(1));
            }
            return concatStr.ToString();
        }
    }
}
