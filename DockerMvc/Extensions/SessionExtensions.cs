using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DockerMvc.Extensions
{
    public static class SessionExtensions
    {
        public static void SetInt32(this ISession session, string key, int value, string userKey)
        {
            session.SetInt32($"{userKey}_{key}", value);
        }

        public static int? GetInt32(this ISession session, string key, string userKey)
        {
            return session.GetInt32($"{userKey}_{key}");
        }

        public static void SetDateTime(this ISession session, string key, DateTime value, string userKey)
        {
            session.SetString($"{userKey}_{key}", value.ToString("o")); // Utiliza el formato ISO 8601
        }

        public static DateTime? GetDateTime(this ISession session, string key, string userKey)
        {
            var value = session.GetString($"{userKey}_{key}");
            return value == null ? (DateTime?)null : DateTime.Parse(value, null, System.Globalization.DateTimeStyles.RoundtripKind);
        }
    }

}


