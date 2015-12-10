using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Model
{
    public class SqlScript : KeyedModel
    {
        public string Identity { get; set; }

        public SqlSession Session { get; set; }

        public SqlScriptConfiguration Configuration { get; set; }

        public string GetSqlText()
        {
            return File.ReadAllText(this.Identity);
        }
    }
}
