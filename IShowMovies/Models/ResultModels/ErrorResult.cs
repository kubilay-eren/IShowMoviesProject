using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Models.ResultModels
{
    public class ErrorResult
    {
        public String ErrorId { get; set; }

        public String ErrorCode { get; set; }

        public String ErrorMessage { get; set; }

        public String ErrorDetailedMessage { get; set; }
    }
}
