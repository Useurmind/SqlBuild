using System.Linq;

using Autofac;

using SqlBuild.Logging;
using SqlBuild.Model;
using SqlBuild.Syntax;

namespace SqlBuild.Container
{
    /// <summary>
    ///     The factory for the container that does the dependency injection.
    /// </summary>
    public class ContainerFactory
    {
        /// <summary>
        ///     The container builder.
        /// </summary>
        private ContainerBuilder builder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContainerFactory" /> class.
        /// </summary>
        public ContainerFactory(ISqlBuildLog sqlBuildLog, SqlBuildSetup setup)
        {
            setup.ConnectReferences();

            this.builder = new ContainerBuilder();

            this.builder.RegisterInstance(sqlBuildLog).As<ISqlBuildLog>();
            this.builder.RegisterInstance(setup);
            this.builder.RegisterInstance(setup.ActiveGlobalConfiguration.Connection);
            this.builder.RegisterInstance(setup.ActiveGlobalConfiguration);

            this.builder.Register(c => new SqlBuilder()
                                           {
                                               Log = c.Resolve<ISqlBuildLog>(),
                                               Setup = c.Resolve<SqlBuildSetup>(),
                                               BatchExtractor = c.Resolve<IBatchExtractor>()
                                           }).As<ISqlBuilder>();

            this.builder.Register(c => new SessionFactory(c.Resolve<SqlConnection>())).As<ISessionFactory>();

            this.builder.Register(c => new ParserFactory(c.Resolve<ISqlBuildLog>(), c.Resolve<SqlConnection>().ServerVersion)).As<IParserFactory>();

            this.builder.Register(c => new BatchExtractor(
                                              parserFactory: c.Resolve<IParserFactory>(),
                                              log: c.Resolve<ISqlBuildLog>()
                                            )).As<IBatchExtractor>();
        }

        /// <summary>
        ///     Creates a new container that resolves components.
        /// </summary>
        /// <returns>The created container.</returns>
        public IContainer CreateContainer()
        {
            return builder.Build();
        }
    }
}