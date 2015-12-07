using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Deployment;
using SqlBuild.Model;

using Xunit;

namespace SqlBuild.Test.Deployment
{
    public class ConnectionStringFactoryTest
    {
        private ConnectionStringFactory factory;

        public ConnectionStringFactoryTest()
        {
            factory = new ConnectionStringFactory();
        }

        [Fact]
        public void server_database_integratedsecurity()
        {
            var expectedConnectionString = "Data Source=server;Initial Catalog=database;Integrated Security=true";
            var connection = new SqlConnection("server")
                                 {
                                     Database = "database"
                                 };
            var login = new SqlLogin();

            var connectionString = factory.CreateConnectionString(connection, login);

            Assert.Equal(expectedConnectionString, connectionString);
        }

        [Fact]
        public void server_nodatabase_integratedsecurity()
        {
            var expectedConnectionString = "Data Source=server;Integrated Security=true";
            var connection = new SqlConnection("server");
            var login = new SqlLogin();

            var connectionString = factory.CreateConnectionString(connection, login);

            Assert.Equal(expectedConnectionString, connectionString);
        }

        [Fact]
        public void server_database_user()
        {
            var expectedConnectionString = "Data Source=server;Initial Catalog=database;User Id=user;Password=password";
            var connection = new SqlConnection("server")
            {
                Database = "database"
            };
            var login = new SqlLogin();
            login.SetUsernamePassword("user", "password");

            var connectionString = factory.CreateConnectionString(connection, login);

            Assert.Equal(expectedConnectionString, connectionString);
        }

        [Fact]
        public void server_nodatabase_user()
        {
            var expectedConnectionString = "Data Source=server;User Id=user;Password=password";
            var connection = new SqlConnection("server");
            var login = new SqlLogin();
            login.SetUsernamePassword("user", "password");

            var connectionString = factory.CreateConnectionString(connection, login);

            Assert.Equal(expectedConnectionString, connectionString);
        }
    }
}
