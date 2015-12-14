using System.Linq;

using Microsoft.SqlServer.TransactSql.ScriptDom;

using SqlBuild.Logging;
using SqlBuild.Model;

namespace SqlBuild.Syntax
{
    /// <summary>
    /// The ParserFactory interface.
    /// </summary>
    public interface IParserFactory
    {
        /// <summary>
        /// Create a parser for TSQL.
        /// </summary>
        /// <param name="version">
        /// The version of the sql server.
        /// </param>
        /// <returns>
        /// The <see cref="TSqlParser"/>.
        /// </returns>
        TSqlParser CreateParser(ServerVersion version);
    }

    /// <summary>
    /// The parser factory.
    /// </summary>
    public class ParserFactory : IParserFactory
    {
        /// <summary>
        /// Gets or sets the error log.
        /// </summary>
        public ISqlBuildLog SqlBuildLog { get; set; }

        /// <inheritdoc />
        public TSqlParser CreateParser(ServerVersion version)
        {
            switch (version)
            {
                case ServerVersion.None:
                    this.SqlBuildLog.WriteSqlServerVersionNotSupported(version);
                    return null;
                case ServerVersion.SqlServer2008:
                    return new TSql100Parser(false);
                    break;
                case ServerVersion.SqlServer2012:
                    return new TSql110Parser(false);
                    break;
                case ServerVersion.SqlServer2014:
                    return new TSql120Parser(false);
                    break;
                default:
                    this.SqlBuildLog.WriteSqlServerVersionNotSupported(version);
                    return null;
            }
        }
    }
}