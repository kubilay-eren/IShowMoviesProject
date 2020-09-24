using IShowMovies.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Extensions
{
    public static class SessionExtensions
    {
        private static JsonSerializerSettings jsonSettings;

        static SessionExtensions()
        {
            jsonSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public static void SetValue<T>(this ISession Session, String Name, T Value)
        {
            if (Session == null) throw new ArgumentNullException(nameof(Session));

            String strVal = JsonConvert.SerializeObject(Value);
            Session.SetString(Name, strVal);
        }

        public static void ClearValue(this ISession Session, String Name)
        {
            Session.Remove(Name);
        }

        public static T GetValue<T>(this ISession Session, String Name)
        {
            if (Session == null) throw new ArgumentNullException(nameof(Session));

            String strVal = Session.GetString(Name);

            if (String.IsNullOrEmpty(strVal))
                return default(T);

            return JsonConvert.DeserializeObject<T>(strVal, jsonSettings);
        }



        public static void SetMovie(this ISession Session, List<Movies> Movies) => Session?.SetValue("MOVIES", Movies);
        public static List<Movies> GetMovie(this ISession Session) => Session?.GetValue<List<Movies>>("MOVIES");
        public static void ClearMovie(this ISession Session) => Session?.ClearValue("MOVIES");

        //public static string GetUserName(this ISession Session) => Session?.GetValue<string>("USERNAME");
        //public static void SetUserName(this ISession Session, string UserName) => Session?.SetValue("USERNAME", UserName);
    }
}
