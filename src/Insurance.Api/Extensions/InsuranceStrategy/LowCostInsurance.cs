using Insurance.Api.Service.BaseServices;
using Insurance.Api.Service.Interfaces;

namespace Insurance.Api.Service.InsuranceStrategy
{
    public class LowCostInsurance : InsuranceService, IInsuranceService
    {
        public override float CalculateInsurance(Product product)
        {

            return product.AdditionalInsuranceCost;

        }
    }
}
