using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalVet.Helper
{
    public class AppSettings
    {

        public static string UrlRootAdmin
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["UrlRootAdmin"] == null)
                {
                    return null;
                }

                return System.Configuration.ConfigurationManager.AppSettings["UrlRootAdmin"].ToString();
            }
        }


        public static string UrlRoot
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["UrlRoot"] == null)
                {
                    return null;
                }

                return System.Configuration.ConfigurationManager.AppSettings["UrlRoot"].ToString();
            }
        }
    }
}