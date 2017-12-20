using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace VidaHouseManage.Filter
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class PreventSpamFilterAttribute : ActionFilterAttribute
    {
        //处理请求之间的延迟
        public int DelayRequest = 10;
        //防止多次请求时的错误提示信息
        public string ErrorMessage = "Excessive Request Attempts Detected.";
        //出错时URL重定向
        public string RedirectURL;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        { //Store our HttpContext 
            var request = filterContext.HttpContext.Request;
            //Store our HttpContext.Cache 
            var cache = filterContext.HttpContext.Cache;

            //Grab the IP Address from the originating Request 
            var originationInfo = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;

            //Append the User Agent
            originationInfo += request.UserAgent;

            //目标URL信息
            var targetInfo = request.RawUrl + request.QueryString;

            //创建希哈值
            var hashValue = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(originationInfo + targetInfo)).Select(s => s.ToString("x2")));

            //如果希哈值在缓存中,(重复提交)
            if (cache[hashValue] != null)
            {
                //添加错误信息
                filterContext.Controller.ViewData.ModelState.AddModelError("ExcessiveRequests", ErrorMessage);
               // filterContext.Result = new RedirectResult("~/Account/ErrorPage");

            }
            else
            {
                //使用希哈值的key添加一个空对象到缓存中(决定是否过期)
                //if the Request is valid or not
                cache.Add(hashValue, hashValue, null, DateTime.Now.AddSeconds(DelayRequest), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            base.OnActionExecuting(filterContext);
        }

        
    }
}
