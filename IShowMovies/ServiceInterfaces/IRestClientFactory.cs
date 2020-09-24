using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IShowMovies.ServiceInterfaces
{
    public interface IRestClientFactory
    {
        HttpClient Create(String BaseUrl = "", int TimeOutSecond = 100);
    }
}
