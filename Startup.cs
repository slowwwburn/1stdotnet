using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddOcelot().AddDelegatingHandler<CustomDelegatingHandler>();
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Use your custom middleware here, if needed
        app.UseMiddleware<ModifyDownstreamPathMiddleware>();
        app.UseMiddleware<LogDownstreamPathMiddleware>();
        app.UseRouting();
        // Use Ocelot's middleware
        await app.UseOcelot();
        // app.UseMiddleware<LogDownstreamPathMiddleware>();
    }
}
