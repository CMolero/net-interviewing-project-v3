using Insurance.Api.Service.Interfaces;

namespace Insurance.Api.Service.BaseServices
{
    public abstract class InsuranceService : IInsuranceService
    {
        public abstract float CalculateInsurance(Product product);
    }
}
