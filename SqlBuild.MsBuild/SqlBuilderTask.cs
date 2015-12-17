using System;
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

        private SqlBuildMode BuildModeTyped
        {
            get
            {
                SqlBuildMode mode;
                if (!Enum.TryParse(BuildMode, out mode))
                {
                    var validValues = string.Join(", ", Enum.GetNames(typeof(SqlBuildMode)));
                    var errorMessage =
                        string.Format(
                            "Build mode '{0}' not valid for SqlBuild. It must have one of the values: {1}",
                            BuildMode,
                            validValues);

                    throw new SqlBuildException(errorMessage);
                }

                return mode;
            }
        }

        /// <summary>
        /// Gets or sets the build mode.
        /// </summary>
        [Required]
        public string BuildMode { get; set; }

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
        [Required]
        public ITaskItem[] Connections { get; set; }

        /// <summary>
        /// Gets or sets the logins.
        /// </summary>
        [Required]
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
        [Required]
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
                this.EnsureTaskItemArrays();

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

                var log = new TaskLog(this.Log);
                var sqlSetup = new SqlBuildSetup() { SqlBuildLog = log };

                var mapper = new TaskItemMapper() { Log = log };

                this.Log.LogCommandLine(MessageImportance.Low, "Mapping input task items to sql build setup...");
                mapper.MapTo(mapperInput, sqlSetup);

                if (sqlBuildContainer == null)
                {
                    this.Log.LogCommandLine(MessageImportance.Low, "Creating DI container...");
                    var containerFactory = new ContainerFactory(log, sqlSetup);
                    sqlBuildContainer = containerFactory.CreateContainer();
                }

                var sqlBuilder = sqlBuildContainer.Resolve<ISqlBuilder>();

                switch (BuildModeTyped)
                {
                    case SqlBuildMode.Compile:
                        this.Log.LogCommandLine(MessageImportance.Low, "Performing Compile on SqlBuildScripts");
                        sqlBuilder.Compile();
                        break;
                    case SqlBuildMode.CompileAndDeploy:
                        this.Log.LogCommandLine(MessageImportance.Low, "Performing CompileAndDeploy on SqlBuildScripts");
                        sqlBuilder.CompileAndDeploy();
                        break;
                }

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

        private void EnsureTaskItemArrays()
        {
            if (this.Connections == null)
            {
                this.Connections = new ITaskItem[0];
            }
            if (this.GlobalConfigurations == null)
            {
                this.GlobalConfigurations = new ITaskItem[0];
            }
            if (this.Logins == null)
            {
                this.Logins = new ITaskItem[0];
            }
            if (this.ScriptConfigurations == null)
            {
                this.ScriptConfigurations = new ITaskItem[0];
            }
            if (this.ScriptMappings == null)
            {
                this.ScriptMappings = new ITaskItem[0];
            }
            if (this.Scripts == null)
            {
                this.Scripts = new ITaskItem[0];
            }
            if (this.Sessions == null)
            {
                this.Sessions = new ITaskItem[0];
            }
        }
    }
}