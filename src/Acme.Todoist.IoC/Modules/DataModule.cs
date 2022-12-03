using Acme.Todoist.Data.Factories;
using Acme.Todoist.Infrastructure.Data;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Acme.Todoist.IoC.Modules
{
    public sealed class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(provider =>
                    new AcmeDatabaseConnectionFactory(provider.Resolve<IConfiguration>().GetConnectionString("defaultConnection")))
                .As<IDatabaseConnectionFactory>()
                .SingleInstance();
        }
    }
}
