﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Model
{
    public class SqlGlobalConfiguration : KeyedModel
    {
        /// <summary>
        /// Gets or sets the name of the project which must be unique if several 
        /// projects are installed into the same database.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the session key that is used to manage the SQL build information
        /// inside the database (must be able to create, query and update tables and schemas).
        /// </summary>
        public string SqlBuildInfoSessionKey { get; set; }

        /// <summary>
        /// Gets or sets the SQL build information session.
        /// <see cref="SqlBuildInfoSessionKey"/>.
        /// </summary>
        public SqlSession SqlBuildInfoSession { get; set; }
    }
}
