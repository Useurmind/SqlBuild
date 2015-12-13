using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Model
{
    public class SqlScript : IModel
    {
        public string ItemSpec { get; set; }

        public string Identity { get; set; }

        public string GetSqlText()
        {
            return File.ReadAllText(this.Identity);
        }
    }
}
