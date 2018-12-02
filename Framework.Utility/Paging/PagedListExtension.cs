using System.Linq;

namespace Framework.Utility.Paging
{
    public static class PagedListExtension
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int page, int pageSize, int totalRecords)
        {
            return new PagedList<T>(source, page, pageSize, totalRecords);
        }
    }
}
