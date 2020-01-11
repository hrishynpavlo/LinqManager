using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Models
{
    public interface IPagedModel<out TCollection> where TCollection : IEnumerable
    {
        TCollection Items { get; }
        int Count { get; set; }
    }

    public class QueryPagedModel<T> : IPagedModel<IQueryable<T>> where T : class
    {
        public QueryPagedModel(IQueryable<T> query, int count)
        {
            Items = query;
            Count = count;
        }
        public IQueryable<T> Items { get; private set; } = Enumerable.Empty<T>().AsQueryable();
        public int Count { get; set; }
    }
    public class ObjectPagedModel<T> : IPagedModel<IEnumerable<T>> where T : class
    {

        public ObjectPagedModel(List<T> list, int count)
        {
            Items = list;
            Count = count;
        }
        public IEnumerable<T> Items { get; } = new List<T>();

        public int Count { get; set; }
    }
}