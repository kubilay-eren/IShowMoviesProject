using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Models
{
    public class MovieRate
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public double Point { get; set; }
        public int RateCount { get; set; }
        public string UserName { get; set; }
    }
}
