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
        public ContainerFactory(ISqlBuildLog sqlBuildLog)
        {
            this.builder = new ContainerBuilder();

            this.builder.RegisterInstance(sqlBuildLog).As<ISqlBuildLog>();

            this.builder.Register(c => new SqlBuilder() { Setup = c.Resolve<SqlBuildSetup>() }).As<ISqlBuilder>();

            this.builder.Register(c => new SqlBuildSetup() { SqlBuildLog = c.Resolve<ISqlBuildLog>() });

            this.builder.Register(c => new ParserFactory() { SqlBuildLog = c.Resolve<ISqlBuildLog>() }).As<IParserFactory>();

            this.builder.Register(c => new BatchExtractor() { ParserFactory = c.Resolve<ParserFactory>() });
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