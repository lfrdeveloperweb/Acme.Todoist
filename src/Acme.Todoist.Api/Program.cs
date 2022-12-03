using Acme.Todoist.Api.Filters;
using Acme.Todoist.Api.TypeConverters;
using Acme.Todoist.Application;
using Acme.Todoist.IoC;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
    InjectorBootstrapper.Inject(containerBuilder, builder.Configuration, builder.Services));

builder.Services.AddControllers(options =>
{
    TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));
    //TypeDescriptor.AddAttributes(typeof(TimeOnly), new TypeConverterAttribute(typeof(TimeOnlyTypeConverter)));

    // Returns status code 406 to "content types" not accept
    options.RespectBrowserAcceptHeader = true;

    // Global error handler
    options.Filters.Add<ApiExceptionFilter>();

    // Ensure that number of records per page is in allowed limits.
    options.Filters.Add<PagingParametersFilter>();
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ACME - Todoist", Version = "v1" });
});

builder.Services.AddMediatR(AssemblyReference.Assembly);
builder.Services.AddAutoMapper(AssemblyReference.Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
