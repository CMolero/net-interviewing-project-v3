using Insurace.Api.Model.DTOs;
using Insurance.Api.Models.DTOs;
using Insurance.Api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [ApiController]
    public class InsuranceController : ControllerBase
    {
        private readonly ILogger<InsuranceController> _logger;
        private readonly IInsuranceProduct _productInsurance;


        public InsuranceController(ILogger<InsuranceController> logger, IInsuranceProduct productInsurance)
        {
            _productInsurance = productInsurance ?? throw new ArgumentNullException(nameof(productInsurance));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //Same returning value to avoid issues but async
        [HttpPost]
        [Route("api/insurance/product")]
        public async Task<InsuranceDto> CalculateInsurance([FromBody] InsuranceDto toInsure)
        {
            try
            {
                toInsure.InsuranceValue = await _productInsurance.CalculateInsuranceAsync(toInsure.ProductId);
                _logger.LogInformation("Calculated insurance for productid {Id}", toInsure.ProductId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("There was a problem while calculating insurance for productid {Id} {ex}", toInsure.ProductId, ex);
            }
            return toInsure;
        }

        [HttpPost]
        [Route("api/insurance/products")]
        public async Task<ResponseMessageDto<OrderInsuranceDto>> CalculateInsurance([FromBody] List<InsuranceDto> insuranceDtos)
        {
            OrderInsuranceDto orderInsurance = new();
            ResponseMessageDto<OrderInsuranceDto> response;
            try
            {
                foreach (var product in insuranceDtos)
                {
                    product.InsuranceValue = await _productInsurance.CalculateInsuranceAsync(product.ProductId);
                    orderInsurance.TotalInsuranceValue += product.InsuranceValue;
                }
                response = new ResponseMessageDto<OrderInsuranceDto>(true, "Surcharge update successfully", orderInsurance);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error gertting insurance for an order {ex}", ex);
                response = new ResponseMessageDto<OrderInsuranceDto>(false, "Error gertting insurance for an order", null);
            }

            return response;
        }

    }
}