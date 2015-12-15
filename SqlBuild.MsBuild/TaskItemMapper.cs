using System;
using System.Linq;
using System.Reflection;

using Microsoft.Build.Framework;

using SqlBuild.Logging;
using SqlBuild.Model;
using SqlBuild.Utility;

namespace SqlBuild.MsBuild
{
    /// <summary>
    /// This class maps task items to the internal object model.
    /// No references are created during this stage, only object creation and property mapping is done.
    /// If any errors happen during this stage the process is not yet aborted.
    /// Reference mapping is executed before aborting the process.
    /// </summary>
    public class TaskItemMapper
    {
        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        public ISqlBuildLog Log { get; set; }

        /// <summary>
        /// Maps the task items in the input to objects in the setup for sql build.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="setup">The setup.</param>
        public void MapTo(TaskItemMapperInput input, SqlBuildSetup setup)
        {
            Log.WriteTraceFormat("Mapping {0} SqlLogin items", input.Logins.Count());
            foreach (var loginItem in input.Logins)
            {
                var login = setup.Logins.GetOrCreate(loginItem.ItemSpec);

                SetIfNotEmpty(loginItem, login, login.GetProperty(x => x.UserName));
                SetIfNotEmpty(loginItem, login, login.GetProperty(x => x.Password));
                SetIfNotEmpty(loginItem, login, login.GetProperty(x => x.IntegratedSecurity));
            }

            Log.WriteTraceFormat("Mapping {0} SqlConnection items", input.Connections.Count());
            foreach (var connectionItem in input.Connections)
            {
                var connection = setup.Connections.GetOrCreate(connectionItem.ItemSpec);

                SetIfNotEmpty(connectionItem, connection, connection.GetProperty(x => x.Server));
                SetIfNotEmpty(connectionItem, connection, connection.GetProperty(x => x.Database));
                SetIfNotEmpty(connectionItem, connection, connection.GetProperty(x => x.Schema));

                SetEnumIfNotEmpty<SqlConnection, ServerVersion>(connectionItem, connection, connection.GetProperty(x => x.ServerVersion));
            }

            Log.WriteTraceFormat("Mapping {0} SqlGlobalConfiguration items", input.GlobalConfigurations.Count());
            foreach (var globalConfigItem in input.GlobalConfigurations)
            {
                var globalConfig = setup.GlobalConfigurations.GetOrCreate(globalConfigItem.ItemSpec);
            }

            Log.WriteTraceFormat("Mapping {0} SqlScriptConfiguration items", input.ScriptConfigurations.Count());
            foreach (var scriptConfigItem in input.ScriptConfigurations)
            {
                var scriptConfig = setup.ScriptConfigurations.GetOrCreate(scriptConfigItem.ItemSpec);
            }

            Log.WriteTraceFormat("Mapping {0} SqlSession items", input.Sessions.Count());
            foreach (var sessionItem in input.Sessions)
            {
                var session = setup.Sessions.GetOrCreate(sessionItem.ItemSpec);

                SetKeyIfNotEmpty(sessionItem, session, session.GetProperty(x => x.LoginKey));
                SetKeyIfNotEmpty(sessionItem, session, session.GetProperty(x => x.ConnectionKey));
            }

            Log.WriteTraceFormat("Mapping {0} SqlScriptMapping items", input.ScriptMappings.Count());
            foreach (var scriptMappingItem in input.ScriptMappings)
            {
                var scriptMapping = setup.ScriptMappings.GetOrCreate(scriptMappingItem.ItemSpec);

                SetIfNotEmpty(scriptMappingItem, scriptMapping, scriptMapping.GetProperty(x => x.ScriptPattern));
                SetKeyIfNotEmpty(scriptMappingItem, scriptMapping, scriptMapping.GetProperty(x => x.SessionKey));
                SetKeyIfNotEmpty(scriptMappingItem, scriptMapping, scriptMapping.GetProperty(x => x.ConfigurationKey));
            }

            Log.WriteTraceFormat("Mapping {0} SqlBuildScript items", input.Scripts.Count());
            foreach (var scriptItem in input.Scripts)
            {
                var script = new SqlScript() { ItemSpec = scriptItem.ItemSpec };

                SetIfNotEmpty(scriptItem, script, script.GetProperty(x => x.Identity));

                setup.Scripts.Add(script);
            }
        }

        private static void SetIfNotEmpty<T>(ITaskItem item, T model, PropertyInfo propertyInfo) where T : IModel
        {
            SetIfNotEmpty(item, model, model.GetMetadataName(propertyInfo.Name), propertyInfo);
        }

        private static void SetKeyIfNotEmpty<T>(ITaskItem item, T model, PropertyInfo propertyInfo) where T : IModel
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

        private void SetEnumIfNotEmpty<TModel, TEnum>(ITaskItem item, TModel model, PropertyInfo propertyInfo)
            where TModel : IModel where TEnum : struct
        {
            string metaData = item.GetMetadata(model.GetMetadataName(propertyInfo.Name));
            TEnum value;
            if (!Enum.TryParse(metaData, out value))
            {
                Log.WriteCouldNotParsePropertyValue<TModel>(metaData, propertyInfo);
            }
            else
            {
                propertyInfo.SetValue(model, value, null);
            }
        }
    }
}