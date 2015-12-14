using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;

namespace SqlBuild.Syntax
{
    public class BatchExtractor
    {
        public IParserFactory ParserFactory { get; set; }

        public void ExtractBatches(SqlScript script)
        {
            string sqlText = script.GetSqlText();

            
        }
    }
}
