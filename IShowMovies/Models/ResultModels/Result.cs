using IShowMovies.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Models.ResultModels
{
    public class Result<T> : BaseResult
    {
        public Result() { }

        public Result(T Value) : base() => this.Value = Value;

        public T Value { get; set; }
    }
}
