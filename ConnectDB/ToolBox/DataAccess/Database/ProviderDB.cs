using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.DataAccess.Database
{
    public static class ProviderDB
    {
        public static string SqlClient { get; } = "System.Data.SqlClient";
        public static string OracleClient { get; } = "System.Data.OracleClient";
        public static string OdbcClient { get; } = "System.Data.Odbc";
        public static string OleDblient { get; } = "System.Data.OleDb";

        public static void ShowAllClients()
        {
            foreach (DataRow client in DbProviderFactories.GetFactoryClasses().Rows)
            {
                foreach (var m in client.ItemArray)
                {
                    
                    Console.WriteLine(m.ToString());
                }

            }
        }

    }
}
