using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;

using Xunit;

namespace SqlBuild.Test.Model
{
    public class SqlScriptMappingTest
    {
        [Fact]
        public void FindMatchingScripts_only_extracts_matching_scripts()
        {
            var matchingScript1 = new SqlScript() { Identity = "abc.sql" };
            var matchingScript2 = new SqlScript() { Identity = @"some\path\to\abc.sql" };
            var matchingScript3 = new SqlScript() { Identity = @"some\abc\path\to\file.sql" };

            var scripts = new[]
                              {
                                  new SqlScript() { Identity = "c" },
                                  matchingScript1,
                                  new SqlScript() { Identity = "d" },
                                  new SqlScript() { Identity = "e" },
                                  matchingScript2,
                                  matchingScript3,
                                  new SqlScript() { Identity = "f" }
                              };

            var scriptMapping = new SqlScriptMapping() { ScriptPattern = "abc" };

            scriptMapping.FindMatchingScripts(scripts);

            Assert.Equal(3, scriptMapping.MatchingScripts.Count());
            Assert.Same(matchingScript1, scriptMapping.MatchingScripts.ElementAt(0));
            Assert.Same(matchingScript2, scriptMapping.MatchingScripts.ElementAt(1));
            Assert.Same(matchingScript3, scriptMapping.MatchingScripts.ElementAt(2));
        }
    }
}
