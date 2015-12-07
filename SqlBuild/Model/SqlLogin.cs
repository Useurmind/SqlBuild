using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Model
{
    public class SqlLogin
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public bool IntegratedSecurity { get; private set; }

        public SqlLogin()
        {
            this.SetIntegratedSecurity();
        }

        public void SetUsernamePassword(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("Provided username must not be empty");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Provided password must not be empty");
            }

            UserName = username;
            Password = password;
            IntegratedSecurity = false;
        }

        public void SetIntegratedSecurity()
        {
            IntegratedSecurity = true;
            UserName = null;
            Password = null;
        }
    }
}
