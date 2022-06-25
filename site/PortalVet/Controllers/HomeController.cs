using PortalVet.App_Start;
using PortalVet.Data.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalVet.Controllers
{
    public class HomeController : BaseSiteController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            var list = new EmpresaService().Carregar();

            ViewData["unidades"] = list;
            return View();
        }
    }
}
