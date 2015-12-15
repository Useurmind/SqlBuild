using System;
using System.Reflection;

using SqlBuild.Model;
using System.Linq;

using SqlBuild.Utility;

namespace SqlBuild.Logging
{
    /// <summary>
    /// Contains convenience functions to write special error messages.
    /// </summary>
    public static class ErrorLogExtensions
    {
        public static void WriteTraceFormat(this ISqlBuildLog sqlBuildLog, string message, params object[] parameters)
        {
            sqlBuildLog.WriteTrace(string.Format(message, parameters));
        }

        /// <summary>
        /// Writes the error message and inserts the given format parameters into it using <see cref="string.Format(string,object)"/>.
        /// </summary>
        /// <param name="sqlBuildLog">The SQL build log.</param>
        /// <param name="error">The error.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The full error message that was logged.</returns>
        public static string WriteErrorFormat(this ISqlBuildLog sqlBuildLog, string error, params object[] parameters)
        {
            return sqlBuildLog.WriteError(string.Format(error, parameters));
        }

        public static void WriteReferencedElementNotFound<TParent, TReference>(this ISqlBuildLog sqlBuildLog, string parentKey, string referenceKey)
        {
            sqlBuildLog.WriteErrorFormat(Errors.ReferencedElementNotFound, typeof(TParent).Name, parentKey,
                typeof(TReference).Name, referenceKey);
        }

        public static void WriteSqlServerVersionNotSupported(this ISqlBuildLog sqlBuildLog, ServerVersion version)
        {
            sqlBuildLog.WriteErrorFormat(Errors.SqlServerVersionNotSupported, version);
        }

        public static void WriteSqlServerVersionRequired(this ISqlBuildLog sqlBuildLog, string sqlConnectionKey)
        {
            sqlBuildLog.WriteErrorFormat(Errors.SqlServerVersionRequired, sqlConnectionKey);
        }

        public static void WriteCouldNotParsePropertyValue<TModel>(
            this ISqlBuildLog sqlBuildLog,
            string value,
            PropertyInfo propertyInfo)
        {
            sqlBuildLog.WriteErrorFormat(Errors.CouldNotParsePropertyValue, value, propertyInfo.Name, propertyInfo.PropertyType.Name, typeof(TModel).Name);
        }
    }
}