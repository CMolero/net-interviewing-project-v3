namespace Insurance.Api.Service.Interfaces
{
    public interface IEntity<T> where T : class
    {
        Task<T> GetById(int id);
    }
}
