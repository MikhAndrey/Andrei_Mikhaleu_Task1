namespace Andrei_Mikhaleu_Task1.Models.Repos
{
    public interface IRepository<T> : IDisposable 
        where T : class
    {
        Task<T> GetById(int id);
        Task Add(T item);
        Task Update(T item);
        Task Delete(T item);
    }
}
