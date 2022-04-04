using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Insurance.Test.IntegrationTest
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup>, IDisposable where TStartup : class
    {
        public IHost Host;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();
            });
            Host = new HostBuilder()
                 .ConfigureWebHostDefaults(
                      b => b.UseUrls("http://localhost:5002")
                            .UseStartup<ControllerTestStartup>()
                  )
                 .Build();

        }
    }
    public class ControllerTestStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(
                ep =>
                {
                    ep.MapGet(
                        "products/{id:int}",
                        context =>
                        {
                            int productId = int.Parse((string)context.Request.RouteValues["id"]);
                            var product = new
                            {
                                id = productId,
                                name = "Test Product",
                                productTypeId = 2,
                                salesPrice = 750
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                        }
                    );
                    ep.MapGet(
                        "product_types/{id:int}",
                        context =>
                        {
                            int productTypeId = int.Parse((string)context.Request.RouteValues["id"]);
                            var productType = new
                            {
                                id = productTypeId,
                                name = "Laptops",
                                canBeInsured = true
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productType));
                        }
                    );
                }
            );
        }
    }
}


