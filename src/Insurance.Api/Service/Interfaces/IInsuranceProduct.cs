namespace Insurance.Api.Service.Interfaces
{
    public interface IInsuranceProduct
    {
        Task<float> CalculateInsuranceAsync(int productId);
    }
}
