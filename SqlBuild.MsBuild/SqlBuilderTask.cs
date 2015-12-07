using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using SqlBuild.Model;

namespace SqlBuild.MsBuild
{
    public class SqlBuilderTask : Task
    {
        private ISqlBuilder sqlBuilder;

        private SqlLogin defaultLogin;
        private SqlConnection defaultConnection;

        private SqlBuilderConfiguration configuration;

        public string DefaultServer { get; set; }
        public string DefaultDatabase { get; set; }
        public string DefaultSchema { get; set; }

        public string DefaultUserName { get; set; }
        public string DefaultPassword { get; set; }
        public bool DefaultIntegratedSecurity { get; set; }

        public ITaskItem[] Scripts { get; set; }

        public SqlBuilderTask()
        {
            sqlBuilder = new SqlBuilder();
            Scripts = new ITaskItem[0];
            configuration = new SqlBuilderConfiguration();
        }

        public void SetBuilder(ISqlBuilder builder)
        {
            sqlBuilder = builder;
        }

        public override bool Execute()
        {
            defaultLogin = new SqlLogin()
                               {
                                   UserName = DefaultUserName,
                                   Password = DefaultPassword,
                                   IntegratedSecurity = DefaultIntegratedSecurity
                               };

            defaultConnection = new SqlConnection()
                                    {
                                        Server = DefaultServer,
                                        Database = DefaultDatabase,
                                        Schema = DefaultSchema
                                    };

            AddScripts();

            sqlBuilder.SetConfiguration(configuration);

            return sqlBuilder.Execute();
        }

        private void AddScripts()
        {
            foreach (var scriptItem in Scripts)
            {
                var script = this.GetScript(scriptItem);

                sqlBuilder.AddScript(script);
            }
        }

        private SqlScript GetScript(ITaskItem taskItem)
        {
            var login = GetLogin(taskItem);

            var connection = GetConnection(taskItem);

            var script = new SqlScript()
                             {
                                 Identity = taskItem.GetMetadata("Identity"),
                                 Connection = connection,
                                 Login = login
                             };

            return script;
        }

        private SqlConnection GetConnection(ITaskItem taskItem)
        {
            SqlConnection connection = defaultConnection;

            var server = taskItem.GetMetadata(SqlScriptTaskItemMetaData.Server);
            var database = taskItem.GetMetadata(SqlScriptTaskItemMetaData.Database);
            var schema = taskItem.GetMetadata(SqlScriptTaskItemMetaData.Schema);

            if (!string.IsNullOrEmpty(server))
            {
                connection.Server = server;
            }

            if (!string.IsNullOrEmpty(database))
            {
                connection.Database = database;
            }

            if (!string.IsNullOrEmpty(schema))
            {
                connection.Schema = schema;
            }

            return connection;
        }

        private SqlLogin GetLogin(ITaskItem taskItem)
        {
            SqlLogin login = defaultLogin;

            var userName = taskItem.GetMetadata(SqlScriptTaskItemMetaData.UserName);
            var integratedSecurityString = taskItem.GetMetadata(SqlScriptTaskItemMetaData.IntegratedSecurity);
            if (!string.IsNullOrEmpty(integratedSecurityString) ||
                !string.IsNullOrEmpty(userName))
            {
                login = new SqlLogin()
                            {
                                UserName = taskItem.GetMetadata(SqlScriptTaskItemMetaData.UserName),
                                Password = taskItem.GetMetadata(SqlScriptTaskItemMetaData.Password),
                                IntegratedSecurity = string.IsNullOrEmpty(integratedSecurityString) ? false : bool.Parse(integratedSecurityString)
                            };
            }

            return login;
        }
    }
}
