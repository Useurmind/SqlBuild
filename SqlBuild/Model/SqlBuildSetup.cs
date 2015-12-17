using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Logging;

namespace SqlBuild.Model
{
    public class SqlBuildSetup
    {
        public string ActiveGlobalConfigurationKey { get; set; }

        public SqlGlobalConfiguration ActiveGlobalConfiguration { get; private set; }

        public SqlConnection DefaultConnection
        {
            get
            {
                return Connections[Constants.DefaultKey];
            }
        }

        public SqlGlobalConfiguration DefaultGlobalConfiguration
        {
            get
            {
                return GlobalConfigurations[Constants.DefaultKey];
            }
        }

        public SqlScriptConfiguration DefaultScriptConfiguration
        {
            get
            {
                return ScriptConfigurations[Constants.DefaultKey];
            }
        }

        public SqlLogin DefaultLogin
        {
            get
            {
                return Logins[Constants.DefaultKey];
            }
        }

        public SqlScriptMapping DefaultScriptMapping
        {
            get
            {
                return ScriptMappings[Constants.DefaultKey];
            }
        }

        public IDictionary<string, SqlConnection> Connections { get; private set; }
        public IDictionary<string, SqlGlobalConfiguration> GlobalConfigurations { get; private set; }
        public IDictionary<string, SqlScriptConfiguration> ScriptConfigurations { get; private set; }
        public IDictionary<string, SqlLogin> Logins { get; private set; }
        public IDictionary<string, SqlScriptMapping> ScriptMappings { get; private set; }
        public IList<SqlScript> Scripts { get; private set; }

        public ISqlBuildLog SqlBuildLog { get; set; }

        public SqlBuildSetup()
        {
            this.ActiveGlobalConfigurationKey = Constants.DefaultKey;

            Connections = new Dictionary<string, SqlConnection>();
            GlobalConfigurations = new Dictionary<string, SqlGlobalConfiguration>();
            ScriptConfigurations = new Dictionary<string, SqlScriptConfiguration>();
            Logins = new Dictionary<string, SqlLogin>();
            Scripts = new List<SqlScript>();
            ScriptMappings = new Dictionary<string, SqlScriptMapping>();

            Connections.Add(Constants.DefaultKey, new SqlConnection() { Key = Constants.DefaultKey });
            GlobalConfigurations.Add(Constants.DefaultKey, new SqlGlobalConfiguration()
                                                               {
                                                                   Key = Constants.DefaultKey,
                                                                   SqlBuildInfoLoginKey = Constants.DefaultKey,
                                                                   ConnectionKey = Constants.DefaultKey
                                                               });
            ScriptConfigurations.Add(Constants.DefaultKey, new SqlScriptConfiguration() { Key = Constants.DefaultKey });
            Logins.Add(Constants.DefaultKey, new SqlLogin() { Key = Constants.DefaultKey });

            ScriptMappings.Add(Constants.DefaultKey, new SqlScriptMapping()
                                                         {
                                                             Key = Constants.DefaultKey,
                                                             LoginKey = Constants.DefaultKey,
                                                             ConfigurationKey = Constants.DefaultKey
                                                         });
        }

        public void ConnectReferences()
        {
            SetGlobalConfiguration();

            ConnectMappings();

            this.ConnectGlobalConfigurations();
        }

        private void SetGlobalConfiguration()
        {
            SqlGlobalConfiguration configuration;
            if (!GlobalConfigurations.TryGetValue(ActiveGlobalConfigurationKey, out configuration))
            {
                this.SqlBuildLog.WriteReferencedElementNotFound<SqlBuildSetup, SqlGlobalConfiguration>(
                    "<none>",
                    ActiveGlobalConfigurationKey);
            }
            else
            {
                ActiveGlobalConfiguration = configuration;
            }
        }

        private void ConnectMappings()
        {
            foreach (var scriptMapping in ScriptMappings.Values)
            {
                SqlScriptConfiguration configuration;
                if (!ScriptConfigurations.TryGetValue(scriptMapping.ConfigurationKey, out configuration))
                {
                    this.SqlBuildLog.WriteReferencedElementNotFound<SqlScriptMapping, SqlScriptConfiguration>(
                        scriptMapping.Key,
                        scriptMapping.ConfigurationKey);
                }
                else
                {
                    scriptMapping.Configuration = configuration;
                }

                SqlLogin login;
                if (!Logins.TryGetValue(scriptMapping.LoginKey, out login))
                {
                    this.SqlBuildLog.WriteReferencedElementNotFound<SqlScriptMapping, SqlSession>(
                        scriptMapping.Key,
                        scriptMapping.LoginKey);
                }
                else
                {
                    scriptMapping.Login = login;
                }
            }
        }

        private void ConnectGlobalConfigurations()
        {
            foreach (var globalConfig in GlobalConfigurations.Values)
            {
                SqlLogin login = null;
                if (!Logins.TryGetValue(globalConfig.SqlBuildInfoLoginKey, out login))
                {
                    this.SqlBuildLog.WriteReferencedElementNotFound<SqlGlobalConfiguration, SqlLogin>(globalConfig.Key, globalConfig.SqlBuildInfoLoginKey);
                }
                else
                {
                    globalConfig.SqlBuildInfoLogin = login;
                }

                SqlConnection connection = null;
                if (!Connections.TryGetValue(globalConfig.ConnectionKey, out connection))
                {
                    this.SqlBuildLog.WriteReferencedElementNotFound<SqlGlobalConfiguration, SqlConnection>(globalConfig.Key, globalConfig.ConnectionKey);
                }
                else
                {
                    globalConfig.Connection = connection;
                }
            }
        }
    }
}
