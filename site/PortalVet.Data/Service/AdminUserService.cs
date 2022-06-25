using PortalVet.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using PortalVet.Data.Interface;
using PortalVet.Data.Helper;

namespace PortalVet.Data.Service
{
    public class AdminUserService : ProviderConnection, IAdminUserService
    {

        public AdminUserItem ValidateUser(int Id)
        {
            AdminUserItem entity = new AdminUserItem();
            try
            {
                StringBuilder sbSQL = new StringBuilder();


                //deixa acesso de despachante igual de cliente
                sbSQL.AppendLine(" SELECT distinct AdminUser.Id,  AdminUser.Nome, AdminUser.Email, AdminUser.Imagem FROM AdminUser  ");
                sbSQL.AppendLine(" JOIN AdminUserProfile on AdminUserProfile.UserId = AdminUser.ID  ");
                sbSQL.AppendLine(" JOIN AdminProfile on AdminProfile.ID = AdminUserProfile.PROFILEID ");                
                sbSQL.AppendLine(" WHERE  ");
                sbSQL.AppendLine("  AdminUser.ID = @ID AND AdminUser.Ativo = 1 ");
                sbSQL.AppendLine("  order by AdminProfile.ID LIMIT 0, 1 ");
                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, Id));
                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            entity = new AdminUserItem();
                            entity.Id = Int32.Parse(dr["ID"].ToString());
                            entity.Nome = dr["Nome"].ToString();
                            if (dr["Imagem"] != DBNull.Value)
                                entity.Imagem = dr["Imagem"].ToString();

                            entity.Email = dr["Email"].ToString();

                        }
                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }

            //if (entity.Id > 0)
           // {
             //   entity.Perfis = CarregarPerfisListAdmin(entity.ID);
              //  entity.Empresas = CarregarEmpresasListAdmin(entity.ID);
            //}
            return entity;
        }




        public AdminUserItem GetByEmail(string email)
        {
            AdminUserItem entidade = new AdminUserItem();
            try
            {

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.AppendLine(" SELECT * FROM   ");
                sbSQL.AppendLine(" AdminUser ");
                sbSQL.AppendLine(" WHERE Email = @EMAIL ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@EMAIL", MySqlDbType.VarChar, email));

                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Email = dr["Email"].ToString();

                            if (dr["Sobrenome"] != DBNull.Value)
                                entidade.Sobrenome = dr["Sobrenome"].ToString();

                            if (dr["Imagem"] != DBNull.Value)
                                entidade.Imagem = dr["Imagem"].ToString();

                            if (dr["Telefone"] != DBNull.Value)
                                entidade.Telefone = dr["Telefone"].ToString();


                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }

            if (entidade.Id > 0)
            {
                entidade.Empresas = CarregarCompanies(entidade.Id);

                entidade.Perfis = CarregarProfiles(entidade.Id);
            }

            return entidade;
        }

        public AdminUserItem GetById(int usuarioid)
        {
            AdminUserItem entidade = new AdminUserItem();
            try
            {

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.AppendLine(" SELECT * FROM   ");
                sbSQL.AppendLine(" AdminUser ");
                sbSQL.AppendLine(" WHERE Id = @ID ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, usuarioid));

                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Email = dr["Email"].ToString();

                            if (dr["Sobrenome"] != DBNull.Value)
                                entidade.Sobrenome = dr["Sobrenome"].ToString();

                            if (dr["Imagem"] != DBNull.Value)
                                entidade.Imagem = dr["Imagem"].ToString();

                            if (dr["Telefone"] != DBNull.Value)
                                entidade.Telefone = dr["Telefone"].ToString();

                            if (dr["Ativo"] != DBNull.Value)
                            {
                                entidade.Active = false;
                                switch (dr["Ativo"].ToString().ToLower())
                                {
                                    case "1":
                                    case "true":
                                        entidade.Active = true;
                                        break;

                                }
                            }
                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }

            if (entidade.Id > 0)
            {
                entidade.Empresas = CarregarCompanies(entidade.Id);

                entidade.Perfis = CarregarProfiles(entidade.Id);
            }

            return entidade;
        }



        public AdminUserItem GetById(int usuarioid, int empresaId)
        {
            AdminUserItem entidade = new AdminUserItem();
            try
            {

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.AppendLine(" SELECT AdminUser.* FROM   ");
                sbSQL.AppendLine(" AdminUser ");
                sbSQL.AppendLine(" JOIN adminusercompany ON adminusercompany.UserId  = adminuser.ID ");
                sbSQL.AppendLine(" WHERE adminusercompany.CompanyId = @COMPANYID and AdminUser.Id = @ID ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@COMPANYID", MySqlDbType.Int32, empresaId));
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, usuarioid));

                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Email = dr["Email"].ToString();

                            if (dr["Sobrenome"] != DBNull.Value)
                                entidade.Sobrenome = dr["Sobrenome"].ToString();

                            if (dr["CPFCNPJ"] != DBNull.Value)
                                entidade.CPFCNPJ = dr["CPFCNPJ"].ToString();

                            if (dr["Imagem"] != DBNull.Value)
                                entidade.Imagem = dr["Imagem"].ToString();

                            if (dr["Telefone"] != DBNull.Value)
                                entidade.Telefone = dr["Telefone"].ToString();

                            if (dr["Telefone2"] != DBNull.Value)
                                entidade.Telefone2 = dr["Telefone2"].ToString();

                            if (dr["Ativo"] != DBNull.Value)
                            {
                                entidade.Active = false;
                                switch (dr["Ativo"].ToString().ToLower())
                                {
                                    case "1":
                                    case "true":
                                        entidade.Active = true;
                                        break;

                                }
                            }
                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }

            if (entidade.Id > 0)
            {
                entidade.Empresas = CarregarCompanies(entidade.Id);

                entidade.Perfis = CarregarProfiles(entidade.Id);
            }

            return entidade;
        }

        public List<int> GetProfiles(int usuarioid)
        {
            List<int> list = new List<int>();

            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(" SELECT AdminProfile.Id ");
                sbSQL.AppendLine(" FROM  AdminProfile ");
                sbSQL.AppendLine(" join AdminUserProfile on AdminUserProfile.ProfileId = AdminProfile.Id ");
                sbSQL.AppendLine(" WHERE AdminUserProfile.UserId = @USERID ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@USERID", MySqlDbType.Int32, usuarioid));

                        IDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            if (dr["Id"] != DBNull.Value)
                                list.Add(Int32.Parse(dr["Id"].ToString()));
                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }

            return list;
        }



        public List<AdminAcesso> GetAllByName(int perfilid, int empresaid)
        {
            List<AdminAcesso> list = new List<AdminAcesso>();

            try
            {

                StringBuilder sbSQL = new StringBuilder();

                if (empresaid == 0)
                {
                    sbSQL.AppendLine(" SELECT Chave, Pagina, MenuId,   ");
                    sbSQL.AppendLine(" MenuPaiId,  ");
                    sbSQL.AppendLine(" MenuPaiNome, ");
                    sbSQL.AppendLine(" Nome,  ");
                    sbSQL.AppendLine(" Ordem,  ");
                    sbSQL.AppendLine(" Ativo,  ");
                    sbSQL.AppendLine(" AtivoTeste,  ");
                    sbSQL.AppendLine(" Caminho,  ");
                    sbSQL.AppendLine(" AreaName,  ");
                    sbSQL.AppendLine(" ControllerName,  ");
                    sbSQL.AppendLine(" ActionName,  ");
                    sbSQL.AppendLine(" Tipo,  ");
                    sbSQL.AppendLine(" IconeCss,  ");
                    sbSQL.AppendLine(" Modulo FROM   ");
                    sbSQL.AppendLine(" (   ");
                    sbSQL.AppendLine(" SELECT distinct P.PROFILEID as PerfilId, R.Key as Chave, R.Page as Pagina, R.MenuId as MenuId, M.AreaName, M.ControllerName, M.ActionName,  MP.NAME as MenuPaiNome, M.PARENTID as MenuPaiId, M.Name as Nome, M.ORDERNUMBER as Ordem, M.Active as Ativo, 0 as AtivoTeste, M.Path as Caminho, null as Tipo, M.Icon as IconeCss, M.Module as Modulo  ");
                    sbSQL.AppendLine(" FROM AdminRole R    ");
                    sbSQL.AppendLine(" JOIN AdminMenu M ON M.Id = R.MenuId ");
                    sbSQL.AppendLine(" LEFT JOIN AdminMenu MP ON MP.Id = M.PARENTID ");
                    sbSQL.AppendLine(" JOIN AdminPermission P ON R.Id = P.ROLEID    ");
                    sbSQL.AppendLine(" WHERE M.Active = 1 and R.MenuId is not null  ");
                    sbSQL.AppendLine(" union   ");
                    sbSQL.AppendLine(" SELECT distinct  P.PROFILEID as PerfilId, R.Key as Chave, R.Page as Pagina, R.MenuId as MenuId, M.AreaName, M.ControllerName, M.ActionName,'' as MenuPaiNome, null as MenuPaiId, null as Nome, null as Ordem, null as Ativo, 0 as AtivoTeste, null as Caminho, null as Tipo, null as IconeCss, null as Modulo FROM AdminRole R    ");
                    sbSQL.AppendLine(" JOIN AdminMenu M ON M.Id = R.MenuId    ");
                    sbSQL.AppendLine(" LEFT JOIN AdminPermission P ON R.Id = P.ROLEID ");
                    sbSQL.AppendLine(" WHERE M.ACTIVE = 1 and R.MenuId is null   ");
                    sbSQL.AppendLine(" ) as TB  ");
                    sbSQL.AppendLine(" WHERE PerfilId = @ID ORDER BY Ordem ");
                }
                else
                {
                    sbSQL.AppendLine(" SELECT Chave, Pagina, MenuId,   ");
                    sbSQL.AppendLine(" MenuPaiId,  ");
                    sbSQL.AppendLine(" MenuPaiNome, ");
                    sbSQL.AppendLine(" Nome,  ");
                    sbSQL.AppendLine(" Ordem,  ");
                    sbSQL.AppendLine(" Ativo,  ");
                    sbSQL.AppendLine(" AtivoTeste,  ");
                    sbSQL.AppendLine(" Caminho,  ");
                    sbSQL.AppendLine(" AreaName,  ");
                    sbSQL.AppendLine(" ControllerName,  ");
                    sbSQL.AppendLine(" ActionName,  ");
                    sbSQL.AppendLine(" Tipo,  ");
                    sbSQL.AppendLine(" IconeCss,  ");
                    sbSQL.AppendLine(" Modulo FROM   ");
                    sbSQL.AppendLine(" (   ");
                    sbSQL.AppendLine(" SELECT distinct P.PROFILEID as PerfilId, R.Key as Chave, R.Page as Pagina, R.MenuId as MenuId, M.AreaName, M.ControllerName, M.ActionName,  MP.NAME as MenuPaiNome, M.PARENTID as MenuPaiId, M.Name as Nome, M.ORDERNUMBER as Ordem, M.Active as Ativo, CASE (select count(*)  from AdminMenuTeste where EmpresaId = @EMPRESAID and MenuId = M.ID) WHEN 0 THEN 0 ELSE 1 END as AtivoTeste, M.Path as Caminho, null as Tipo, M.Icon as IconeCss, M.Module as Modulo  ");
                    sbSQL.AppendLine(" FROM AdminRole R    ");
                    sbSQL.AppendLine(" JOIN AdminMenu M ON M.Id = R.MenuId ");
                    sbSQL.AppendLine(" LEFT JOIN AdminMenu MP ON MP.Id = M.PARENTID ");
                    sbSQL.AppendLine(" JOIN AdminPermission P ON R.Id = P.ROLEID    ");
                    sbSQL.AppendLine(@" WHERE (M.ACTIVE = 1 or 
                                         (select count(*)  from AdminMenuTeste where EmpresaId = @EMPRESAID and MenuId = M.ID) > 0) and R.MenuId is not null  ");
                    sbSQL.AppendLine(" union   ");
                    sbSQL.AppendLine(" SELECT distinct  P.PROFILEID as PerfilId, R.Key as Chave, R.Page as Pagina, R.MenuId as MenuId, M.AreaName, M.ControllerName, M.ActionName,'' as MenuPaiNome, null as MenuPaiId, null as Nome, null as Ordem, null as Ativo, CASE (select count(*)  from AdminMenuTeste where EmpresaId = @EMPRESAID and MenuId = M.ID) WHEN 0 THEN 0 ELSE 1 END as AtivoTeste, null as Caminho, null as Tipo, null as IconeCss, null as Modulo FROM AdminRole R    ");
                    sbSQL.AppendLine(" JOIN AdminMenu M ON M.Id = R.MenuId    ");
                    sbSQL.AppendLine(" LEFT JOIN AdminPermission P ON R.Id = P.ROLEID ");
                    sbSQL.AppendLine(@" WHERE (M.ACTIVE = 1 or 
                                         (select count(*)  from AdminMenuTeste where EmpresaId = @EMPRESAID and MenuId = M.ID) > 0) and R.MenuId is null   ");
                    sbSQL.AppendLine(" ) as TB  ");
                    sbSQL.AppendLine(" WHERE PerfilId = @ID ORDER BY Ordem ");
                }
                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, perfilid));
                        if (empresaid > 0)
                        {
                            cmd.Parameters.Add(_DBBuildParameter(cmd, "@EMPRESAID", MySqlDbType.Int32, empresaid));
                        }
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            AdminAcesso entidade = new AdminAcesso();
                            if (dr["MenuId"] != DBNull.Value)
                                entidade.MenuId = Int32.Parse(dr["MenuId"].ToString());
                            else
                                entidade.MenuId = -1;


                            if (dr["MenuPaiId"] != DBNull.Value)
                                entidade.ParentId = Int32.Parse(dr["MenuPaiId"].ToString());

                            if (dr["MenuPaiNome"] != DBNull.Value)
                                entidade.ParentNome = dr["MenuPaiNome"].ToString();

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Chave"] != DBNull.Value)
                                entidade.Chave = dr["Chave"].ToString();

                            if (dr["IconeCss"] != DBNull.Value)
                                entidade.IconeCss = dr["IconeCss"].ToString();

                            if (dr["Caminho"] != DBNull.Value)
                                entidade.Path = dr["Caminho"].ToString();

                            if (dr["AreaName"] != DBNull.Value)
                                entidade.Area = dr["AreaName"].ToString();

                            if (dr["ControllerName"] != DBNull.Value)
                                entidade.Controller = dr["ControllerName"].ToString();

                            if (dr["ActionName"] != DBNull.Value)
                                entidade.Action = dr["ActionName"].ToString();

                            if (dr["Pagina"] != DBNull.Value)
                                entidade.Pagina = dr["Pagina"].ToString();

                            if (dr["Tipo"] != DBNull.Value)
                                entidade.Tipo = Int32.Parse(dr["Tipo"].ToString());

                            if (dr["Modulo"] != DBNull.Value)
                                entidade.Modulo = dr["Modulo"].ToString();

                            if (dr["Ordem"] != DBNull.Value)
                                entidade.Ordem = Int32.Parse(dr["Ordem"].ToString());

                            if (dr["Ativo"] != DBNull.Value)
                            {
                                entidade.Ativo = false;
                                switch (dr["Ativo"].ToString().ToLower())
                                {
                                    case "1":
                                    case "true":
                                        entidade.Ativo = true;
                                        break;

                                }
                            }

                            if (dr["AtivoTeste"] != DBNull.Value)
                            {
                                if (dr["AtivoTeste"].ToString() == "1")
                                {
                                    entidade.Ativo = true;
                                    entidade.AtivoTeste = true;
                                }
                            }

                            list.Add(entidade);

                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }


            return list;
        }

        public AdminUserItem VerificaAdminUser(string email, string senha)
        {
            AdminUserItem entidade = new AdminUserItem();
            try
            {

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.AppendLine(" SELECT * FROM   ");
                sbSQL.AppendLine(" AdminUser ");
                sbSQL.AppendLine(" WHERE Email = @EMAIL and Senha = @SENHA ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@EMAIL", MySqlDbType.VarChar, email));

                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@SENHA", MySqlDbType.VarChar, Util.GerarHashMd5(senha)));
                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Email = dr["Email"].ToString();

                            if (dr["Sobrenome"] != DBNull.Value)
                                entidade.Sobrenome = dr["Sobrenome"].ToString();

                            if (dr["Imagem"] != DBNull.Value)
                                entidade.Imagem = dr["Imagem"].ToString();

                            if (dr["Telefone"] != DBNull.Value)
                                entidade.Telefone = dr["Telefone"].ToString();


                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }

            if (entidade.Id > 0)
            {
                entidade.Empresas = CarregarCompanies(entidade.Id);

                entidade.Perfis = CarregarProfiles(entidade.Id);
            }

            return entidade;
        }


        public AdminUserItem VerificaAdminUserTelefone(string telefone)
        {
            AdminUserItem entidade = new AdminUserItem();
            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(" SELECT AdminUser.* FROM   ");
                sbSQL.AppendLine(" AdminUser ");
                sbSQL.AppendLine(" join AdminUserProfile on AdminUserProfile.UserId = AdminUser.Id  ");
                sbSQL.AppendLine(" WHERE AdminUser.Telefone = @Telefone and AdminUserProfile.ProfileId = @PROFILEID  ");

                using (var connection = _DBGetConnection())
                {
                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var aux = telefone.Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", "");
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@Telefone", MySqlDbType.VarChar, aux));
                        int pid = EnumAdminProfile.Cliente.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@PROFILEID", MySqlDbType.Int32, pid));

                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Email = dr["Email"].ToString();

                            if (dr["Sobrenome"] != DBNull.Value)
                                entidade.Sobrenome = dr["Sobrenome"].ToString();

                            if (dr["Imagem"] != DBNull.Value)
                                entidade.Imagem = dr["Imagem"].ToString();

                            if (dr["Telefone"] != DBNull.Value)
                                entidade.Telefone = dr["Telefone"].ToString();


                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }

            if (entidade.Id > 0)
            {
                entidade.Empresas = CarregarCompanies(entidade.Id);

                entidade.Perfis = CarregarProfiles(entidade.Id);
            }

            return entidade;
        }

        public AdminUserItem VerificaAdminUser(string cpf_cnpj)
        {
            AdminUserItem entidade = new AdminUserItem();
            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(" SELECT AdminUser.* FROM   ");
                sbSQL.AppendLine(" AdminUser ");
                sbSQL.AppendLine(" join AdminUserProfile on AdminUserProfile.UserId = AdminUser.Id  ");
                sbSQL.AppendLine(" WHERE AdminUser.CPFCNPJ = @CPFCNPJ and AdminUserProfile.ProfileId = @PROFILEID  ");

                using (var connection = _DBGetConnection())
                {
                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var aux = cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", "");
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@CPFCNPJ", MySqlDbType.VarChar, aux));
                        int pid = EnumAdminProfile.Cliente.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@PROFILEID", MySqlDbType.Int32, pid));

                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Email = dr["Email"].ToString();

                            if (dr["Sobrenome"] != DBNull.Value)
                                entidade.Sobrenome = dr["Sobrenome"].ToString();

                            if (dr["Imagem"] != DBNull.Value)
                                entidade.Imagem = dr["Imagem"].ToString();

                            if (dr["Telefone"] != DBNull.Value)
                                entidade.Telefone = dr["Telefone"].ToString();


                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }

            if (entidade.Id > 0)
            {
                entidade.Empresas = CarregarCompanies(entidade.Id);

                entidade.Perfis = CarregarProfiles(entidade.Id);
            }

            return entidade;
        }

        private List<AdminCompanyItem> CarregarCompanies(int id)
        {
            var entidades = new List<AdminCompanyItem>();
            try
            {

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.AppendLine(" SELECT Company.* FROM   ");
                sbSQL.AppendLine(" AdminUserCompany ");
                sbSQL.AppendLine(" join Company on Company.Id = AdminUserCompany.CompanyId ");
                sbSQL.AppendLine(" WHERE AdminUserCompany.UserId = @USERID ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@USERID", MySqlDbType.Int32, id));

                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            var entidade = new AdminCompanyItem();

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            //if (dr["Email"] != DBNull.Value)
                            //    entidade.Email = dr["Email"].ToString();

                            //if (dr["Sobrenome"] != DBNull.Value)
                            //    entidade.Sobrenome = dr["Sobrenome"].ToString();

                            if (dr["Imagem"] != DBNull.Value)
                                entidade.Imagem = dr["Imagem"].ToString();

                            //if (dr["Telefone"] != DBNull.Value)
                            //    entidade.Telefone = dr["Telefone"].ToString();

                            entidades.Add(entidade);
                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }
            return entidades;
        }


        public EmpresaItem CarregarCompany(int companyid)
        {
            var entidade = new EmpresaItem();
            try
            {

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.AppendLine(" SELECT Company.* FROM   ");
                sbSQL.AppendLine(" AdminUserCompany ");
                sbSQL.AppendLine(" join Company on Company.Id = AdminUserCompany.CompanyId ");
                sbSQL.AppendLine(" WHERE Company.Id = @ID ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, companyid));

                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Imagem"] != DBNull.Value)
                                entidade.Imagem = dr["Imagem"].ToString();

                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }
            return entidade;
        }

        private List<AdminProfileItem> CarregarProfiles(int id)
        {
            var entidades = new List<AdminProfileItem>();
            try
            {

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.AppendLine(" SELECT AdminProfile.* FROM   ");
                sbSQL.AppendLine(" AdminUserProfile ");
                sbSQL.AppendLine(" join AdminProfile on AdminProfile.Id = AdminUserProfile.ProfileId ");
                sbSQL.AppendLine(" WHERE AdminUserProfile.UserId = @USERID ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@USERID", MySqlDbType.Int32, id));

                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            var entidade = new AdminProfileItem();

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            entidades.Add(entidade);
                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }
            return entidades;
        }

        public List<AdminUserItem> List(out int pageTotal, int pageIndex, int pageSize, int orderCol, string orderSort, Dictionary<string, object> dicFilter)
        {
            List<AdminUserItem> listagem = new List<AdminUserItem>();
            pageTotal = 0;


            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder clauseWhere = new StringBuilder();
                    if (dicFilter != null)
                        if (dicFilter.Count > 0)
                        {
                            foreach (var item in dicFilter)
                            {
                                if (clauseWhere.Length > 0)
                                {
                                    if (item.Value.GetType() == typeof(int))
                                        clauseWhere.AppendFormat(" AND {0} = {1}", item.Key, item.Value);

                                    if (item.Value.GetType() == typeof(string))
                                        clauseWhere.AppendFormat(" AND {0} like '%{1}%'", item.Key, item.Value);
                                }
                                else
                                {
                                    if (item.Value.GetType() == typeof(int))
                                        clauseWhere.AppendFormat(" {0} = {1}", item.Key, item.Value);

                                    if (item.Value.GetType() == typeof(string))
                                        clauseWhere.AppendFormat(" {0} like '%{1}%'", item.Key, item.Value);
                                }
                            }
                        }

                    StringBuilder sbSQLPaged = new StringBuilder();




                    //< th class="col_sort" col_order="1">Empresa</th>
                    //<th class="col_sort" col_order="2">Nome</th>
                    //<th class="col_sort" col_order="3">Usuário</th>
                    //<th class="col_sort" col_order="4">Perfil</th>
                    //<th class="col_sort" col_order="5">Ativo</th>
                    //<th class="col_sort" col_order="6">Login</th>

                    var strOrder = "TT.NOME";

                    if (orderCol > 0)
                    {
                        switch (orderCol)
                        {
                            case 1:
                                strOrder = $"TT.NOME {orderSort}";
                                break;
                            case 2:
                                strOrder = $"TT.Nome {orderSort}";
                                break;
                            case 3:
                                strOrder = $"TT.EMAIL {orderSort}";
                                break;
                            case 4:
                                strOrder = $"TT.Active {orderSort}";
                                //strOrder = $"AdminProfile.PROFILE_NAME {orderSort}";
                                break;
                                //case 5:
                                //    strOrder = $"Active {orderSort}";
                                //    break;
                        }
                    }
                    string queryIn = "";
                    if (clauseWhere.Length > 0)
                    {
                        queryIn = $" WHERE {clauseWhere.ToString()} ";
                    }


                    var strColumnsJoin = "TT.ID, TT.Sobrenome, TT.NOME, TT.Password, TT.EMAIL, TT.ACTIVE ";
                    var strTable = $@" (
                                    SELECT distinct AdminUser.Id, AdminUser.Sobrenome, AdminUser.Nome, AdminUser.Senha as Password, AdminUser.Email, AdminUser.Ativo as Active FROM  AdminUser
                                    LEFT JOIN AdminUserProfile on AdminUserProfile.USERID = AdminUser.Id
                                    LEFT JOIN AdminProfile ON AdminProfile.ID = AdminUserProfile.PROFILEID
                                    LEFT JOIN AdminUserCompany on AdminUserCompany.UserId = AdminUser.ID
                                    {queryIn}
                                    ) as TT ";



                    //var strColumns = "*";
                    //sbSQLPaged.AppendFormat(" SELECT TOP ({0}) {1} ", pageSize, strColumns);
                    //sbSQLPaged.AppendFormat(" FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY {0} ) AS RowNum, {1} ", strOrder, strColumnsJoin);
                    //sbSQLPaged.AppendFormat(" FROM  {0} ", strTable);
                    //sbSQLPaged.AppendLine(" ) AS RowConstrainedResult ");
                    //sbSQLPaged.AppendFormat(" WHERE   RowNum > (({0} - 1) * {1}) ", pageIndex, pageSize);
                    //sbSQLPaged.AppendLine(" ORDER BY RowNum ");


                    sbSQLPaged.AppendLine($" SELECT {strColumnsJoin} ");
                    sbSQLPaged.AppendFormat(" FROM  {0} ", strTable);
                    pageIndex = pageIndex - 1;
                    sbSQLPaged.AppendLine($" ORDER BY {strOrder} ");
                    sbSQLPaged.AppendFormat(" LIMIT {0},{1} ", pageIndex * pageSize, pageSize);


                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminUserItem entidade = new AdminUserItem();
                        entidade.Id = Int32.Parse(dr["Id"].ToString());
                        entidade.Nome = dr["Nome"].ToString();

                        if (dr["Sobrenome"] != DBNull.Value)
                            entidade.Sobrenome = dr["Sobrenome"].ToString();

                        entidade.Password = dr["password"].ToString();
                        entidade.Email = dr["Email"].ToString();


                        if (dr["Active"] != DBNull.Value)
                        {
                            entidade.Active = false;
                            switch (dr["Active"].ToString().ToLower())
                            {
                                case "1":
                                case "true":
                                    entidade.Active = true;
                                    break;

                            }
                        }
                        listagem.Add(entidade);
                    }



                    #region TOTAL

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.AppendFormat("SELECT count(*) as total FROM {0}", strTable);

                    //if (clauseWhere.Length > 0)
                    //{
                    //    sbSQL.AppendFormat(" WHERE {0} ", clauseWhere.ToString());
                    //}

                    dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    // executa comando.
                    dr.Close();
                    dr = dataProc.ExecuteReader();
                    if (dr.Read())
                    {
                        pageTotal = Int32.Parse(dr["total"].ToString());
                    }

                    #endregion

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); 
                throw ex;
            }


            listagem.ForEach(r =>
            {
                r.Empresas = CarregarCompanies(r.Id);
                r.Perfis = CarregarProfiles(r.Id);
            });

            return listagem;
        }


        public DashReportItem DashReport(int perfilId, int empresaId, int usuarioId)
        {
            var report = new DashReportItem();


            try
            {

                switch ((EnumAdminProfile)perfilId)
                {
                    case EnumAdminProfile.Administrador:


                        #region QUERY

                        using (var connection = _DBGetConnection())
                        {

                            StringBuilder sbSQL = new StringBuilder();

                            sbSQL.AppendLine(@"SELECT exame.SituacaoId, COUNT(exame.Id) AS TOTAL
                                            FROM exame
                                            WHERE exame.SituacaoId in (@SITUACAOID_A, @SITUACAOID_B, @SITUACAOID_C, @SITUACAOID_D); ");

                            var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                            //dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CLIENTEID", MySqlDbType.Int32, usuarioId));
                            //dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, empresaId));
                            var pidA = EnumExameStatus.Aguardando_Atendimento.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_A", MySqlDbType.Int32, pidA));
                            var pidB = EnumExameStatus.Cancelado.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_B", MySqlDbType.Int32, pidB));
                            var pidC = EnumExameStatus.Concluido.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_C", MySqlDbType.Int32, pidC));
                            var pidD = EnumExameStatus.Cancelado.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_D", MySqlDbType.Int32, pidD));
                            // executa comando.
                            IDataReader dr = dataProc.ExecuteReader();

                            while (dr.Read())
                            {
                                if (dr["SituacaoId"] != DBNull.Value)
                                {
                                    var exame = (EnumExameStatus)Int32.Parse(dr["SituacaoId"].ToString());
                                    switch (exame)
                                    {
                                        case EnumExameStatus.Aguardando_Atendimento:
                                            report.totalExamesAguardandoAtendimento = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                        case EnumExameStatus.Em_Andamento:
                                            report.totalExamesEmAndamento = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                        case EnumExameStatus.Cancelado:
                                            report.totalExamesCancelados = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                        case EnumExameStatus.Concluido:
                                            report.totalExamesConcluidos = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                    }

                                }
                            }
                            dataProc.Dispose();
                        }
                        using (var connection = _DBGetConnection())
                        {

                            StringBuilder sbSQL = new StringBuilder();

                            sbSQL.AppendLine(@"SELECT 'CLIENTE' AS TIPO, COUNT(adminuser.Id) AS TOTAL
                                            FROM adminuser
                                            JOIN adminuserprofile ON adminuserprofile.UserId = adminuser.Id
                                            JOIN adminusercompany ON adminusercompany.UserId = adminuser.Id
                                            WHERE adminuserprofile.ProfileId = @PROFILEIDA
                                            union
                                            SELECT 'GERENTE' AS TIPO, COUNT(adminuser.Id) AS TOTAL
                                            FROM adminuser
                                            JOIN adminuserprofile ON adminuserprofile.UserId = adminuser.Id
                                            JOIN adminusercompany ON adminusercompany.UserId = adminuser.Id
                                            WHERE adminuserprofile.ProfileId = @PROFILEIDB; ");

                            var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                            //dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, empresaId));
                            var pidA = EnumAdminProfile.Cliente.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PROFILEIDA", MySqlDbType.Int32, pidA));
                            var pidB = EnumAdminProfile.Gerente.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PROFILEIDB", MySqlDbType.Int32, pidB));

                            // executa comando.
                            IDataReader dr = dataProc.ExecuteReader();

                            while (dr.Read())
                            {
                                if (dr["TIPO"].ToString() == "CLIENTE")
                                {
                                    report.totalClientes = Int32.Parse(dr["TOTAL"].ToString());
                                }
                                if (dr["TIPO"].ToString() == "GERENTE")
                                {
                                    report.totalGerentes = Int32.Parse(dr["TOTAL"].ToString());
                                }
                            }
                            dataProc.Dispose();
                        }
                        #endregion QUERY


                        break;
                    case EnumAdminProfile.Cliente:

                        #region QUERY
                        using (var connection = _DBGetConnection())
                        {

                            StringBuilder sbSQL = new StringBuilder();

                            sbSQL.AppendLine(@"SELECT exame.SituacaoId, COUNT(exame.Id) AS TOTAL
                                            FROM exame
                                            JOIN adminusercompany ON adminusercompany.UserId = exame.clienteId
                                            WHERE exame.ClienteId = @CLIENTEID AND exame.CompanyId = @COMPANYID
                                            AND exame.SituacaoId in (@SITUACAOID_A, @SITUACAOID_B, @SITUACAOID_C, @SITUACAOID_D); ");

                            var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CLIENTEID", MySqlDbType.Int32, usuarioId));
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, empresaId));
                            var pidA = EnumExameStatus.Aguardando_Atendimento.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_A", MySqlDbType.Int32, pidA));
                            var pidB = EnumExameStatus.Cancelado.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_B", MySqlDbType.Int32, pidB));
                            var pidC = EnumExameStatus.Concluido.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_C", MySqlDbType.Int32, pidC));
                            var pidD = EnumExameStatus.Cancelado.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_D", MySqlDbType.Int32, pidD));
                            // executa comando.
                            IDataReader dr = dataProc.ExecuteReader();

                            while (dr.Read())
                            {
                                if (dr["SituacaoId"] != DBNull.Value)
                                {
                                    var exame = (EnumExameStatus)Int32.Parse(dr["SituacaoId"].ToString());
                                    switch (exame)
                                    {
                                        case EnumExameStatus.Aguardando_Atendimento:
                                            report.totalExamesAguardandoAtendimento = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                        case EnumExameStatus.Em_Andamento:
                                            report.totalExamesEmAndamento = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                        case EnumExameStatus.Cancelado:
                                            report.totalExamesCancelados = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                        case EnumExameStatus.Concluido:
                                            report.totalExamesConcluidos = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                    }

                                }
                            }
                            dataProc.Dispose();
                        }
                        #endregion QUERY

                        break;
                    case EnumAdminProfile.Clinica:

                        #region QUERY

                        using (var connection = _DBGetConnection())
                        {

                            StringBuilder sbSQL = new StringBuilder();

                            sbSQL.AppendLine(@"SELECT exame.SituacaoId, COUNT(exame.Id) AS TOTAL
                                            FROM exame
                                           JOIN adminusercompany ON adminusercompany.CompanyId = exame.CompanyId
                                            WHERE  exame.CompanyId = @COMPANYID
                                            AND exame.SituacaoId in (@SITUACAOID_A, @SITUACAOID_B, @SITUACAOID_C, @SITUACAOID_D); ");

                            var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                            //dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CLIENTEID", MySqlDbType.Int32, usuarioId));
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, empresaId));
                            var pidA = EnumExameStatus.Aguardando_Atendimento.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_A", MySqlDbType.Int32, pidA));
                            var pidB = EnumExameStatus.Cancelado.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_B", MySqlDbType.Int32, pidB));
                            var pidC = EnumExameStatus.Concluido.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_C", MySqlDbType.Int32, pidC));
                            var pidD = EnumExameStatus.Cancelado.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_D", MySqlDbType.Int32, pidD));
                            // executa comando.
                            IDataReader dr = dataProc.ExecuteReader();

                            while (dr.Read())
                            {
                                if (dr["SituacaoId"] != DBNull.Value)
                                {
                                    var exame = (EnumExameStatus)Int32.Parse(dr["SituacaoId"].ToString());
                                    switch (exame)
                                    {
                                        case EnumExameStatus.Aguardando_Atendimento:
                                            report.totalExamesAguardandoAtendimento = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                        case EnumExameStatus.Em_Andamento:
                                            report.totalExamesEmAndamento = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                        case EnumExameStatus.Cancelado:
                                            report.totalExamesCancelados = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                        case EnumExameStatus.Concluido:
                                            report.totalExamesConcluidos = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                    }

                                }
                            }
                            dataProc.Dispose();
                        }
                        using (var connection = _DBGetConnection())
                        {

                            StringBuilder sbSQL = new StringBuilder();

                            sbSQL.AppendLine(@"SELECT 'CLIENTE' AS TIPO, COUNT(adminuser.Id) AS TOTAL
                                            FROM adminuser
                                            JOIN adminuserprofile ON adminuserprofile.UserId = adminuser.Id
                                            JOIN adminusercompany ON adminusercompany.UserId = adminuser.Id
                                            WHERE adminuserprofile.ProfileId = @PROFILEIDA AND adminusercompany.CompanyId = @COMPANYID
                                            union
                                            SELECT 'GERENTE' AS TIPO, COUNT(adminuser.Id) AS TOTAL
                                            FROM adminuser
                                            JOIN adminuserprofile ON adminuserprofile.UserId = adminuser.Id
                                            JOIN adminusercompany ON adminusercompany.UserId = adminuser.Id
                                            WHERE adminuserprofile.ProfileId = @PROFILEIDB AND adminusercompany.CompanyId = @COMPANYID; ");

                            var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, empresaId));
                            var pidA = EnumAdminProfile.Cliente.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PROFILEIDA", MySqlDbType.Int32, pidA));
                            var pidB = EnumAdminProfile.Gerente.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PROFILEIDB", MySqlDbType.Int32, pidB));

                            // executa comando.
                            IDataReader dr = dataProc.ExecuteReader();

                            while (dr.Read())
                            {
                                if (dr["TIPO"].ToString() == "CLIENTE")
                                {
                                    report.totalClientes = Int32.Parse(dr["TOTAL"].ToString());
                                }
                                if (dr["TIPO"].ToString() == "GERENTE")
                                {
                                    report.totalGerentes = Int32.Parse(dr["TOTAL"].ToString());
                                }
                            }
                            dataProc.Dispose();
                        }
                        #endregion QUERY

                        break;
                    case EnumAdminProfile.Gerente:

                        #region QUERY
                        using (var connection = _DBGetConnection())
                        {

                            StringBuilder sbSQL = new StringBuilder();

                            sbSQL.AppendLine(@"SELECT exame.SituacaoId, COUNT(exame.Id) AS TOTAL
                                            FROM exame
                                            JOIN adminusercompany ON adminusercompany.CompanyId = exame.CompanyId
                                            WHERE  exame.CompanyId = @COMPANYID
                                            AND exame.SituacaoId in (@SITUACAOID_A, @SITUACAOID_B, @SITUACAOID_C, @SITUACAOID_D); ");

                            var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                            //dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CLIENTEID", MySqlDbType.Int32, usuarioId));
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, empresaId));
                            var pidA = EnumExameStatus.Aguardando_Atendimento.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_A", MySqlDbType.Int32, pidA));
                            var pidB = EnumExameStatus.Cancelado.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_B", MySqlDbType.Int32, pidB));
                            var pidC = EnumExameStatus.Concluido.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_C", MySqlDbType.Int32, pidC));
                            var pidD = EnumExameStatus.Cancelado.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SITUACAOID_D", MySqlDbType.Int32, pidD));
                            // executa comando.
                            IDataReader dr = dataProc.ExecuteReader();

                            while (dr.Read())
                            {
                                if (dr["SituacaoId"] != DBNull.Value)
                                {
                                    var exame = (EnumExameStatus)Int32.Parse(dr["SituacaoId"].ToString());
                                    switch (exame)
                                    {
                                        case EnumExameStatus.Aguardando_Atendimento:
                                            report.totalExamesAguardandoAtendimento = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                        case EnumExameStatus.Em_Andamento:
                                            report.totalExamesEmAndamento = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                        case EnumExameStatus.Cancelado:
                                            report.totalExamesCancelados = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                        case EnumExameStatus.Concluido:
                                            report.totalExamesConcluidos = Int32.Parse(dr["TOTAL"].ToString());
                                            break;
                                    }

                                }
                            }
                            dataProc.Dispose();
                        }
                        using (var connection = _DBGetConnection())
                        {

                            StringBuilder sbSQL = new StringBuilder();

                            sbSQL.AppendLine(@"SELECT 'CLIENTE' AS TIPO, COUNT(adminuser.Id) AS TOTAL
                                            FROM adminuser
                                            JOIN adminuserprofile ON adminuserprofile.UserId = adminuser.Id
                                            JOIN adminusercompany ON adminusercompany.UserId = adminuser.Id
                                            WHERE adminuserprofile.ProfileId = @PROFILEIDA AND adminusercompany.CompanyId = @COMPANYID
                                            union
                                            SELECT 'GERENTE' AS TIPO, COUNT(adminuser.Id) AS TOTAL
                                            FROM adminuser
                                            JOIN adminuserprofile ON adminuserprofile.UserId = adminuser.Id
                                            JOIN adminusercompany ON adminusercompany.UserId = adminuser.Id
                                            WHERE adminuserprofile.ProfileId = @PROFILEIDB AND adminusercompany.CompanyId = @COMPANYID; ");

                            var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, empresaId));
                            var pidA = EnumAdminProfile.Cliente.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PROFILEIDA", MySqlDbType.Int32, pidA));
                            var pidB = EnumAdminProfile.Gerente.GetHashCode();
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PROFILEIDB", MySqlDbType.Int32, pidB));

                            // executa comando.
                            IDataReader dr = dataProc.ExecuteReader();

                            while (dr.Read())
                            {
                                if (dr["TIPO"].ToString() == "CLIENTE")
                                {
                                    report.totalClientes = Int32.Parse(dr["TOTAL"].ToString());
                                }
                                if (dr["TIPO"].ToString() == "GERENTE")
                                {
                                    report.totalGerentes = Int32.Parse(dr["TOTAL"].ToString());
                                }
                            }
                            dataProc.Dispose();
                        }
                        #endregion QUERY

                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return report;
        }



        public void DesvincularAdminUserProfile(int id, EnumAdminProfile profile, int empresaId)
        {
            var exist = ExisteUserCompany(id, empresaId);
            if (exist == 1)
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {

                        StringBuilder sbSQL = new StringBuilder();

                        //sbSQL.AppendLine(" DELETE FROM AdminUserProfile WHERE USERID = @ADMINUSERID and  PROFILEID = @PROFILEID;");
                        sbSQL.AppendLine(" DELETE FROM AdminUserCompany WHERE USERID = @ADMINUSERID and  COMPANYID = @COMPANYID;");

                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, empresaId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ADMINUSERID", MySqlDbType.Int32, id));
                        //var pid = profile.GetHashCode();
                        // dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PROFILEID", MySqlDbType.Int32, pid));

                        dataProc.ExecuteNonQuery();
                        dataProc.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //var exist2 = ExisteUserCompany(id, empresaId);
            //if (exist2 != null)
            //{
            //    db.AdminUserEmpresa.Remove(exist2);
            //    db.SaveChanges();
            //}

        }




        public List<AdminUserItem> ListUserPerfil(out int pageTotal, int pageIndex, int pageSize, int orderCol, string orderSort, Dictionary<string, object> dicFilter)
        {
            List<AdminUserItem> listagem = new List<AdminUserItem>();
            pageTotal = 0;

            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder clauseWhere = new StringBuilder();
                    if (dicFilter != null)
                        if (dicFilter.Count > 0)
                        {
                            foreach (var item in dicFilter)
                            {
                                if (clauseWhere.Length > 0)
                                {
                                    if (item.Value.GetType() == typeof(int))
                                        clauseWhere.AppendFormat(" AND {0} = {1}", item.Key, item.Value);

                                    if (item.Value.GetType() == typeof(string))
                                        clauseWhere.AppendFormat(" AND {0} like '%{1}%'", item.Key, item.Value);
                                }
                                else
                                {
                                    if (item.Value.GetType() == typeof(int))
                                        clauseWhere.AppendFormat(" {0} = {1}", item.Key, item.Value);

                                    if (item.Value.GetType() == typeof(string))
                                        clauseWhere.AppendFormat(" {0} like '%{1}%'", item.Key, item.Value);
                                }
                            }
                        }

                    StringBuilder sbSQLPaged = new StringBuilder();


                    var strOrder = "TT.Nome";

                    if (orderCol > 0)
                    {
                        switch (orderCol)
                        {
                            case 1:
                                strOrder = $"TT.Nome {orderSort}";
                                break;
                            case 2:
                                strOrder = $"TT.Sobrenome {orderSort}";
                                break;
                            case 3:
                                strOrder = $"TT.Email {orderSort}";
                                break;
                            case 4:
                                strOrder = $"TT.CPFCNPJ {orderSort}";
                                break;
                            case 5:
                                strOrder = $"TT.Telefone {orderSort}";
                                break;
                        }
                    }
                    string queryIn = "";
                    if (clauseWhere.Length > 0)
                    {
                        queryIn = $" WHERE {clauseWhere.ToString()} ";
                    }


                    var strColumnsJoin = "*";
                    var strTable = $@" (
                                        SELECT adminuser.* FROM adminuser
                                       JOIN adminuserprofile ON adminuserprofile.UserId = adminuser.ID
                                       JOIN adminusercompany ON adminusercompany.UserId  = adminuser.ID
                                    {queryIn}
                                    ) as TT ";



                    sbSQLPaged.AppendLine($" SELECT {strColumnsJoin} ");
                    sbSQLPaged.AppendFormat(" FROM  {0} ", strTable);
                    pageIndex = pageIndex - 1;
                    sbSQLPaged.AppendLine($" ORDER BY {strOrder} ");
                    sbSQLPaged.AppendFormat(" LIMIT {0},{1} ", pageIndex * pageSize, pageSize);


                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminUserItem entidade = new AdminUserItem();

                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();

                        if (dr["Email"] != DBNull.Value)
                            entidade.Email = dr["Email"].ToString();
                        else
                            entidade.Email = string.Empty;

                        if (dr["Sobrenome"] != DBNull.Value)
                            entidade.Sobrenome = dr["Sobrenome"].ToString();
                        else
                            entidade.Sobrenome = string.Empty;

                        if (dr["CPFCNPJ"] != DBNull.Value)
                            entidade.CPFCNPJ = dr["CPFCNPJ"].ToString();
                        else
                            entidade.CPFCNPJ = string.Empty;

                        if (dr["Imagem"] != DBNull.Value)
                            entidade.Imagem = dr["Imagem"].ToString();

                        if (dr["Telefone"] != DBNull.Value)
                            entidade.Telefone = dr["Telefone"].ToString();
                        else
                            entidade.Telefone = string.Empty;

                        if (dr["Telefone2"] != DBNull.Value)
                            entidade.Telefone2 = dr["Telefone2"].ToString();
                        else
                            entidade.Telefone2 = string.Empty;
                        listagem.Add(entidade);
                    }



                    #region TOTAL

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.AppendFormat("SELECT count(*) as total FROM {0}", strTable);

                    dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    // executa comando.
                    dr.Close();
                    dr = dataProc.ExecuteReader();
                    if (dr.Read())
                    {
                        pageTotal = Int32.Parse(dr["total"].ToString());
                    }

                    #endregion

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); 
                throw ex;
            }


            listagem.ForEach(r =>
            {
                r.Empresas = CarregarCompanies(r.Id);
                r.Perfis = CarregarProfiles(r.Id);
            });

            return listagem;
        }
        public List<AdminProfileItem> ListProfiles(int adminUserId)
        {
            List<AdminProfileItem> listagem = new List<AdminProfileItem>();

            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder sbSQLPaged = new StringBuilder();

                    sbSQLPaged.AppendLine("SELECT AdminProfile.ID, AdminProfile.NOME FROM AdminProfile ");
                    sbSQLPaged.AppendLine(" join AdminUserProfile on AdminUserProfile.ProfileId = AdminProfile.ID ");
                    sbSQLPaged.AppendLine(" WHERE AdminUserProfile.UserId = @USERID ");

                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@USERID", MySqlDbType.Int32, adminUserId));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        var entidade = new AdminProfileItem();
                        entidade.Id = Int32.Parse(dr["ID"].ToString());
                        entidade.Nome = dr["NOME"].ToString();
                        listagem.Add(entidade);
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return listagem;
        }


        public AdminUserItem SaveMultiPerfis(AdminUserItem entidade, out string message)
        {

            var validacao = ValidacaoUserEmail(entidade.Id, entidade.Email, out int idexist);

            if (validacao == 1) // Email ja existe
            {
                message = "E-mail já cadastrado";
                return entidade;
            }

            //validacao = ValidacaoUserName(entidade.ID, entidade.USERNAME);

            //if (validacao == 1) // Login ja existe
            //{
            //    message = "Login já cadastrado";
            //    return entidade;
            //}


            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();


                    if (entidade.Id > 0)
                    {
                        if (!String.IsNullOrEmpty(entidade.Password))
                        {
                            sbSQL.AppendLine(" UPDATE AdminUser SET IMAGEM = @PICTURE,  Senha = @PASSWORD, NOME = @NAME, SOBRENOME = @LASTNAME, EMAIL = @EMAIL, Telefone = @PHONE,  Ativo = @ACTIVE  ");
                            sbSQL.AppendLine(" WHERE ID = @ID ");
                        }
                        else
                        {
                            sbSQL.AppendLine(" UPDATE AdminUser SET IMAGEM = @PICTURE, NOME = @NAME, SOBRENOME = @LASTNAME, EMAIL = @EMAIL,  Ativo = @ACTIVE, Telefone = @PHONE  ");
                            sbSQL.AppendLine(" WHERE ID = @ID ");
                        }
                    }
                    else
                    {
                        sbSQL.AppendLine(" INSERT INTO AdminUser (IMAGEM, NOME, Telefone,  SOBRENOME, EMAIL, Senha, Ativo, DtCriacao ) VALUES (@PICTURE, @NAME, @PHONE, @LASTNAME, @EMAIL, @PASSWORD, @ACTIVE, @CREATEDDATE ); SELECT LAST_INSERT_ID(); ");
                    }

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);


                    if (entidade.Id > 0)
                    {

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, entidade.Id));
                    }
                    else
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CREATEDDATE", MySqlDbType.DateTime, DateTime.Now));

                    }

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@NAME", MySqlDbType.String, entidade.Nome));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@LASTNAME", MySqlDbType.String, entidade.Sobrenome));


                    if (!String.IsNullOrEmpty(entidade.Password))
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PASSWORD", MySqlDbType.String, Util.GerarHashMd5(entidade.Password)));
                    }


                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EMAIL", MySqlDbType.String, entidade.Email));

                    if (!String.IsNullOrEmpty(entidade.Telefone))
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PHONE", MySqlDbType.String, entidade.Telefone));
                    else
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PHONE", MySqlDbType.String, DBNull.Value));

                    if (!String.IsNullOrEmpty(entidade.Imagem))
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PICTURE", MySqlDbType.String, entidade.Imagem));
                    else
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PICTURE", MySqlDbType.String, DBNull.Value));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ACTIVE", MySqlDbType.Bit, entidade.Active));


                    if (entidade.Id == 0)
                    {
                        entidade.Id = Convert.ToInt32(dataProc.ExecuteScalar());
                        dataProc.Dispose();
                    }
                    else
                    {
                        // executa comando.
                        dataProc.ExecuteNonQuery();
                        dataProc.Dispose();
                    }

                }
                message = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }


            if (entidade.Id > 0)
            {
                RemoverAdminUserProfile(entidade.Id);
                entidade.Perfis.ForEach(code =>
                {
                    InserirAdminUserProfile(entidade.Id, code.Id);
                });

                RemoverAdminUserEmpresas(entidade.Id);
                entidade.Empresas.ForEach(code =>
                {
                    InserirAdminUserEmpresas(entidade.Id, code.Id);
                });
            }

            return entidade;
        }


        public bool VincularPorEmail(int empresaId, string email, out int? idexist)
        {
            bool result = false;
            idexist = null;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.Append(" select ID ");
                    sbSQL.Append(" from AdminUser ");
                    sbSQL.Append(" where (email = @EMAIL) ");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EMAIL", MySqlDbType.String, email));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr["ID"] != DBNull.Value)
                        {
                            idexist = Int32.Parse(dr["ID"].ToString());
                        }
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (idexist.HasValue)
            {
                int total = 0;
                try
                {
                    using (var connection = _DBGetConnection())
                    {
                        StringBuilder sbSQL = new StringBuilder();
                        sbSQL.Append(@" select COUNT(adminuser.ID) AS TOTAL
                                     from adminuser
                                     JOIN adminusercompany ON adminusercompany.UserId = adminuser.Id
                                     WHERE adminuser.email = @EMAIL
                                     AND adminusercompany.CompanyId = @EMPRESAID ");

                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                        dataProc.CommandType = CommandType.Text;
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EMAIL", MySqlDbType.String, email));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EMPRESAID", MySqlDbType.Int32, empresaId));

                        // executa comando.
                        IDataReader dr = dataProc.ExecuteReader();

                        if (dr.Read())
                        {
                            if (dr["TOTAL"] != DBNull.Value)
                            {
                                total = Int32.Parse(dr["TOTAL"].ToString());
                            }
                        }

                        dataProc.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }


                if (total == 0)
                {
                    try
                    {
                        using (var connection = _DBGetConnection())
                        {
                            StringBuilder sbSQL = new StringBuilder();
                            sbSQL.Append(@" INSERT INTO adminusercompany (UserId, CompanyId) VALUES (@USERID, @COMPANYID)");

                            var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                            dataProc.CommandType = CommandType.Text;
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@USERID", MySqlDbType.Int32, idexist.Value));
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, empresaId));

                            // executa comando.
                            IDataReader dr = dataProc.ExecuteReader();

                            if (dr.Read())
                            {
                                if (dr["TOTAL"] != DBNull.Value)
                                {
                                    total = Int32.Parse(dr["TOTAL"].ToString());
                                }
                            }

                            dataProc.Dispose();
                        }
                        result = true;

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return result;
        }


        public bool VincularPorCPFCNPJ(int empresaId, string cpfcnpj, out int? idexist)
        {
            bool result = false;
            idexist = null;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.Append(" select ID ");
                    sbSQL.Append(" from AdminUser ");
                    sbSQL.Append(" where (CPFCNPJ = @CPFCNPJ) ");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CPFCNPJ", MySqlDbType.String, cpfcnpj));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr["ID"] != DBNull.Value)
                        {
                            idexist = Int32.Parse(dr["ID"].ToString());
                        }
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (idexist.HasValue)
            {
                int total = 0;
                try
                {
                    using (var connection = _DBGetConnection())
                    {
                        StringBuilder sbSQL = new StringBuilder();
                        sbSQL.Append(@" select COUNT(adminuser.ID) AS TOTAL
                                     from adminuser
                                     JOIN adminusercompany ON adminusercompany.UserId = adminuser.Id
                                     WHERE adminuser.cpfcnpj = @CPFCNPJ
                                     AND adminusercompany.CompanyId = @EMPRESAID ");

                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                        dataProc.CommandType = CommandType.Text;
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CPFCNPJ", MySqlDbType.String, cpfcnpj));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EMPRESAID", MySqlDbType.Int32, empresaId));

                        // executa comando.
                        IDataReader dr = dataProc.ExecuteReader();

                        if (dr.Read())
                        {
                            if (dr["TOTAL"] != DBNull.Value)
                            {
                                total = Int32.Parse(dr["TOTAL"].ToString());
                            }
                        }

                        dataProc.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }


                if (total == 0)
                {
                    try
                    {
                        using (var connection = _DBGetConnection())
                        {
                            StringBuilder sbSQL = new StringBuilder();
                            sbSQL.Append(@" INSERT INTO adminusercompany (UserId, CompanyId) VALUES (@USERID, @COMPANYID)");

                            var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                            dataProc.CommandType = CommandType.Text;
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@USERID", MySqlDbType.Int32, idexist.Value));
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, empresaId));

                            // executa comando.
                            IDataReader dr = dataProc.ExecuteReader();

                            if (dr.Read())
                            {
                                if (dr["TOTAL"] != DBNull.Value)
                                {
                                    total = Int32.Parse(dr["TOTAL"].ToString());
                                }
                            }

                            dataProc.Dispose();
                        }
                        result = true;

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return result;
        }
        public AdminUserItem SaveUserPerfil(AdminUserItem entidade, out string message, out int idexist)
        {
            idexist = 0;
            if (!String.IsNullOrEmpty(entidade.Email))
            {
                var validacao = ValidacaoUserEmail(entidade.Id, entidade.Email, out idexist);

                if (validacao == 1) // Email ja existe
                {
                    if (ExisteVinculoUserCompany(idexist, entidade.CompanyId))
                    {
                        message = "EMAIL_CADASTRADO_EMPRESA";
                    }
                    else
                    {
                        message = "EMAIL_CADASTRADO";
                    }
                    return entidade;
                }
            }

            if (!String.IsNullOrEmpty(entidade.CPFCNPJ))
            {
                var validacao2 = ValidacaoUserCPFCNPJ(entidade.Id, entidade.CPFCNPJ, out idexist);
                if (validacao2 == 1) // Email ja existe
                {
                    if (ExisteVinculoUserCompany(idexist, entidade.CompanyId))
                    {
                        message = "CPFCNPJ_CADASTRADO_EMPRESA";

                        //if (ExisteVinculoUserProfile(idexist, entidade.PerfilId.GetHashCode()))
                        // {
                        //}
                    }
                    else
                    {
                        message = "CPFCNPJ_CADASTRADO";
                    }
                    return entidade;
                }
            }

            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();


                    if (entidade.Id > 0)
                    {
                        if (!String.IsNullOrEmpty(entidade.Password))
                        {
                            sbSQL.AppendLine(" UPDATE AdminUser SET IMAGEM = @PICTURE,  Senha = @PASSWORD, NOME = @NAME, SOBRENOME = @LASTNAME, EMAIL = @EMAIL, CPFCNPJ = @CPFCNPJ, Telefone = @PHONE, Telefone2 = @PHONE2,  Ativo = @ACTIVE  ");
                            sbSQL.AppendLine(" WHERE ID = @ID ");
                        }
                        else
                        {
                            sbSQL.AppendLine(" UPDATE AdminUser SET IMAGEM = @PICTURE, NOME = @NAME, SOBRENOME = @LASTNAME, EMAIL = @EMAIL,  Ativo = @ACTIVE, CPFCNPJ = @CPFCNPJ, Telefone = @PHONE, Telefone2 = @PHONE2  ");
                            sbSQL.AppendLine(" WHERE ID = @ID ");
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(entidade.Password))
                        {
                            sbSQL.AppendLine(" INSERT INTO AdminUser (IMAGEM, NOME, Telefone, Telefone2,  SOBRENOME, EMAIL, Senha, Ativo, DtCriacao, CPFCNPJ ) VALUES (@PICTURE, @NAME, @PHONE, @PHONE2, @LASTNAME, @EMAIL, '', @ACTIVE, @CREATEDDATE, @CPFCNPJ ); SELECT LAST_INSERT_ID(); ");
                        }
                        else
                        {
                            sbSQL.AppendLine(" INSERT INTO AdminUser (IMAGEM, NOME, Telefone, Telefone2,  SOBRENOME, EMAIL, Senha, Ativo, DtCriacao, CPFCNPJ ) VALUES (@PICTURE, @NAME, @PHONE, @PHONE2, @LASTNAME, @EMAIL, @PASSWORD, @ACTIVE, @CREATEDDATE, @CPFCNPJ ); SELECT LAST_INSERT_ID(); ");
                        }
                    }

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);


                    if (entidade.Id > 0)
                    {

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, entidade.Id));
                    }
                    else
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CREATEDDATE", MySqlDbType.DateTime, DateTime.Now));

                    }

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@NAME", MySqlDbType.String, entidade.Nome));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@LASTNAME", MySqlDbType.String, entidade.Sobrenome));


                    if (!String.IsNullOrEmpty(entidade.Password))
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PASSWORD", MySqlDbType.String, Util.GerarHashMd5(entidade.Password)));
                    }


                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EMAIL", MySqlDbType.String, entidade.Email));


                    if (!String.IsNullOrEmpty(entidade.CPFCNPJ))
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CPFCNPJ", MySqlDbType.String, entidade.CPFCNPJ));
                    else
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CPFCNPJ", MySqlDbType.String, DBNull.Value));



                    if (!String.IsNullOrEmpty(entidade.Telefone))
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PHONE", MySqlDbType.String, entidade.Telefone));
                    else
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PHONE", MySqlDbType.String, DBNull.Value));



                    if (!String.IsNullOrEmpty(entidade.Telefone2))
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PHONE2", MySqlDbType.String, entidade.Telefone2));
                    else
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PHONE2", MySqlDbType.String, DBNull.Value));

                    if (!String.IsNullOrEmpty(entidade.Imagem))
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PICTURE", MySqlDbType.String, entidade.Imagem));
                    else
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PICTURE", MySqlDbType.String, DBNull.Value));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ACTIVE", MySqlDbType.Bit, entidade.Active));


                    if (entidade.Id == 0)
                    {
                        entidade.Id = Convert.ToInt32(dataProc.ExecuteScalar());
                        dataProc.Dispose();
                    }
                    else
                    {
                        // executa comando.
                        dataProc.ExecuteNonQuery();
                        dataProc.Dispose();
                    }

                }
                message = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }


            if (entidade.Id > 0)
            {
                SaveAdminUserProfile(entidade.Id, entidade.PerfilId.GetHashCode());
                SaveAdminUserEmpresas(entidade.Id, entidade.CompanyId);
            }

            return entidade;
        }

        public string CarregarPerfisEmpresas(int usuarioId)
        {
            var listagem = "";
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.Append(" SELECT c.Nome FROM adminusercompany au ");
                    sbSQL.Append(" JOIN company c ON c.Id = au.CompanyId ");
                    sbSQL.Append(" WHERE c.Ativo = 1 and au.UserId = @USERID ");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@USERID", MySqlDbType.Int32, usuarioId));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    List<string> list = new List<string>();
                    while(dr.Read())
                    {
                        list.Add(dr["Nome"].ToString());
                    }

                    dataProc.Dispose();

                    listagem = string.Join(",", list);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listagem;

        }

        public AdminUserItem SaveUserPerfilDireto(AdminUserItem entidade, out string message, out int idexist)
        {
            idexist = 0;
            message = "";
            if (!String.IsNullOrEmpty(entidade.Email))
            {
                var validacao = ValidacaoUserEmail(entidade.Id, entidade.Email, out idexist);

                if (validacao == 1) // Email ja existe
                {
                    VincularPorEmail(entidade.CompanyId, entidade.Email, out int? aux);

                    if (aux.HasValue)
                    {
                        idexist = aux.Value;
                        SaveAdminUserProfile(idexist, entidade.PerfilId.GetHashCode());
                    }
                    /*if (ExisteVinculoUserCompany(idexist, entidade.CompanyId))
                    {
                        message = "EMAIL_CADASTRADO_EMPRESA";
                    }
                    else
                    {
                        message = "EMAIL_CADASTRADO";
                    }*/
                    return entidade;
                }
            }

            if (!String.IsNullOrEmpty(entidade.CPFCNPJ))
            {
                var validacao2 = ValidacaoUserCPFCNPJ(entidade.Id, entidade.CPFCNPJ, out idexist);
                if (validacao2 == 1) // Email ja existe
                {
                    VincularPorCPFCNPJ(entidade.CompanyId, entidade.Email, out int? aux);

                    if (aux.HasValue)
                    {
                        idexist = aux.Value;
                        SaveAdminUserProfile(idexist, entidade.PerfilId.GetHashCode());
                    }
                    //if (ExisteVinculoUserCompany(idexist, entidade.CompanyId))
                    //{
                    //    message = "CPFCNPJ_CADASTRADO_EMPRESA";

                    //}
                    //else
                    //{
                    //    message = "CPFCNPJ_CADASTRADO";
                    //}
                    return entidade;
                }
            }

            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();


                    if (entidade.Id > 0)
                    {
                        if (!String.IsNullOrEmpty(entidade.Password))
                        {
                            sbSQL.AppendLine(" UPDATE AdminUser SET IMAGEM = @PICTURE,  Senha = @PASSWORD, NOME = @NAME, SOBRENOME = @LASTNAME, EMAIL = @EMAIL, CPFCNPJ = @CPFCNPJ, Telefone = @PHONE, Telefone2 = @PHONE2,  Ativo = @ACTIVE  ");
                            sbSQL.AppendLine(" WHERE ID = @ID ");
                        }
                        else
                        {
                            sbSQL.AppendLine(" UPDATE AdminUser SET IMAGEM = @PICTURE, NOME = @NAME, SOBRENOME = @LASTNAME, EMAIL = @EMAIL,  Ativo = @ACTIVE, CPFCNPJ = @CPFCNPJ, Telefone = @PHONE, Telefone2 = @PHONE2  ");
                            sbSQL.AppendLine(" WHERE ID = @ID ");
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(entidade.Password))
                        {
                            sbSQL.AppendLine(" INSERT INTO AdminUser (IMAGEM, NOME, Telefone, Telefone2,  SOBRENOME, EMAIL, Senha, Ativo, DtCriacao, CPFCNPJ ) VALUES (@PICTURE, @NAME, @PHONE, @PHONE2, @LASTNAME, @EMAIL, '', @ACTIVE, @CREATEDDATE, @CPFCNPJ ); SELECT LAST_INSERT_ID(); ");
                        }
                        else
                        {
                            sbSQL.AppendLine(" INSERT INTO AdminUser (IMAGEM, NOME, Telefone, Telefone2,  SOBRENOME, EMAIL, Senha, Ativo, DtCriacao, CPFCNPJ ) VALUES (@PICTURE, @NAME, @PHONE, @PHONE2, @LASTNAME, @EMAIL, @PASSWORD, @ACTIVE, @CREATEDDATE, @CPFCNPJ ); SELECT LAST_INSERT_ID(); ");
                        }
                    }

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);


                    if (entidade.Id > 0)
                    {

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, entidade.Id));
                    }
                    else
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CREATEDDATE", MySqlDbType.DateTime, DateTime.Now));

                    }

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@NAME", MySqlDbType.String, entidade.Nome));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@LASTNAME", MySqlDbType.String, entidade.Sobrenome));


                    if (!String.IsNullOrEmpty(entidade.Password))
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PASSWORD", MySqlDbType.String, Util.GerarHashMd5(entidade.Password)));
                    }


                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EMAIL", MySqlDbType.String, entidade.Email));


                    if (!String.IsNullOrEmpty(entidade.CPFCNPJ))
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CPFCNPJ", MySqlDbType.String, entidade.CPFCNPJ));
                    else
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CPFCNPJ", MySqlDbType.String, DBNull.Value));



                    if (!String.IsNullOrEmpty(entidade.Telefone))
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PHONE", MySqlDbType.String, entidade.Telefone));
                    else
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PHONE", MySqlDbType.String, DBNull.Value));



                    if (!String.IsNullOrEmpty(entidade.Telefone2))
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PHONE2", MySqlDbType.String, entidade.Telefone2));
                    else
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PHONE2", MySqlDbType.String, DBNull.Value));

                    if (!String.IsNullOrEmpty(entidade.Imagem))
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PICTURE", MySqlDbType.String, entidade.Imagem));
                    else
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PICTURE", MySqlDbType.String, DBNull.Value));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ACTIVE", MySqlDbType.Bit, entidade.Active));


                    if (entidade.Id == 0)
                    {
                        entidade.Id = Convert.ToInt32(dataProc.ExecuteScalar());
                        dataProc.Dispose();
                    }
                    else
                    {
                        // executa comando.
                        dataProc.ExecuteNonQuery();
                        dataProc.Dispose();
                    }

                }
                message = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }


            if (entidade.Id > 0)
            {
                SaveAdminUserProfile(entidade.Id, entidade.PerfilId.GetHashCode());
                SaveAdminUserEmpresas(entidade.Id, entidade.CompanyId);
            }

            return entidade;
        }

        public int ExisteUserProfile(int id, int profileid)
        {
            int result = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.Append(" select ID ");
                    sbSQL.Append(" from adminuserprofile ");
                    sbSQL.Append(" where UserId = @USERID and ProfileId = @PROFILEID ");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@USERID", MySqlDbType.Int32, id));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PROFILEID", MySqlDbType.Int32, profileid));


                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        result = 1;
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }




        public int ExisteUserCompany(int id, int companyId)
        {
            int result = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.Append(" select ID ");
                    sbSQL.Append(" from adminusercompany ");
                    sbSQL.Append(" where UserId = @USERID and CompanyId = @COMPANYID ");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@USERID", MySqlDbType.Int32, id));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, companyId));


                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        result = 1;
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public bool SaveAdminUserEmpresas(int adminUserID, int companyId)
        {
            var vinculou = false;
            var validacao = ExisteUserCompany(adminUserID, companyId);

            if (validacao == 0) // SE NAO EXISTE ENTAO INSERE
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {
                        StringBuilder sbSQL = new StringBuilder();

                        sbSQL.AppendLine(" INSERT INTO adminusercompany ( UserId, CompanyId ) VALUES (@USERID, @COMPANYID);  ");

                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@USERID", MySqlDbType.Int32, adminUserID));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, companyId));

                        // executa comando.
                        dataProc.ExecuteNonQuery();
                        dataProc.Dispose();
                        vinculou = true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return vinculou;
        }

        public void SaveAdminUserProfile(int adminUserID, int perfilId)
        {
            var validacao = ExisteUserProfile(adminUserID, perfilId);

            if (validacao == 0) // SE NAO EXISTE ENTAO INSERE
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {
                        StringBuilder sbSQL = new StringBuilder();

                        sbSQL.AppendLine(" INSERT INTO adminuserprofile (ProfileId, UserId ) VALUES (@PROFILEID, @USERID);  ");


                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PROFILEID", MySqlDbType.Int32, perfilId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@USERID", MySqlDbType.Int32, adminUserID));

                        // executa comando.
                        dataProc.ExecuteNonQuery();
                        dataProc.Dispose();

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }


        public int ValidacaoUserEmail(int? id, string email, out int idexist)
        {
            int result = 0;
            idexist = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.Append(" select ID, ");
                    sbSQL.Append("  IF(email = @EMAIL, 1, 0) as ExistEmail ");
                    sbSQL.Append(" from AdminUser ");
                    sbSQL.Append(" where (email = @EMAIL) ");
                    if (id.HasValue && id.Value > 0)
                    {
                        sbSQL.Append(" and ID <> @ID ");
                    }


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EMAIL", MySqlDbType.String, email));


                    if (id.HasValue && id.Value > 0)
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, id.Value));
                    }

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        if (!id.HasValue || id.Value == 0)
                        {
                            if (dr["ID"] != DBNull.Value)
                            {
                                idexist = Int32.Parse(dr["ID"].ToString());
                            }
                        }

                        if (dr["ExistEmail"] != DBNull.Value)
                        {
                            result = Int32.Parse(dr["ExistEmail"].ToString());
                        }
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        public bool ExisteVinculoUserCompany(int usuarioId, int companyId)
        {
            int result = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.Append(" select IF(adminusercompany.CompanyId = @COMPANYID, 1, 0) as ExistUser ");
                    sbSQL.Append(" from adminuser ");
                    sbSQL.Append(" LEFT join adminusercompany ON adminusercompany.UserId = adminuser.Id ");
                    sbSQL.Append(" where (adminusercompany.CompanyId = @COMPANYID and adminuser.Id = @USUARIOID) ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@USUARIOID", MySqlDbType.Int32, usuarioId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, companyId));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr["ExistUser"] != DBNull.Value)
                        {
                            result = Int32.Parse(dr["ExistUser"].ToString());
                        }
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result == 1;
        }




        public bool ExisteVinculoUserProfile(int usuarioId, int profileid)
        {
            int result = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.Append(" select IF(adminuserprofile.ProfileId = @PROFILEID, 1, 0) as ExistUser ");
                    sbSQL.Append(" from adminuser ");
                    sbSQL.Append(" LEFT join adminuserprofile ON adminuserprofile.UserId = adminuser.Id ");
                    sbSQL.Append(" where (adminuserprofile.ProfileId = @PROFILEID and adminuser.Id = @USUARIOID) ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@USUARIOID", MySqlDbType.Int32, usuarioId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PROFILEID", MySqlDbType.Int32, profileid));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr["ExistUser"] != DBNull.Value)
                        {
                            result = Int32.Parse(dr["ExistUser"].ToString());
                        }
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result == 1;
        }


        public int ValidacaoUserCPFCNPJ(int? id, string cpfcnpj, out int idexist)
        {
            int result = 0;
            idexist = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.Append(" select ID, ");
                    sbSQL.Append("  IF(CPFCNPJ = @CPFCNPJ, 1, 0) as ExistCPFCNPJ ");
                    sbSQL.Append(" from AdminUser ");
                    sbSQL.Append(" where (CPFCNPJ = @CPFCNPJ) ");
                    if (id.HasValue && id.Value > 0)
                    {
                        sbSQL.Append(" and ID <> @ID ");
                    }

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    var aux = cpfcnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", "");
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CPFCNPJ", MySqlDbType.String, aux));


                    if (id.HasValue && id.Value > 0)
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, id.Value));
                    }

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        if (!id.HasValue || id.Value == 0)
                        {
                            if (dr["ID"] != DBNull.Value)
                            {
                                idexist = Int32.Parse(dr["ID"].ToString());
                            }
                        }

                        if (dr["ExistCPFCNPJ"] != DBNull.Value)
                        {
                            result = Int32.Parse(dr["ExistCPFCNPJ"].ToString());
                        }
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }



        private void InserirAdminUserEmpresas(int adminUserId, int empresaId)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine(" INSERT INTO AdminUserCompany (USERID, COMPANYID) VALUES (@USERID, @EMPRESAID)");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@USERID", MySqlDbType.Int32, adminUserId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EMPRESAID", MySqlDbType.Int32, empresaId));


                    // executa comando.
                    dataProc.ExecuteNonQuery();
                    dataProc.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void InserirAdminUserProfile(int adminUserId, int profileId)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(" INSERT INTO AdminUserProfile (USERID, PROFILEID) VALUES (@USERID, @PROFILEID)");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@USERID", MySqlDbType.Int32, adminUserId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PROFILEID", MySqlDbType.Int32, profileId));

                    // executa comando.
                    var r = dataProc.ExecuteReader();
                    dataProc.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RemoverAdminUserProfile(int adminUserId)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine(" DELETE FROM AdminUserProfile WHERE USERID = @ADMINUSERID;");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ADMINUSERID", MySqlDbType.Int32, adminUserId));

                    dataProc.ExecuteNonQuery();
                    dataProc.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void RemoverAdminUserEmpresas(int adminUserId)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine(" DELETE FROM AdminUserCompany WHERE USERID = @ADMINUSERID;");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ADMINUSERID", MySqlDbType.Int32, adminUserId));

                    dataProc.ExecuteNonQuery();
                    dataProc.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public void SaveImageUser(int id, string image)
        {
            bool result = false;
            using (var connection = _DBGetConnection())
            {
                StringBuilder sbSQL = new StringBuilder();

                if (id > 0)
                {
                    try
                    {
                        sbSQL.AppendLine(" UPDATE AdminUser SET Imagem = @PICTURE ");
                        sbSQL.AppendLine(" WHERE ID = @ID ");
                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, id));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PICTURE", MySqlDbType.String, image));

                        // executa comando.
                        dataProc.ExecuteNonQuery();
                        dataProc.Dispose();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
        }



        public void RemoveUser(int userId)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder sbSQLPaged = new StringBuilder();

                    sbSQLPaged.AppendLine("DELETE FROM AdminUser WHERE ID = @ID ");

                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, userId));

                    // executa comando.
                    dataProc.ExecuteNonQuery();

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AdminProfileItem> ListPerfil()
        {
            List<AdminProfileItem> listagem = new List<AdminProfileItem>();


            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder sbSQLPaged = new StringBuilder();

                    sbSQLPaged.AppendLine("SELECT ID, NOME FROM AdminProfile ");

                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        var entidade = new AdminProfileItem();
                        entidade.Id = Int32.Parse(dr["ID"].ToString());
                        entidade.Nome = dr["NOME"].ToString();
                        listagem.Add(entidade);
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return listagem;
        }



        public bool UpdatePasswordUser(string email, string password)
        {
            var result = false;
            try
            {

                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine("UPDATE AdminUser SET SENHA = @PASSWORD ");
                    sbSQL.AppendLine("WHERE  EMAIL = @EMAIL");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);


                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EMAIL", MySqlDbType.String, email));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PASSWORD", MySqlDbType.String, Util.GerarHashMd5(password)));

                    // executa comando.
                    dataProc.ExecuteNonQuery();
                    dataProc.Dispose();

                    result = true;
                }

            }
            catch (Exception ex)
            {
                // new SOH.Data.Helper.Util().EventLog(ex); 
                throw ex;
            }
            return result;
        }

        public AdminUserItem CarregarUsuarioAcesso(string usuario, string senha)
        {
            AdminUserItem entidade = new AdminUserItem();
            try
            {

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.AppendLine(@" SELECT (
                                SELECT CompanyId FROM adminusercompany WHERE adminusercompany.UserId = adminuser.Id LIMIT 0, 1) CompanyId, 
                                adminuser.*
                                FROM adminuser
                                JOIN adminuserprofile ON adminuserprofile.UserId = adminuser.Id
                                WHERE adminuserprofile.ProfileId = 2 and
                                adminuser.EMAIL = @EMAIL, adminuser.SENHA = @SENHA ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@EMAIL", MySqlDbType.String, usuario));
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@SENHA", MySqlDbType.String, Util.GerarHashMd5(senha)));

                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Email = dr["Email"].ToString();

                            if (dr["Sobrenome"] != DBNull.Value)
                                entidade.Sobrenome = dr["Sobrenome"].ToString();

                            if (dr["CompanyId"] != DBNull.Value)
                                entidade.CompanyId = Int32.Parse(dr["CompanyId"].ToString());

                            if (dr["Imagem"] != DBNull.Value)
                                entidade.Imagem = dr["Imagem"].ToString();

                            if (dr["Telefone"] != DBNull.Value)
                                entidade.Telefone = dr["Telefone"].ToString();

                            if (dr["Ativo"] != DBNull.Value)
                            {
                                entidade.Active = false;
                                switch (dr["Ativo"].ToString().ToLower())
                                {
                                    case "1":
                                    case "true":
                                        entidade.Active = true;
                                        break;

                                }
                            }
                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }

            if (entidade.Id > 0)
            {
                entidade.Empresas = CarregarCompanies(entidade.Id);

                entidade.Perfis = CarregarProfiles(entidade.Id);
            }

            return entidade;


        }
    }
}
