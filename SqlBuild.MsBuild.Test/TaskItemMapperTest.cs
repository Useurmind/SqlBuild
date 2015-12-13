using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;

using NSubstitute;

using SqlBuild.Model;
using SqlBuild.Utility;

using Xunit;

namespace SqlBuild.MsBuild.Test
{
    public class TaskItemMapperTest
    {
        private TaskItemMapperInput input;
        private TaskItemMapper mapper;
        private SqlBuildSetup output;

        public TaskItemMapperTest()
        {
            input = new TaskItemMapperInput();
            output = new SqlBuildSetup();
            mapper = new TaskItemMapper();
        }

        [Fact]
        public void a_single_named_global_configuration_is_mapped_correctly()
        {
            string globalConfigurationName = "alsfjsdlkfjslk";
            input.GlobalConfigurations = new[] { new TaskItem(globalConfigurationName) };

            mapper.MapTo(input, output);

            var globalConfig = output.GlobalConfigurations[globalConfigurationName];

            Assert.Equal(globalConfigurationName, globalConfig.Key);
        }

        [Fact]
        public void a_single_named_script_configuration_is_mapped_correctly()
        {
            string scriptConfigurationName = "sdflkdjgsfg";

            input.ScriptConfigurations = new[] { new TaskItem(scriptConfigurationName) };

            mapper.MapTo(input, output);

            var scriptConfig = output.ScriptConfigurations[scriptConfigurationName];

            Assert.Equal(scriptConfigurationName, scriptConfig.Key);
        }

        [Fact]
        public void a_single_named_login_with_username_is_mapped_correctly()
        {
            string loginKey = "asdflkjdsfglkdsfg";
            string loginName = "flödfdölh";

            var loginItem = new TaskItem(loginKey);
            loginItem.SetMetadata(ModelExtensions.GetMetadataName<SqlLogin, string>(x => x.UserName), loginName);

            input.Logins = new[] { loginItem };

            mapper.MapTo(input, output);

            var login = output.Logins[loginKey];

            Assert.Equal(loginName, login.UserName);
            Assert.True(string.IsNullOrEmpty(login.IntegratedSecurity));
        }

        [Fact]
        public void a_single_named_login_with_integrated_security_is_mapped_correctly()
        {
            string loginKey = "asdflkjdsfglkdsfg";
            string integratedSecurity = "flödfdölh";

            var loginItem = new TaskItem(loginKey);
            loginItem.SetMetadata(ModelExtensions.GetMetadataName<SqlLogin, string>(x => x.IntegratedSecurity), integratedSecurity);

            input.Logins = new[] { loginItem };

            mapper.MapTo(input, output);

            var login = output.Logins[loginKey];

            Assert.Equal(integratedSecurity, login.IntegratedSecurity);
            Assert.True(string.IsNullOrEmpty(login.UserName));
        }



        [Fact]
        public void a_single_named_login_with_username_and_integrated_security_is_mapped_correctly()
        {
            // yes this is correct, the validity of the data is checked in a later stage
            string loginKey = "asdflkjdsfglkdsfg";
            string userName = "sdlkfjgsdflgdfg";
            string integratedSecurity = "flödfdölh";

            var loginItem = new TaskItem(loginKey);
            loginItem.SetMetadata(ModelExtensions.GetMetadataName<SqlLogin, string>(x => x.UserName), userName);
            loginItem.SetMetadata(ModelExtensions.GetMetadataName<SqlLogin, string>(x => x.IntegratedSecurity), integratedSecurity);

            input.Logins = new[] { loginItem };

            mapper.MapTo(input, output);

            var login = output.Logins[loginKey];

            Assert.Equal(loginKey, login.Key);
            Assert.Equal(userName, login.UserName);
            Assert.Equal(integratedSecurity, login.IntegratedSecurity);
            Assert.Equal(userName, login.UserName);
        }

        [Fact]
        public void single_connection_with_server_database_and_schema_is_mapped_correctly()
        {
            string connectionKey = "sdflkslödfsgd";
            string server = "sdölkjsdfgdsfg";
            string database = "dfshdgjhfdghjfhg";
            string schema = "afdshdfjhfhjkghk";

            var connectionItem = new TaskItem(connectionKey);
            connectionItem.SetMetadata(ModelExtensions.GetMetadataName<SqlConnection, string>(x => x.Server), server);
            connectionItem.SetMetadata(ModelExtensions.GetMetadataName<SqlConnection, string>(x => x.Database), database);
            connectionItem.SetMetadata(ModelExtensions.GetMetadataName<SqlConnection, string>(x => x.Schema), schema);

            input.Connections = new[] { connectionItem };

            mapper.MapTo(input, output);

            var connection = output.Connections[connectionKey];

            Assert.Equal(connectionKey, connection.Key);
            Assert.Equal(server, connection.Server);
            Assert.Equal(database, connection.Database);
            Assert.Equal(schema, connection.Schema);
        }

        [Fact]
        public void single_session_with_login_and_connection_is_mapped_correctly()
        {
            string sessionKey = "sdflkslödfsgd";
            string loginKey = "sdölkjsdfgdsfg";
            string connectionKey = "dfshdgjhfdghjfhg";

            var sessionItem = new TaskItem(sessionKey);
            sessionItem.SetMetadata(ModelExtensions.GetMetadataName<SqlSession, string>(x => x.ConnectionKey), connectionKey);
            sessionItem.SetMetadata(ModelExtensions.GetMetadataName<SqlSession, string>(x => x.LoginKey), loginKey);

            input.Sessions = new[] { sessionItem };

            mapper.MapTo(input, output);

            var session = output.Sessions[sessionKey];

            Assert.Equal(sessionKey, session.Key);
            Assert.Equal(loginKey, session.LoginKey);
            Assert.Equal(connectionKey, session.ConnectionKey);
        }

        [Fact]
        public void single_script_mapping_with_session_and_configuration_is_mapped_correctly()
        {
            string scriptMappingKey = "asflfdjgsdl";
            string scriptMappingPattern = "Adsdfgsdfg";
            string sessionKey = "sdflkslödfsgd";
            string configurationKey= "dfshdgjhfdghjfhg";

            var mappingItem = new TaskItem(scriptMappingKey);
            mappingItem.SetMetadata(ModelExtensions.GetMetadataName<SqlScriptMapping, string>(x => x.ScriptPattern), scriptMappingPattern);
            mappingItem.SetMetadata(ModelExtensions.GetMetadataName<SqlScriptMapping, string>(x => x.SessionKey), sessionKey);
            mappingItem.SetMetadata(ModelExtensions.GetMetadataName<SqlScriptMapping, string>(x => x.ConfigurationKey), configurationKey);

            input.ScriptMappings = new[] { mappingItem };

            mapper.MapTo(input, output);
            
            var mapping = output.ScriptMappings[scriptMappingKey];

            Assert.Equal(scriptMappingPattern, mapping.ScriptPattern);
            Assert.Equal(sessionKey, mapping.SessionKey);
            Assert.Equal(configurationKey, mapping.ConfigurationKey);
        }

        [Fact]
        public void single_script_is_mapped_correctly()
        {
            string scriptSpec = "asgdshdghdfh";
            string scriptIdentity = "sdglkjsdfgldfg";

            var scriptItem = Substitute.For<ITaskItem>();
            scriptItem.ItemSpec.Returns(scriptSpec);
            scriptItem.GetMetadata("Identity").Returns(scriptIdentity);

            input.Scripts = new[] { scriptItem };

            mapper.MapTo(input, output);

            var script = output.Scripts.First(x => x.Identity == scriptIdentity);

            Assert.Equal(scriptSpec, script.ItemSpec);
        }
    }
}
