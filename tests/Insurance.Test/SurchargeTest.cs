using Insurance.Api.Controllers;
using Insurance.Api.Data;
using Insurance.Api.Models;
using Insurance.Api.Models.DTOs;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests
{
    public class SurchargeTest
    {
        private readonly Mock<ILogger<InsuranceSurchargeController>> _logger;
        private readonly Mock<ISurcharges> _surcharges;

        public SurchargeTest()
        {
            _logger = new Mock<ILogger<InsuranceSurchargeController>>();
            _surcharges = new Mock<ISurcharges>();

        }

        [Fact]
        public async void SurchargeInsurance_GivenSurchargeByProductType_ShouldUpload()
        {

            var surcharge = new SurchargeDto
            {
                ProductTypeId = 5,
                SurchargeValue = 550
            };

            var responseTask = Task.FromResult(new Surcharge
            {
                ProductTypeId = 5,
                SurchargeValue = 550
            });
            _surcharges.Setup(s => s.Upsert(surcharge)).Returns(responseTask);

            var sut = new InsuranceSurchargeController(_logger.Object, _surcharges.Object);
            var result = await sut.UploadInsuranceSurcharge(surcharge);

            Assert.True(result.IsSuccess, "OK");
            Assert.Equal(surcharge.ProductTypeId, result.Data.ProductTypeId);
            Assert.Equal(surcharge.SurchargeValue, result.Data.SurchargeValue);

        }

    }
}
