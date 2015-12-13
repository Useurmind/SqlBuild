using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Model
{
    public class SqlSession : KeyedModel
    {
        public string LoginKey { get; set; }
        public string ConnectionKey { get; set; }

        public SqlLogin Login { get; set; }
        public SqlConnection Connection { get; set; }
    }
}
