using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalVet.Data.Interface
{
    public interface IAdminProfileService
    {
        List<AdminProfileItem> List(out int pageTotal, int pageIndex, int pageSize, Dictionary<string, object> dicFilter);
        int Remove(int id);
        List<AdminMenuItem> GetMenus();
        List<AdminRoleItem> GetRegras(int? menuid);
        List<AdminPermissionItem> GetPermissao(int PerfilId);
        AdminProfileItem Get(int ID);
        bool SaveAction(int menuid, string nome, string chave, string pagina);
        int RemoveRegra(int regraid, int perfilId);
        void SavePermissao(int PerfilId, int RoleId);

        AdminProfileItem Save(AdminProfileItem entidade);
    }
}
