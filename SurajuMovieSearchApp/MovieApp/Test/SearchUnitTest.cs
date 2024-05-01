
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using MovieApp.Model;
using MovieApp.Services;
using Xunit;

namespace MovieApp.Test
{
    public class SearchUnitTest
    {
        [Fact]
        public async Task SearchMoviesAsync_ValidResponse_ReturnsMovieDetails()
        {
            // Arrange
            var expectedTitle = "The Matrix";
            var expectedMovieDetails = new MovieDetails { Title = expectedTitle, Year = "1999" }; // Define expected movie details

            // Create a mock of HttpClient
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedMovieDetails), Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var omdbService = new OmdbService(httpClient, "test_api_key"); // Instantiate OmdbService with the mocked HttpClient

            // Act
            var movieDetails = await omdbService.SearchMoviesAsync(expectedTitle);

            // Assert
            Assert.NotNull(movieDetails);
            Assert.Equal(expectedTitle, movieDetails.Title);
            // Add more assertions based on expected movie details
        }

        [Fact]
        public async Task SearchMoviesAsync_ErrorResponse_ThrowsException()
        {
            // Arrange
            var expectedTitle = "InvalidTitle";

            // Create a mock of HttpClient
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"Response\": \"False\", \"Error\": \"Movie not found\"}", Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var omdbService = new OmdbService(httpClient, "test_api_key"); // Instantiate OmdbService with the mocked HttpClient

            // Act and assert
            await Assert.ThrowsAsync<Exception>(() => omdbService.SearchMoviesAsync(expectedTitle));
        }
    }
}

    
