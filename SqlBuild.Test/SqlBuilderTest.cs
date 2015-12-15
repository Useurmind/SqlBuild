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
            var containerFactory = new ContainerFactory(new ExceptionSqlBuildLog());

            var container = containerFactory.CreateContainer();

            setup = container.Resolve<SqlBuildSetup>();
            sqlBuilder = container.Resolve<ISqlBuilder>();
        }

        [Fact]
        public void compile_single_file()
        {
            setup.Scripts.Add(new SqlScript()
                                  {
                                      Identity = @"..\..\TestData\create_three_procedures.sql"
                                  });

            setup.Connections[Constants.DefaultKey].ServerVersion = ServerVersion.SqlServer2008;


            sqlBuilder.Compile();
        }
    }
}
