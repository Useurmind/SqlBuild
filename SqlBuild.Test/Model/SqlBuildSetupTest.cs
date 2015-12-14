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
            Assert.Equal(1, setup.Sessions.Count);
            Assert.Equal(1, setup.ScriptMappings.Count);
            Assert.Equal(0, setup.Scripts.Count);

            Assert.NotNull(setup.DefaultConnection);
            Assert.NotNull(setup.DefaultLogin);
            Assert.NotNull(setup.DefaultScriptConfiguration);
            Assert.NotNull(setup.DefaultScriptMapping);
            Assert.NotNull(setup.DefaultSession);
            

            Assert.Equal(Constants.DefaultKey, setup.DefaultConnection.Key);
            Assert.Equal(Constants.DefaultKey, setup.DefaultLogin.Key);
            Assert.Equal(Constants.DefaultKey, setup.DefaultScriptConfiguration.Key);
            Assert.Equal(Constants.DefaultKey, setup.DefaultScriptMapping.Key);
            Assert.Equal(Constants.DefaultKey, setup.DefaultSession.Key);
            Assert.Equal(Constants.DefaultKey, setup.DefaultScriptMapping.Key);

            Assert.Equal(Constants.DefaultKey, setup.Sessions.First().Value.LoginKey);
            Assert.Equal(Constants.DefaultKey, setup.Sessions.First().Value.ConnectionKey);
            
            Assert.True(string.IsNullOrEmpty(setup.ScriptMappings.First().Value.ScriptPattern));
            Assert.Equal(Constants.DefaultKey, setup.ScriptMappings.First().Value.SessionKey);
            Assert.Equal(Constants.DefaultKey, setup.ScriptMappings.First().Value.ConfigurationKey);
        }

        [Fact]
        public void when_connect_graph_was_called_with_defaults_the_default_objects_are_connected()
        {
            setup.ConnectReferences();

            Assert.Same(setup.DefaultGlobalConfiguration, setup.ActiveGlobalConfiguration);

            Assert.Same(setup.DefaultSession, setup.DefaultScriptMapping.Session);
            Assert.Same(setup.DefaultScriptConfiguration, setup.DefaultScriptMapping.Configuration);

            Assert.Same(setup.DefaultConnection, setup.DefaultSession.Connection);
            Assert.Same(setup.DefaultLogin, setup.DefaultSession.Login);
        }
        [Fact]
        public void when_connect_graph_was_called_with_non_default_elements_all_non_default_elements_are_connected()
        {
            var globalConfiguration = new SqlGlobalConfiguration() { Key = "aslfgjsdhlkghh" };
            var configuration = new SqlScriptConfiguration()
            {
                Key = "fdhjfzt7er5"
            };
            var login = new SqlLogin() { Key = "sgfdhgdsfg" };
            var connection = new SqlConnection() { Key = "dlknjgfdlkjg" };
            var session = new SqlSession()
            {
                Key = "sesionasdsadfgsfg",
                ConnectionKey = connection.Key,
                LoginKey = login.Key
            };
            var mapping = new SqlScriptMapping()
            {
                Key = "afdfshf",
                ConfigurationKey = configuration.Key,
                SessionKey = session.Key
            };
            setup.ActiveGlobalConfigurationKey = globalConfiguration.Key;

            setup.GlobalConfigurations.Add(globalConfiguration.Key, globalConfiguration);
            setup.ScriptConfigurations.Add(configuration.Key, configuration);
            setup.Logins.Add(login.Key, login);
            setup.Connections.Add(connection.Key, connection);
            setup.Sessions.Add(session.Key, session);
            setup.ScriptMappings.Add(mapping.Key, mapping);

            setup.ConnectReferences();

            Assert.Same(globalConfiguration, setup.ActiveGlobalConfiguration);

            Assert.Same(login, session.Login);
            Assert.Same(connection, session.Connection);

            Assert.Same(configuration, mapping.Configuration);
            Assert.Same(session, mapping.Session);
        }

        [Fact]
        public void when_connect_graph_was_called_with_missing_login_in_session_error_is_generated()
        {
            var connection = new SqlConnection() { Key = "sgfdhgdsfg" };
            var sessionMissingLogin = new SqlSession()
                                          {
                                              Key = "sesionasdsadfgsfg",
                                              ConnectionKey = connection.Key,
                                              LoginKey = "slkdfgjsdflkhgshg"
                                          };

            setup.Connections.Add(connection.Key, connection);
            setup.Sessions.Add(sessionMissingLogin.Key, sessionMissingLogin);

            var exception = Assert.Throws<SqlBuildException>(() => setup.ConnectReferences());

            Assert.True(exception.Message.Contains(typeof(SqlSession).Name));
            Assert.True(exception.Message.Contains(typeof(SqlLogin).Name));
            Assert.True(exception.Message.Contains(sessionMissingLogin.Key));
            Assert.True(exception.Message.Contains(sessionMissingLogin.LoginKey));
        }

        [Fact]
        public void when_connect_graph_was_called_with_missing_connection_in_session_error_is_generated()
        {
            var login = new SqlLogin() { Key = "sgfdhgdsfg" };
            var sessionMissingLogin = new SqlSession()
            {
                Key = "sesionasdsadfgsfg",
                ConnectionKey = "asdflköjsdlft",
                LoginKey = login.Key
            };

            setup.Logins.Add(login.Key, login);
            setup.Sessions.Add(sessionMissingLogin.Key, sessionMissingLogin);

            var exception = Assert.Throws<SqlBuildException>(() => setup.ConnectReferences());

            Assert.True(exception.Message.Contains(typeof(SqlSession).Name));
            Assert.True(exception.Message.Contains(typeof(SqlConnection).Name));
            Assert.True(exception.Message.Contains(sessionMissingLogin.Key));
            Assert.True(exception.Message.Contains(sessionMissingLogin.ConnectionKey));
        }

        [Fact]
        public void when_connect_graph_was_called_with_missing_configuration_in_mapping_error_is_generated()
        {
            var login = new SqlLogin() { Key = "sgfdhgdsfg" };
            var connection = new SqlConnection() { Key = "dlknjgfdlkjg" };
            var session= new SqlSession()
            {
                Key = "sesionasdsadfgsfg",
                ConnectionKey = connection.Key,
                LoginKey = login.Key
            };
            var mapping = new SqlScriptMapping()
                              {
                                  Key = "afdfshf",
                                  ConfigurationKey = "öljhflkztrz",
                                  SessionKey = session.Key
                              };

            setup.Logins.Add(login.Key, login);
            setup.Connections.Add(connection.Key, connection);
            setup.Sessions.Add(session.Key, session);
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
                SessionKey = "asdfshfgjkzu"
            };

            setup.ScriptConfigurations.Add(configuration.Key, configuration);
            setup.ScriptMappings.Add(mapping.Key, mapping);

            var exception = Assert.Throws<SqlBuildException>(() => setup.ConnectReferences());

            Assert.True(exception.Message.Contains(typeof(SqlScriptMapping).Name));
            Assert.True(exception.Message.Contains(typeof(SqlSession).Name));
            Assert.True(exception.Message.Contains(mapping.Key));
            Assert.True(exception.Message.Contains(mapping.SessionKey));
        }
    }
}
