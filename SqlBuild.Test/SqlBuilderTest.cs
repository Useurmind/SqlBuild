using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using SqlBuild.Container;
using SqlBuild.Test.Model;
using SqlBuild.Model;

using Xunit;

namespace SqlBuild.Test
{
    public class SqlBuilderTest
    {
        private ISqlBuilder sqlBuilder;
        private SqlBuildSetup setup;

        public SqlBuilderTest()
        {
            setup = new SqlBuildSetup() { SqlBuildLog = new ExceptionSqlBuildLog() };

            setup.Connections[Constants.DefaultKey].ServerVersion = ServerVersion.SqlServer2008;

            var containerFactory = new ContainerFactory(new ExceptionSqlBuildLog(), setup);

            var container = containerFactory.CreateContainer();

            sqlBuilder = container.Resolve<ISqlBuilder>();
        }

        [Fact]
        public void compile_single_file()
        {
            setup.Scripts.Add(new SqlScript()
                                  {
                                      Identity = @"..\..\TestData\create_three_procedures.sql"
                                  });            


            sqlBuilder.Compile();
        }
    }
}
