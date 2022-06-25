using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalVet.Data.Interface
{
    public interface IAdminCompanyService
    {

        AdminCompanyItem Get(int Id);
        List<AdminCompanyItem> List(out int pgTotal, int pageIndex, int pageSize, int pageOrderCol, string pageOrderSort, Dictionary<string, object> dictionaries);
        void Remove(int id);

       AdminCompanyItem CarregarEmpresaAdmin(int adminUserId, int empresaId);
        AdminCompanyItem Save(AdminCompanyItem entidade);
        void SaveImageUser(int id, string picture);
    }
}
