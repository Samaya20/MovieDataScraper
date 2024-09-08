using MovieDataScraper.Data;
using MovieDataScraper.Services;
using Newtonsoft.Json;

namespace MovieDataScraper.Services
{
    public class MovieService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public MovieService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<Movie>> FetchMoviesByLetterAsync(char letter)
        {
            var baseUrl = _configuration["MovieApi:BaseUrl"];
            var apiKey = _configuration["MovieApi:ApiKey"];

            var response = await _httpClient.GetAsync($"{baseUrl}?s={letter}&apikey={apiKey}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OmdbApiResponse>(content);
            return result?.Search ?? new List<Movie>();
        }

        public char GetRandomLetter()
        {
            var random = new Random();
            var letters = "abcdefghijklmnopqrstuvwxyz";
            return letters[random.Next(letters.Length)];
        }


    }
}




