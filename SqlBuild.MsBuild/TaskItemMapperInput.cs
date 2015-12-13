using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Framework;

namespace SqlBuild.MsBuild
{
    public class TaskItemMapperInput
    {
        public IEnumerable<ITaskItem> Connections { get; set; }
        public IEnumerable<ITaskItem> GlobalConfigurations { get; set; }
        public IEnumerable<ITaskItem> ScriptConfigurations { get; set; }
        public IEnumerable<ITaskItem> Logins { get; set; }
        public IEnumerable<ITaskItem> Scripts { get; set; }
        public IEnumerable<ITaskItem> Sessions { get; set; }
        public IEnumerable<ITaskItem> ScriptMappings { get; set; }

        public TaskItemMapperInput()
        {
            Connections = new List<ITaskItem>();
            GlobalConfigurations = new List<ITaskItem>();
            ScriptConfigurations = new List<ITaskItem>();
            Logins = new List<ITaskItem>();
            Scripts = new List<ITaskItem>();
            Sessions = new List<ITaskItem>();
            ScriptMappings = new List<ITaskItem>();
        }
    }
}