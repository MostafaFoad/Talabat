using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities;

namespace Talapat.Core.Repositories
{
	public interface IUnitOfWork:IAsyncDisposable
	{
		
		IGenericRepository<T>? CreateRepo<T>() where T : BaseEntity;
		 Task<int>Complete();
	}
}
