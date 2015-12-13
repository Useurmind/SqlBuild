using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

                SetIfNotEmpty(loginItem, login, login.GetProperty(x => x.UserName));
                SetIfNotEmpty(loginItem, login, login.GetProperty(x => x.Password));
                SetIfNotEmpty(loginItem, login, login.GetProperty(x => x.IntegratedSecurity));
            }

            foreach (var connectionItem in input.Connections)
            {
                var connection = setup.Connections.GetOrCreate(connectionItem.ItemSpec);

                SetIfNotEmpty(connectionItem, connection, connection.GetProperty(x => x.Server));
                SetIfNotEmpty(connectionItem, connection, connection.GetProperty(x => x.Database));
                SetIfNotEmpty(connectionItem, connection, connection.GetProperty(x => x.Schema));
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

            foreach (var sessionItem in input.Sessions)
            {
                var session = setup.Sessions.GetOrCreate(sessionItem.ItemSpec);

                SetKeyIfNotEmpty(sessionItem, session, session.GetProperty(x => x.LoginKey));
                SetKeyIfNotEmpty(sessionItem, session, session.GetProperty(x => x.ConnectionKey));
            }

            foreach (var scriptItem in input.Scripts)
            {
                var script = new SqlScript() { ItemSpec = scriptItem.ItemSpec };

                SetIfNotEmpty(scriptItem, script, script.GetProperty(x => x.Identity));

                setup.Scripts.Add(script);
            }

            foreach (var scriptMappingItem in input.ScriptMappings)
            {
                var scriptMapping = new SqlScriptMapping() { ScriptPattern = scriptMappingItem.ItemSpec };

                SetKeyIfNotEmpty(scriptMappingItem, scriptMapping, scriptMapping.GetProperty(x => x.SessionKey));
                SetKeyIfNotEmpty(scriptMappingItem, scriptMapping, scriptMapping.GetProperty(x => x.ConfigurationKey));

                setup.ScriptMappings.Add(scriptMapping);
            }
        }

        private static void SetIfNotEmpty<T>(ITaskItem item, T model, PropertyInfo propertyInfo)
            where T : IModel
        {
            SetIfNotEmpty(item, model, model.GetMetadataName(propertyInfo.Name), propertyInfo);
        }

        private static void SetKeyIfNotEmpty<T>(ITaskItem item, T model, PropertyInfo propertyInfo)
            where T : IModel
        {
            SetIfNotEmpty(item, model, propertyInfo);
        }

        private static void SetIfNotEmpty<T>(ITaskItem item, T model, string metaDataKey, PropertyInfo propertyInfo)
        {
            var metaData = item.GetMetadata(metaDataKey);
            if (!string.IsNullOrEmpty(metaData))
            {
                propertyInfo.SetValue(model, metaData, null);
            }
        }
    }
}
