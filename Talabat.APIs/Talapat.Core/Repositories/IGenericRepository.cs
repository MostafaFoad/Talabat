
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities;

namespace Talapat.Core.Repositories
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		Task<IReadOnlyList<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);

		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec);
		Task<T> GetByIdWithSpecAsync(ISpecification<T> Spec);
		Task<int> GetConutWithSpecAsync(ISpecification<T> Spec);
		Task AddAsync(T Item);
		void Update(T Item);
		void Delete(T Item);
	}
}
