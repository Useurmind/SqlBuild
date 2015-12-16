using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;

namespace SqlBuild.Database
{
    public static class ConnectionStringFactory
    {
        public static string CreateConnectionString(SqlConnection connection, SqlLogin login)
        {
            var connStringBuilder = new StringBuilder();

            connStringBuilder.AppendFormat("Data Source={0}", connection.Server);

            if (!string.IsNullOrEmpty(connection.Database))
            {
                connStringBuilder.Append(";").AppendFormat("Initial Catalog={0}", connection.Database);
            }

            if (!string.IsNullOrEmpty(login.IntegratedSecurity))
            {
                connStringBuilder.Append(";").AppendFormat("Integrated Security=true");
            }
            else
            {
                connStringBuilder.Append(";").AppendFormat("User Id={0}", login.UserName);
                connStringBuilder.Append(";").AppendFormat("Password={0}", login.Password);
            }

            return connStringBuilder.ToString();
        }
    }
}
