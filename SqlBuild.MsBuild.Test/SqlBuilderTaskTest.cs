using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NSubstitute;

using SqlBuild.Model;

using Xunit;

namespace SqlBuild.MsBuild.Test
{
    public class SqlBuilderTaskTest
    {
        private SqlBuilderTask task;
        private ISqlBuilder builderMock;

        public SqlBuilderTaskTest()
        {
            task = new SqlBuilderTask();
            builderMock = NSubstitute.Substitute.For<ISqlBuilder>();

            task.SetBuilder(builderMock);
        }

        [Fact]
        public void a_single_script_is_added_to_underlying_builder()
        {
            var identity = "fileIdentity";
            var item = new TaskItem(identity);

            task.Scripts = new ITaskItem[] { item };

            task.Execute();

            builderMock.Received().AddScript(Arg.Is<SqlScript>(s => s.Identity == identity));
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void builder_is_executed_and_return_value_returned(bool returnValue)
        {
            builderMock.Execute().Returns(returnValue);

            var actualReturnValue = task.Execute();

            builderMock.Received(1).Execute();
            Assert.Equal(returnValue, actualReturnValue);
        }

        [Theory]
        [InlineData("", "", "", "server", "database", "schema", "server", "database", "schema")]  // no default
        [InlineData("server1", "database1", "schema1", "server", "database", "schema", "server", "database", "schema")] // ignore default
        [InlineData("server1", "database1", "schema1", "server", "", "", "server", "database1", "schema1")] // take only item server
        [InlineData("server1", "database1", "schema1", "", "database", "", "server1", "database", "schema1")] // take only item datbase
        [InlineData("server1", "database1", "schema1", "", "", "schema", "server1", "database1", "schema")] // take only item schema
        public void connection_data_is_integrated_in_script(
            string defaultServer,
            string defaultDatabase,
            string defaultSchema,
            string itemServer,
            string itemDatabase,
            string itemSchema,
            string expectedServer,
            string expectedDatabase,
            string expectedSchema)
        {
            task.DefaultServer = defaultServer;
            task.DefaultDatabase = defaultDatabase;
            task.DefaultSchema = defaultSchema;

            var identity = "fileIdentity";
            var item = new TaskItem(identity);
            item.SetMetadata(SqlScriptTaskItemMetaData.Server, itemServer);
            item.SetMetadata(SqlScriptTaskItemMetaData.Database, itemDatabase);
            item.SetMetadata(SqlScriptTaskItemMetaData.Schema, itemSchema);

            task.Scripts = new ITaskItem[] { item };

            task.Execute();

            builderMock.Received(1).AddScript(Arg.Is<SqlScript>(
                s => identity == s.Identity && 
                    expectedServer == s.Connection.Server && 
                    expectedDatabase == s.Connection.Database && 
                    expectedSchema == s.Connection.Schema));
        }

        [Theory]
        [InlineData("", "", false, "user", "password", "false", "user", "password", false)]  // no default
        [InlineData("defUser", "defPassword", false, "", "", "", "defUser", "defPassword", false)]  // default user
        [InlineData("defUser", "defPassword", false, "itemUser", "itemPassword", "", "itemUser", "itemPassword", false)]  // item user
        public void login_data_is_integrated_in_script(
            string defaultUser,
            string defaultPassword,
            bool defaultIntegratedSecurity,
            string itemUser,
            string itemPassword,
            string itemIntegratedSecurity,
            string expectedUser,
            string expectedPassword,
            bool expectedIntegratedSecurity)
        {
            task.DefaultUserName = defaultUser;
            task.DefaultPassword = defaultPassword;
            task.DefaultIntegratedSecurity = defaultIntegratedSecurity;

            var identity = "fileIdentity";
            var item = new TaskItem(identity);
            item.SetMetadata(SqlScriptTaskItemMetaData.UserName, itemUser);
            item.SetMetadata(SqlScriptTaskItemMetaData.Password, itemPassword);
            item.SetMetadata(SqlScriptTaskItemMetaData.IntegratedSecurity, itemIntegratedSecurity);

            task.Scripts = new ITaskItem[] { item };

            task.Execute();

            builderMock.Received(1).AddScript(Arg.Is<SqlScript>(
                s => identity == s.Identity &&
                    expectedUser == s.Login.UserName &&
                    expectedPassword == s.Login.Password &&
                    expectedIntegratedSecurity == s.Login.IntegratedSecurity));
        }

        [Fact]
        public void builder_configuration_is_set()
        {
            task.Execute();

            builderMock.Received(1).SetConfiguration(Arg.Is<SqlBuilderConfiguration>(c => c != null));
        }
    }
}
