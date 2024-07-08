using CM.WebApi.Brokers;
using CM.WebApi.Middlewares;
using CM.WebApi.Services;
using CM.WebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CM.WebApi.Extensions;

public static class ConfigurationsExtensions
{
    public static WebApplicationBuilder ConfigureDefaults(this WebApplicationBuilder builder)
    {
        #region Logger

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(LogEventLevel.Information)
            .WriteTo.File("logs/*", LogEventLevel.Information)
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        #endregion

        #region WebHost

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(8080);
        });

        #endregion

        #region Services

        builder.Services.AddScoped<ICandidateManager, CandidateManager>();

        #endregion

        #region DbContext

        builder.Services.AddDbContextPool<AppDbContext>(optionsBuilder =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
            optionsBuilder
                // .UseNpgsql(connectionString,
                //     contextOptionsBuilder =>
                //         contextOptionsBuilder
                //             .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                // )
                .UseSqlite(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        #endregion

        #region AutoMapper

        builder.Services.AddAutoMapper(typeof(Program));

        #endregion

        #region Swagger

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.Configure<SwaggerGenOptions>(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo()
                {
                    Title = "Candidate manager API",
                    Version = "v1.0.0",
                    Contact = new OpenApiContact() { Email = "0605AbMu@gmail.com", Name = "Abdumannon Dev." },
                });

            options.UseInlineDefinitionsForEnums();
        });

        #endregion

        #region Others

        builder.Services.AddControllers();

        builder.Services.AddHealthChecks();
        builder.Services.AddScoped<GlobalExceptionHandler>();

        #endregion

        return builder;
    }

    public static async Task<WebApplication> ConfigureDefaults(this WebApplication app)
    {
        await ForceMigrateDb(app);

        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHealthChecks("/healthy");

        app.UseMiddleware<GlobalExceptionHandler>();


        return app;
    }

    private static async Task ForceMigrateDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

// #if DEBUG
//         await dbContext.Database.EnsureDeletedAsync();
// #endif
        await dbContext.Database.MigrateAsync();
    }
}
