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
        private SqlSession session;

        public ScriptExecutor(SqlSession session)
        {
            this.session = session;
        }

        public void ExecuteScript(SqlScript script)
        {
            foreach (var batch in script.Batches)
            {
                var command = session.OpenDBConnection().CreateCommand();

                command.CommandText = batch.SqlText;
                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();
            }
        }
    }
}
