using System.Linq;

using Autofac;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using SqlBuild.Container;
using SqlBuild.Logging;
using SqlBuild.Model;
using SqlBuild.Utility;

namespace SqlBuild.MsBuild
{
    /// <summary>
    /// The task that executes sql build.
    /// </summary>
    public class SqlBuilderTask : Task
    {
        /// <summary>
        /// The sql build DI container.
        /// </summary>
        private IContainer sqlBuildContainer;

        /// <summary>
        /// Gets or sets the active global configuration key.
        /// </summary>
        public string ActiveGlobalConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the global configurations.
        /// </summary>
        public ITaskItem[] GlobalConfigurations { get; set; }

        /// <summary>
        /// Gets or sets the script configurations.
        /// </summary>
        public ITaskItem[] ScriptConfigurations { get; set; }

        /// <summary>
        /// Gets or sets the connections.
        /// </summary>
        public ITaskItem[] Connections { get; set; }

        /// <summary>
        /// Gets or sets the logins.
        /// </summary>
        public ITaskItem[] Logins { get; set; }

        /// <summary>
        /// Gets or sets the sessions.
        /// </summary>
        public ITaskItem[] Sessions { get; set; }

        /// <summary>
        /// Gets or sets the script mappings.
        /// </summary>
        public ITaskItem[] ScriptMappings { get; set; }

        /// <summary>
        /// Gets or sets the scripts.
        /// </summary>
        public ITaskItem[] Scripts { get; set; }

        /// <summary>
        /// Set the DI container to resolve dependencies (only for testing).
        /// </summary>
        /// <param name="container">
        /// The DI container.
        /// </param>
        public void SetContainer(IContainer container)
        {
            sqlBuildContainer = container;
        }

        /// <summary>
        /// Execute the task.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Execute()
        {
            try
            {
                if (sqlBuildContainer == null)
                {
                    this.Log.LogCommandLine(MessageImportance.Low, "Creating DI container...");
                    var containerFactory = new ContainerFactory(new TaskLog(this.Log));
                    sqlBuildContainer = containerFactory.CreateContainer();
                }

                var mapperInput = new TaskItemMapperInput()
                                      {
                                          Connections = Connections,
                                          GlobalConfigurations = GlobalConfigurations,
                                          Logins = Logins,
                                          ScriptConfigurations = ScriptConfigurations,
                                          ScriptMappings = ScriptMappings,
                                          Scripts = Scripts,
                                          Sessions = Sessions
                                      };

                var sqlSetup = sqlBuildContainer.Resolve<SqlBuildSetup>();

                var mapper = new TaskItemMapper() { Log = sqlBuildContainer.Resolve<ISqlBuildLog>() };

                this.Log.LogCommandLine(MessageImportance.Low, "Creating DI container...");
                mapper.MapTo(mapperInput, sqlSetup);

                var sqlBuilder = sqlBuildContainer.Resolve<ISqlBuilder>();

                sqlBuilder.Execute();
            }
            catch (SqlBuildException sqlBuildEx)
            {
                this.Log.LogError(sqlBuildEx.Message);
                return false;
            }
            finally
            {
                sqlBuildContainer.Dispose();
            }

            return true;
        }
    }
}