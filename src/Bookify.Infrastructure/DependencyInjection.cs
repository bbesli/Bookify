﻿using Asp.Versioning;
using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Caching;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Authentication;
using Bookify.Infrastructure.Authorization;
using Bookify.Infrastructure.Caching;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Data;
using Bookify.Infrastructure.Email;
using Bookify.Infrastructure.Outbox;
using Bookify.Infrastructure.Repositories;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using Quartz;
using SharedKernel;
using AuthenticationOptions = Bookify.Infrastructure.Authentication.AuthenticationOptions;
using AuthenticationService = Bookify.Infrastructure.Authentication.AuthenticationService;
using IAuthenticationService = Bookify.Application.Abstractions.Authentication.IAuthenticationService;

namespace Bookify.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        services.AddTransient<IEmailService, EmailService>();

        AddPersistence(services, configuration);

        AddCaching(services, configuration);

        AddAuthentication(services, configuration);

        AddAuthorization(services);

        AddHealthChecks(services, configuration);

        AddBackgroundJobs(services, configuration);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database") ??
                                  throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IApartmentRepository, ApartmentRepository>();

        services.AddScoped<IBookingRepository, BookingRepository>();

        services.AddScoped<IReviewRepository, ReviewRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

        services.AddTransient<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpClient) =>
        {
            KeycloakOptions keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
        })
        .AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
        {
            KeycloakOptions keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });

        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();
    }

    private static void AddAuthorization(IServiceCollection services)
    {
        services.AddScoped<AuthorizationService>();

        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Cache") ??
                                  throw new ArgumentNullException(nameof(configuration));

        services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!)
            .AddRedis(configuration.GetConnectionString("Cache")!)
            .AddUrlGroup(new Uri(configuration["KeyCloak:BaseUrl"]!), HttpMethod.Get, "keycloak");
    }

    private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));

        services.AddQuartz();

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.ConfigureOptions<ProcessOutboxMessagesJobSetup>();
    }
}


//public static class DependencyInjection
//{
//    public static IServiceCollection AddInfrastructure(
//        this IServiceCollection services,
//        IConfiguration configuration) =>
//        services
//            .AddServices()
//            .AddDatabase(configuration)
//            .AddCaching(configuration)
//            .AddHealthChecks(configuration);

//    private static IServiceCollection AddServices(this IServiceCollection services)
//    {
//        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

//        return services;
//    }

//    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
//    {
//        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

//        string? connectionString = configuration.GetConnectionString("Database");
//        Ensure.NotNullOrEmpty(connectionString);

//        services.AddSingleton<IDbConnectionFactory>(_ =>
//            new DbConnectionFactory(new NpgsqlDataSourceBuilder(connectionString).Build()));

//        services.AddDbContext<ApplicationDbContext>(
//            options => options
//                .UseNpgsql(connectionString, npgsqlOptions =>
//                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
//                .UseSnakeCaseNamingConvention());

//        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

//        services.AddScoped<IUserRepository, UserRepository>();

//        return services;
//    }

//    private static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
//    {
//        string redisConnectionString = configuration.GetConnectionString("Cache")!;

//        services.AddStackExchangeRedisCache(options => options.Configuration = redisConnectionString);

//        services.AddSingleton<ICacheService, CacheService>();

//        return services;
//    }

//    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
//    {
//        services
//            .AddHealthChecks()
//            .AddNpgSql(configuration.GetConnectionString("Database")!)
//            .AddRedis(configuration.GetConnectionString("Cache")!);

//        return services;
//    }
//}