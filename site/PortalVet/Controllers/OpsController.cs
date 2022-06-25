using PortalVet.Helper;
using PortalVet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalVet.Controllers
{
    public class OpsController : Controller
    {
        // GET: Ops
        public ActionResult Index()
        {

            var urlFinal = AppSettings.UrlRootAdmin;// Url.Action("Index", "Home", new { area = "" });
            if (SessionsAdmin.UsuarioId > 0)
            {
                urlFinal = Url.Action("Index", "Dashboard", new { area = "" });
                //var AreaName = RouteData.DataTokens["area"] != null ? RouteData.DataTokens["area"].ToString() : "";
                //var ControllerName = RouteData.Values["controller"] != null ? RouteData.Values["controller"].ToString() : "";
                //var ActionName = RouteData.Values["action"] != null ? RouteData.Values["action"].ToString() : "";
                //var idName = RouteData.Values["id"] != null ? RouteData.Values["id"].ToString() : "";


                //if (!String.IsNullOrEmpty(AreaName) &&
                //    !String.IsNullOrEmpty(ControllerName) &&
                //     !String.IsNullOrEmpty(ActionName) &&
                //     String.IsNullOrEmpty(idName))
                //{
                //    urlFinal = Url.Action(ActionName, ControllerName, new { area = AreaName });

                //}
                //else if (!String.IsNullOrEmpty(AreaName) &&
                //  !String.IsNullOrEmpty(ControllerName) &&
                //   !String.IsNullOrEmpty(ActionName) &&
                //   !String.IsNullOrEmpty(idName))
                //{
                //    urlFinal = Url.Action(ActionName, ControllerName, new { area = AreaName, id = idName });

                //}
                //else if (String.IsNullOrEmpty(AreaName) &&
                // !String.IsNullOrEmpty(ControllerName) &&
                //  !String.IsNullOrEmpty(ActionName) &&
                //  !String.IsNullOrEmpty(idName))
                //{
                //    urlFinal = Url.Action(ActionName, ControllerName, new { area = "", id = idName });

                //}
                //else if (String.IsNullOrEmpty(AreaName) &&
                //  !String.IsNullOrEmpty(ControllerName) &&
                //   !String.IsNullOrEmpty(ActionName) &&
                //   String.IsNullOrEmpty(idName))
                //{
                //    urlFinal = Url.Action(ActionName, ControllerName, new { area = "" });

                //}

                //else
                //{
                //    urlFinal = Url.Action("Index", "Dashboard", new { area = "" });
                //}

            }



            return View(new ViewOps() { URL = urlFinal });
        }
    }
}