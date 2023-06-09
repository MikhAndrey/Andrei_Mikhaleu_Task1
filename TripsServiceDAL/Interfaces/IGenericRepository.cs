namespace TripsServiceDAL.Interfaces
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
		Task<TEntity?> GetByIdAsync(int id);
		Task AddAsync(TEntity item);
		void Update(TEntity item);
		void Delete(TEntity item);
	}
}
