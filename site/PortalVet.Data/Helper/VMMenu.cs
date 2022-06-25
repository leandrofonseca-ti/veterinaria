using PortalVet.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Helper
{
    public class VMMenu
    {
        public VMMenu()
        {
            SubMenus = new List<VMMenu>();
            Acessos = new List<AdminAcesso>();
            Active = true;
            TestActive = false;
        }

        public int MenuParentId { get; set; }
        public int? MenuId { get; set; }
        public string Name { get; set; }
        public string AreaName { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string FontIcon { get; set; }
        public string Page { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public bool TestActive { get; set; }
        public List<VMMenu> SubMenus { get; set; }

        public List<AdminAcesso> Acessos { get; set; }
    }
}
