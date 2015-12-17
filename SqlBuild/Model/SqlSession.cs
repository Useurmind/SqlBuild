using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Database;

namespace SqlBuild.Model
{
    public class SqlSession : KeyedModel, IDisposable
    {
        private System.Data.SqlClient.SqlConnection dbConnection;

        public SqlLogin Login { get; private set; }
        public SqlConnection Connection { get; private set; }

        public SqlSession(SqlLogin login, SqlConnection connection)
        {
            Login = login;
            Connection = connection;
        }

        public System.Data.SqlClient.SqlConnection OpenDBConnection()
        {
            if (dbConnection == null)
            {
                var connectionString = ConnectionStringFactory.CreateConnectionString(Connection, Login);
                dbConnection = new System.Data.SqlClient.SqlConnection(connectionString);
                dbConnection.Open();
            }

            return dbConnection;
        }

        public void Dispose()
        {
            if (dbConnection != null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
                dbConnection = null;
            }
        }
    }
}
