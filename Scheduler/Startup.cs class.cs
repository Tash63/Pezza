using Common.Behaviour;
using Hangfire;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Scheduler.Jobs;
using System.Reflection;
using System.Text.Json.Serialization;

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
        });

        services.AddDbContext<DatabaseContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString())
        );
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });
        services.AddHangfire(config =>
            config
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseInMemoryStorage());
        services.AddHangfireServer();
        services.AddResponseCompression();
        services.AddScoped<IOrderCompleteJob, OrderCompleteJob>();
    }
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pezza API V1"));
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseResponseCompression();
        app.UseHangfireDashboard();
        app.UseMiddleware(typeof(ExceptionHandlerMiddleware));
        app.UseEndpoints(endpoints => endpoints.MapControllers());
        app.UseAuthorization();

        var jobOptions = new RecurringJobOptions()
        {
            TimeZone = TimeZoneInfo.Local
        };
        RecurringJob.AddOrUpdate<IOrderCompleteJob>("SendNotificationAsync", x => x.SendNotificationAsync(), "0 * * ? * *");
        app.Run();
    }
}