using Insurance.Api.Service.Interfaces;

namespace Insurance.Api.Service.BaseServices
{
    public abstract class InsuranceExtraCostService : IInsuranceExtraCost
    {
        public abstract float CalculateAdditionalInsurance(Product product);
    }
}
