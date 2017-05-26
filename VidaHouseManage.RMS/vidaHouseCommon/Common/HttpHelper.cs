using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VidaHouseManage.Common.Common
{

    [Serializable, JsonObject(MemberSerialization.OptOut)]
    public class SOAResponse<T>
    {
        public SOAResponse()
        {
            StatusCode = 200;
        }
        public bool Success { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }

        public T Data;
    }

    public static class SOAResponseCreate
    {
        public static SOAResponse<T> CreateResponse<T>(this T result, bool success = false, int statusCode = 200, string message = "")
        {
            return new SOAResponse<T>()
            {
                Data = result,
                StatusCode = statusCode,
                Success = success,
                Message = message
            };
        }
    }

    public class HttpHelper
    {

        #region common

        public SOAResponse<string> RequestReuse(string URL, string Json, string Method, string ContentType, Dictionary<string, string> hearderParams = null)
        {
            byte[] postData = new byte[0];

            if (!string.IsNullOrEmpty(Json))
            {
                postData = Encoding.UTF8.GetBytes(Json);
            }

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);

            httpWebRequest.ContentType = ContentType;

            httpWebRequest.Method = Method;

            httpWebRequest.Headers.Add("Authorization", "bearer " + HttpContext.Current.Session["Token"]);

            if (hearderParams != null)
            {
                foreach (KeyValuePair<string, string> item in hearderParams)
                {
                    httpWebRequest.Headers.Add(item.Key, item.Value);

                }
            }

            if (Method == "GET")
            {
                httpWebRequest.CachePolicy = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);
            }
            else
            {

                httpWebRequest.ContentLength = postData.Length;

                Stream stream = httpWebRequest.GetRequestStream();

                stream.Write(postData, 0, postData.Length);

                stream.Close();
            }

            HttpWebResponse httpWebResponse = null;
            try
            {
                using (httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        var statusCode = (int)httpWebResponse.StatusCode;

                        var result = streamReader.ReadToEnd();

                        return result.CreateResponse(success: true, statusCode: statusCode);
                    }
                }
            }
            catch (WebException ex)
            {
                var exHttpWebResponse = ex.Response as System.Net.HttpWebResponse;

                var statusCode = (int)exHttpWebResponse.StatusCode;

                StreamReader sr = new StreamReader(exHttpWebResponse.GetResponseStream());

                return sr.ReadToEnd().CreateResponse(false, statusCode);
            }
        }


        #endregion


        #region GET

        public SOAResponse<string> RequestGETReuse(string URL, string Json = "")
        {
            return RequestReuse(URL, Json, "GET", "application/json");
        }

        #endregion


        #region POST

        public SOAResponse<string> RequestPostReuse(string URL, string Json)
        {
            return RequestReuse(URL, Json, "POST", "application/json");
        }

        #endregion


        #region PUT

        public SOAResponse<string> RequestPutReuse(string URL, string Json)
        {
            return RequestReuse(URL, Json, "PUT", "application/json");
        }

        #endregion

        #region DELETE

        public SOAResponse<string> RequestDeleteReuse(string URL, string Json = "")
        {
            return RequestReuse(URL, Json, "DELETE", "application/json");
        }

        #endregion
    }
}
