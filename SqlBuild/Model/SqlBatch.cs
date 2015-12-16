using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlBuild.Model
{
    public class SqlBatch
    {
        public string SqlText{ get; set; }

        public int StartLine { get; set; }

        public int StartColumn{ get; set; }
    }
}
