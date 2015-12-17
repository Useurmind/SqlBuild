using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Database;
using SqlBuild.Model;
using SqlBuild.Syntax;

using Xunit;

using SqlConnection = System.Data.SqlClient.SqlConnection;

namespace SqlBuild.Test.Data
{
    public class SqlBuildRepositoryTest : IDisposable
    {
        private SqlBuildRepository repository;
        private SqlConnection connection;
        private SqlSession session;

        public SqlBuildRepositoryTest()
        {
            session = new SqlSession(
                                  login: new SqlLogin() { UserName = "JochenGruen", Password = "admin" },
                                  connection:
                                      new SqlBuild.Model.SqlConnection()
                                          {
                                              Server = "192.168.126.50",
                                              Database = "SqlBuildTest"
                                          }
                              );

            connection = session.OpenDBConnection();

            repository = new SqlBuildRepository(
                session: session,
                batchExtractor: new BatchExtractor(
                    log: new ExceptionSqlBuildLog(),
                    parserFactory: new ParserFactory(new ExceptionSqlBuildLog(), ServerVersion.SqlServer2014)
                    ),
                scriptExecutor: new ScriptExecutor(session));
        }

        [Fact]
        [Trait("integration", "")]
        public void if_schema_is_created_DoesSqlBuildSchemaExist_returns_true()
        {
            this.CreateSchema();

            var exists = repository.DoesSqlBuildSchemaExist();

            Assert.True(exists);
        }

        [Fact]
        [Trait("integration", "")]
        public void if_schema_is_dropped_DoesSqlBuildSchemaExist_returns_false()
        {
            this.DropSchema();

            var exists = repository.DoesSqlBuildSchemaExist();

            Assert.False(exists);
        }

        private void DropSchema()
        {
            try
            {
                var command = connection.CreateCommand();

                command.CommandText = string.Format("drop schema {0}", Constants.SqlBuildSchemaName);
                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();
            }
            catch
            {
                // ignore any drop failures
            }
        }

        private void CreateSchema()
        {
            try
            {
                var command = connection.CreateCommand();

                command.CommandText = string.Format("create schema {0}", Constants.SqlBuildSchemaName);
                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();
            }
            catch
            {
                // ignore any create failures
            }
        }

        public void Dispose()
        {
            this.DropSchema();

            connection.Close();
            connection.Dispose();
        }
    }
}
