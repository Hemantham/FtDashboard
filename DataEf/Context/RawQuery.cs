using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEf.Context
{
   public  class RawQuery
    {

        public  IEnumerable<dynamic> CollectionFromSql(string connectionString, string Sql, Dictionary<string, object> Parameters)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = Sql;
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    foreach (KeyValuePair<string, object> param in Parameters)
                    {
                        DbParameter dbParameter = cmd.CreateParameter();
                        dbParameter.ParameterName = param.Key;
                        dbParameter.Value = param.Value;
                        cmd.Parameters.Add(dbParameter);
                    }

                    //var retObject = new List<dynamic>();
                    using (var dataReader = cmd.ExecuteReader())
                    {

                        while (dataReader.Read())
                        {
                            var dataRow = GetDataRow(dataReader);
                            yield return dataRow;

                        }
                    }


                }
            }
        }

        private static dynamic GetDataRow(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                dataRow.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
            return dataRow;
        }
    }
}
