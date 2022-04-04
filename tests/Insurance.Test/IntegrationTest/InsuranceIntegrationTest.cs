using Insurace.Api.Model.DTOs;
using Insurance.Api.Service;
using Insurance.Api.Service.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Insurance.Test.IntegrationTest.InsuranceIntegrationTest;

namespace Insurance.Test.IntegrationTest
{
    public class InsuranceIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        public InsuranceIntegrationTest(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;

            factory.Host.Start();

        }

        [Fact]
        public async Task CalculateInsurance_GivenSalesPrice750EurosLaptop_Should1500EurosToInsuranceCost()
        {

            //Arrange 
            const string expectedInsuranceValue = "1500";
            var insuranceDto = new InsuranceDto
            {
                ProductId = 1,
            };

            //Act  
            var response = await _client.PostAsync("api/insurance/product", new StringContent(JsonConvert.SerializeObject(insuranceDto), Encoding.Default, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            //Assert  
            Assert.Contains(expectedInsuranceValue, responseString);
            _factory.Host.Dispose();
        }

        //[Fact]
        //public async Task CalculateInsurance_GivenListOfProducts_Should1500EurosToInsuranceCost()
        //{

        //    //Arrange 

        //    const string expectedInsuranceValue = "1500";
        //    var insuranceDto = new InsuranceDto
        //    {
        //        ProductId = 1,
        //    };

        //    var insuranceDtos = new List<InsuranceDto> { insuranceDto
        //    };
        //    //Act  
        //    var response = await _client.PostAsync("api/insurance/products", new StringContent(JsonConvert.SerializeObject(insuranceDtos), Encoding.Default, "application/json"));

        //    response.EnsureSuccessStatusCode();

        //    var responseString = await response.Content.ReadAsStringAsync();

        //    //Assert  
        //    Assert.Contains(expectedInsuranceValue, responseString);

        //}





    }
}
