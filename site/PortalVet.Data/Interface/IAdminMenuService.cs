using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalVet.Data.Interface
{
    public interface IAdminMenuService
    {
        List<AdminMenuItem> List(out int pgTotal, int pageIndex, int pageSize, Dictionary<string, object> dictionaries);
        void Remove(int id);
        AdminMenuItem Get(int v);
        List<AdminMenuItem> ListMenuPai();
        AdminMenuItem Save(AdminMenuItem adminMenuItem);
    }
}
