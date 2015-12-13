using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Model
{
    public class SqlLogin : KeyedModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IntegratedSecurity { get; set; }

        public bool UseIntegratedSecurity
        {
            get
            {
                return !string.IsNullOrEmpty(IntegratedSecurity);
            }
        }
    }
}
