using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Interface;

namespace MovieApp.Services
{

    public class SearchQueryService : ISearchQueryService
    {
        private readonly List<string> _searchQueries;
        private const int MaxSearchQueryCount = 5;

        public SearchQueryService()
        {
            _searchQueries = new List<string>();
        }

        public void SaveSearchQuery(string query)
        {
            if (_searchQueries.Contains(query))
                _searchQueries.Remove(query);

            _searchQueries.Insert(0, query);

            if (_searchQueries.Count > MaxSearchQueryCount)
                _searchQueries.RemoveAt(_searchQueries.Count - 1);
        }

        public List<string> GetLatestSearchQueries()
        {
            return _searchQueries;
        }
    }

}
