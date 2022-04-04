using Insurance.Api.Models;
using Insurance.Api.Models.DTOs;

namespace Insurance.Api.Data
{
    public interface ISurcharges
    {
        List<Surcharge> GetAll();
        Surcharge GetByProductTypeId(int Id);
        Task<Surcharge> Upsert(SurchargeDto surcharge);
    }
}