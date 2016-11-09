using airen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace airen
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        //protected void Session_End()
        //{
        //    var db = new airenEntities();
        //    var phone = Request.Cookies["Myuser"]["phone"];
        //    //var phone = Session["phone"].ToString();
        //    var user = db.user.Where(p => p.Phone == phone).FirstOrDefault();
        //    user.IsOnline = "离线";
        //    db.SaveChanges();
        //}

    }
}
