using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Interface
{
    public interface ISearchQueryService
    {
    void SaveSearchQuery(string query);
    List<string> GetLatestSearchQueries();
    }
}