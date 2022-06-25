using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Entity
{
 
    public class AdminPermissionItem
    {
        public AdminPermissionItem()
        {
            SubItems = new List<AdminPermissionSubItem>();
        }
        public int MenuId { get; set; }
        public string Nome { get; set; }
        public string Module { get; set; }
        public List<AdminPermissionSubItem> SubItems { get; set; }
    }

    public class AdminPermissionSubItem
    {
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public string Code { get; set; }
        public string Nome { get; set; }
    }
}
