using System.Linq;

namespace LinqManager.EFCoreExtensions
{
    public class LinqAsyncResponse<T> where T: class
    {
        public IQueryable<T> Query { get; set; } = Enumerable.Empty<T>().AsQueryable();
        public int Count { get; set; }
    }
}