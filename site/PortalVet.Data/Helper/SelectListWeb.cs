using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Helper
{
    [Serializable]
    public class SelectListWeb
    {
        public string ID { get; set; }

        public int Total { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
