using System.Collections.Generic;
using System.Linq;

namespace Framework.Utility.Paging
{
    public class PagedList<T> : List<T>
    {
        public int TotalCount { get; set; }
        public int PageCount  { get; set; }
        public int Page       { get; set; }
        public int PageSize   { get; set; }

        public PagedList()
        {
            /* empty constructor */
        } 

        public PagedList(IQueryable<T> source, int page, int pageSize, int totalRecords)
        {
            TotalCount  = totalRecords;
            PageCount   = GetPageCount(pageSize, TotalCount);
            Page        = page < 1 ? 0 : page - 1;
            PageSize    = pageSize;
            AddRange(source.Skip(Page * PageSize).Take(PageSize).ToList());
        }

        public static int GetPageCount(int pageSize, int totalCount)
        {
            if (pageSize == 0)
                return 0;
            var remainder = totalCount % pageSize;
            return totalCount / pageSize + (remainder == 0 ? 0 : 1);
        }
    }
}
