using Microsoft.AspNetCore.Http.Features;
using OracaoApp.Utils.Attributes;
using System.Reflection;

namespace OracaoApp.API.Extensions;

public static class AppBuilderExtensions
{
    private static List<Type>? GetClassesWithAttributes<T>(Assembly assembly) where T : Attribute => assembly.GetTypes().Where(type => type.GetCustomAttribute(typeof(T)) != null).ToList();

    private static void AddCustomServicesForAssembly(this WebApplicationBuilder builder, Assembly assembly)
    {
        var scopedServices = GetClassesWithAttributes<ScopedServiceAttribute>(assembly);
        scopedServices?.ForEach(s => builder.Services.AddScoped(s));

        var transcientServices = GetClassesWithAttributes<TranscientServiceAttribute>(assembly);
        transcientServices?.ForEach(s => builder.Services.AddTransient(s));

        var singletonServices = GetClassesWithAttributes<SingletonServiceAttribute>(assembly);
        singletonServices?.ForEach(s => builder.Services.AddSingleton(s));
    }

    private static WebApplicationBuilder AddCustomCors(this WebApplicationBuilder builder)
    {
        var corsArray = builder.Configuration.GetRequiredValue("CorsOrigins").Split(',', StringSplitOptions.RemoveEmptyEntries);
        builder.Services
            .AddCors(action => action
                .AddDefaultPolicy(policy => policy
                    .WithOrigins(corsArray)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()));
        return builder;
    }

    private static WebApplicationBuilder AddCustomDbContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetRequiredValue("ConnectionString");

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
        // Prevent error: Cannot write DateTime with Kind=UTC to PostgreSQL type 'timestamp without time zone'
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return builder;
    }

    private static WebApplicationBuilder AddCustomOpenApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{typeof(Program).Assembly.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return builder;
    }

    private static WebApplicationBuilder AddCustomAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication().AddJwtBearer(options => builder.Configuration.Bind("JwtBearer", options));
        return builder;
    }

    private static WebApplicationBuilder AddScopedServices(this WebApplicationBuilder builder)
    {

        var apiAssembly = Assembly.Load("OracaoApp.API") ?? throw new ApplicationException("Assembly OracaoApp.API not found");
        builder.AddCustomServicesForAssembly(apiAssembly);

        var dataAssembly = Assembly.Load("OracaoApp.Data") ?? throw new ApplicationException("Assembly OracaoApp.Data not found");
        builder.AddCustomServicesForAssembly(dataAssembly);

        var utilsAssembly = Assembly.Load("OracaoApp.Utils") ?? throw new ApplicationException("Assembly OracaoApp.SRC.Utils not found");
        builder.AddCustomServicesForAssembly(utilsAssembly);

        return builder;
    }

    public static WebApplicationBuilder AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();
        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 500 * 1024 * 1024;
        });

        return builder
            .AddCustomCors()
            .AddCustomDbContext()
            .AddCustomOpenApi()
            .AddCustomAuthentication()
            .AddScopedServices();
    }


}
