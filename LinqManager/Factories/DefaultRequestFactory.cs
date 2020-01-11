using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqManager.Factories
{
    public class DefaultRequestFactory : LinqProcessRequestFactory
    {
        private readonly string _errorDescription = "Filter by parameter has to be in format " +
            "\"propertyName1=propertyValue1,propertyNameN=propertyValueN\"";

        private readonly string _filterBy;
        private readonly string _sortBy;
        private readonly int _page;
        private readonly int _pageSize;

        public DefaultRequestFactory(string filterBy, string sortyBy)
        {
            _filterBy = filterBy;
            _sortBy = sortyBy;
        }

        public DefaultRequestFactory(string filterBy, string sortBy, int page, int pageSize)
        {
            _filterBy = filterBy;
            _sortBy = sortBy;
            _page = page;
            _pageSize = pageSize;
        }

        protected override LinqProccesRequest BuildRequest()
        {
            if(_page == 0 || _pageSize == 0)
                return new LinqProccesRequest (GetFilters(), GetSorts(), null);

            return new LinqProccesRequest(GetFilters(), GetSorts(), new Pagination { Page = _page, PageSize = _pageSize });
        }

        private List<Filter<string>> GetFilters()
        {
            if (string.IsNullOrEmpty(_filterBy))
                return new List<Filter<string>>();

            if (!_filterBy.Contains("="))
                throw new FormatException($"{nameof(_filterBy)} {_errorDescription}");

            return _filterBy.Split(',').Select(s => 
                { var kv = s.Split('='); return new Filter<string> { PropertyName = kv[0], PropertyValue = kv[1] }; }
                ).ToList();
        }

        private List<Filter<bool>> GetSorts()
        {
            if (string.IsNullOrEmpty(_sortBy))
                return new List<Filter<bool>>();

            if (!_sortBy.Contains("="))
                throw new FormatException($"{nameof(_sortBy)} {_errorDescription}");

            return _sortBy.Split(',').Select(s => 
                { var kv = s.Split('='); return new Filter<bool> { PropertyName = kv[0], PropertyValue = bool.Parse(kv[1]) }; }
                ).ToList();
        }
    }
}