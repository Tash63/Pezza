﻿using Common.Behaviour;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
public class Startup
{
    public IConfiguration configRoot
    {
        get;
    }
    public Startup(IConfiguration configuration)
    {
        configRoot = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
             .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
             .AddNewtonsoftJson(x => x.SerializerSettings.ContractResolver = new DefaultContractResolver())
             .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        services.AddLazyCache();
        DependencyInjection.AddApplication(services);

        // Swagger Config
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Pezza API",
                Version = "v1"
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Please enter the auth token",
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference=new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        },
                    },
                    []
                }
            });
        });

        services.AddCors(options =>
        {
            options.AddPolicy(
                "CorsPolicy",
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        services.AddDbContext<DatabaseContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString())
        );

        // adds a ploicy for each claim so if an endpoint has the follwoing prlocuy it will check if a user has the claim they say they do
        //TODO: continue with the authrization
        services.AddAuthorization(options =>
        {
            options.AddPolicy("StaffPolicy", policy => policy.RequireClaim("Role", "Staff"));
            options.AddPolicy("CustomerPolicy", policy => policy.RequireClaim("Role", "Customer"));
            options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("Role", "Admin"));
        });

        services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddEntityFrameworkStores<DatabaseContext>();

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });
        services.AddResponseCompression();
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var dbContext = serviceProvider.GetRequiredService<DatabaseContext>();
            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
            dbContext.Dispose();
        }
    }
    public void  Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UseCors("CorsPolicy");
        app.UseOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pezza API V1"));
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseResponseCompression();
        app.UseMiddleware(typeof(ExceptionHandlerMiddleware));
        app.UseEndpoints(endpoints => endpoints.MapControllers());
        app.UseAuthorization();
        app.Run();
    }
}