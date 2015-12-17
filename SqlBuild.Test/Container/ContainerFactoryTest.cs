using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using SqlBuild.Container;
using SqlBuild.Model;

using Xunit;

namespace SqlBuild.Test.Container
{
    public class ContainerFactoryTest
    {
        [Fact]
        public void creating_container_and_resolving_sql_builder_works()
        {
            var containerFactory = new ContainerFactory(new ExceptionSqlBuildLog(), new SqlBuildSetup() { SqlBuildLog = new ExceptionSqlBuildLog() });

            var container = containerFactory.CreateContainer();

            var sqlBuilder = container.Resolve<ISqlBuilder>();

            Assert.NotNull(sqlBuilder);
        }
    }
}
