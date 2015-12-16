using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;

using SqlConnection = System.Data.SqlClient.SqlConnection;

namespace SqlBuild.Database
{
    public interface IScriptExecutor
    {
        void ExecuteScript(SqlScript script);
    }

    public class ScriptExecutor : IScriptExecutor
    {
        private SqlConnection dbConnection;

        public ScriptExecutor(SqlConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void ExecuteScript(SqlScript script)
        {
            foreach (var batch in script.Batches)
            {
                var command = dbConnection.CreateCommand();

                command.CommandText = batch.SqlText;
                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();
            }
        }
    }
}
