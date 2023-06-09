using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
	public class EFGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		protected readonly DbSet<TEntity> _dbSet;

		public EFGenericRepository(DbContext context)
		{
			_dbSet = context.Set<TEntity>();
		}

		public async Task AddAsync(TEntity entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public void Update(TEntity entity)
		{
			_dbSet.Update(entity);
		}

		public async Task<TEntity?> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public void Delete(TEntity entity)
		{
			_dbSet.Remove(entity);
		}
	}
}
