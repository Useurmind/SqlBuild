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

        public SqlSession DefaultSession
        {
            get
            {
                return Sessions[Constants.DefaultKey];
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
        public IDictionary<string, SqlSession> Sessions { get; private set; }
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
            Sessions = new Dictionary<string, SqlSession>();
            ScriptMappings = new Dictionary<string, SqlScriptMapping>();

            Connections.Add(Constants.DefaultKey, new SqlConnection() { Key = Constants.DefaultKey });
            GlobalConfigurations.Add(Constants.DefaultKey, new SqlGlobalConfiguration() { Key = Constants.DefaultKey });
            ScriptConfigurations.Add(Constants.DefaultKey, new SqlScriptConfiguration() { Key = Constants.DefaultKey });
            Logins.Add(Constants.DefaultKey, new SqlLogin() { Key = Constants.DefaultKey });
            Sessions.Add(Constants.DefaultKey, new SqlSession()
                                                   {
                                                       Key = Constants.DefaultKey,
                                                       ConnectionKey = Constants.DefaultKey,
                                                       LoginKey = Constants.DefaultKey,
                                                   });
            ScriptMappings.Add(Constants.DefaultKey, new SqlScriptMapping()
                                                         {
                                                             Key = Constants.DefaultKey,
                                                             SessionKey = Constants.DefaultKey,
                                                             ConfigurationKey = Constants.DefaultKey
                                                         });
        }

        public void ConnectReferences()
        {
            SetGlobalConfiguration();

            ConnectSessions();

            ConnectMappings();
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

        private void ConnectSessions()
        {
            foreach (var session in Sessions.Values)
            {
                SqlLogin login = null;
                if (!Logins.TryGetValue(session.LoginKey, out login))
                {
                    this.SqlBuildLog.WriteReferencedElementNotFound<SqlSession, SqlLogin>(session.Key, session.LoginKey);
                }
                else
                {
                    session.Login = login;
                }

                SqlConnection connection = null;
                if (!Connections.TryGetValue(session.ConnectionKey, out connection))
                {
                    this.SqlBuildLog.WriteReferencedElementNotFound<SqlSession, SqlConnection>(
                        session.Key,
                        session.ConnectionKey);
                }
                else
                {
                    session.Connection = connection;
                }
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

                SqlSession session;
                if (!Sessions.TryGetValue(scriptMapping.SessionKey, out session))
                {
                    this.SqlBuildLog.WriteReferencedElementNotFound<SqlScriptMapping, SqlSession>(
                        scriptMapping.Key,
                        scriptMapping.SessionKey);
                }
                else
                {
                    scriptMapping.Session = session;
                }
            }
        }
    }
}
