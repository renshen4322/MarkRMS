using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using VidaHouseManage.MvcProvider;

[assembly: OwinStartup(typeof(VidaHouseManage.Startup))]

namespace VidaHouseManage
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            //// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            //HttpConfiguration httpConfiguration = new HttpConfiguration();
            //httpConfiguration.Filters.Add(new LoginAttribute());
            
        }
    }
}
