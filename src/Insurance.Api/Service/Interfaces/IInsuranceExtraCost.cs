

namespace Insurance.Api.Service.Interfaces
{
    public interface IInsuranceExtraCost
    {
        float CalculateAdditionalInsurance(Product product);
    }
}