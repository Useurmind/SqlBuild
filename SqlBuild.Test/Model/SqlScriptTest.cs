using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;
using SqlBuild.Utility;

using Xunit;

namespace SqlBuild.Test.Model
{
    public class SqlScriptTest
    {
        [Fact]
        public void GetSqlText_returns_sql_text_after_it_was_set()
        {
            var sql = "adlgfdslöldsfmt, dg ";
            var script = new SqlScript();

            script.SetSqlText(sql);

            var result = script.GetSqlText();

            Assert.Equal(sql, result);
        }

        [Fact]
        public void GetSqlText_returns_sql_text_of_Identity_file()
        {
            var sql = "select * from table1;";
            var script = new SqlScript() { Identity = @"TestData\simple_select.sql" };

            var result = script.GetSqlText();

            Assert.Equal(sql, result);
        }

        [Fact]
        public void GetSqlText_throws_for_missing_identity_and_sql_text()
        {
            var script = new SqlScript();

            Assert.Throws<SqlBuildException>(() => script.GetSqlText());
        }

        [Fact]
        public void GetSqlText_throws_for_non_existing_file()
        {
            var script = new SqlScript() { Identity = "some/non/existing/path" };

            Assert.Throws<SqlBuildException>(() => script.GetSqlText());
        }
    }
}
