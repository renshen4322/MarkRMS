using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidaHouseManage.Common.Common;

namespace VidaHouseManage.MvcProvider
{
    public class LoginAttribute: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            bool bl = true;

            if (HttpContext.Current.Session["Token"] == null)
            {
                bl = false;

            };

            var loginUrl = string.IsNullOrEmpty(ConfigurationManager.AppSettings["LoginUrl"]) ? string.Empty : ConfigurationManager.AppSettings["LoginUrl"].ToString();

            if (!bl)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest() == false)
                {
                    string LoginUrl = loginUrl + "?returnurl=" + HttpUtility.UrlEncode(filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri);

                    filterContext.Result = new RedirectResult(LoginUrl);

                    return;
                }
                else
                {
                    var Result = new ServiceResult
                    {
                        Code = -100,
                        Success = false,
                        Data = loginUrl + "?returnurl=" + HttpUtility.UrlEncode(filterContext.RequestContext.HttpContext.Request.UrlReferrer.AbsoluteUri)
                    };

                    filterContext.Result = new JsonResult() { Data = Result };

                    return;
                }
            }
        }

    }
}