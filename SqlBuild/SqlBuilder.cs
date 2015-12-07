using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Deployment;
using SqlBuild.Model;
using SqlBuild.Validation;

namespace SqlBuild
{
    public interface ISqlBuilder
    {
        void SetConfiguration(SqlBuilderConfiguration configuration);

        void AddScript(SqlScript script);

        bool Execute();
    }

    public class SqlBuilder : ISqlBuilder
    {
        private IList<SqlScript> scripts;

        private SqlBuilderConfiguration configuration;

        public SqlBuilder()
        {
            scripts = new List<SqlScript>();
        }

        public void SetConfiguration(SqlBuilderConfiguration configuration)
        {
            configuration = new SqlBuilderConfiguration();
        }

        public void AddScript(SqlScript script)
        {
            scripts.Add(script);
        }

        public bool Execute()
        {
            foreach (var script in scripts)
            {
                new ScriptExistsCheck(script);
            }

            var executor = new ScriptExecutor();
            foreach (var script in scripts)
            {
                executor.Execute(script);
            }

            return true;
        }
    }
}
