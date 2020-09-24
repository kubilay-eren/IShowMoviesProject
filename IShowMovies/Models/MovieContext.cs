using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Models
{
    public class MovieContext: DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
            :base(options)
        {

        }

        public DbSet<MovieRate> MovieRates { get; set; }

        public DbSet<MovieNotes> MovieNotes { get; set; }

        public DbSet<UserMovie> UserMovie { get; set; }
    }
}
