using IShowMovies.Models;
using IShowMovies.Models.QueryModels;
using IShowMovies.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.ServiceInterfaces
{
    public interface IMovieService
    {
        public Task<List<Movies>> GetMovies(MovieQueryParameters movieQuery);

        public Task UpdateMoviesList();

        public Task AddMovieRate(int MovieId ,String Note, int Point);

        public Task<MovieDetails> GetMovieById(int MovieId);

        public Task SendMovieAdvice(int MovieID, string Email);
    }
}
