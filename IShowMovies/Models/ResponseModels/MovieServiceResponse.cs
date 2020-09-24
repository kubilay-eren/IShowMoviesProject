using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Models.ResponseModels
{
    public class MovieServiceResponse<T>
    {
        public int page { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
        public T results { get; set; }
    }
}
