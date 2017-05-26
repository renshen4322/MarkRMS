using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidaHouseManage.Common;
using VidaHouseManage.Models;
using VidaHouseManage.Entity;
using VidaHouseManage.Common.Common;
using VidaHouseManage.Service;

namespace VidaHouseManage.Controllers
{
    public class AccountController : Controller
    {
        private string tokenUrl = "https://api.vidahouse.com/Token"; //获取key url
      
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }   

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            signVlidaService _signService = new signVlidaService();
            if (ModelState.IsValid)
            {
                Dictionary<string, string> httpParams = new Dictionary<string, string>();
                httpParams.Add("client_id", new ClientClass().client_id);
                httpParams.Add("device_id", _signService.GetDiviceKey());
                httpParams.Add("grant_type", "password");
                httpParams.Add("username", model.UserName);
                httpParams.Add("password", model.Password);
                httpParams.Add("client_secret", _signService.Getclient_secret(new ClientClass(),model.UserName,model.Password));
                httpParams.Add("useSign", "0");
                httpParams.Add("debugger", "true");
                var respon = HttpRequstHelper.OperateRequest(EnumExtenstions.GetDescription(MethodType.POST), tokenUrl, httpParams, "application/x-www-form-urlencoded");
                
                TokenResponseEntity entity = new TokenResponseEntity();
                if (respon.StatusCode!=200)
                {
                    ModelState.AddModelError("", "用户名或密码不正确");
                }
                else
                {
                    if (model.RememberMe == true)
                    {
                        HttpCookie cookie = new HttpCookie("COOKIE_NAME_FOR_USER");
                        cookie.Expires = DateTime.Now.AddMinutes(30);
                        cookie["COOKIE_USER_NAME"] = model.UserName;
                        cookie["COOKIE_USER_PASS"] = model.Password;
                        System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        HttpCookie cookie = new HttpCookie("COOKIE_NAME_FOR_USER");
                        cookie.Expires = DateTime.Now.AddMinutes(-30);
                        Request.Cookies.Add(cookie);
                        cookie["COOKIE_USER_NAME"] = null;
                        cookie["COOKIE_USER_PASS"] = null;
                        System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                    }
                    var result = JsonConvert.DeserializeObject<TokenResponseEntity>(respon.Body.ToString());
                    SessionHelper.SetSession("UserName", result.UserName);
                    SessionHelper.SetSession("Token", result.Access_token);
                    return RedirectToAction("Index", "Products");
                }

            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Session.RemoveAll();
            HttpCookie cookie = new HttpCookie("COOKIE_NAME_FOR_USER");
            cookie.Expires = DateTime.Now.AddMinutes(-70);
            Request.Cookies.Add(cookie);
            cookie["COOKIE_USER_NAME"] = null;
            cookie["COOKIE_USER_PASS"] = null;
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            return RedirectToAction("Login", "Account");
        }
    }
}