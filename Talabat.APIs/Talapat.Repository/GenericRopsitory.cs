
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities;
using Talapat.Core.Repositories;
using Talapat.Repository.DataBase;

namespace Talapat.Repository
{
	public class GenericRopsitory<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _dbContext;
		public GenericRopsitory(StoreContext dbContext) 
		{
			//dbContext= new MVCContext();
			_dbContext = dbContext;
		}
		

	
		public async Task<T> GetByIdAsync(int id)
		{

			//_dbContext.Set<T>().Where(P=>P.Id== id).Include().FirstOrDefaultAsync();
			return await _dbContext.Set<T>().FindAsync(id);
		}

		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			
			return await _dbContext.Set<T>().ToListAsync();
		}

		public async Task<T> GetByIdWithSpecAsync(ISpecification<T>Spec)
		{

		return await GetSpec(Spec).FirstOrDefaultAsync();
		
		}

		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec)
		{
			
			return await GetSpec(Spec).ToListAsync();

		}
		public async Task<int> GetConutWithSpecAsync(ISpecification<T> Spec)
		{
			return await GetSpec(Spec).CountAsync();
		}

		public IQueryable<T> GetSpec(ISpecification<T> spec)
		{
			return BuildQueryWithSpecification<T>.GetQuery(_dbContext.Set<T>(), spec);
		}

		public async Task AddAsync(T Item)
		{
			await _dbContext.Set<T>().AddAsync(Item);
		}

		public void Update(T Item)
		{
			_dbContext.Set<T>().Update(Item);
		}

		public void Delete(T Item)
		{
			_dbContext.Set<T>().Remove(Item);
		}

	}
}
