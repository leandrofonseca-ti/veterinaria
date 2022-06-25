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
    public class AdminMenuService : ProviderConnection, IAdminMenuService
    {


        public List<AdminMenuItem> List(out int pageTotal, int pageIndex, int pageSize, Dictionary<string, object> dicFilter)
        {
            List<AdminMenuItem> listagem = new List<AdminMenuItem>();
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


                    var strOrder = "M1.ParentId, M1.ORDERNUMBER";
                    var strColumnsJoin = "M1.ID as MenuId,BUSCA_PARENT_MENU(M1.ID) as ParentNome, M1.ParentId as MenuPaiId, M1.Name as Nome, M1.Module as Modulo, M1.ORDERNUMBER as Ordem, M1.Active as Ativo, M1.AreaName, M1.ControllerName, M1.ActionName ";
                    var strTable = "AdminMenu M1 left join AdminMenu M2 on M2.ID = M1.ParentId ";
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



                    sbSQLPaged.AppendLine($" SELECT {strColumnsJoin} ");
                    sbSQLPaged.AppendLine($" FROM {strTable}");

                    if (clauseWhere.Length > 0)
                    {
                        sbSQLPaged.AppendFormat("WHERE {0} ", clauseWhere.ToString());
                    }

                    pageIndex = pageIndex - 1;
                    sbSQLPaged.AppendLine($" ORDER BY {strOrder} ");
                    sbSQLPaged.AppendFormat(" LIMIT {0},{1} ", pageIndex * pageSize, pageSize);

                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminMenuItem entidade = new AdminMenuItem();

                        entidade.MenuId = Int32.Parse(dr["MenuId"].ToString());

                        if (dr["MenuPaiId"] != DBNull.Value)
                            entidade.ParentId = Int32.Parse(dr["MenuPaiId"].ToString());

                        if (dr["ParentNome"] != DBNull.Value)
                            entidade.ParentNome = dr["ParentNome"].ToString();

                        if (dr["Modulo"] != DBNull.Value)
                            entidade.Modulo = dr["Modulo"].ToString();


                        if (dr["AreaName"] != DBNull.Value)
                            entidade.AreaNome = dr["AreaName"].ToString();

                        if (dr["ControllerName"] != DBNull.Value)
                            entidade.ControllerNome = dr["ControllerName"].ToString();

                        if (dr["ActionName"] != DBNull.Value)
                            entidade.ActionNome = dr["ActionName"].ToString();

                        entidade.Nome = dr["Nome"].ToString();

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

                        listagem.Add(entidade);
                    }



                    #region TOTAL

                    StringBuilder sbSQL = new StringBuilder();
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


        public void Remove(int id)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();
                    
                    sbSQL.AppendLine($"DELETE FROM AdminMenu WHERE Id in ({id})");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

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

        public List<AdminMenuItem> ListMenuPai()
        {
            List<AdminMenuItem> listagem = new List<AdminMenuItem>();


            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder sbSQL = new StringBuilder();

                    // sbSQL.AppendLine(" select * from AdminMenu where ParentId = 0 ORDER BY NAME ");


                    sbSQL.AppendLine(" SELECT ");
                    sbSQL.AppendLine(" * FROM ");
                    sbSQL.AppendLine(" ( ");
                    sbSQL.AppendLine(" select* from AdminMenu where ParentId = 0 ");
                    sbSQL.AppendLine(" UNION ");
                    sbSQL.AppendLine(" select   ");
                    sbSQL.AppendLine(" A1.ID, ");
                    sbSQL.AppendLine(" IFNULL((select A2.NAME from AdminMenu A2 where Id = A1.ParentId and A2.PARENTID = 0),'') +' / ' + A1.NAME as NAME, ");
                    sbSQL.AppendLine(" A1.ORDERNUMBER, ");
                    sbSQL.AppendLine(" A1.PATH, ");
                    sbSQL.AppendLine(" A1.MODULE, ");
                    sbSQL.AppendLine(" A1.PARENTID, ");
                    sbSQL.AppendLine(" A1.ACTIVE, ");
                    sbSQL.AppendLine(" A1.ICON, ");
                    sbSQL.AppendLine(" A1.AREANAME, ");
                    sbSQL.AppendLine(" A1.CONTROLLERNAME, ");
                    sbSQL.AppendLine(" A1.ACTIONNAME ");
                    sbSQL.AppendLine(" from AdminMenu A1 ");
                    sbSQL.AppendLine(" where ParentId != 0 and ");
                    sbSQL.AppendLine(" (select A2.NAME from AdminMenu A2 where Id = A1.ParentId and A2.PARENTID = 0) is not null ");
                    sbSQL.AppendLine(" ) AS TB ");
                    sbSQL.AppendLine(" ORDER BY ParentId, ORDERNUMBER ");



                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminMenuItem entidade = new AdminMenuItem();

                        entidade.MenuId = Int32.Parse(dr["ID"].ToString());

                        entidade.Nome = dr["NAME"].ToString();

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


        public AdminMenuItem Get(int ID)
        {
            AdminMenuItem entidade = new AdminMenuItem();
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.AppendLine("SELECT Id, ParentId, Name, AdminMenu.ORDERNUMBER, Active, Path, Module, Icon, ActionName, AreaName, ControllerName  FROM AdminMenu ");
                    sbSQL.AppendLine("WHERE Id = @ID ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, ID));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        entidade = new AdminMenuItem();
                        entidade.MenuId = Int32.Parse(dr["Id"].ToString());

                        if (dr["ParentId"] != DBNull.Value)
                            entidade.ParentId = Int32.Parse(dr["ParentId"].ToString());

                        entidade.Nome = dr["Name"].ToString();

                        if (dr["Icon"] != DBNull.Value)
                            entidade.IconeCss = dr["Icon"].ToString();

                        if (dr["Module"] != DBNull.Value)
                            entidade.Modulo = dr["Module"].ToString();

                        if (dr["Path"] != DBNull.Value)
                            entidade.Path = dr["Path"].ToString();

                        if (dr["AreaName"] != DBNull.Value)
                            entidade.AreaNome = dr["AreaName"].ToString();

                        if (dr["ControllerName"] != DBNull.Value)
                            entidade.ControllerNome = dr["ControllerName"].ToString();

                        if (dr["ActionName"] != DBNull.Value)
                            entidade.ActionNome = dr["ActionName"].ToString();

                        if (dr["ORDERNUMBER"] != DBNull.Value)
                            entidade.Ordem = Int32.Parse(dr["ORDERNUMBER"].ToString());

                        if (dr["Active"] != DBNull.Value)
                        {
                            entidade.Ativo = false;
                            switch (dr["Active"].ToString().ToLower())
                            {
                                case "1":
                                case "true":
                                    entidade.Ativo = true;
                                    break;

                            }
                        }

                    }
                    dr.Close();
                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //List<AdminMenuTesteItem> listTestes = new List<AdminMenuTesteItem>();
            //if (entidade != null && entidade.MenuId > 0)
            //{
            //    Data.Model.sohimoveisEntities db = new Model.sohimoveisEntities();

            //    var queryTestes = (from c in db.AdminMenuTeste
            //                       where c.MenuId == entidade.MenuId
            //                       select c).ToList();

            //    foreach (var item in queryTestes)
            //    {
            //        listTestes.Add(new AdminMenuTesteItem()
            //        {
            //            Id = item.Id,
            //            EmpresaId = item.EmpresaId,
            //            MenuId = item.MenuId
            //        });
            //    }
            //}

            //entidade.ListEmpresasTeste = listTestes;
            return entidade;
        }


        public AdminMenuItem Save(AdminMenuItem entidade)
        {

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    if (entidade.MenuId > 0)
                    {

                        sbSQL.AppendLine("UPDATE AdminMenu SET PARENTID = @MenuPaiId, NAME = @Nome, ORDERNUMBER = @Ordem, ACTIVE = @Ativo, PATH = @Caminho,  ICON = @IconeCss, MODULE = @Modulo, ControllerName = @ControllerName, ActionName = @ActionName, AreaName = @AreaName ");
                        sbSQL.AppendLine("WHERE ID = @MenuId ");
                    }
                    else
                    {
                        sbSQL.AppendLine("INSERT INTO AdminMenu (PARENTID, NAME, ORDERNUMBER, ACTIVE, PATH,  ICON, MODULE,  CONTROLLERNAME, ACTIONNAME, AREANAME) VALUES ( @MenuPaiId, @Nome, @Ordem, @Ativo, @Caminho,  @IconeCss, @Modulo, @ControllerName, @ActionName, @AreaName); SELECT LAST_INSERT_ID(); ");
                    }


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    if (entidade.MenuId > 0)
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@MenuId", MySqlDbType.Int32, entidade.MenuId));
                    }

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@MenuPaiId", MySqlDbType.Int32, entidade.ParentId));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Ordem", MySqlDbType.Int32, entidade.Ordem));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Nome", MySqlDbType.String, entidade.Nome));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Modulo", MySqlDbType.String, entidade.Modulo));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Ativo", MySqlDbType.Bit, entidade.Ativo));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Caminho", MySqlDbType.String, entidade.Path));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@IconeCss", MySqlDbType.String, entidade.IconeCss));


                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@AreaName", MySqlDbType.String, entidade.AreaNome));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ControllerName", MySqlDbType.String, entidade.ControllerNome));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ActionName", MySqlDbType.String, entidade.ActionNome));

                    // executa comando.
                    if (entidade.MenuId == 0)
                    {
                        // executa comando.                    
                        entidade.MenuId = Convert.ToInt32(dataProc.ExecuteScalar());
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
    }
}
