using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.TransactSql.ScriptDom;

using SqlBuild.Utility;

namespace SqlBuild.Model
{
    public class SqlScript : IModel
    {
        private string sqlText;

        public string ItemSpec { get; set; }

        public string Identity { get; set; }

        public string GetSqlText()
        {
            if (string.IsNullOrEmpty(sqlText))
            {
                if (string.IsNullOrEmpty(Identity))
                {
                    throw new SqlBuildException(Errors.SqlScriptMissingIdentity, ItemSpec);
                }

                if (!File.Exists(Identity))
                {
                    throw new SqlBuildException(Errors.SqlScriptFileDoesNotExists, Identity, ItemSpec);
                }

                sqlText = File.ReadAllText(this.Identity);
            }

            return sqlText;
        }

        public void SetSqlText(string sqlText)
        {
            this.sqlText = sqlText;
        }

        public IEnumerable<SqlBatch> Batches { get; set; } 
    }
}
