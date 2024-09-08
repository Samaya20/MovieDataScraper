using MovieDataScraper.Data;

namespace MovieDataScraper.Services
{
    public class MovieWorker : BackgroundService
    {
        private readonly MovieService _movieService;
        private readonly MovieDbContext _dbContext;
        private readonly ILogger<MovieWorker> _logger;
        private readonly TimeSpan _interval;

        public MovieWorker(MovieService movieService, MovieDbContext dbContext, IConfiguration configuration, ILogger<MovieWorker> logger)
        {
            _movieService = movieService;
            _dbContext = dbContext;
            _logger = logger;
            _interval = TimeSpan.FromMinutes(configuration.GetValue<int>("Minute"));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var letter = _movieService.GetRandomLetter();
                    var movies = await _movieService.FetchMoviesByLetterAsync(letter);

                    foreach (var movie in movies)
                    {
                        if (!_dbContext.Movies.Any(m => m.Title == movie.Title))
                        {
                            _dbContext.Movies.Add(movie);
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation("Movies updated successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating movies.");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }

}
