using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Models.ResponseModels
{
    public class MovieDetails
    {
        public double MovieRate { get; set; }
        public string name { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }

        public UserMovie UserMovie { get; set; }
    }
}
