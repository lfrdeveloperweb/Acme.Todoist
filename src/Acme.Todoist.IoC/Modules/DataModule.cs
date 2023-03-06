using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Application.Services;
using Acme.Todoist.Data.Contexts;
using Acme.Todoist.Data.Factories;
using Acme.Todoist.Infrastructure.Data;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

namespace Acme.Todoist.IoC.Modules
{
    public sealed class DataModule : Module
    {
        private readonly IConfiguration _configuration;

        public DataModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new AcmeDatabaseConnectionFactory(_configuration.GetConnectionString("defaultConnection")))
                .As<IDatabaseConnectionFactory>()
                .SingleInstance();

            //_services.AddDbContextPool<MainContext>(options => options
            //    .UseNpgsql(_configuration.GetConnectionString("defaultConnection"))
            //    .ConfigureWarnings(c => c.Log((RelationalEventId.CommandExecuting, LogLevel.Debug))));

            builder.Register(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<MainContext>();
                optionsBuilder
                    .UseNpgsql(_configuration.GetConnectionString("defaultConnection"), options => options.EnableRetryOnFailure(3))
                    .UseSnakeCaseNamingConvention()

                    // https://www.milanjovanovic.tech/blog/entity-framework-query-splitting
                    //.UseNpgsql(_configuration.GetConnectionString("defaultConnection"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))

                    .ConfigureWarnings(c => c.Log((RelationalEventId.CommandExecuting, LogLevel.Debug)))
                    .LogTo(Console.WriteLine, new [] { DbLoggerCategory.Database.Command.Name })
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();

                return new MainContext(optionsBuilder.Options, provider.Resolve<IIdentityService>(), provider.Resolve<ISystemClock>());
            })
            .InstancePerLifetimeScope();

            builder.Register(provider => provider.Resolve<MainContext>())
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
        }
    }
}
