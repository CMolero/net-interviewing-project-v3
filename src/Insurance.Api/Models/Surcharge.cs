namespace Insurance.Api.Models
{
    public class Surcharge
    {
        public int Id { get; set; }
        public int ProductTypeId { get; set; }
        public float SurchargeValue { get; set; }
        public Guid Version { get; set; }

    }
}
