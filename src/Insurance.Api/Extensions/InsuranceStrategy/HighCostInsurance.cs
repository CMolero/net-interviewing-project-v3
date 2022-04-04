using Insurance.Api.Service.BaseServices;
using Insurance.Api.Service.Interfaces;

namespace Insurance.Api.Service.InsuranceStrategy
{
    public class HighCostInsurance : InsuranceService, IInsuranceService
    {
        public override float CalculateInsurance(Product product)
        {
            return product.AdditionalInsuranceCost + 2000;
        }
    }
}
