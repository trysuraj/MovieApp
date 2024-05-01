using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Model;

namespace MovieApp.Interface
{
    public interface IOmdbService        
{
    Task<MovieDetails> SearchMoviesAsync(string title);
    Task<MovieDetails> GetMovieDetailsAsync(string imdbId);
}
}