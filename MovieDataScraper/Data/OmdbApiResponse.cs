namespace MovieDataScraper.Data
{
    public class OmdbApiResponse
    {
        public string Response { get; set; }
        public List<Movie> Search { get; set; }
    }
}
