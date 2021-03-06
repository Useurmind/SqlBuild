﻿using System.Linq;

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
        /// <returns>
        /// The <see cref="TSqlParser"/>.
        /// </returns>
        TSqlParser CreateParser();
    }

    /// <summary>
    /// The parser factory.
    /// </summary>
    public class ParserFactory : IParserFactory
    {
        private ServerVersion version;

        /// <summary>
        /// Gets or sets the error log.
        /// </summary>
        private ISqlBuildLog log;

        public ParserFactory(ISqlBuildLog log, ServerVersion serverVersion)
        {
            this.log = log;
            version = serverVersion;
        }

        /// <inheritdoc />
        public TSqlParser CreateParser()
        {
            switch (version)
            {
                case ServerVersion.None:
                    this.log.WriteSqlServerVersionNotSupported(version);
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
                    this.log.WriteSqlServerVersionNotSupported(version);
                    return null;
            }
        }
    }
}