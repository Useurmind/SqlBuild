using System.Linq;

namespace SqlBuild.Logging
{
    /// <summary>
    ///     The ErrorLog interface.
    /// </summary>
    public interface ISqlBuildLog
    {
        /// <summary>
        ///     Gets a value indicating whether any errors were written to the log.
        /// </summary>
        /// <value>
        ///     <c>true</c> if any errors were written to the log; otherwise, <c>false</c>.
        /// </value>
        bool WroteAnyErrors { get; }

        void WriteTrace(string message);

        /// <summary>
        /// Write an error to the log.
        /// </summary>
        /// <param name="error">
        /// The user readable error text.
        /// </param>
        /// <returns>
        /// The full error message that was created.
        /// </returns>
        string WriteError(string error);

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="columnNumber">The column number.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        string WriteError(string filePath, int lineNumber, int columnNumber, string errorCode, string errorMessage);
    }
}