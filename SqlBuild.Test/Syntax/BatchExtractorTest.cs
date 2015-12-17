using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;
using SqlBuild.Syntax;
using SqlBuild.Utility;

using Xunit;

namespace SqlBuild.Test.Syntax
{
    public class BatchExtractorTest
    {
        private BatchExtractor batchSeparator;
        private ServerVersion serverVersion;

        public BatchExtractorTest()
        {
            this.batchSeparator = new BatchExtractor(
                                     parserFactory: new ParserFactory(new ExceptionSqlBuildLog(), ServerVersion.SqlServer2014),
                                     log: new ExceptionSqlBuildLog()
                                 );
        }

        [Fact]
        public void extract_batches_from_script_with_three_procedures_extracts_three_batches()
        {
            var createProcedure1Statement = @"create procedure procedure1
as
begin
    select * from table1;
end";

            var createProcedure2Statement = @"create procedure procedure2
as
begin
    select * from table2;
end";

            var createProcedure3Statement = @"create procedure procedure3
as
begin
    exec procedure1;
end";

            var sqlScript = new SqlScript()
                                {
                                    Identity = Path.GetFullPath(@"..\..\TestData\create_three_procedures.sql")
                                };

            this.batchSeparator.ExtractBatches(sqlScript);

            var batch1 = sqlScript.Batches.ElementAt(0);
            var batch2 = sqlScript.Batches.ElementAt(1);
            var batch3 = sqlScript.Batches.ElementAt(2);

            Assert.Equal(3, sqlScript.Batches.Count());

            Assert.Equal(1, batch1.StartLine);
            Assert.Equal(1, batch1.StartColumn);
            Assert.Equal(createProcedure1Statement, batch1.SqlText);

            // empty lines are ommited
            Assert.Equal(9, batch2.StartLine);
            Assert.Equal(1, batch2.StartColumn);
            Assert.Equal(createProcedure2Statement, batch2.SqlText);

            Assert.Equal(17, batch3.StartLine);
            Assert.Equal(1, batch3.StartColumn);
            Assert.Equal(createProcedure3Statement, batch3.SqlText);
        }

        [Fact]
        public void extract_batches_from_script_with_errors()
        {
            var sqlScript = new SqlScript()
            {
                Identity = Path.GetFullPath(@"..\..\TestData\script_with_parsing_errors.sql")
            };

            var exception = Assert.Throws<SqlBuildException>(
                () =>
                    {
                        this.batchSeparator.ExtractBatches(sqlScript);
                    });
        }
    }
}
