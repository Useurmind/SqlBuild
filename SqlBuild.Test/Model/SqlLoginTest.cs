using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;

using Xunit;

namespace SqlBuild.Test.Model
{
    public class SqlLoginTest
    {
        private SqlLogin login;

        public SqlLoginTest()
        {
            login = new SqlLogin();
        }

        [Fact]
        public void default_login_is_integrated_security()
        {
            Assert.True(login.IntegratedSecurity);
            Assert.Null(login.UserName);
            Assert.Null(login.Password);
        }

        [Fact]
        public void set_username_password_sets_integrated_security_to_false()
        {
            var username = "user";
            var password = "pw";

            login.SetUsernamePassword(username, password);

            Assert.False(login.IntegratedSecurity);
            Assert.Equal(username, login.UserName);
            Assert.Equal(password, login.Password);
        }

        [Fact]
        public void set_integrated_security_sets_username_password_null()
        {
            var username = "user";
            var password = "pw";

            login.SetUsernamePassword(username, password);
            login.SetIntegratedSecurity();

            Assert.True(login.IntegratedSecurity);
            Assert.Null(login.UserName);
            Assert.Null(login.Password);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData("", "asfsdgf")]
        [InlineData(null, "asfsdgf")]
        [InlineData("hgfjfghjfgh", "")]
        [InlineData("hgfjfghjfgh", null)]
        public void setting_username_password_to_null_throws(string username, string password)
        {
            Assert.Throws<Exception>(() => { login.SetUsernamePassword(username, password); });
        }
    }
}
