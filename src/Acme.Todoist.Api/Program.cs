using Acme.Todoist.Api.Filters;
using Acme.Todoist.Api.Settings;
using Acme.Todoist.Api.TypeConverters;
using Acme.Todoist.IoC;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Destructurama;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Acme.Todoist.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

IdentityModelEventSource.ShowPII = true;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        //.ReadFrom.Configuration(configuration)
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
        //.WriteTo.Seq("http://localhost:5341")
        .Destructure.UsingAttributes();
        //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
        //.Enrich.FromLogContext()
        //.Enrich.WithProperty(nameof(IWebHostEnvironment.ApplicationName), builder.Environment.ApplicationName)
        //.Enrich.WithProperty(nameof(IWebHostEnvironment.EnvironmentName), builder.Environment.EnvironmentName);

});


builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateOnBuild = true;
});

// For EF Core
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// For Dapper
// AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

builder.Services.AddSingleton<ISystemClock, SystemClock>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((context, containerBuilder) => 
    InjectorBootstrapper.Inject(containerBuilder, builder.Configuration, builder.Services, Assembly.GetExecutingAssembly()));



// TODO: Move to infrastructure.
//builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
//builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

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
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Culture = CultureInfo.GetCultureInfo("pt-BR");
    // options.SerializerSettings.ContractResolver = new CamelCaseAndIgnoreEmptyEnumerablesContractResolver();
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;

    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
    {
        Culture = options.SerializerSettings.Culture,
        ContractResolver = options.SerializerSettings.ContractResolver,
        NullValueHandling = options.SerializerSettings.NullValueHandling,
        DateFormatHandling = options.SerializerSettings.DateFormatHandling,
        ReferenceLoopHandling = options.SerializerSettings.ReferenceLoopHandling,
        ConstructorHandling = options.SerializerSettings.ConstructorHandling,
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddOptions();
builder.Services.AddMemoryCache();

builder.Services.ConfigureOptions<SettingsSetup>();
builder.Services.ConfigureOptions<JwtBearerSettingsSetup>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ACME - Todoist", Version = "v1" });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddMediatR(Acme.Todoist.Application.AssemblyReference.Assembly);
builder.Services.AddAutoMapper(Acme.Todoist.Application.AssemblyReference.Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
