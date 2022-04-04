using Insurance.Api.Service.InsuranceStrategy;
using Insurance.Api.Service.Interfaces;

namespace Insurance.Api.Service
{
    /// <summary>
    /// Business logic Product Insurance 
    /// </summary>
    public class InsuranceProduct : IInsuranceProduct
    {
        protected readonly IEntity<Product> _productService;
        protected Product _product { get; private set; }
        public InsuranceProduct(IEntity<Product> productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _product = new Product();
        }

        /// <summary>
        /// Refactor 
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public async Task<float> CalculateInsuranceAsync(int productId)
        {

            _product = await _productService.GetById(productId);

            //do nothing if the product can't be insured
            if (!_product.ProductType.CanBeInsured)
            {
                return 0f;
            }
                
            //Add extra cost by type of product
            IInsuranceExtraCost extraCost = new InsuranceExtraCost();
            extraCost.CalculateAdditionalInsurance(_product);

            //Add insurance surcharge logic
            extraCost = new InsuranceSurcharge();
            extraCost.CalculateAdditionalInsurance(_product);

            //Breaking the if else logic using strategy pattern 
            IInsuranceService insurance = new LowCostInsurance();
            if (_product.SalesPrice < 500)
            {
                insurance = new LowCostInsurance();
            }
            else if (_product.SalesPrice < 2000)
            {
                insurance = new MediumCostInsurance();
            }
            else if (_product.SalesPrice >= 2000)
            {
                insurance = new HighCostInsurance();
            }

            return insurance.CalculateInsurance(_product);
        }
    }
}
