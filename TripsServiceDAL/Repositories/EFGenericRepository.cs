using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;
using TripsServiceDAL.Utils;

namespace TripsServiceDAL.Repos
{
	public class EFGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IIdentifiable
	{
		protected readonly DbSet<TEntity> _dbSet;

		public EFGenericRepository(DbContext context)
		{
			_dbSet = context.Set<TEntity>();
		}

		public IQueryable<TEntity> GetAll()
		{
			return _dbSet;
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

		public bool Exists(int id)
		{
			return _dbSet.Any(item => item.Id == id);
		}

		public void ThrowErrorIfNotExists(int id) 
		{
			if (!Exists(id)) 
			{
				throw new EntityNotFoundException(UtilConstants.GetEntityNotFoundMessage<TEntity>()());
			}
		}
	}
}
