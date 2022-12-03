using Acme.Todoist.Application.Services;
using Acme.Todoist.Core.Repositories;
using Acme.Todoist.Data;
using Acme.Todoist.Infrastructure.Utils;
using Acme.Todoist.IoC.Modules;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Acme.Todoist.IoC
{
    public static class InjectorBootstrapper
    {
        /// <summary>
        /// Makes use of Autofac to set up the service provider, bringing together Autofac registrations and ASP.NET Core framework registrations.
        /// </summary>
        public static void Inject(ContainerBuilder builder, IConfiguration configuration, IServiceCollection services = null)
        {
            var assemblies = new[]
            {
                Assembly.Load("Acme.Todoist.Api"),
                typeof(IUnitOfWork).Assembly,
                typeof(AppServiceBase).Assembly,
                typeof(UnitOfWork).Assembly,
                typeof(IDateTimeProvider).Assembly,
            };

            builder
                .RegisterAssemblyTypes(assemblies)
                // Exclude all types that extends from INotificationHandler by avoid duplicate registers with the extension 'AddMediatR' present in 'Startup'.
                //.Where(type => !type.IsAssignableToGenericType(typeof(INotificationHandler<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterModule(new DataModule());
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new ApplicationModule());
        }
    }
}
