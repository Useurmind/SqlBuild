using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Model
{
    public class SqlScript
    {
        public string Identity { get; set; }

        public SqlLogin Login { get; set; }

        public SqlConnection Connection { get; set; }
    }
}
