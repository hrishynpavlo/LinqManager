using LinqManager.Factories;
using System.Collections.Generic;

namespace LinqManager
{
    public class LinqProccesRequest
    {
        public List<Filter<string>> FilterBy { get; private set; }
        public List<Filter<bool>> SortBy { get; private set; }
        public Pagination Pagination { get; private set; }

        public static implicit operator LinqProccesRequest(LinqProcessRequestFactory factory)
        {
            return factory.CreateRequest();
        }

        private LinqProccesRequest() { }

        public LinqProccesRequest(List<Filter<string>> filterBy, List<Filter<bool>> sortBy, Pagination pagination)
        {
            FilterBy = filterBy;
            SortBy = sortBy;
            Pagination = pagination;
        }
    }

    public class Pagination
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class Filter<T>
    {
        public string PropertyName { get; set; }
        public T PropertyValue { get; set; }
    }
}