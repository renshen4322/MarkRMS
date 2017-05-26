using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidaHouseManage.Common.Common;
using VidaHouseManage.MvcProvider;

namespace VidaHouseManage.Controllers
{
    [LoginAttribute]
    public class BaseController : Controller
    {

    }
}