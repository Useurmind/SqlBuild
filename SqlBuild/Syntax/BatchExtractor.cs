using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.TransactSql.ScriptDom;

using SqlBuild.Logging;
using SqlBuild.Model;

namespace SqlBuild.Syntax
{
    public interface IBatchExtractor
    {
        void ExtractBatches(SqlScript script, ServerVersion serverVersion);
    }

    public class BatchExtractor : IBatchExtractor
    {
        public ISqlBuildLog Log { get; set; }

        public IParserFactory ParserFactory { get; set; }

        public void ExtractBatches(SqlScript script, ServerVersion serverVersion)
        {
            string sqlText = script.GetSqlText();

            var parser = ParserFactory.CreateParser(serverVersion);

            IList<ParseError> errors;
            var fragment = parser.Parse(new StringReader(sqlText), out errors);

            foreach (var parseError in errors)
            {
                Log.WriteError(script.Identity, parseError.Line, parseError.Column, string.Empty, parseError.Message);
            }

            var tsqlScript = fragment as TSqlScript;

            script.Batches = tsqlScript.Batches.Select(batch => new SqlBatch(batch));
        }
    }
}
