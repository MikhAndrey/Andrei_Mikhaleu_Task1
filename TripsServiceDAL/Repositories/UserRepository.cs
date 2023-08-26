﻿using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos;

public class UserRepository : EFGenericRepository<User>, IUserRepository
{
	public UserRepository(TripsDBContext context) : base(context)
	{
	}

	public async Task<User?> GetByUsernameAsync(string username)
	{
		return await _dbSet.Include(u => u.Role)
			.FirstOrDefaultAsync(u => u.UserName == username);
	}

	public async Task<User?> GetByEmailAsync(string email)
	{
		return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
	}

	public IQueryable<User> GetAllWithRoles()
	{
		return _dbSet.Include(u => u.Role);
	}

	public IQueryable<User> GetAllWithTrips()
	{
		return _dbSet.Include(u => u.Trips);
	}
}
