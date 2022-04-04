using Insurance.Api.Data;
using Insurance.Api.Service;
using Insurance.Api.Service.Interfaces;

namespace Insurance.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureExternalApi(this IServiceCollection services, IConfiguration config)
        {
            var ApiUrl = config.GetSection("ProductApi").Value;

            services.AddHttpClient("ProductApi", c =>
            {
                c.BaseAddress = new Uri(ApiUrl);
            });
        }
        public static void ConfigureServicesWrapper(this IServiceCollection services)
        {
            services.AddScoped<IInsuranceProduct, InsuranceProduct>()
                   .AddScoped<IEntity<Product>, ProductRepository>()
                   .AddScoped<IEntity<ProductType>, ProductTypeRepository>()
                   .AddScoped<IInsuranceExtraCost, InsuranceExtraCost>()
                   .AddScoped<ISurcharges, Surcharges>()
                   .AddScoped<IInsuranceExtraCost, InsuranceSurcharge>();
        }
    }
}
