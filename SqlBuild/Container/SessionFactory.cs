using SqlBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Container
{
    public interface ISessionFactory
    {
        SqlSession GetSession(SqlLogin login);
    }

    public class SessionFactory : ISessionFactory
    {
        private SqlConnection connection;

        private IDictionary<SqlLogin, SqlSession> sessions; 

        public SessionFactory(SqlConnection connection)
        {
            this.connection = connection;
            this.sessions = new Dictionary<SqlLogin, SqlSession>();
        }

        public SqlSession GetSession(SqlLogin login)
        {
            SqlSession session = null;
            if (!sessions.TryGetValue(login, out session))
            {
                session = new SqlSession(login, connection);
                sessions.Add(login, session);
            }
            return session;
        }
    }
}
