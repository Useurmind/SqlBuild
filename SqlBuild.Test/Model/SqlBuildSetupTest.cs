using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;

using Xunit;

namespace SqlBuild.Test.Model
{
    public class SqlBuildSetupTest
    {
        [Fact]
        public void default_models_are_created_and_defaults_are_correct()
        {
            var setup = new SqlBuildSetup();

            Assert.Equal(Constants.DefaultKey, setup.ActiveGlobalConfiguration);

            Assert.Equal(1, setup.GlobalConfigurations.Count);
            Assert.Equal(1, setup.ScriptConfigurations.Count);
            Assert.Equal(1, setup.Connections.Count);
            Assert.Equal(1, setup.Logins.Count);
            Assert.Equal(1, setup.Sessions.Count);
            Assert.Equal(1, setup.ScriptMappings.Count);
            Assert.Equal(0, setup.Scripts.Count);
            

            Assert.Equal(Constants.DefaultKey, setup.GlobalConfigurations.First().Value.Key);
            Assert.Equal(Constants.DefaultKey, setup.ScriptConfigurations.First().Value.Key);
            Assert.Equal(Constants.DefaultKey, setup.Connections.First().Value.Key);
            Assert.Equal(Constants.DefaultKey, setup.Logins.First().Value.Key);
            Assert.Equal(Constants.DefaultKey, setup.Sessions.First().Value.Key);
            Assert.Equal(Constants.DefaultKey, setup.ScriptMappings.First().Value.Key);

            Assert.Equal(Constants.DefaultKey, setup.Sessions.First().Value.LoginKey);
            Assert.Equal(Constants.DefaultKey, setup.Sessions.First().Value.ConnectionKey);
            
            Assert.True(string.IsNullOrEmpty(setup.ScriptMappings.First().Value.ScriptPattern));
            Assert.Equal(Constants.DefaultKey, setup.ScriptMappings.First().Value.SessionKey);
            Assert.Equal(Constants.DefaultKey, setup.ScriptMappings.First().Value.ConfigurationKey);
        }
    }
}
