﻿namespace TripsServiceDAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class, IIdentifiable
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity item);
        void Update(TEntity item);
        void Delete(TEntity item);
        bool Exists(int id);
        void ThrowErrorIfEntityIsNull(TEntity? entity);
        void ThrowErrorIfNotExists(int id);
    }
}
