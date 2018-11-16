using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyXApi.ViewModels
{
    public class PaginatedResponse<TEntity> where TEntity : class
    {
        public int PageIndex { get; private set; }
        public int Count { get; private set; }
        public long Total { get; private set; }
        public IEnumerable<TEntity> Employees { get; private set; }

        public PaginatedResponse(int pageIndex, int count, long total, IEnumerable<TEntity> employees)
        {
            this.PageIndex = pageIndex;
            this.Count = count;
            this.Total = total;
            this.Employees = employees;
        }
    }
}
