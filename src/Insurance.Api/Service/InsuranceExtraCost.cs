using Insurance.Api.Service.BaseServices;
using Insurance.Api.Service.Interfaces;

namespace Insurance.Api.Service
{
    public class InsuranceExtraCost : InsuranceExtraCostService, IInsuranceExtraCost
    {
        protected Dictionary<string, float> _productTypesExtraCost;
        public InsuranceExtraCost()
        {
            _productTypesExtraCost = new Dictionary<string, float>() { { "Smartphones", 500 }, { "Laptops", 500 }, { "Digital cameras", 500 } };
        }
        public override float CalculateAdditionalInsurance(Product product)
        {

            if (_productTypesExtraCost.ContainsKey(product.ProductType.Name))
            {
                return product.AdditionalInsuranceCost += _productTypesExtraCost[product.ProductType.Name];
            }
            return product.AdditionalInsuranceCost;
        }


    }


}
