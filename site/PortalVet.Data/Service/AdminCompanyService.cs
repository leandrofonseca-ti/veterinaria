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
    public class AdminCompanyService : ProviderConnection, IAdminCompanyService
    {

        public AdminCompanyItem Get(int companyId)
        {
            AdminCompanyItem entidade = new AdminCompanyItem();
            try
            {

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.AppendLine(" SELECT * FROM   ");
                sbSQL.AppendLine(" Company ");
                sbSQL.AppendLine(" WHERE Id = @ID ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, companyId));

                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Imagem"] != DBNull.Value)
                                entidade.Imagem = dr["Imagem"].ToString();



                            if (dr["Email"] != DBNull.Value)
                                entidade.Email = dr["Email"].ToString();


                            if (dr["Url"] != DBNull.Value)
                                entidade.Url = dr["Url"].ToString();

                            if (dr["Whatsapp"] != DBNull.Value)
                                entidade.Whatsapp = dr["Whatsapp"].ToString();

                            if (dr["Chave"] != DBNull.Value)
                                entidade.Chave = dr["Chave"].ToString();

                            if (dr["Texto"] != DBNull.Value)
                                entidade.Texto = dr["Texto"].ToString();



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


        public AdminCompanyItem CarregarEmpresaAdmin(int adminUserId, int empresaId)
        {
            AdminCompanyItem entidade = new AdminCompanyItem();

            try
            {

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.AppendLine(" SELECT company.Id, company.Imagem, company.Nome FROM adminusercompany ");
                sbSQL.AppendLine(" JOIN company ON company.Id = adminusercompany.CompanyId ");
                sbSQL.AppendLine(" WHERE adminusercompany.UserId = @USERID ");
                sbSQL.AppendLine(" AND company.Ativo = 1 and company.Id = @COMPANYID ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@USERID", MySqlDbType.Int32, adminUserId));
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@COMPANYID", MySqlDbType.Int32, empresaId));

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
        public List<AdminCompanyItem> List(out int pageTotal, int pageIndex, int pageSize, int orderCol, string orderSort, Dictionary<string, object> dicFilter)
        {
            List<AdminCompanyItem> listagem = new List<AdminCompanyItem>();
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

                    var strOrder = "Nome";

                    if (orderCol > 0)
                    {
                        switch (orderCol)
                        {
                            case 1:
                                strOrder = $"Nome {orderSort}";
                                break;

                        }
                    }
                    string queryIn = "";
                    if (clauseWhere.Length > 0)
                    {
                        queryIn = $" WHERE {clauseWhere.ToString()} ";
                    }


                    var strColumnsJoin = " Id, Nome, Imagem, Chave, Ativo ";
                    var strTable = $@" Company ";



                    //var strColumns = "*";
                    //sbSQLPaged.AppendFormat(" SELECT TOP ({0}) {1} ", pageSize, strColumns);
                    //sbSQLPaged.AppendFormat(" FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY {0} ) AS RowNum, {1} ", strOrder, strColumnsJoin);
                    //sbSQLPaged.AppendFormat(" FROM  {0} ", strTable);
                    //sbSQLPaged.AppendLine(" ) AS RowConstrainedResult ");
                    //sbSQLPaged.AppendFormat(" WHERE   RowNum > (({0} - 1) * {1}) ", pageIndex, pageSize);
                    //sbSQLPaged.AppendLine(" ORDER BY RowNum ");


                    sbSQLPaged.AppendLine($" SELECT {strColumnsJoin} ");
                    sbSQLPaged.AppendFormat(" FROM  {0} {1} ", strTable, queryIn);
                    pageIndex = pageIndex - 1;
                    sbSQLPaged.AppendLine($" ORDER BY {strOrder} ");
                    sbSQLPaged.AppendFormat(" LIMIT {0},{1} ", pageIndex * pageSize, pageSize);


                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AdminCompanyItem entidade = new AdminCompanyItem();
                        entidade.Id = Int32.Parse(dr["Id"].ToString());
                        entidade.Nome = dr["Nome"].ToString();

                        if (dr["Imagem"] != DBNull.Value)
                            entidade.Imagem = dr["Imagem"].ToString();
                        else
                            entidade.Imagem = string.Empty;


                        entidade.Chave = dr["Chave"].ToString();

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
                    sbSQL.AppendFormat("SELECT count(*) as total FROM {0} {1}", strTable, queryIn);

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


            return listagem;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Remover(int id)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    //sbSQL.AppendLine(" DELETE FROM AdminUserProfile WHERE USERID = @ADMINUSERID and  PROFILEID = @PROFILEID;");
                    sbSQL.AppendLine(" UPDATE Company SET Ativo = 0 WHERE ID = @ID;");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, id));

                    dataProc.ExecuteNonQuery();
                    dataProc.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AdminCompanyItem Save(AdminCompanyItem entidade)
        {

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    if (entidade.Id > 0)
                    {

                        sbSQL.AppendLine("UPDATE Company SET Nome = @Nome,  Texto = @Texto, Imagem = @Imagem, Whatsapp = @Whatsapp, Url = @Url, Email = @Email, Ativo = @Ativo ");
                        sbSQL.AppendLine("WHERE ID = @Id ");
                    }
                    else
                    {
                        sbSQL.AppendLine("INSERT INTO Company (Nome, Texto, Imagem, Whatsapp, Url, Email, Ativo) VALUES ( @Nome, @Texto, @Imagem, @Whatsapp, @Url, @Email, @Ativo); SELECT LAST_INSERT_ID(); ");
                    }


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    if (entidade.Id > 0)
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Id", MySqlDbType.Int32, entidade.Id));
                    }

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Texto", MySqlDbType.String, entidade.Texto));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Nome", MySqlDbType.String, entidade.Nome));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Ativo", MySqlDbType.Bit, entidade.Ativo));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Imagem", MySqlDbType.String, entidade.Imagem));


                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Whatsapp", MySqlDbType.String, entidade.Whatsapp));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Url", MySqlDbType.String, entidade.Url));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Email", MySqlDbType.String, entidade.Email));


                    // executa comando.
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

        public void SaveImageUser(int id, string picture)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine("UPDATE Company SET Imagem = @Imagem ");
                    sbSQL.AppendLine("WHERE ID = @Id ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Id", MySqlDbType.Int32, id));




                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Imagem", MySqlDbType.String, picture));


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

}
