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
        void ExtractBatches(SqlScript script);
    }

    public class BatchExtractor : IBatchExtractor
    {
        private ISqlBuildLog Log { get; set; }
        
        private IParserFactory ParserFactory { get; set; }

        public BatchExtractor(ISqlBuildLog log, IParserFactory parserFactory)
        {
            Log = log;
            ParserFactory = parserFactory;
        }

        public void ExtractBatches(SqlScript script)
        {
            string sqlText = script.GetSqlText();

            var parser = ParserFactory.CreateParser();

            IList<ParseError> errors;
            var fragment = parser.Parse(new StringReader(sqlText), out errors);

            foreach (var parseError in errors)
            {
                Log.WriteError(script.Identity, parseError.Line, parseError.Column, string.Empty, parseError.Message);
            }

            var tsqlScript = fragment as TSqlScript;

            script.Batches = tsqlScript.Batches.Select(batch => new SqlBatch()
            {
                SqlText = batch.ScriptTokenStream.Skip(batch.FirstTokenIndex)
                                                            .Take(batch.LastTokenIndex - batch.FirstTokenIndex + 1)
                                                            .Select(t => t.Text)
                                                            .Aggregate(new StringBuilder(), (builder, s) => builder.Append(s), builder => builder.ToString()),
                StartLine = batch.StartLine,
                StartColumn = batch.StartColumn,
            });
        }
    }
}
