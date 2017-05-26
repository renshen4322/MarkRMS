using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VidaHouseManage.Common.Common
{
    public class CacheHelper
    {
        public static T Get<T>(string key, Func<T> func)
        {
            if (HttpRuntime.Cache[key] == null)
            {
                Insert(key, func, 5);
            }

            return (T)HttpRuntime.Cache[key];
        }

        public static T Get<T>(string key, Func<T> func, int expirationMin)
        {
            if (HttpRuntime.Cache[key] == null)
            {
                Insert(key, func, expirationMin);
            }

            return (T)HttpRuntime.Cache[key];
        }

        public static T Get<T>(string key)
        {
            Object value = HttpRuntime.Cache[key];

            if (value == null)
                return default(T);
            else
                return (T)value;
        }

        public static List<T> ListGet<T>(string key)
        {
            Object value = HttpRuntime.Cache[key];

            if (value == null)
                return default(List<T>);
            else
                return (List<T>)value;
        }

        public static void Insert<T>(string key, T value, int expirationMin)
        {
            HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(expirationMin), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public static void Insert<T>(string key, List<T> value, int expirationMin)
        {
            HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(expirationMin), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public static void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        private static void Insert<T>(string key, Func<T> func, int expirationMin)
        {
            HttpRuntime.Cache.Insert(key, func(), null, DateTime.Now.AddMinutes(expirationMin), System.Web.Caching.Cache.NoSlidingExpiration);
        }

    }
}
