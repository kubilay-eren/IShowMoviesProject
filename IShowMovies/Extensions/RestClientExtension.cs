using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IShowMovies.Extensions
{
    public static class RestClientExtension
    {
        public async static Task<T> GetAsync<T>(this HttpClient Client, string RequestUrl = null)
        {
            var res = await Client.GetAsync(System.IO.Path.Combine(Client.BaseAddress.ToString(), RequestUrl));

            if (res.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(res.Content.ToString());

            return JsonConvert.DeserializeObject<T>(await res.Content.ReadAsStringAsync());
        }

        public async static Task<T> SendAsync<T>(this HttpClient Client, HttpRequestMessage RequestMessage)
        {
            var res = await Client.SendAsync(RequestMessage);

            if (res.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(res.Content.ToString());

            return JsonConvert.DeserializeObject<T>(await res.Content.ReadAsStringAsync());
        }

        public async static Task<T> PostAsync<T>(this HttpClient Client, object Json, String RequestUrl = null)
        {
            var res = await Client.PostAsync(System.IO.Path.Combine(Client.BaseAddress.ToString(), RequestUrl),
                                            new StringContent(JsonConvert.SerializeObject(Json), Encoding.UTF8, "application/json"));

            if (res.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(res.Content.ToString());

            return JsonConvert.DeserializeObject<T>(await res.Content.ReadAsStringAsync());
        }

        public async static Task<T> PutAsync<T>(this HttpClient Client, object Json, String RequestUrl = null)
        {
            var res = await Client.PutAsync(System.IO.Path.Combine(Client.BaseAddress.ToString(), RequestUrl),
                                            new StringContent(JsonConvert.SerializeObject(Json), Encoding.UTF8, "application/json"));

            if (res.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(res.Content.ToString());

            return JsonConvert.DeserializeObject<T>(await res.Content.ReadAsStringAsync());
        }
    }
}
