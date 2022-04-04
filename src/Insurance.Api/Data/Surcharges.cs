using Insurance.Api.Models;
using Insurance.Api.Models.DTOs;

namespace Insurance.Api.Data
{
    public class Surcharges : ISurcharges
    {
        //dummy surcharge repository
        private List<Surcharge> _surcharge { get; set; }

        public Surcharges()
        {
            //Dummy data
            _surcharge = new List<Surcharge>
            {
                 new Surcharge
                {
                    Id = 1,
                    ProductTypeId = 21,
                    SurchargeValue = 700,
                    Version =  Guid.NewGuid()
                },
                new Surcharge
                {
                    Id = 2,
                    ProductTypeId = 32,
                    SurchargeValue = 700,
                    Version =  Guid.NewGuid()
                },
                new Surcharge
                {
                    Id = 3,
                    ProductTypeId = 33,
                    SurchargeValue = 700,
                    Version =  Guid.NewGuid()
                },
                new Surcharge
                {
                    Id = 4,
                    ProductTypeId = 4,
                    SurchargeValue = 700,
                    Version =  Guid.NewGuid()
                },
                new Surcharge
                {
                    Id = 5,
                    ProductTypeId = 5,
                    SurchargeValue = 700,
                    Version =  Guid.NewGuid()
                }

            };
        }

        public List<Surcharge> GetAll() => _surcharge;

        public Surcharge GetByProductTypeId(int Id)
        {
            return _surcharge.FirstOrDefault(s => s.ProductTypeId == Id);
        }

        public async Task<Surcharge> Upsert(SurchargeDto surcharge)
        {
            var surchargeRep = _surcharge.FirstOrDefault(s => s.ProductTypeId == surcharge.ProductTypeId);
            if (surchargeRep != null)
                surchargeRep.SurchargeValue = surcharge.SurchargeValue;
            else
            {
                int newId = _surcharge.Max(s => s.Id) + 1;
                surchargeRep = new Surcharge { Id = newId, ProductTypeId = surcharge.ProductTypeId, SurchargeValue = surcharge.SurchargeValue };
                _surcharge.Add(surchargeRep);
            }

            return surchargeRep;


        }
    }
}
