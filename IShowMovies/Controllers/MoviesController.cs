using IShowMovies.Models;
using IShowMovies.Models.BaseModels;
using IShowMovies.Models.QueryModels;
using IShowMovies.Models.ResponseModels;
using IShowMovies.Models.ResultModels;
using IShowMovies.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class MoviesController: ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("GetMovies")]
        public async Task<Result<List<Movies>>> GetMovies([FromQuery] MovieQueryParameters queryParameters)
        {

            Result<List<Movies>> result = new Result<List<Movies>>
            {
                Value = await _movieService.GetMovies(queryParameters)
            };

            return result;
        }

        [HttpGet("AddMovieRate")]
        public async Task<BaseResult> AddMovieRate(int MovieId,String Note, int Point)
        {
            if (MovieId == 0)
                throw new ArgumentException("MovieId bilgisi boş olamaz");
            if (Point < 1 || Point > 10)
                throw new ArgumentException("Puan 1 ile 10 arsında tam sayı olmalıdır.");

            BaseResult result = new BaseResult();

            await _movieService.AddMovieRate(MovieId ,Note, Point);

            return result;

        }

        [HttpGet("GetMovieById")]
        public async Task<Result<MovieDetails>> GetMovieById(int MovieId)
        {
            Result<MovieDetails> result = new Result<MovieDetails>()
            {
                Value = await _movieService.GetMovieById(MovieId)
            };

            return result;
        }

        [HttpGet("SendMovieAdvice")]
        public async Task<BaseResult> SendMovieAdvice(int MovieId, string Email)
        {
            BaseResult result = new BaseResult();

            await _movieService.SendMovieAdvice(MovieId, Email);

            return result;
        }
    }
}
