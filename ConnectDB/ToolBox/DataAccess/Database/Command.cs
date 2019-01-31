using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.DataAccess.Database
{
    public sealed class Command
    {
        internal IDictionary<string, object> Parameters { get; private set; }
        internal string Query { get; private set; }
        internal bool IsStoredProcedure { get; private set; }
        internal Nullable<ParameterDirection> Direction { get; private set; } = null;

        /// <summary>
        /// Creating new command with only a query.  To add parameters, use AddParameter(string Name, object Value) method
        /// </summary>
        /// <param name="Query"></param>
        public Command(string Query) : this(Query, false)
        {
           
        }

        /// <summary>
        /// Creating new command with a query or a stored procedure.  To add parameters, use AddParameter(string Name, object Value) method
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="IsStoredProcedure"></param>
        public Command(string Query, bool IsStoredProcedure)
        {
            if (string.IsNullOrWhiteSpace(Query))
                throw new ArgumentException("Query can't be empty or null");

            Parameters = new Dictionary<string, object>();
            this.Query = Query;
            this.IsStoredProcedure = IsStoredProcedure;
           
        }
        public Command(string Query, bool IsStoredProcedure, ParameterDirection Direction) : this(Query, true)
        {
            this.Direction = Direction;
        }

        public void AddParameter(string ParameterName, object Value)
        {
            Parameters.Add(ParameterName, Value);
        }
    }
}
