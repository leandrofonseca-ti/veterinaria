using Microsoft.AspNet.SignalR;
using PortalVet.Data.Helper;
using PortalVet.Data.Service;
using PortalVet.Helper;
using PortalVet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PortalVet.App_Start
{
    [RequireSSL]
    public class BaseSiteController : Controller
    {
        public string rootUrl = string.Empty;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            ViewData["rootUrl"] = "";
            ViewData["rootUrlDominio"] = Helper.AppSettings.UrlRoot;

            var context = requestContext.HttpContext;

            if (context != null)
            {
                if (context.Request.IsLocal)
                {
                    rootUrl = string.Format("{0}://{1}{2}{3}",
                                               "https",
                                               context.Request.Url.Host,
                                               context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port,
                                               context.Request.ApplicationPath);

                    rootUrl = Helper.AppSettings.UrlRoot;
                }
                else
                {
                    rootUrl = string.Format("{0}://{1}{2}{3}",
                                           "https",
                                           context.Request.Url.Host,
                                           context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port,
                                           context.Request.ApplicationPath);
                }
                if (!rootUrl.EndsWith("/"))
                {
                    rootUrl += "/";
                }

                ViewData["rootUrl"] = rootUrl;
            }

            base.Initialize(requestContext);
        }

    }
}