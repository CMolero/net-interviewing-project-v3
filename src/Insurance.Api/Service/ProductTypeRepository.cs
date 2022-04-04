using Insurance.Api.Service.Interfaces;
using Newtonsoft.Json;

namespace Insurance.Api.Service
{
    public class ProductTypeRepository : IEntity<ProductType>
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public ProductTypeRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<ProductType> GetById(int Id)
        {
            ProductType productType;
            var client = _httpClientFactory.CreateClient("ProductApi");
            var response = await client.GetAsync(string.Format("/product_types/{0:G}", Id));
            if (response.IsSuccessStatusCode)
            {
                productType = JsonConvert.DeserializeObject<ProductType>(await response.Content.ReadAsStringAsync());

            }
            else
            {
                throw new Exception("Product type doesn't exist");
            }
            return productType;
        }
    }
}
