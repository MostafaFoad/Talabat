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
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _storeContext;

		public Dictionary<string, object> Repositories { get; set; }
		public UnitOfWork(StoreContext storeContext)
		{
			_storeContext = storeContext;
			Repositories=new Dictionary<string, object>();
		}


		public IGenericRepository<T>? CreateRepo<T>()where T:BaseEntity 
		{
			if (!Repositories.ContainsKey(typeof(T).Name))
			{
				
				Repositories.Add(typeof(T).Name, new GenericRopsitory<T>(_storeContext));
			}
			return Repositories[typeof(T).Name] as IGenericRepository<T>;
		}
		public async Task<int> Complete()
		{
			return await _storeContext.SaveChangesAsync();
		}

		public async ValueTask DisposeAsync()
		{
			await _storeContext.DisposeAsync();
		}
	}
}
 