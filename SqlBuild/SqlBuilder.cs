using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Logging;
using SqlBuild.Model;
using SqlBuild.Syntax;
using SqlBuild.Validation;

namespace SqlBuild
{
    public interface ISqlBuilder
    {
        SqlBuildSetup Setup { get; }

        void Compile();

        void CompileAndDeploy();
    }

    public class SqlBuilder : ISqlBuilder
    {
        public ISqlBuildLog Log { get; set; }
        public IBatchExtractor BatchExtractor { get; set; }
        public SqlBuildSetup Setup { get; set; }

        public void Compile()
        {
            IEnumerable<SqlScript> remainingScripts = Setup.Scripts;

            Log.WriteTrace("Assigning scripts to script mappings");

            foreach (var mapping in Setup.ScriptMappings.Values)
            {
                mapping.FindMatchingScripts(remainingScripts);

                Log.WriteTraceFormat("Found {0} matching scripts for script mapping '{1}'", mapping.MatchingScripts.Count(), mapping.Key);

                remainingScripts = remainingScripts.Except(mapping.MatchingScripts);

                Log.WriteTraceFormat("{0} scripts remaining", remainingScripts.Count());

                foreach (var matchingScript in mapping.MatchingScripts)
                {
                    BatchExtractor.ExtractBatches(matchingScript);
                }
            }
        }

        public void CompileAndDeploy()
        {
            throw new NotImplementedException();
        }
    }
}
