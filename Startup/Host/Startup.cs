using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (ctx, next) =>
            {
                var kind = ctx.Request.Headers["X-KIND"].FirstOrDefault();

                if (kind != "jit" && kind != "r2r" && kind != "aot")
                {
                    throw new ArgumentException("unexpected kind");
                }

                var proxyWatch = Stopwatch.StartNew();

                using var process = new CaptiveProcess($"{Environment.CurrentDirectory}/App/out/{kind}/App", "--urls http://localhost:5004");
                using var client = new HttpClient();
                
                var watch = Stopwatch.StartNew();
                var timeout = TimeSpan.FromSeconds(10);
                while (true)
                {
                    try
                    {
                        using var req = new HttpRequestMessage
                        {
                            Method = HttpMethod.Get,
                            RequestUri = new Uri($"http://localhost:5004{ctx.Request.GetEncodedPathAndQuery()}")
                        };

                        using var res = await client.SendAsync(req);

                        proxyWatch.Stop();
                        ctx.Response.Headers["X-TAKEN"] = proxyWatch.ElapsedMilliseconds.ToString();

                        ctx.Response.StatusCode = (int) res.StatusCode;
                        await res.Content.CopyToAsync(ctx.Response.Body);
                        break;
                    }
                    catch
                    {
                        if (watch.Elapsed > timeout)
                        {
                            throw;
                        }
                    }
                }
            });
        }
    }
}
