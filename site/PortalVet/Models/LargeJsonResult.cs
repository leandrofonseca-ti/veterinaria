using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalVet.Models
{
    public class LargeJsonResult : JsonResult
    {

        public LargeJsonResult()
        {
            MaxJsonLength = Int32.MaxValue;

        }
    }
}