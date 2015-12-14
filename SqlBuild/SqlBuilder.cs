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

        bool Execute();
    }

    public class SqlBuilder : ISqlBuilder
    {
        public SqlBuildSetup Setup { get; set; }

        public bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}
