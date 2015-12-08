using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.TransactSql.ScriptDom;

using Xunit;

namespace SqlBuild.Test.Syntax
{
    public class ParserTest
    {
        [Fact]
        public void parse_something()
        {
            TSql120Parser parser = new TSql120Parser(false);

            var statement = @"
select * from MyTable; 

go 

insert into [MyDatabase].MySchema.MyTable (col1) Values (value1);";
            var statementReader = new StringReader(statement);
            IList<ParseError> errors;

            var fragment = parser.Parse(statementReader, out errors);

            int a = 1;
        }
    }
}
