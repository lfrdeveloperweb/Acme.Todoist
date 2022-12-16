using Acme.Todoist.Application.Services;
using Autofac;

namespace Acme.Todoist.IoC.Modules
{
    public sealed class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountAppService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProjectAppService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TodoAppService>()
                .InstancePerLifetimeScope();

            // builder.Register(_ => _configuration.GetSection("orderAttachmentSettings").Get<OrderAttachmentSettings>());
        }
    }
}
