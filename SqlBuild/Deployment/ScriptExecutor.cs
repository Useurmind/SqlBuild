using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

using SqlBuild.Model;

namespace SqlBuild.Deployment
{
    public class ScriptExecutor
    {
        public void Execute(SqlScript script)
        {
            string connectionString = new ConnectionStringFactory().CreateConnectionString(script.Connection, script.Login);

            var connection = new System.Data.SqlClient.SqlConnection(connectionString);
            var server = new Server(new ServerConnection(connection));

            server.ConnectionContext.ExecuteNonQuery(script.GetSqlText());
        }
    }
}
