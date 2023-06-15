using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
	public class EFGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IIdentifiable
	{
		protected readonly DbSet<TEntity> _dbSet;

		public EFGenericRepository(DbContext context)
		{
			_dbSet = context.Set<TEntity>();
		}

		public async Task AddAsync(TEntity entity)
		{
			_ = await _dbSet.AddAsync(entity);
		}

		public void Update(TEntity entity)
		{
			_ = _dbSet.Update(entity);
		}

		public async Task<TEntity?> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public void Delete(TEntity entity)
		{
			_ = _dbSet.Remove(entity);
		}

		public bool Exists(int id)
		{
			return _dbSet.Any(item => item.Id == id);
		}
	}
}
