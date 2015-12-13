using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Model
{
    public class SqlScriptMapping : KeyedModel
    {
        /// <summary>
        /// Gets or sets pattern that determines the scripts to which this mapping is applied.
        /// The script patterns is applied to the property <see cref="SqlScript.Identity"/>.
        /// All matching scripts are associated with this mapping.
        /// </summary>
        public string ScriptPattern { get; set; }

        public string SessionKey { get; set; }

        public string ConfigurationKey { get; set; }

        public SqlSession Session { get; set; }

        public SqlScriptConfiguration Configuration { get; set; }
    }
}
