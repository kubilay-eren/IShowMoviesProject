using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Models
{
    public class UserMovie
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int MovieId { get; set; }
        public int Point { get; set; }
        public string Note { get; set; }
    }
}
