using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Data.Contexts;
using Acme.Todoist.Data.Factories;
using Acme.Todoist.Infrastructure.Data;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Acme.Todoist.IoC.Modules
{
    public sealed class DataModule : Module
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public DataModule(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var connectionString = _configuration.GetConnectionString("defaultConnection");

            builder.Register(_ => new AcmeDatabaseConnectionFactory(connectionString))
                .As<IDatabaseConnectionFactory>()
                .SingleInstance();

            _services.AddDbContextPool<MainContext>(options => options
                .UseNpgsql(connectionString)
                .ConfigureWarnings(c => c.Log((RelationalEventId.CommandExecuting, LogLevel.Debug))));

            builder.Register(provider => provider.Resolve<MainContext>())
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
        }
    }
}
