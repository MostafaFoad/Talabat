using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities;

namespace Talapat.Core.Repositories
{
	public interface ISpecification<T>where T:BaseEntity
	{
		public Expression<Func<T,bool>> Crietria { get; set; }
		public Expression<Func<T, object>> OrderByCrietria { get; set; }
		public Expression<Func<T, object>> OrderByDescCrietria { get; set; }
		public List<Expression<Func<T,object>>> IncludeCrietria { get; set; }

		public int Take { get; set; }
		public int Skip { get; set; }
		public bool	 IsPaginationEnapled { get; set; }
	}
}
