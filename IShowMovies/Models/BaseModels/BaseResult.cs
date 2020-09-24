using IShowMovies.Models.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Models.BaseModels
{
    public class BaseResult
    {
        public BaseResult() => Success = true;

        public bool Success { get; set; }

        public ErrorResult Error { get; set; }


        public void SetError(Exception Ex)
        {
            Success = false;
            Error = new ErrorResult()
            {
                ErrorId = Guid.NewGuid().ToString(),
                ErrorCode = Ex.GetType().ToString(),
                ErrorMessage = Ex.Message,
                ErrorDetailedMessage = Ex.ToString()
            };
        }
    }
}
