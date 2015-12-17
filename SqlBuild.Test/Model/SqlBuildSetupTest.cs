using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;
using SqlBuild.Utility;

using Xunit;

namespace SqlBuild.Test.Model
{
    public class SqlBuildSetupTest
    {
        private  SqlBuildSetup setup;

        public SqlBuildSetupTest()
        {   
            setup = new SqlBuildSetup()
                        {
                            SqlBuildLog = new ExceptionSqlBuildLog()
                        };
        }

        [Fact]
        public void default_models_are_created_and_defaults_are_correct()
        {
            Assert.Equal(Constants.DefaultKey, setup.ActiveGlobalConfigurationKey);

            Assert.Equal(1, setup.GlobalConfigurations.Count);
            Assert.Equal(1, setup.ScriptConfigurations.Count);
            Assert.Equal(1, setup.Connections.Count);
            Assert.Equal(1, setup.Logins.Count);
            Assert.Equal(1, setup.ScriptMappings.Count);
            Assert.Equal(0, setup.Scripts.Count);

            Assert.NotNull(setup.DefaultConnection);
            Assert.NotNull(setup.DefaultLogin);
            Assert.NotNull(setup.DefaultScriptConfiguration);
            Assert.NotNull(setup.DefaultScriptMapping);
            
            Assert.Equal(Constants.DefaultKey, setup.DefaultGlobalConfiguration.SqlBuildInfoLoginKey);
            Assert.Equal(Constants.DefaultKey, setup.DefaultGlobalConfiguration.ConnectionKey);

            Assert.Equal(Constants.DefaultKey, setup.DefaultConnection.Key);
            Assert.Equal(Constants.DefaultKey, setup.DefaultLogin.Key);
            Assert.Equal(Constants.DefaultKey, setup.DefaultScriptConfiguration.Key);
            Assert.Equal(Constants.DefaultKey, setup.DefaultScriptMapping.Key);
            Assert.Equal(Constants.DefaultKey, setup.DefaultScriptMapping.Key);
            
            Assert.True(string.IsNullOrEmpty(setup.ScriptMappings.First().Value.ScriptPattern));
            Assert.Equal(Constants.DefaultKey, setup.ScriptMappings.First().Value.LoginKey);
            Assert.Equal(Constants.DefaultKey, setup.ScriptMappings.First().Value.ConfigurationKey);
        }

        [Fact]
        public void when_connect_graph_was_called_with_defaults_the_default_objects_are_connected()
        {
            setup.ConnectReferences();

            Assert.Same(setup.DefaultGlobalConfiguration, setup.ActiveGlobalConfiguration);

            Assert.Same(setup.DefaultLogin, setup.DefaultScriptMapping.Login);
            Assert.Same(setup.DefaultScriptConfiguration, setup.DefaultScriptMapping.Configuration);

            Assert.Same(setup.DefaultLogin, setup.DefaultGlobalConfiguration.SqlBuildInfoLogin);
            Assert.Same(setup.DefaultConnection, setup.DefaultGlobalConfiguration.Connection);
        }
        [Fact]
        public void when_connect_graph_was_called_with_non_default_elements_all_non_default_elements_are_connected()
        {
            var configuration = new SqlScriptConfiguration()
            {
                Key = "fdhjfzt7er5"
            };
            var login = new SqlLogin() { Key = "sgfdhgdsfg" };
            var connection = new SqlConnection() { Key = "dlknjgfdlkjg" };
            var mapping = new SqlScriptMapping()
            {
                Key = "afdfshf",
                ConfigurationKey = configuration.Key,
                LoginKey = login.Key
            };
            var globalConfiguration = new SqlGlobalConfiguration()
                                          {
                                              Key = "aslfgjsdhlkghh",
                                              SqlBuildInfoLoginKey = login.Key,
                                              ConnectionKey = connection.Key
                                          };
            setup.ActiveGlobalConfigurationKey = globalConfiguration.Key;

            setup.GlobalConfigurations.Add(globalConfiguration.Key, globalConfiguration);
            setup.ScriptConfigurations.Add(configuration.Key, configuration);
            setup.Logins.Add(login.Key, login);
            setup.Connections.Add(connection.Key, connection);
            setup.ScriptMappings.Add(mapping.Key, mapping);

            setup.ConnectReferences();

            Assert.Same(globalConfiguration, setup.ActiveGlobalConfiguration);

            Assert.Same(configuration, mapping.Configuration);
            Assert.Same(login, mapping.Login);

            Assert.Same(login, globalConfiguration.SqlBuildInfoLogin);
            Assert.Same(connection, globalConfiguration.Connection);
        }

        [Fact]
        public void when_connect_graph_was_called_with_missing_configuration_in_mapping_error_is_generated()
        {
            var login = new SqlLogin() { Key = "sgfdhgdsfg" };
            var connection = new SqlConnection() { Key = "dlknjgfdlkjg" };
            var mapping = new SqlScriptMapping()
                              {
                                  Key = "afdfshf",
                                  ConfigurationKey = "öljhflkztrz",
                                  LoginKey = login.Key
                              };

            setup.Logins.Add(login.Key, login);
            setup.Connections.Add(connection.Key, connection);
            setup.ScriptMappings.Add(mapping.Key, mapping);

            var exception = Assert.Throws<SqlBuildException>(() => setup.ConnectReferences());

            Assert.True(exception.Message.Contains(typeof(SqlScriptMapping).Name));
            Assert.True(exception.Message.Contains(typeof(SqlScriptConfiguration).Name));
            Assert.True(exception.Message.Contains(mapping.Key));
            Assert.True(exception.Message.Contains(mapping.ConfigurationKey));
        }

        [Fact]
        public void when_connect_graph_was_called_with_missing_session_in_mapping_error_is_generated()
        {
            var configuration = new SqlScriptConfiguration()
            {
                Key = "sesionasdsadfgsfg"
            };
            var mapping = new SqlScriptMapping()
            {
                Key = "afdfshf",
                ConfigurationKey = configuration.Key,
                LoginKey = "asdfshfgjkzu"
            };

            setup.ScriptConfigurations.Add(configuration.Key, configuration);
            setup.ScriptMappings.Add(mapping.Key, mapping);

            var exception = Assert.Throws<SqlBuildException>(() => setup.ConnectReferences());

            Assert.True(exception.Message.Contains(typeof(SqlScriptMapping).Name));
            Assert.True(exception.Message.Contains(typeof(SqlSession).Name));
            Assert.True(exception.Message.Contains(mapping.Key));
            Assert.True(exception.Message.Contains(mapping.LoginKey));
        }
    }
}
