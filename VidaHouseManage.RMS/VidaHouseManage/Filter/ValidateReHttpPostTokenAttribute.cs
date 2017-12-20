using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidaHouseManage.PageToken;

namespace VidaHouseManage.Filter
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateReHttpPostTokenAttribute: ModelStateTempDataTransfer
    {
        public IPageTokenView PageTokenView { get; set; }
       
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateReHttpPostTokenAttribute"/> class.
        /// </summary>
        public ValidateReHttpPostTokenAttribute()
        {
            //It would be better use DI inject it.
            PageTokenView = new SessionPageTokenView();
        }

        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (!PageTokenView.TokensMatch)
            {
                //log...
                filterContext.Result = new RedirectResult("~/Account/ErrorPage");
                //throw new Exception("Invaild Http Post,Excessive Request Attempts Detected.");

            }//做token，流程开始重置token，提交时带上token，然后后台判断token比对是否一致，判断之后立刻重置，重复提交则抛出错误。

        }

       
    }


    // Following best practices as listed here for storing / restoring model data:
    // http://weblogs.asp.net/rashid/archive/2009/04/01/asp-net-mvc-best-practices-part-1.aspx#prg
    public  class ModelStateTempDataTransfer :  AuthorizeAttribute
    {
       
    }
}