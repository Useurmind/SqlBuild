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
    }
}