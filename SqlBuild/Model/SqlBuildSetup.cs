using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Model
{
    public class SqlBuildSetup
    {
        public string ActiveGlobalConfiguration { get; set; }

        public IDictionary<string, SqlConnection> Connections { get; private set; }
        public IDictionary<string, SqlGlobalConfiguration> GlobalConfigurations { get; private set; }
        public IDictionary<string, SqlScriptConfiguration> ScriptConfigurations { get; private set; }
        public IDictionary<string, SqlLogin> Logins { get; private set; }
        public IDictionary<string, SqlSession> Sessions { get; private set; }
        public IDictionary<string, SqlScriptMapping> ScriptMappings { get; private set; }
        public IList<SqlScript> Scripts { get; private set; }

        public SqlBuildSetup()
        {
            ActiveGlobalConfiguration = Constants.DefaultKey;

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
                                                       LoginKey= Constants.DefaultKey,
                                                   });
            ScriptMappings.Add(Constants.DefaultKey, new SqlScriptMapping()
                                                         {
                                                             Key = Constants.DefaultKey,
                                                             SessionKey = Constants.DefaultKey,
                                                             ConfigurationKey = Constants.DefaultKey
                                                         });
        }

        public void ConnectGraph()
        {

        }
    }
}
