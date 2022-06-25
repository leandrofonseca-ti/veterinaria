using MySql.Data.MySqlClient;
using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortalVet.Data.Service
{
    public class AssinaturaService : ProviderConnection, IAssinaturaService
    {

        public List<AssinaturaItem> ListarLaudador(int laudadorid)
        {
            List<AssinaturaItem> listagem = new List<AssinaturaItem>();

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder clauseWhere = new StringBuilder();

                    StringBuilder sbSQLPaged = new StringBuilder();


                        clauseWhere.Append($" LaudadorId = {laudadorid} ");


                    var strOrder = "Nome";


                    string queryIn = "";
                    if (clauseWhere.Length > 0)
                    {
                        queryIn = $" WHERE {clauseWhere.ToString()} ";
                    }


                    var strColumnsJoin = " * ";
                    var strTable = $@" assinaturamodelo ";


                    sbSQLPaged.AppendLine($" SELECT {strColumnsJoin} ");
                    sbSQLPaged.AppendFormat(" FROM  {0} {1} ", strTable, queryIn);
                    sbSQLPaged.AppendLine($" ORDER BY {strOrder} ");


                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AssinaturaItem entidade = new AssinaturaItem();
                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();

                        listagem.Add(entidade);
                    }
 

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

        public List<AssinaturaItem> ListarLaudadorCompany(int companyid)
        {
            List<AssinaturaItem> listagem = new List<AssinaturaItem>();

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder clauseWhere = new StringBuilder();

                    StringBuilder sbSQLPaged = new StringBuilder();


                    clauseWhere.Append($" CompanyId = {companyid} ");


                    var strOrder = "Nome";


                    string queryIn = "";
                    if (clauseWhere.Length > 0)
                    {
                        queryIn = $" WHERE {clauseWhere.ToString()} ";
                    }


                    var strColumnsJoin = " * ";
                    var strTable = $@" assinaturamodelo ";


                    sbSQLPaged.AppendLine($" SELECT {strColumnsJoin} ");
                    sbSQLPaged.AppendFormat(" FROM  {0} {1} ", strTable, queryIn);
                    sbSQLPaged.AppendLine($" ORDER BY {strOrder} ");


                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        AssinaturaItem entidade = new AssinaturaItem();
                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();

                        listagem.Add(entidade);
                    }


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

        public List<AssinaturaItem> Listar(out int pgTotal, int pageIndex, int pageSize, int usuarioId, int perfilId)
        {
            List<AssinaturaItem> listagem = new List<AssinaturaItem>();
            pgTotal = 0;



            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder clauseWhere = new StringBuilder();

                    StringBuilder sbSQLPaged = new StringBuilder();


                    if (EnumAdminProfile.Laudador.GetHashCode() == perfilId)
                    {
                        clauseWhere.Append($" LaudadorId = {usuarioId} ");
                    }


                    var strOrder = "Nome";


                    string queryIn = "";
                    if (clauseWhere.Length > 0)
                    {
                        queryIn = $" WHERE {clauseWhere.ToString()} ";
                    }


                    var strColumnsJoin = " * ";
                    var strTable = $@" assinaturamodelo ";


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
                        AssinaturaItem entidade = new AssinaturaItem();
                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();

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
                        pgTotal = Int32.Parse(dr["total"].ToString());
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


        public void Remover(int id)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.AppendLine($"DELETE FROM AssinaturaModelo WHERE ID = {id}");

                    MySqlCommand dataProc = new MySqlCommand(sbSQL.ToString(), connection);
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

        public AssinaturaItem Salvar(AssinaturaItem entidade)
        {


            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();


                    if (entidade.Id > 0)
                    {
                        sbSQL.AppendLine(" UPDATE AssinaturaModelo SET  CompanyId = @CompanyId, Nome = @Nome,  LaudadorId = @LaudadorId,  AssinaturaNome = @AssinaturaNome,  AssinaturaCRM = @AssinaturaCRM, AssinaturaProfissao = @AssinaturaProfissao, AssinaturaImagem = @AssinaturaImagem ");
                        sbSQL.AppendLine(" WHERE ID = @Id ");

                    }
                    else
                    {
                        sbSQL.AppendLine(" INSERT INTO AssinaturaModelo ( CompanyId, Nome, LaudadorId, AssinaturaNome,  AssinaturaCRM, AssinaturaProfissao, AssinaturaImagem, DtCadastro ) VALUES ( @CompanyId, @Nome, @LaudadorId, @AssinaturaNome,  @AssinaturaCRM, @AssinaturaProfissao, @AssinaturaImagem, @DtCadastro ); SELECT LAST_INSERT_ID(); "); // LASTNAME, GENDER, BIRTHDAY
                    }

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);


                    if (entidade.Id > 0)
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Id", MySqlDbType.Int32, entidade.Id));
                    }
                    else
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@DtCadastro", MySqlDbType.DateTime, DateTime.Now));

                    }
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, entidade.CompanyId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Nome", MySqlDbType.String, entidade.Nome));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@LaudadorId", MySqlDbType.Int32, entidade.LaudadorId));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@AssinaturaNome", MySqlDbType.String, entidade.AssinaturaNome));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@AssinaturaCRM", MySqlDbType.String, entidade.AssinaturaCRM));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@AssinaturaProfissao", MySqlDbType.String, entidade.AssinaturaProfissao));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@AssinaturaImagem", MySqlDbType.String, entidade.AssinaturaImagem));

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
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entidade;

        }


 
        public AssinaturaItem Carregar(int id)
        {

            AssinaturaItem entidade = new AssinaturaItem();
            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder sbSQLPaged = new StringBuilder();

                    sbSQLPaged.AppendLine("SELECT * FROM assinaturamodelo WHERE ID = @Id ");

                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Id", MySqlDbType.Int32, id));
                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        entidade = new AssinaturaItem();
                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["CompanyId"] != DBNull.Value)
                            entidade.CompanyId = Int32.Parse(dr["CompanyId"].ToString());

                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();

                        if (dr["AssinaturaNome"] != DBNull.Value)
                            entidade.AssinaturaNome = dr["AssinaturaNome"].ToString();

                        if (dr["AssinaturaCRM"] != DBNull.Value)
                            entidade.AssinaturaCRM = dr["AssinaturaCRM"].ToString();

                        if (dr["AssinaturaProfissao"] != DBNull.Value)
                            entidade.AssinaturaProfissao = dr["AssinaturaProfissao"].ToString();

                        if (dr["AssinaturaImagem"] != DBNull.Value)
                            entidade.AssinaturaImagem = dr["AssinaturaImagem"].ToString();
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
