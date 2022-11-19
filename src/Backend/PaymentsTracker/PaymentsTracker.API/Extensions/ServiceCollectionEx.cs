using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PaymentsTracker.Common.Mapping;
using PaymentsTracker.Common.Options;
using PaymentsTracker.Models.Data;
using PaymentsTracker.Repositories.Business;
using PaymentsTracker.Repositories.Interfaces;
using PaymentsTracker.Services.Business;
using PaymentsTracker.Services.Interfaces;

namespace PaymentsTracker.API.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        services.AddTransient<IAuthService, AuthService>();
        services.AddAutoMapper(typeof(UserMapping).Assembly);
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider(true);
        var authenticationKey = scope.GetRequiredService<IOptions<JwtOptions>>().Value.SecretKey;
        services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateActor = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationKey))
                };
            });
        return services;
    }

    public static IServiceCollection AddSwaggerWithBearerAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.DescribeAllParametersInCamelCase();
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        return services;
    }

    public static async Task ApplyPendingMigrations(this IServiceCollection services)
    {
        if (Environment.CurrentDirectory.Contains("dotnet-ef", StringComparison.CurrentCultureIgnoreCase))
            return;
        using var serviceScope = services.BuildServiceProvider().CreateScope();
        var scope = serviceScope.ServiceProvider;
        await using var dbContext = scope.GetRequiredService<AppDbContext>();
        var logger = scope.GetRequiredService<ILogger<Program>>();
        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            logger.LogInformation("Applying pending migrations...");
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Pending migrations applied successfully!");
        }
    }
}