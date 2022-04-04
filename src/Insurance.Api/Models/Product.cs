using Insurance.Api.Models;

namespace Insurance.Api.Service.Interfaces
{
    public class Product
    {
        public int Id { get; set; }
        public float InsuranceValue { get; set; }
        public float SalesPrice { get; set; }
        public int ProductTypeId { get; set; }
        public float AdditionalInsuranceCost { get; set; }
        public ProductType ProductType { get; set; } = new ProductType();
        public Surcharge Surcharge { get; set; } = new Surcharge();
    }
}
