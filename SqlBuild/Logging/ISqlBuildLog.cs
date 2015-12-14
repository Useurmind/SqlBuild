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
    }
}