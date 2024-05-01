using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using MovieApp.Model;
using MovieApp.Services;
using Xunit;

namespace MovieApp.Test
{
    public class GetByTittleTest
    {
        [Fact]
        public async Task GetMovieDetailsAsync_SuccessfulResponse_ReturnsMovieDetails()
        {
            // Arrange
            var imdbId = "tt1234567";
            var apiKey = "your_api_key";
            var expectedMovieDetails = new MovieDetails
            {
                // Set expected property values here
                Title = "Sample Movie",
                Year = "2022",
                // Add more properties as needed
            };

            // Create a mock HttpMessageHandler to simulate HttpClient
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedMovieDetails)),
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new OmdbService(httpClient, apiKey);

            // Act
            var actualMovieDetails = await service.GetMovieDetailsAsync(imdbId);

            // Assert
            Assert.NotNull(actualMovieDetails);
            Assert.Equal(expectedMovieDetails.Title, actualMovieDetails.Title);
            Assert.Equal(expectedMovieDetails.Year, actualMovieDetails.Year);
            // Assert other properties as needed
        }

        [Fact]
        public async Task GetMovieDetailsAsync_ErrorResponse_ThrowsException()
        {
            // Arrange
            var imdbId = "tt1234567";
            var apiKey = "your_api_key";
            var errorMessage = "Movie not found";

            // Create a mock HttpMessageHandler to simulate HttpClient
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"{{\"Response\": \"False\", \"Error\": \"{errorMessage}\"}}"),
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new OmdbService(httpClient, apiKey);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => service.GetMovieDetailsAsync(imdbId));
        }
    }
}

