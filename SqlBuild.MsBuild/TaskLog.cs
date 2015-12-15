using System.Linq;

using Microsoft.Build.Framework;
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
        public void WriteTrace(string message)
        {
            this.log.LogCommandLine(MessageImportance.Low, message);
        }

        /// <inheritdoc />
        public string WriteError(string error)
        {
            string fullErrorText = string.Format("error: {0}", error);

            this.log.LogError("", "", "", "", 0, 0, 0, 0, fullErrorText);

            this.wroteAnyErrors = true;

            return fullErrorText;
        }


        public string WriteError(string filePath, int lineNumber, int columnNumber, string errorCode, string errorMessage)
        {
            string fullErrorText = string.Format("{0}({1},{2}): error {3}: {4}", 
                filePath,
                lineNumber,
                columnNumber,
                errorCode,
                errorMessage);

            this.log.LogError("", "", "", filePath, lineNumber, columnNumber, 0, 0, errorMessage);

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