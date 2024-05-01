using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Interface;

namespace MovieApp.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IOmdbService _omdbService;
        private readonly ISearchQueryService _searchQueryService;

        public MovieController(IOmdbService omdbService, ISearchQueryService searchQueryService)
        {
            _omdbService = omdbService;
            _searchQueryService = searchQueryService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies(string title)
        {
            try
            {
                var searchResults = await _omdbService.SearchMoviesAsync(title);
                _searchQueryService.SaveSearchQuery(title);
                return Ok(searchResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving movie details. No Movie found");
            }
        }


        [HttpGet("{imdbId}")]
        public async Task<IActionResult> GetMovieDetails(string imdbId)
        {
            try
            {
                var movieDetails = await _omdbService.GetMovieDetailsAsync(imdbId);
                return Ok(movieDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving movie details.");
            }
        }

        [HttpGet("latest-search-queries")]
        public async Task<IActionResult> GetLatestSearchQueries()
        {
            var searchQueries = _searchQueryService.GetLatestSearchQueries();
            return Ok(searchQueries);
        }

    }
}
