using Insurance.Api.Models.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Test.IntegrationTest
{
    public class SurchargeIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SurchargeIntegrationTest(CustomWebApplicationFactory<Program> factory)
            => _client = factory.CreateClient();


        [Fact]
        public async Task CreateSurcharge_GivenNonExistingProductType_ShouldAddSurcharge()
        {

            //Arrange 
            string expectedResult = "550";
            var surcharge = new SurchargeDto
            {
                ProductTypeId = 55,
                SurchargeValue = 550
            };

            //Act  
            var response = await _client.PostAsync("api/insurance/surcharge", new StringContent(JsonConvert.SerializeObject(surcharge), Encoding.Default, "application/json")); 

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            //Assert  
            Assert.Contains(expectedResult, responseString);

        }

        [Fact]
        public async Task UpdateSurcharge_GivenExistingProductType_ShouldUpdateSurcharge()
        {
            //Arrange 
            string expectedResult = "550";
            var surcharge = new SurchargeDto
            {
                ProductTypeId = 5,
                SurchargeValue = 550
            };

            //Act  
            var response = await _client.PostAsync("api/insurance/surcharge", new StringContent(JsonConvert.SerializeObject(surcharge), Encoding.Default, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            //Assert  
            Assert.Contains(expectedResult, responseString);

        }

    }
}
