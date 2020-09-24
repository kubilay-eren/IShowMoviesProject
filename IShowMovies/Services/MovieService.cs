using IShowMovies.Extensions;
using IShowMovies.Models;
using IShowMovies.Models.QueryModels;
using IShowMovies.Models.ResponseModels;
using IShowMovies.ServiceInterfaces;
using IShowMovies.Settings.AppSetting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IShowMovies.Services
{
    public class MovieService : IMovieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        IRestClientFactory _restClientFactory;
        private readonly IOptions<AppSettings> _appsettings;
        private readonly MovieContext _context;

        List<Movies> movies;

        public MovieService(IHttpContextAccessor HttpContextAccessor, IRestClientFactory RestClientFactory, IOptions<AppSettings> appsettings, MovieContext context)
        {
            _httpContextAccessor = HttpContextAccessor;
            _restClientFactory = RestClientFactory;
            _appsettings = appsettings;
            _context = context;
        }

        public async Task<List<Movies>> GetMovies(MovieQueryParameters movieQuery)
        {
            movies = _httpContextAccessor?.HttpContext.Session.GetMovie();

            if (movies == null)
            {
                await UpdateMoviesList();
            }

            return movies.OrderBy(o => o.title)
                .Skip((movieQuery.PageNumber - 1) * movieQuery.PageSize)
                .Take(movieQuery.PageSize)
                .ToList();
        }

        public async Task UpdateMoviesList()
        {
            string Url = "popular?language=en-US";
            movies = new List<Movies>();

            using (var client = _restClientFactory.Create())
            {
                var res = await client.GetAsync<MovieServiceResponse<List<Movies>>>(Url);

                movies = res.results.Take(_appsettings.Value.movieSettings.MovieCount).ToList();

                _httpContextAccessor.HttpContext.Session.SetMovie(movies);
            }

        }

        public async Task AddMovieRate(int MovieId, string Note, int Point)
        {
            string UserName = _httpContextAccessor.HttpContext.GetUserName();

            var userMovie = _context.UserMovie.Where(i => i.MovieId == MovieId && i.UserName == UserName).FirstOrDefault();

            if (userMovie != null)
                throw new Exception("Kullanıcı daha önce bu filme oy vermiş.");
            else
            {
                UserMovie userMovieDb = new UserMovie()
                {
                    MovieId = MovieId,
                    Point = Point,
                    UserName = UserName,
                    Note = Note
                };

                _context.UserMovie.Add(userMovieDb);
                _context.SaveChanges();
            }

            MovieNotes notes = new MovieNotes()
            {
                MovieId = MovieId,
                Note = Note
            };

            _context.MovieNotes.Add(notes);
            _context.SaveChanges();

            var movie = _context.MovieRates.FirstOrDefault(i => i.MovieId == MovieId);

            if(movie != null)
            {
                movie.Point = ((movie.Point * movie.RateCount) + Point) / (movie.RateCount + 1);
                movie.RateCount += 1;

                _context.MovieRates.Update(movie);
            }
            else
            {
                MovieRate rate = new MovieRate()
                {
                    MovieId = MovieId,
                    Point = Point,
                    RateCount = 1
                };

                _context.MovieRates.Add(rate);
            }

            _context.SaveChanges();
        }

        public async Task<MovieDetails> GetMovieById(int MovieId)
        {
            string Url = $"{MovieId}?language=en-US";
            MovieDetails movieDetails = new MovieDetails();

            using (var client = _restClientFactory.Create())
            {
                var res = await client.GetAsync<MovieServiceResponse<MovieDetails>>(Url);

                movieDetails = res.results;
            }

            if (movieDetails == null)
                throw new ArgumentException("Bu id li kayıt bulunamadı");

            movieDetails.MovieRate = _context.MovieRates.Where(i => i.MovieId == MovieId).FirstOrDefault().Point;

            movieDetails.UserMovie = _context.UserMovie.Where(i => i.UserName == _httpContextAccessor.HttpContext.GetUserName()).FirstOrDefault();

            return movieDetails;
        }

        public async Task SendMovieAdvice(int MovieID, string Email)
        {
            if(!IsValidEmail(Email))
                throw new ValidationException("Email bilgisini yanlış girdiniz!");

            var movieDetail = await GetMovieById(MovieID);

            string Body = @$"{movieDetail.original_title}\n Film : {movieDetail.name} \n Puanı :  {movieDetail.MovieRate} \n Dil : {movieDetail.original_language}";

            Mail.MailSender(Email, _appsettings.Value.mailSettings.Subject, Body);
        }

        private bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
