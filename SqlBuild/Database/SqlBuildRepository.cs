using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;
using SqlBuild.Syntax;

using SqlConnection = System.Data.SqlClient.SqlConnection;

namespace SqlBuild.Database
{
    public class DBVersion
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
    }

    public class SqlBuildInfo
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DBVersion Version { get; set; }
        public DateTime InstallTime { get; set; }
    }

    public interface ISqlBuildRepository
    { }

    public class SqlBuildRepository : ISqlBuildRepository
    {
        private const string createSchemaScript = @"SqlScripts\SqlBuildInfoSchema.sql";

        private SqlSession session;
        private SqlConnection dbConnection;
        private IBatchExtractor batchExtractor;
        private IScriptExecutor scriptExecutor;

        public SqlBuildRepository(SqlSession session, IBatchExtractor batchExtractor, IScriptExecutor scriptExecutor)
        {
            this.scriptExecutor = scriptExecutor;
            this.batchExtractor = batchExtractor;
            this.session = session;
            this.dbConnection = session.OpenDBConnection();
        }

        public bool DoesSqlBuildSchemaExist()
        {
            var command = dbConnection.CreateCommand();
            command.CommandText = string.Format(
                "select name from sys.schemas where name  = '{0}'",
                Constants.SqlBuildSchemaName);
            command.CommandType = CommandType.Text;

            var sqlReader = command.ExecuteReader();

            return sqlReader.Read();
        }

        public void CreateSqlBuildSchema()
        {
            var createSchemaScript = new SqlScript()
                                         {
                                             Identity = @"SqlScripts\SqlBuildInfoSchema.sql"
                                         };

            batchExtractor.ExtractBatches(createSchemaScript, session.Connection.ServerVersion);

            scriptExecutor.ExecuteScript(createSchemaScript);
        }

        public DBVersion GetCurrentDBVersion()
        {
            return new DBVersion();
        }

        public void SetCurrentDBVersion(DBVersion version)
        {
            
        }
    }
}
