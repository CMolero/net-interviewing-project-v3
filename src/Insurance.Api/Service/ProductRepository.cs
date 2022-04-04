using Insurance.Api.Data;
using Insurance.Api.Service.Interfaces;
using Newtonsoft.Json;

namespace Insurance.Api.Service
{
    public class ProductRepository : IEntity<Product>
    {
        public readonly IHttpClientFactory _httpClientFactory;
        private readonly IEntity<ProductType> _productTypeService;
        private readonly ISurcharges _surcharges;

        public ProductRepository(IHttpClientFactory httpClientFactory, IEntity<ProductType> productTypeService, ISurcharges surcharges)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _productTypeService = productTypeService ?? throw new ArgumentNullException(nameof(productTypeService));
            _surcharges = surcharges ?? throw new ArgumentNullException(nameof(surcharges));
        }

        public async Task<Product> GetById(int id)
        {
            Product product = new Product();

            var client = _httpClientFactory.CreateClient("ProductApi");
            var response = await client.GetAsync(string.Format("/products/{0:G}", id));
            if (response.IsSuccessStatusCode)
            {
                product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());
                if (product != null)
                {
                    product.ProductType = await _productTypeService.GetById(product.ProductTypeId);
                    product.Surcharge = _surcharges.GetByProductTypeId(product.ProductTypeId);
                }
                else
                {
                    throw new Exception("Product doesn't exist");
                }

            }
            return product;
        }
    }
}
