using PortalVet.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Linq;
using PortalVet.Data.Interface;
using PortalVet.Data.Helper;

namespace PortalVet.Data.Service
{
    public class AdminProfileService : ProviderConnection, IAdminProfileService
    {
        public List<AdminProfileItem> List(out int pageTotal, int pageIndex, int pageSize, Dictionary<string, object> dicFilter)
        {
            List<AdminProfileItem> listagem = new List<AdminProfileItem>();
            pageTotal = 0;

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

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

                    //var strOrder = "NOME";
                    //var strColumnsJoin = "ID, NOME";
                    var strTable = "AdminProfile";
                    //var strColumns = "*";
                    //sbSQLPaged.AppendFormat(" SELECT TOP ({0}) {1} ", pageSize, strColumns);
                    //sbSQLPaged.AppendFormat(" FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY {0} ) AS RowNum, {1} ", strOrder, strColumnsJoin);
                    //sbSQLPaged.AppendFormat(" FROM  {0} ", strTable);
                    //if (clauseWhere.Length > 0)
                    //{
                    //    sbSQLPaged.AppendFormat("WHERE {0} ", clauseWhere.ToString());
                    //}
                    //sbSQLPaged.AppendLine(" ) AS RowConstrainedResult ");
                    //sbSQLPaged.AppendFormat(" WHERE   RowNum > (({0} - 1) * {1}) ", pageIndex, pageSize);
                    //sbSQLPaged.AppendLine(" ORDER BY RowNum ");



                    sbSQLPaged.AppendLine(" SELECT ID, NOME ");
                    sbSQLPaged.AppendLine($" FROM {strTable} ");

                    if (clauseWhere.Length > 0)
                    {
                        sbSQLPaged.AppendFormat("WHERE {0} ", clauseWhere.ToString());
                    }

                    pageIndex = pageIndex - 1;
                    sbSQLPaged.AppendLine(" ORDER BY NOME ");
                    sbSQLPaged.AppendFormat(" LIMIT {0},{1} ", pageIndex * pageSize, pageSize);


                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminProfileItem entidade = new AdminProfileItem();

                        entidade.Id = Int32.Parse(dr["ID"].ToString());
                        entidade.Nome = dr["NOME"].ToString();

                        listagem.Add(entidade);
                    }



                    #region TOTAL

                    sbSQL = new StringBuilder();
                    sbSQL.AppendFormat("SELECT count(*) as total FROM {0}", strTable);



                    if (clauseWhere.Length > 0)
                    {
                        sbSQL.AppendFormat(" WHERE {0} ", clauseWhere.ToString());
                    }

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
                throw ex;
            }



            return listagem;
        }

        public int Remove(int id)
        {
            int ret = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.AppendLine($"DELETE FROM AdminPermission WHERE PROFILEID in ({id});DELETE FROM AdminProfile WHERE ID in ({id})");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    // executa comando.
                    ret = dataProc.ExecuteNonQuery();

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        public List<AdminMenuItem> GetMenus()
        {
            List<AdminMenuItem> listagem = new List<AdminMenuItem>();
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(" SELECT * FROM ");
                    sbSQL.AppendLine(" (");
                    sbSQL.AppendLine(" select M.ID as MenuId, M.PARENTID as MenuPaiId, M.NAME as Nome, M.OrderNumber as Ordem, null as OrdemFilho, M.Active as Ativo, M.PATH as Caminho, M.ICON as IconeCss, M.MODULE as Modulo, '' as ParentName from AdminMenu M ");
                    sbSQL.AppendLine(" where M.PARENTID = 0 ");
                    sbSQL.AppendLine(" union ");
                    sbSQL.AppendLine(" select M.ID as MenuId, M.PARENTID as MenuPaiId, concat(BUSCA_PARENT_MENU(M.ID) , ' / ', M.NAME) as Nome, MP.OrderNumber as Ordem, M.OrderNumber as OrdemFilho, M.Active as Ativo, M.PATH as Caminho, M.ICON as IconeCss, M.MODULE as Modulo, MP.Name as ParentName  from AdminMenu M ");
                    sbSQL.AppendLine(" ");
                    sbSQL.AppendLine(" JOIN AdminMenu MP on MP.ID = M.PARENTID ");
                    sbSQL.AppendLine(" where M.PARENTID <> 0 ");
                    sbSQL.AppendLine(" ) as TB ");
                    sbSQL.AppendLine(" ORDER BY Ordem ");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminMenuItem entidade = new AdminMenuItem();
                        entidade.MenuId = Int32.Parse(dr["MenuId"].ToString());
                        entidade.Nome = dr["Nome"].ToString();
                        listagem.Add(entidade);
                    }



                    dr.Close();
                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listagem;
        }



        public List<AdminRoleItem> GetRegras(int? menuid)
        {
            List<AdminRoleItem> listagem = new List<AdminRoleItem>();
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    if (menuid.HasValue)
                        sbSQL.Append("SELECT ID, NAME, PAGE FROM AdminROLE WHERE MENUID = @MenuId ORDER BY NAME");
                    else
                        sbSQL.Append("SELECT ID, NAME, PAGE FROM AdminROLE WHERE MENUID is null ORDER BY NAME");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    if (menuid.HasValue)
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@MenuId", MySqlDbType.Int32, menuid.Value));
                    }

                    dataProc.CommandType = CommandType.Text;

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminRoleItem entidade = new AdminRoleItem();
                        entidade.Id = Int32.Parse(dr["ID"].ToString());
                        entidade.Nome = dr["NAME"].ToString();

                        if (dr["PAGE"] != DBNull.Value)
                            entidade.Pagina = dr["PAGE"].ToString();
                        listagem.Add(entidade);
                    }

                    dr.Close();
                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }



            return listagem;
        }



        public List<AdminPermissionItem> GetPermissao(int PerfilId)
        {
            List<AdminPermissionItem> listagem = new List<AdminPermissionItem>();

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(" SELECT distinct T1.MenuId, T1.NomeMenu, T1.MODULE FROM ");
                    sbSQL.AppendLine(" ( ");
                    sbSQL.AppendLine(" select M.ID AS MenuId,   ");
                    sbSQL.AppendLine(" (CASE when MP.Name is null then M.Name else concat(BUSCA_PARENT_MENU(M.ID),' / ', M.Name) end) as NomeMenu,  ");
                    sbSQL.AppendLine(" M.MODULE ");
                    sbSQL.AppendLine(" from AdminMenu M ");
                    sbSQL.AppendLine(" left join AdminMenu MP on M.ParentId = MP.Id   ");
                    sbSQL.AppendLine(" ) as T1  ");
                    sbSQL.AppendLine(" LEFT JOIN (   ");
                    sbSQL.AppendLine(" SELECT * FROM    ");
                    sbSQL.AppendLine(" (    ");
                    sbSQL.AppendLine(" SELECT distinct R.MENUID as MenuId, P.PROFILEID as PerfilId, CASE WHEN P.ID is null THEN 0 ELSE 1 END as Ativo, R.Id as RegraId, R.Page as Pagina,  R.Name as NomeRegra FROM AdminRole R   ");
                    sbSQL.AppendLine(" JOIN AdminMenu M ON M.Id = R.MenuId      ");
                    sbSQL.AppendLine(" JOIN AdminPermission P ON R.Id = P.ROLEID and P.PROFILEID = @PerfilId ");
                    sbSQL.AppendLine(" LEFT JOIN AdminMenu MP on MP.Id = M.ParentId    ");

                    sbSQL.AppendLine(" WHERE R.MenuId is not null  AND M.ParentId is not null    ");
                    sbSQL.AppendLine(" union    ");
                    sbSQL.AppendLine(" SELECT distinct R.MENUID as MenuId, P.PROFILEID as PerfilId, CASE WHEN P.ID is null THEN 0 ELSE 1 END as Ativo, R.Id as RegraId, R.Page as Pagina,  R.Name as NomeRegra FROM AdminRole R      ");
                    sbSQL.AppendLine(" JOIN AdminMenu M ON M.Id = R.MenuId      ");
                    sbSQL.AppendLine(" JOIN AdminPermission P ON R.ID = P.ROLEID  and P.PROFILEID =  @PerfilId  ");
                    sbSQL.AppendLine(" WHERE R.MenuId is not null AND M.ParentId is null      ");
                    sbSQL.AppendLine(" union     ");
                    sbSQL.AppendLine(" SELECT distinct R.MENUID as MenuId, P.PROFILEID as PerfilId, CASE WHEN P.ID is null THEN 0 ELSE 1 END as Ativo, R.Id as RegraId, R.Page as Pagina,  R.Name as NomeRegra FROM AdminRole R      ");
                    sbSQL.AppendLine(" JOIN AdminPermission P ON R.Id = P.ROLEID   and P.PROFILEID =  @PerfilId ");
                    sbSQL.AppendLine(" WHERE R.MenuId is null    ");
                    sbSQL.AppendLine(" ) as TB   ");
                    sbSQL.AppendLine(" ) as T2 on T2.MenuId = T1.MenuId   ");
                    sbSQL.AppendLine(" WHERE PerfilId is not null ");
                    sbSQL.AppendLine(" ORDER BY NomeMenu ");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PerfilId", MySqlDbType.Int32, PerfilId));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminPermissionItem entidade = new AdminPermissionItem();

                        if (dr["MenuId"] != DBNull.Value)
                            entidade.MenuId = Int32.Parse(dr["MenuId"].ToString());

                        if (dr["NomeMenu"] != DBNull.Value)
                            entidade.Nome = dr["NomeMenu"].ToString();

                        if (dr["MODULE"] != DBNull.Value)
                        {
                            entidade.Module = dr["MODULE"].ToString();
                        }

                        listagem.Add(entidade);
                    }


                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            var listSubRoles = GetPermissaoSubItem(PerfilId);

            for (int i = 0; i < listagem.Count; i++)
            {
                listagem[i].SubItems = listSubRoles.Where(x => x.MenuId == listagem[i].MenuId).ToList();
            }

            return listagem;
        }



        public List<AdminPermissionSubItem> GetPermissaoSubItem(int perfilId)
        {
            List<AdminPermissionSubItem> listagem = new List<AdminPermissionSubItem>();

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine(" SELECT T1.MenuId, T2.PerfilId, T2.RegraId, T2.Pagina, T2.Ativo, T2.NomeRegra , T2.Chave FROM   ");
                    sbSQL.AppendLine(" ( ");
                    sbSQL.AppendLine(" select M.ID AS MenuId,   ");
                    sbSQL.AppendLine(" (CASE when MP.Name is null then M.Name else concat(MP.Name,' / ', M.Name) end) as NomeMenu  ");
                    sbSQL.AppendLine(" from AdminMenu M ");
                    sbSQL.AppendLine(" left join AdminMenu MP on M.ParentId = MP.Id   ");
                    sbSQL.AppendLine(" ) as T1  ");
                    sbSQL.AppendLine(" LEFT JOIN (   ");
                    sbSQL.AppendLine(" SELECT * FROM    ");
                    sbSQL.AppendLine(" (    ");
                    sbSQL.AppendLine(" SELECT distinct R.MENUID as MenuId, P.PROFILEID as PerfilId, CASE WHEN P.ID is null THEN 0 ELSE 1 END as Ativo, R.Id as RegraId, R.Page as Pagina,  R.Name as NomeRegra, R.Key as Chave  FROM AdminRole R   ");
                    sbSQL.AppendLine(" JOIN AdminMenu M ON M.Id = R.MENUID      ");
                    sbSQL.AppendLine(" JOIN AdminPermission P ON R.Id = P.ROLEID and P.PROFILEID = @PerfilId ");
                    sbSQL.AppendLine(" LEFT JOIN AdminMenu MP on MP.Id = M.ParentId    ");

                    sbSQL.AppendLine(" WHERE R.MenuId is not null  AND M.ParentId is not null    ");
                    sbSQL.AppendLine(" union    ");
                    sbSQL.AppendLine(" SELECT distinct R.MENUID as MenuId, P.PROFILEID as PerfilId, CASE WHEN P.ID is null THEN 0 ELSE 1 END as Ativo, R.Id as RegraId, R.Page as Pagina,  R.Name as NomeRegra, R.Key as Chave  FROM AdminRole R      ");
                    sbSQL.AppendLine(" JOIN AdminMenu M ON M.Id = R.MENUID      ");
                    sbSQL.AppendLine(" JOIN AdminPermission P ON R.ID = P.ROLEID  and P.PROFILEID =  @PerfilId  ");
                    sbSQL.AppendLine(" WHERE R.MenuId is not null AND M.ParentId is null      ");
                    sbSQL.AppendLine(" union     ");
                    sbSQL.AppendLine(" SELECT distinct R.MENUID as MenuId, P.PROFILEID as PerfilId, CASE WHEN P.ID is null THEN 0 ELSE 1 END as Ativo, R.Id as RegraId, R.Page as Pagina,  R.Name as NomeRegra, R.Key as Chave  FROM AdminRole R      ");
                    sbSQL.AppendLine(" JOIN AdminPermission P ON R.Id = P.ROLEID   and P.PROFILEID =  @PerfilId ");
                    sbSQL.AppendLine(" WHERE R.MenuId is null    ");
                    sbSQL.AppendLine(" ) as TB   ");
                    sbSQL.AppendLine(" ) as T2 on T2.MenuId = T1.MenuId   ");
                    sbSQL.AppendLine(" WHERE PerfilId is not null ");
                    sbSQL.AppendLine(" ORDER BY NomeMenu ");



                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PerfilId", MySqlDbType.Int32, perfilId));


                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminPermissionSubItem entidade = new AdminPermissionSubItem();

                        if (dr["RegraId"] != DBNull.Value)
                            entidade.RoleId = Int32.Parse(dr["RegraId"].ToString());

                        if (dr["Chave"] != DBNull.Value)
                            entidade.Code = dr["Chave"].ToString();

                        if (dr["NomeRegra"] != DBNull.Value)
                            entidade.Nome = dr["NomeRegra"].ToString();

                        if (dr["MenuId"] != DBNull.Value)
                            entidade.MenuId = Int32.Parse(dr["MenuId"].ToString());

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




        public AdminProfileItem Get(int ID)
        {
            AdminProfileItem entidade = new AdminProfileItem();
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.AppendLine("SELECT ID, NOME FROM AdminProfile ");
                    sbSQL.AppendLine("WHERE ID = @ID ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, ID));
                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        entidade = new AdminProfileItem();
                        entidade.Id = Int32.Parse(dr["ID"].ToString());
                        entidade.Nome = dr["NOME"].ToString();
                    }
                    dr.Close();
                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entidade;
        }


        public AdminProfileItem Save(AdminProfileItem entidade)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    if (entidade.Id > 0)
                    {

                        sbSQL.AppendLine("UPDATE AdminProfile SET NOME = @Nome  ");
                        sbSQL.AppendLine("WHERE ID = @PerfilId ");
                    }
                    else
                    {
                        sbSQL.AppendLine("INSERT INTO AdminProfile (NOME) VALUES ( @Nome );SELECT LAST_INSERT_ID(); ");
                    }


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);


                    if (entidade.Id > 0)
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PerfilId", MySqlDbType.Int32, entidade.Id));
                    }

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Nome", MySqlDbType.String, entidade.Nome));

                    if (entidade.Id == 0)
                    {
                        // executa comando.                    
                        entidade.Id = Convert.ToInt32(dataProc.ExecuteScalar());
                    }
                    else
                    {
                        // executa comando.
                        dataProc.ExecuteNonQuery();
                    }
                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entidade;
        }



        public List<AdminProfileItem> GetAll()
        {
            List<AdminProfileItem> listagem = new List<AdminProfileItem>();
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();
                    string strColumns = "ID, NOME";
                    string strTable = "AdminProfile";
                    string strOrder = "NOME";

                    sbSQL.AppendFormat("SELECT {0} FROM {1} ORDER BY {2}", strColumns, strTable, strOrder);

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminProfileItem entidade = new AdminProfileItem();
                        entidade.Id = Int32.Parse(dr["ID"].ToString());
                        entidade.Nome = dr["NOME"].ToString();
                        listagem.Add(entidade);
                    }


                    dr.Close();
                    dataProc.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



            return listagem;
        }



        public bool CheckPermissaoExist(int PerfilId, int RoleId)
        {
            bool exist = false;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    string strSqlValida = "SELECT * FROM AdminPermission WHERE PROFILEID = @PerfilId and ROLEID = @RoleId ";

                    var dataProc = _DBGetCommand(strSqlValida, connection);
                    dataProc.CommandType = CommandType.Text;

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PerfilId", MySqlDbType.String, PerfilId));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@RoleId", MySqlDbType.Int32, RoleId));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        exist = true;
                    }
                    dataProc.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return exist;
        }


        public void SavePermissao(int PerfilId, int RoleId)
        {

            if (!CheckPermissaoExist(PerfilId, RoleId))
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {

                        StringBuilder sbSQL = new StringBuilder();

                        sbSQL.AppendLine("INSERT INTO AdminPermission (PROFILEID, ROLEID) VALUES (@PerfilId, @RegraId); ");
                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PerfilId", MySqlDbType.Int32, PerfilId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@RegraId", MySqlDbType.Int32, RoleId));

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

        //public void SavePermissoes(int PerfilId, List<AdminPermissionItem> listPermissao)
        //{
        //    try
        //    {
        //        using (var connection = _DBGetConnection())
        //        {

        //            StringBuilder sbSQL = new StringBuilder();

        //            sbSQL.AppendLine("DELETE FROM AdminPermission WHERE PROFILEID = @PerfilId; ");
        //            var dataProcDel = _DBGetCommand(sbSQL.ToString(), connection);

        //            dataProcDel.Parameters.Add(_DBBuildParameter(dataProcDel, "@PerfilId", MySqlDbType.Int32, PerfilId));

        //            dataProcDel.ExecuteNonQuery();
        //            dataProcDel.Dispose();


        //            foreach (var item in listPermissao)
        //            {
        //                sbSQL = new StringBuilder();
        //                sbSQL.AppendLine("INSERT INTO AdminPermission (PROFILEID, ROLEID) VALUES (@PerfilId, @RegraId); ");
        //                var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
        //                dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PerfilId", MySqlDbType.Int32, PerfilId));
        //                dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@RegraId", MySqlDbType.Int32, item.Regra.RegraId));
        //                dataProc.ExecuteNonQuery();
        //                dataProc.Dispose();
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }


        //}

        public bool SaveAction(int menuid, string nome, string chave, string pagina)
        {
            bool result = false;
            bool exist = false;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    string strSqlValida = "SELECT * FROM AdminRole WHERE AdminRole.KEY = @Chave and MENUID = @MenuId ";

                    var dataProc = _DBGetCommand(strSqlValida, connection);
                    dataProc.CommandType = CommandType.Text;

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Chave", MySqlDbType.String, chave));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@MenuId", MySqlDbType.Int32, menuid));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        exist = true;
                    }
                    dataProc.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            try
            {

                if (!exist)
                {

                    using (var connection = _DBGetConnection())
                    {

                        StringBuilder sbSQL = new StringBuilder();
                        sbSQL.AppendLine("INSERT INTO AdminRole(NAME, AdminRole.KEY, PAGE, MENUID) VALUES(@Nome, @Chave, @Pagina, @MenuId); ");

                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);


                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@MenuId", MySqlDbType.Int32, menuid));

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Chave", MySqlDbType.String, chave));

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Pagina", MySqlDbType.String, pagina));

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Nome", MySqlDbType.String, nome));

                        // executa comando.
                        dataProc.ExecuteNonQuery();
                        result = true;
                        dataProc.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public bool SaveAction(int perfilid, int menuid, string nome, string chave, string pagina)
        {
            bool result = false;
            bool exist = false;
            try
            {
                using (var connection = _DBGetConnection())
                {
                    string strSqlValida = "";
                    if (String.IsNullOrEmpty(pagina))
                        strSqlValida = "SELECT ID FROM AdminRole WHERE KEY = @Chave and MENU_ID = @MenuId and PAGE is null ";
                    else
                        strSqlValida = "SELECT ID FROM AdminRole WHERE KEY = @Chave and MENU_ID = @MenuId and PAGE = @Pagina ";


                    var dataProc = _DBGetCommand(strSqlValida, connection);
                    dataProc.CommandType = CommandType.Text;


                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@MenuId", MySqlDbType.Int32, menuid));


                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Chave", MySqlDbType.String, chave));

                    if (!String.IsNullOrEmpty(pagina))
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Pagina", MySqlDbType.String, pagina));
                    }
                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        exist = true;
                    }
                    dataProc.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            try
            {
                if (!exist)
                {
                    using (var connection = _DBGetConnection())
                    {
                        StringBuilder sbSQL = new StringBuilder();
                        sbSQL.AppendLine("INSERT INTO AdminRole (NAME, KEY, PAGE, MENU_ID) VALUES ( @Nome, @Chave, @Pagina, @MenuId); ");

                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@MenuId", MySqlDbType.Int32, menuid));

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Chave", MySqlDbType.String, chave));

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Pagina", MySqlDbType.String, pagina));

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Nome", MySqlDbType.String, nome));
                        // executa comando.
                        dataProc.ExecuteNonQuery();
                        result = true;

                        dataProc.Dispose();
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public int RemoveRegra(int regraid, int perfilId)
        {
            int ret = 0;

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine("DELETE FROM AdminPERMISSION WHERE ROLEID = @RegraId and PROFILEID = @PerfilId;");
                    //sbSQL.AppendLine("DELETE FROM AdminROLE WHERE ID = @RegraId");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@RegraId", MySqlDbType.Int32, regraid));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PerfilId", MySqlDbType.Int32, perfilId));
                    // executa comando.
                    ret = dataProc.ExecuteNonQuery();

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

    }
}
