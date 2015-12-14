using System.Linq;

using Microsoft.Build.Utilities;

using SqlBuild.Logging;

namespace SqlBuild.MsBuild
{
    /// <summary>
    /// An implementation of <see cref="ISqlBuildLog"/> to enable logging in msbuild tasks.
    /// </summary>
    public class TaskLog : ISqlBuildLog
    {
        private bool wroteAnyErrors = false;

        /// <summary>
        /// The msbuild log.
        /// </summary>
        private TaskLoggingHelper log;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskLog"/> class.
        /// </summary>
        /// <param name="log">
        /// The msbuild log.
        /// </param>
        public TaskLog(TaskLoggingHelper log)
        {
            this.log = log;
        }

        /// <inheritdoc />
        public string WriteError(string error)
        {
            string fullErrorText = string.Format("error: {0}", error);

            log.LogError(fullErrorText);

            this.wroteAnyErrors = true;

            return fullErrorText;
        }

        /// <inheritdoc />
        public bool WroteAnyErrors
        {
            get
            {
                return this.wroteAnyErrors;
            }
        }
    }
}