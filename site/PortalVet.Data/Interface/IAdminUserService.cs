using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalVet.Data.Interface
{
    public interface IAdminUserService
    {
        AdminUserItem VerificaAdminUser(string email, string senha);
        AdminUserItem VerificaAdminUser(string cpf_cnpj);
        AdminUserItem VerificaAdminUserTelefone(string telefone);
        AdminUserItem GetByEmail(string clienteEmail);

        AdminUserItem ValidateUser(int Id);
        AdminUserItem GetById(int usuarioId);

        AdminUserItem GetById(int usuarioId, int empresaId);
        bool UpdatePasswordUser(string email, string password);

        List<AdminProfileItem> ListPerfil();
        List<AdminUserItem> List(out int pgTotal, int pageIndex, int pageSize, int pageOrderCol, string pageOrderSort, Dictionary<string, object> dictionaries);
        void RemoveUser(int id);
        void SaveImageUser(int id, string picture);

        AdminUserItem SaveMultiPerfis(AdminUserItem entidade, out string message);
        AdminUserItem CarregarUsuarioAcesso(string usuario, string senha);
        List<AdminProfileItem> ListProfiles(int usuarioId);
        List<AdminUserItem> ListUserPerfil(out int pgTotal, int pageIndex, int pageSize, int pageOrderCol, string pageOrderSort, Dictionary<string, object> dictionaries);
        AdminUserItem SaveUserPerfil(AdminUserItem adminUserItem, out string message, out int idexist);

        AdminUserItem SaveUserPerfilDireto(AdminUserItem adminUserItem, out string message, out int idexist);
        
        void DesvincularAdminUserProfile(int id, EnumAdminProfile cliente, int empresaId);
        DashReportItem DashReport(int perfilId, int empresaId, int usuarioId);
        bool SaveAdminUserEmpresas(int adminUserID, int companyId);
        string CarregarPerfisEmpresas(int usuarioId);
    }
}
