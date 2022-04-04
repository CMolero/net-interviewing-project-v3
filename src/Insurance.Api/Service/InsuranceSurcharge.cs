using Insurance.Api.Service.BaseServices;
using Insurance.Api.Service.Interfaces;

namespace Insurance.Api.Service
{
    public class InsuranceSurcharge : InsuranceExtraCostService, IInsuranceExtraCost
    {
        public override float CalculateAdditionalInsurance(Product product)
        {
            if (product.Surcharge != null && product.Surcharge.SurchargeValue > 0)
            {
                product.AdditionalInsuranceCost += product.Surcharge.SurchargeValue;
            }
            return product.AdditionalInsuranceCost;
        }
    }
}
