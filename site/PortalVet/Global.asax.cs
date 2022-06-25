using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PortalVet
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegistraComponentes();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Web.Helpers.AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            string actualReferrer = "";
            try
            {
                Uri myReferrer = Request.UrlReferrer;
                actualReferrer = myReferrer != null ? myReferrer.ToString() : "";
            }
            catch { }

            string actualRegion = "";
            try
            {
                actualRegion = RegionInfo.CurrentRegion.DisplayName;
            }
            catch { }
            try
            {
                var error = Server.GetLastError();
                HttpUnhandledException httpUnhandledException = new HttpUnhandledException(error.Message, error);

                Exception lastError = error;
                if (error.InnerException != null)
                    lastError = error.InnerException;

                string lastErrorTypeName = lastError.GetType().ToString();
                string lastErrorMessage = lastError.Message;
                string lastErrorStackTrace = lastError.StackTrace;


                var strBody = string.Format(@"
                <div>
                  <h1>WebImagem.vet.br - An Error Has Occurred!</h1>
                  <table cellpadding=""5"" cellspacing=""0"" border=""1"">
                  <tr>
                  <td style=""text-align: right;font-weight: bold"">DateTime:</td>
                  <td>{0}</td>
                  </tr>
                <tr>
                  <td style=""text-align: right;font-weight: bold"">URL:</td>
                  <td>{1}</td>
                  </tr>                
                <tr>
                  <td style=""text-align: right;font-weight: bold"">IPAddress:</td>
                  <td>{2}</td>
                  </tr>
                  <tr>
                  <td style=""text-align: right;font-weight: bold"">User:</td>
                  <td>{3}</td>
                  </tr>
                  <tr>
                  <td style=""text-align: right;font-weight: bold"">Exception Type:</td>
                  <td>{4}</td>
                  </tr>
                  <tr>
                  <td style=""text-align: right;font-weight: bold"">Message:</td>
                  <td>{5}</td>
                  </tr>
                  <tr>
                  <td style=""text-align: right;font-weight: bold"">Stack Trace:</td>
                  <td>{6}</td>
                  </tr> 
                  <tr>
                  <td style=""text-align: right;font-weight: bold"">Url Referrer:</td>
                  <td>{7}</td>
                  </tr> 
                  <tr>
                  <td style=""text-align: right;font-weight: bold"">Region:</td>
                  <td>{8}</td>
                  </tr> 
                  </table>
                </div>
                {9}",
                 DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                 Request.RawUrl,
                 GetLanIPAddress(),
                 User.Identity.Name,
                 lastErrorTypeName,
                 lastErrorMessage,
                 lastErrorStackTrace.Replace(Environment.NewLine, "<br />"),
                 actualReferrer,
                 actualRegion,
                 httpUnhandledException.GetHtmlErrorMessage());

                Session["DataMessage"] = strBody;
                //Data.Helper.Util.SendEmail("Portal Vet - Exception", "leandrofonseca.ti@gmail.com", strBody);
            }
            catch { }

            // if (Helper.AppSettings.EnvironmentType.Equals("PROD"))
            // {
           // Response.Redirect("~/Ops");
            //}
        }

        public String GetLanIPAddress()
        {
            //The X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a 
            //client connecting to a web server through an HTTP proxy or load balancer
            String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }
    }
}
