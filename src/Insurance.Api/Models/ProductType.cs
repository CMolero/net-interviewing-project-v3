namespace Insurance.Api.Service.Interfaces
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool CanBeInsured { get; set; }
    }
}
