using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MovieApp.Interface;
using MovieApp.Model;

namespace MovieApp.Services
{
    public class OmdbService : IOmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OmdbService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = "e2e2ba0";
        }
        public async Task<MovieDetails> GetMovieDetailsAsync(string imdbId)
        {
            try
            {
                var url = $"http://www.omdbapi.com/?i={imdbId}&apikey={_apiKey}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); 

                using var contentStream = await response.Content.ReadAsStreamAsync();
                var jsonDocument = await JsonDocument.ParseAsync(contentStream);

                if (jsonDocument.RootElement.TryGetProperty("Response", out var responseProperty))
                {
                    if (responseProperty.GetString() == "False")
                    {
                        var errorProperty = jsonDocument.RootElement.GetProperty("Error").GetString();
                        throw new Exception($"Error response from the API: {errorProperty}");
                    }
                }
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                };
                contentStream.Seek(0, SeekOrigin.Begin); 
                return await JsonSerializer.DeserializeAsync<MovieDetails>(contentStream, options);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to make HTTP request.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to deserialize JSON response.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }

        public async Task<MovieDetails> SearchMoviesAsync(string title)
        {
            try
            {
                var url = $"http://www.omdbapi.com/?t={Uri.EscapeDataString(title)}&type=movie&apikey={_apiKey}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); 
                using var contentStream = await response.Content.ReadAsStreamAsync();
                var jsonDocument = await JsonDocument.ParseAsync(contentStream);

                if (jsonDocument.RootElement.TryGetProperty("Response", out var responseProperty))
                {
                    if (responseProperty.GetString() == "False")
                    {
                        var errorProperty = jsonDocument.RootElement.GetProperty("Error").GetString();
                        throw new Exception($"Error response from the API: {errorProperty}");
                    }
                }

                contentStream.Seek(0, SeekOrigin.Begin);
                return await JsonSerializer.DeserializeAsync<MovieDetails>(contentStream);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to make HTTP request.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to deserialize JSON response.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }
    }
}
