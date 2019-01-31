using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.DataAccess.Database
{
    public class Connection
    {
        protected string ConnectionString { get; private set; }
       
        protected DbProviderFactory ClientDB { get; private set; }
       
        /// <summary>
        /// Creating new connection.
        /// For ClientDB use ProviderDB static class.
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="ClientDB"></param>
        public Connection(string ConnectionString, string ClientDB)
        {
            if (string.IsNullOrWhiteSpace(ConnectionString) || string.IsNullOrWhiteSpace(ClientDB))
                throw new ArgumentException("Connection can't be null or empty!");

            this.ConnectionString = ConnectionString;
            this.ClientDB = DbProviderFactories.GetFactory(ClientDB);

          
 
        }

        public object ExecuteScalar(Command Command)
        {
            using (DbConnection c = CreateConnection())
            {
                using (DbCommand cmd = CreateCommand(Command, c))
                {
                    c.Open();
                    object o = cmd.ExecuteScalar();
                    c.Close();
                    return (o is DBNull) ? null : o;
                   
                }
            }
        }

        public int ExecuteNonQuery(Command Command)
        {
            using (DbConnection c = CreateConnection())
            {
                using (DbCommand cmd = CreateCommand(Command, c))
                {
                    c.Open();
                    if(Command.Direction == ParameterDirection.Output)
                    {
                        cmd.ExecuteNonQuery();
                        return (int)cmd.Parameters[0].Value;
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command Command, Func<IDataRecord, TResult> Selector)
        {
            using (DbConnection c = CreateConnection())
            {
                using (DbCommand cmd = CreateCommand(Command, c))
                {
                    c.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            yield return Selector(reader);
                        }
                    }
                    
                    c.Close();
                }
            }
        }

        public DataTable GetDataTable(Command command)
        {
            using(DbConnection c = CreateConnection())
            {
                using(DbCommand cmd = CreateCommand(command, c))
                {
                    DbDataAdapter adapter = ClientDB.CreateDataAdapter();
                    adapter.SelectCommand = cmd;
                   

                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    return table;
                }
            }

        }

        private DbCommand CreateCommand(Command Command, DbConnection Connection)
        {
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = Command.Query;
            cmd.CommandType = (Command.IsStoredProcedure) ? CommandType.StoredProcedure : CommandType.Text;
            

            foreach (KeyValuePair<string, object> kvp in Command.Parameters)
            {
                DbParameter Parameter = ClientDB.CreateParameter();
                Parameter.ParameterName = kvp.Key;
                Parameter.Value = kvp.Value ?? DBNull.Value;
                    
                
                //if(Command.Direction != null)
                //{
                //    Parameter.Direction = (ParameterDirection)Command.Direction;
                //}
                cmd.Parameters.Add(Parameter);
            }

            return cmd;
        }

        private DbConnection CreateConnection()
        {
            
            DbConnection c = ClientDB.CreateConnection();
            c.ConnectionString = ConnectionString;
                       
            return c;
        }        
    }
}
