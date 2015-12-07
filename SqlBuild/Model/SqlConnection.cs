using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Model
{
    public class SqlConnection
    {
        private string server;

        public string Server
        {
            get
            {
                return this.server;
            }

            set
            {
                this.server = value;

                if (string.IsNullOrEmpty(this.server))
                {
                    throw new Exception("Server for connection must not be empty");
                }
            }
        }

        public string Database { get; set; }

        public string Schema { get; set; }

        public SqlConnection(string server)
        {
            Server = server;
        }
    }
}
