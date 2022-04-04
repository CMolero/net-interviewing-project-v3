using Insurance.Api.Data;
using Insurance.Api.Models;
using Insurance.Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    public class InsuranceSurchargeController : ControllerBase
    {
        private readonly ILogger<InsuranceSurchargeController> _logger;
        private readonly ISurcharges _surcharges;

        public InsuranceSurchargeController(ILogger<InsuranceSurchargeController> logger, ISurcharges surcharges)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _surcharges = surcharges ?? throw new ArgumentNullException(nameof(surcharges));
        }

        [HttpPost]
        [Route("api/insurance/surcharge")]
        public async Task<ResponseMessageDto<Surcharge>> UploadInsuranceSurcharge([FromBody] SurchargeDto surchargeDto)
        {
            //Asynchronous A form of concurrency that uses futures or callbacks to avoid unnecessary threads
            ResponseMessageDto<Surcharge> response;
            try
            {
                _logger.LogInformation("Uploading Surcharge to product type");
                var surcharge = await _surcharges.Upsert(surchargeDto);
                response = new ResponseMessageDto<Surcharge>(true, "Surcharge updated successfully", surcharge);

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Uploading Surcharge to product type" + ex.Message);
                response = new ResponseMessageDto<Surcharge>(false, "Error updating Surcharge", null);
            }

            return response;
        }
    }
}
