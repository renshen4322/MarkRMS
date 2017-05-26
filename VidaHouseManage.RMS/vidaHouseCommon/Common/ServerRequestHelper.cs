using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace VidaHouseManage.Common
{
    public class ServerRequestHelper
    {
        #region GET
        /// <summary>
        /// 接口请求(Get)
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public Dictionary<string, object> RequestGet(string URL)
        {
            string ObtainText = RequestGetReuse(URL);
            Dictionary<string, object> List = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(ObtainText);
            return List;
        }
        public string RequestGetReuse(string URL)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            httpWebRequest.Method = "GET";
            httpWebRequest.CachePolicy = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);
            httpWebRequest.Headers.Add("Authorization", "bearer " + HttpContext.Current.Session["Token"]);

            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd(); 
                }
            }
        }
        #endregion
        #region PUT
        public string RequestPutReuse(string URL, string Json)
        {
            return RequestReuse(URL, Json, "PUT", "application/json");
        }
        #endregion
        #region POST
        /// <summary>
        /// 接口请求(Post)
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="ParamList"></param>
        /// <returns></returns>
        public Dictionary<string, object> RequestPost(string URL, Dictionary<string, string> ParamList)
        {
            string ObtainText = RequestPostReuse(URL, ParamList);
            Dictionary<string, object> List = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(ObtainText);
            return List;
        }
        /// <summary>
        /// 接口请求(Post)(泛型)
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="ParamList"></param>
        /// <returns></returns>
        public List<T> RequestPost<T>(string URL, Dictionary<string, string> ParamList)
        {
            string ObtainText = RequestPostReuse(URL, ParamList);
            List<T> List = new JavaScriptSerializer().Deserialize<List<T>>(ObtainText);
            return List;
        }
        public string RequestPostReuse(string URL, Dictionary<string, string> ParamList)
        {
            return RequestReuse(URL, ParamList, "POST", "application/x-www-form-urlencoded");
        }
        public string RequestPostReusestr(string URL, string Json)
        {
            return RequestReuse(URL, Json, "POST", "application/json");
        }
        #endregion
        #region DELETE
        /// <summary>
        /// 接口请求(DELETE)
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="Json"></param>
        /// <returns></returns>
        public string RequestDeleteReuse(string URL, string Json = "")
        {
            return RequestReuse(URL, Json, "DELETE", "application/json");
        }
        #endregion
        #region RequestReuse
        public string RequestReuse(string URL, string Json, string Method, string ContentType)
        {
            byte[] postData = new byte[0];
            if (Json != null)
            {
                postData = Encoding.UTF8.GetBytes(Json);
            }
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Method = Method;          
            httpWebRequest.Headers.Add("Authorization", "bearer " + HttpContext.Current.Session["Token"]);
            httpWebRequest.ContentLength = postData.Length;
            Stream stream = httpWebRequest.GetRequestStream();
            stream.Write(postData, 0, postData.Length);
            stream.Close();
            HttpWebResponse httpWebResponse = null;
            try
            {
                using (httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                return sr.ReadToEnd();
            }
        }
        public string RequestReuse(string URL, Dictionary<string, string> paramList, string method, string ContentType)
        {
            string postString = null;
            foreach (var item in paramList)
            {
                postString += WebUtility.UrlEncode(item.Key) + "=" + WebUtility.UrlEncode(item.Value) + "&";
            }
            postString = postString != null ? postString.Substring(0, postString.Length - 1) : "";
            return RequestReuse(URL, postString, method, ContentType);
        }

        #endregion
     
    }
}
