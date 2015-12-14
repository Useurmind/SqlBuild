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
        SqlBuildSetup Setup { get; }

        void Compile();

        void CompileAndDeploy();
    }

    public class SqlBuilder : ISqlBuilder
    {
        public SqlBuildSetup Setup { get; set; }

        public void Compile()
        {
            throw new NotImplementedException();
        }

        public void CompileAndDeploy()
        {
            throw new NotImplementedException();
        }
    }
}
