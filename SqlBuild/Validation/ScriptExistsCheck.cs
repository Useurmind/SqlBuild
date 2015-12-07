using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;

namespace SqlBuild.Validation
{
    public class ScriptExistsCheck
    {
        public ScriptExistsCheck(SqlScript script)
        {
            if (!File.Exists(script.Identity))
            {
                throw new Exception(string.Format("The sql script '{0}' does not exist", script.Identity));
            }
        }
    }
}
