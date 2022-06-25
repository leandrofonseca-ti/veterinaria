using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Entity
{
    public class ApiRetornoPadrao
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public string Action { get; set; }
        public Object Data { get; set; }
    }
}
