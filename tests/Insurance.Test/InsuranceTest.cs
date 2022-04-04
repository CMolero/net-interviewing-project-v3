using Insurace.Api.Model.DTOs;
using Insurance.Api.Controllers;
using Insurance.Api.Data;
using Insurance.Api.Models;
using Insurance.Api.Service;
using Insurance.Api.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests
{
    public class InsuranceTest
    {

        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<ILogger<InsuranceController>> _logger;
        private readonly Mock<ISurcharges> _surcharges;
        private readonly IInsuranceProduct _insuranceService;
        private readonly InsuranceDto _insuranceDto;



        public InsuranceTest()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _logger = new Mock<ILogger<InsuranceController>>();
            _surcharges = new Mock<ISurcharges>();
            _insuranceDto = new InsuranceDto { ProductId = 1 };
            IEntity<ProductType> productTypeService = new ProductTypeRepository(_mockHttpClientFactory.Object);
            IEntity<Product> productService = new ProductRepository(_mockHttpClientFactory.Object, productTypeService, _surcharges.Object);
            _insuranceService = new InsuranceProduct(productService);

        }


        [Theory]
        [InlineData(1, 0, 0, 0)]
        [InlineData(1, 399, 0, 0)]
        [InlineData(1, 200, 0, 0)]
        [InlineData(1, 1, 0, 0)]
        public async Task CalculateInsurance_GivenSalesPriceLessThan500Euros_NoInsuranceAsync(int productTypeId, float salesPrice, int productTypeIndex, float expectedInsuranceValue)
        {
            SetUpProductService(productTypeId, salesPrice, productTypeIndex);
            var result = await CalculateInsurance();
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );

        }

        [Theory]
        [InlineData(1, 1999, 0, 1000)]
        [InlineData(1, 820, 0, 1000)]
        [InlineData(1, 501, 0, 1000)]
        [InlineData(1, 1000, 0, 1000)]
        public async void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost(int productTypeId, float salesPrice, int productTypeIndex, float expectedInsuranceValue)
        {
            SetUpProductService(productTypeId, salesPrice, productTypeIndex);
            var result = await CalculateInsurance();

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Theory]
        [InlineData(1, 2000, 0, 2000)]
        [InlineData(1, 2100, 0, 2000)]
        [InlineData(1, 7100, 0, 2000)]
        [InlineData(1, 5100, 0, 2000)]
        public async void CalculateInsurance_GivenSalesPriceGreaterThan2000Euros_ShouldAddTwoThousandEurosToInsuranceCost(int productTypeId, float salesPrice, int productTypeIndex, float expectedInsuranceValue)
        {
            SetUpProductService(productTypeId, salesPrice, productTypeIndex);
            var result = await CalculateInsurance();
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public async void CalculateInsurance_GivenSalesPriceBetween499And0EurosLaptop_ShouldAddFiveHundredEurosToInsuranceCost()
        {

            SetUpProductService(2, 150, 1);
            const float expectedInsuranceValue = 500;
            var result = await CalculateInsurance();
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }


        [Fact]
        public async void CalculateInsurance_GivenProductTypeSmartphones_ShouldAdd500Euros()
        {
            SetUpProductService(3, 150, 2);
            const float expectedInsuranceValue = 500;
            var result = await CalculateInsurance();
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public async void CalculateInsurance_GivenProductTypeCanBeInsuredFalse_NoInsurance()
        {
            SetUpProductService(4, 2000, 3);
            const float expectedInsuranceValue = 0;
            var result = await CalculateInsurance();
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public async void CalculateInsurance_GivenProductTypeDigitalCameras_ShouldAdd500Euros()
        {
            SetUpProductService(5, 100, 4);
            const float expectedInsuranceValue = 500;
            var result = await CalculateInsurance();
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Theory]
        [InlineData(5, 100, 4, 1000)]
        public async void CalculateInsurance_GivenListProducts_ShouldAdd500Euros(int productTypeId, float salesPrice, int productTypeIndex, float expectedInsuranceValue)
        {
            SetUpProductService(productTypeId, salesPrice, productTypeIndex);
            var sut = new InsuranceController(_logger.Object, _insuranceService);
            var insuranceDtos = new List<InsuranceDto> { _insuranceDto, new InsuranceDto { ProductId = 2 } };
            var result = await sut.CalculateInsurance(insuranceDtos);
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Data.TotalInsuranceValue
            );
        }

        [Theory]
        [InlineData(5, 100, 4, 1050)]
        public async void CalculateInsurance_GivenProductTypeWithSurcharge_ShouldAddSurchargeValue(int productTypeId, float salesPrice, int productTypeIndex, float expectedInsuranceValue)
        {
            SetUpProductService(productTypeId, salesPrice, productTypeIndex);
            var surcharge = new Surcharge
            {
                Id = 1,
                ProductTypeId = 5,
                SurchargeValue = 550
            };
            _surcharges.Setup(s => s.GetByProductTypeId(5)).Returns(surcharge);
            var result = await CalculateInsurance();
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public async void CalculateInsurance_GivenNonExistentProduct_ReturnError()
        {
            SetUpProductService(5, 100, 4);
            const float expectedInsuranceValue = 0;
            _insuranceDto.ProductId = 7;
            var result = await CalculateInsurance();
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue);
        }

        private async Task<InsuranceDto> CalculateInsurance()
        {
            var sut = new InsuranceController(_logger.Object, _insuranceService);
            return await sut.CalculateInsurance(_insuranceDto);
        }


        private void SetUpProductService(int productTypeId, float salesPrice, int productTypeIndex)
        {
            var productTypes = new[]
                    {
                        new
                        {
                            id = 1,
                            name = "Test type",
                            canBeInsured = true
                        },
                        new
                        {
                            id = 2,
                            name = "Laptops",
                            canBeInsured = true
                        },
                        new
                        {
                            id = 3,
                            name = "Smartphones",
                            canBeInsured = true
                        },
                        new
                        {
                            id = 4,
                            name = "Test type",
                            canBeInsured = false
                        },
                        new
                        {
                            id = 5,
                            name = "Digital cameras",
                            canBeInsured = true
                        }
            };
            var products = new[]
            {
                 new
                {
                    id = 1,
                    name = "Test Product",
                    productTypeId = productTypeId,
                    salesPrice = salesPrice
                },
                 new
                {
                    id = 2,
                    name = "Test Product",
                    productTypeId = productTypeId,
                    salesPrice = salesPrice
                }
            };
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.LocalPath == "/products/1"),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(products[0])),
                });

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.LocalPath == "/products/2"),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(products[0])),
                });


            mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.LocalPath == "/product_types"),
               ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(JsonConvert.SerializeObject(productTypes)),
               });

            mockHttpMessageHandler.Protected()
              .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.LocalPath == string.Format("/product_types/{0:G}", productTypeId)),
              ItExpr.IsAny<CancellationToken>())
              .ReturnsAsync(new HttpResponseMessage
              {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(JsonConvert.SerializeObject(productTypes[productTypeIndex])),
              });

            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:5002")
            };

            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
        }
    }
}
