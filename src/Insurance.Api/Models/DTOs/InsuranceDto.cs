using System.Text.Json.Serialization;

namespace Insurace.Api.Model.DTOs
{
    public class InsuranceDto
    {
        public int ProductId { get; set; }
        public float InsuranceValue { get; set; }
        [JsonIgnore]
        public string ProductTypeName { get; set; } = string.Empty;
        [JsonIgnore]
        public bool ProductTypeHasInsurance { get; set; }
        [JsonIgnore]
        public float SalesPrice { get; set; }


    }
}
