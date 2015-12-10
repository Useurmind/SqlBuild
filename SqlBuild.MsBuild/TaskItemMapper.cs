using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Build.Framework;

using SqlBuild.Model;
using SqlBuild.Utility;

namespace SqlBuild.MsBuild
{
    public class TaskItemMapper
    {
        public void MapTo(TaskItemMapperInput input, SqlBuildSetup setup)
        {
            foreach (var loginItem in input.Logins)
            {
                var login = setup.Logins.GetOrCreate(loginItem.ItemSpec);

                SetIfNotEmpty(loginItem, login, "UserName");
                SetIfNotEmpty(loginItem, login, "Password");
                SetIfNotEmpty(loginItem, login, "IntegratedSecurity");
            }

            foreach (var connectionItem in input.Connections)
            {
                var connection = setup.Connections.GetOrCreate(connectionItem.ItemSpec);

                SetIfNotEmpty(connectionItem, connection, "Server");
                SetIfNotEmpty(connectionItem, connection, "Database");
                SetIfNotEmpty(connectionItem, connection, "Schema");
            }

            foreach (var globalConfigItem in input.GlobalConfigurations)
            {
                var globalConfig = setup.GlobalConfigurations.GetOrCreate(globalConfigItem.ItemSpec);
                //SetIfNotEmpty(globalConfigItem, globalConfig, "");
            }

            foreach (var scriptConfigItem in input.ScriptConfigurations)
            {
                var scriptConfig = setup.ScriptConfigurations.GetOrCreate(scriptConfigItem.ItemSpec);
            }

            foreach (var scriptItem in input.Scripts)
            {
                var script = setup.Scripts.GetOrCreate(scriptItem.ItemSpec);

                SetIfNotEmpty(scriptItem, script, "Identity");
                SetIfNotEmpty(scriptItem, script, "Session");
                SetIfNotEmpty(scriptItem, script, "Config");
            }

            foreach (var sessionItem in input.Sessions)
            {
                var session = setup.Sessions.GetOrCreate(sessionItem.ItemSpec);

                SetIfNotEmpty(sessionItem, session, "Login");
                SetIfNotEmpty(sessionItem, session, "Connection");
            }
        }

        private static void SetIfNotEmpty(ITaskItem item, KeyedModel model, string propertyName)
        {
            var metaData = item.GetMetadata(propertyName);
            if (!string.IsNullOrEmpty(metaData))
            {
                model.GetType().GetProperty(propertyName).SetValue(model, metaData, null);
            }
        }
    }
}
