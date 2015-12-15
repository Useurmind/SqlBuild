using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlBuild.Model
{
    public class SqlBatch
    {
        private readonly TSqlBatch tSqlBatch;

        public string SqlText
        {
            get
            {
                var batchTokens = tSqlBatch.ScriptTokenStream.Skip(tSqlBatch.FirstTokenIndex)
                                                            .Take(tSqlBatch.LastTokenIndex - tSqlBatch.FirstTokenIndex + 1)
                                                            .Select(t => t.Text);

                return string.Join("", batchTokens);
            }
        }

        public int StartLine
        {
            get
            {
                return tSqlBatch.StartLine;
            }
        }

        public int StartColumn
        {
            get
            {
                return tSqlBatch.StartColumn;
            }
        }

        public SqlBatch(TSqlBatch tsqlBatch)
        {
            this.tSqlBatch = tsqlBatch;
        }
    }
}
