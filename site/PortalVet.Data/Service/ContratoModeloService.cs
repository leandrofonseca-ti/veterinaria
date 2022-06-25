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
    public class ContratoModeloService : ProviderConnection, IContratoModeloService
    {


        public List<DocumentoVariavelItem> CarregarVariaveis()
        {
            List<DocumentoVariavelItem> listagem = new List<DocumentoVariavelItem>();
            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder sbSQLPaged = new StringBuilder();

                    sbSQLPaged.AppendLine("SELECT * FROM DocumentoVariavel ");

                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        var entidade = new DocumentoVariavelItem();
                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();

                        if (dr["Descricao"] != DBNull.Value)
                            entidade.Descricao = dr["Descricao"].ToString();

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

        public List<DocumentoModeloItem> List(out int pgTotal, int pageIndex, int pageSize, int usuarioId, int perfilId)
        {
            List<DocumentoModeloItem> listagem = new List<DocumentoModeloItem>();
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
                    var strTable = $@" documentomodelo ";


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
                        DocumentoModeloItem entidade = new DocumentoModeloItem();
                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();

                        if (dr["Perfil"] != DBNull.Value)
                            entidade.Perfil = dr["Perfil"].ToString();

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


        public void RemoveDocumento(int id)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.AppendLine($"DELETE FROM DocumentoModelo WHERE ID = {id}");

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

        public DocumentoModeloItem SalvarDocumento(DocumentoModeloItem entidade)
        {


            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();


                    if (entidade.Id > 0)
                    {
                        sbSQL.AppendLine(" UPDATE DocumentoModelo SET  CompanyId = @CompanyId, AssinaturaId = @AssinaturaId, Nome = @Nome,  LaudadorId = @LaudadorId, Perfil = @Perfil, ModeloCabecalho = @ModeloCabecalho, ModeloCorpo = @ModeloCorpo, ModeloRodape = @ModeloRodape ");
                        sbSQL.AppendLine(" WHERE ID = @Id ");

                    }
                    else
                    {
                        sbSQL.AppendLine(" INSERT INTO DocumentoModelo ( CompanyId, AssinaturaId, Nome, LaudadorId, Perfil,  ModeloCabecalho, ModeloCorpo, ModeloRodape, DtCadastro ) VALUES ( @CompanyId, @AssinaturaId, @Nome, @LaudadorId, @Perfil, @ModeloCabecalho, @ModeloCorpo, @ModeloRodape, @DtCadastro ); SELECT LAST_INSERT_ID(); "); // LASTNAME, GENDER, BIRTHDAY
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

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@AssinaturaId", MySqlDbType.Int32, entidade.AssinaturaId)); 
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, entidade.CompanyId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Nome", MySqlDbType.String, entidade.Nome));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@LaudadorId", MySqlDbType.Int32, entidade.LaudadorId)); 
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Perfil", MySqlDbType.String, entidade.Perfil));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ModeloCabecalho", MySqlDbType.String, entidade.ModeloCabecalho));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ModeloCorpo", MySqlDbType.String, entidade.ModeloCorpo));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ModeloRodape", MySqlDbType.String, entidade.ModeloRodape));

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



        public string ReplaceDocumentoModelo(int modeloid, int exameid, int empresaid, string texto)
        {
            var listaVariaveis = CarregarVariaveis().Select(r => r.Nome).ToList();
            var resultadoFinal = texto;
            var dictionary = new Dictionary<string, string>();
            //dynamic entity = new object();
            var entity = new ExameService().Get(exameid);
            var entityCliente = new AdminUserService().GetById(entity.ClienteId);
            var entityLaudador = new AdminUserService().GetById(entity.LaudadorId);
            var entityCompany = new AdminUserService().GetById(entity.CompanyId);

            if (entity != null && entityCliente != null && entityLaudador != null && entityCompany != null)
                listaVariaveis.ForEach(txt =>
                {

                    switch (txt.ToUpper())
                    {
                        case "DATA_EXAME":
                            dictionary.Add($"#{txt.ToUpper()}#", $"{entity.DataExameFmt} {entity.DataExameHH}:{entity.DataExameMM}");
                            break;
                        case "EXAME_IDADE_PACIENTE":
                            dictionary.Add($"#{txt.ToUpper()}#", entity.Idade);
                            break;
                        case "EXAME_VETERINARIO":
                            dictionary.Add($"#{txt.ToUpper()}#", entity.Veterinario);
                            break;
                        case "EXAME_PROPRIETARIO":
                            dictionary.Add($"#{txt.ToUpper()}#", entity.Proprietario);
                            break;
                        case "EXAME_PACIENTE":
                            dictionary.Add($"#{txt.ToUpper()}#", entity.Paciente);
                            break;
                        case "EXAME_ESPECIE":
                            dictionary.Add($"#{txt.ToUpper()}#", entity.EspecieNome);
                            break;
                        case "CLIENTE_NOME":
                            dictionary.Add($"#{txt.ToUpper()}#", entityCliente.Nome);
                            break;
                        case "CLIENTE_EMAIL":
                            dictionary.Add($"#{txt.ToUpper()}#", entityCliente.Email);
                            break;
                        case "CLIENTE_CPF_CNPJ":
                            dictionary.Add($"#{txt.ToUpper()}#", entityCliente.CPFCNPJFmt);
                            break;
                        case "LAUDADOR_NOME":
                            dictionary.Add($"#{txt.ToUpper()}#", entityLaudador.Nome);
                            break;
                        case "LAUDADOR_EMAIL":
                            dictionary.Add($"#{txt.ToUpper()}#", entityLaudador.Email);
                            break;
                        case "LAUDADOR_CPF_CNPJ":
                            dictionary.Add($"#{txt.ToUpper()}#", entityLaudador.CPFCNPJFmt);
                            break;
                        case "CLINICA_NOME":
                            dictionary.Add($"#{txt.ToUpper()}#", entityCompany.Nome);
                            break;

                    }

                });


            foreach (var item in dictionary)
            {
                if (item.Value != null)
                    resultadoFinal = Regex.Replace(resultadoFinal, item.Key, item.Value);
                else
                    resultadoFinal = Regex.Replace(resultadoFinal, item.Key, string.Empty);
            }

            return resultadoFinal;
        }

        public DocumentoModeloItem CarregarDocumento(int id)
        {

            DocumentoModeloItem entidade = new DocumentoModeloItem();
            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder sbSQLPaged = new StringBuilder();

                    sbSQLPaged.AppendLine("SELECT * FROM documentomodelo WHERE ID = @Id ");

                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Id", MySqlDbType.Int32, id));
                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        entidade = new DocumentoModeloItem();
                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["CompanyId"] != DBNull.Value)
                            entidade.CompanyId = Int32.Parse(dr["CompanyId"].ToString());

                        if (dr["AssinaturaId"] != DBNull.Value)
                            entidade.AssinaturaId = Int32.Parse(dr["AssinaturaId"].ToString());

                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();

                        if (dr["Perfil"] != DBNull.Value)
                            entidade.Perfil = dr["Perfil"].ToString();

                        if (dr["ModeloCabecalho"] != DBNull.Value)
                            entidade.ModeloCabecalho = dr["ModeloCabecalho"].ToString();

                        if (dr["ModeloCorpo"] != DBNull.Value)
                            entidade.ModeloCorpo = dr["ModeloCorpo"].ToString();

                        if (dr["ModeloRodape"] != DBNull.Value)
                            entidade.ModeloRodape = dr["ModeloRodape"].ToString();
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

        public DocumentoModeloVersaoItem CarregarDocumentoVersaoUltima(int modeloid, int companyid, int tipomoduloid)
        {
            DocumentoModeloVersaoItem entidade = new DocumentoModeloVersaoItem();
            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder sbSQLPaged = new StringBuilder();

                    sbSQLPaged.AppendLine("SELECT top 1 * FROM DocumentoModeloVersao WHERE CompanyId = @CompanyId and ModeloId = @ModeloId and TipoModuloId = @TipoModuloId order by Id desc ");

                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ModeloId", MySqlDbType.Int32, modeloid));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@TipoModuloId", MySqlDbType.Int32, tipomoduloid));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, companyid));
                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        entidade = new DocumentoModeloVersaoItem();
                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["TipoModuloId"] != DBNull.Value)
                            entidade.TipoModuloId = Int32.Parse(dr["TipoModuloId"].ToString());

                        if (dr["CompanyId"] != DBNull.Value)
                            entidade.CompanyId = Int32.Parse(dr["CompanyId"].ToString());

                        if (dr["ModeloCorpo"] != DBNull.Value)
                            entidade.ModeloCorpo = dr["ModeloCorpo"].ToString();

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

        public DocumentoModeloVersaoItem SalvarDocumentoVersao(int modeloid, int moduloid, int tipomoduloid, int companyid, string texto)
        {
            var entidade = CarregarDocumentoVersao(modeloid, moduloid, tipomoduloid, companyid);
            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();


                    if (entidade.Id > 0)
                    {
                        sbSQL.AppendLine(" UPDATE DocumentoModeloVersao SET ModuloId = @ModuloId, ModeloId = @ModeloId, TipoModuloId = @TipoModuloId,  ModeloCorpo = @ModeloCorpo,   CompanyId = @CompanyId");
                        sbSQL.AppendLine(" WHERE ID = @Id ");

                    }
                    else
                    {
                        sbSQL.AppendLine(" INSERT INTO DocumentoModeloVersao ( ModuloId, ModeloId, TipoModuloId,  ModeloCorpo, DtCadastro, CompanyId ) VALUES ( @ModuloId, @ModeloId, @TipoModuloId,  @ModeloCorpo, @DtCadastro, @CompanyId  ); SELECT LAST_INSERT_ID(); "); // LASTNAME, GENDER, BIRTHDAY
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
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ModuloId", MySqlDbType.Int32, entidade.ModuloId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ModeloId", MySqlDbType.Int32, entidade.ModeloId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@TipoModuloId", MySqlDbType.Int32, entidade.TipoModuloId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, entidade.CompanyId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ModeloCorpo", MySqlDbType.String, entidade.ModeloCorpo));

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

        public DocumentoModeloVersaoItem CarregarDocumentoVersao(int modeloid, int moduloid, int tipomoduloid, int companyid)
        {
            DocumentoModeloVersaoItem entidade = new DocumentoModeloVersaoItem();
            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder sbSQLPaged = new StringBuilder();

                    sbSQLPaged.AppendLine("SELECT top 1 * FROM DocumentoModeloVersao WHERE CompanyId = @CompanyId and ModuloId = @ModuloId and ModeloId = @ModeloId and TipoModuloId = @TipoModuloId order by Id desc ");

                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ModeloId", MySqlDbType.Int32, modeloid));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@TipoModuloId", MySqlDbType.Int32, tipomoduloid));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, companyid));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ModuloId", MySqlDbType.Int32, moduloid));
                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        entidade = new DocumentoModeloVersaoItem();
                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["TipoModuloId"] != DBNull.Value)
                            entidade.TipoModuloId = Int32.Parse(dr["TipoModuloId"].ToString());

                        if (dr["CompanyId"] != DBNull.Value)
                            entidade.CompanyId = Int32.Parse(dr["CompanyId"].ToString());

                        if (dr["ModeloCorpo"] != DBNull.Value)
                            entidade.ModeloCorpo = dr["ModeloCorpo"].ToString();

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
        public DocumentoModeloVersaoItem CarregarDocumentoVersao(int id)
        {
            DocumentoModeloVersaoItem entidade = new DocumentoModeloVersaoItem();
            try
            {
                using (var connection = _DBGetConnection())
                {


                    StringBuilder sbSQLPaged = new StringBuilder();

                    sbSQLPaged.AppendLine("SELECT * FROM DocumentoModeloVersao WHERE Id = @Id");

                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Id", MySqlDbType.Int32, id));
                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        entidade = new DocumentoModeloVersaoItem();
                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["TipoModuloId"] != DBNull.Value)
                            entidade.TipoModuloId = Int32.Parse(dr["TipoModuloId"].ToString());

                        if (dr["CompanyId"] != DBNull.Value)
                            entidade.CompanyId = Int32.Parse(dr["CompanyId"].ToString());

                        if (dr["ModeloCorpo"] != DBNull.Value)
                            entidade.ModeloCorpo = dr["ModeloCorpo"].ToString();

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


        public List<DocumentoModeloItem> CarregarDocumentos(int companyid, int ct)
        {
            List<DocumentoModeloItem> listagem = new List<DocumentoModeloItem>();
            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQLPaged = new StringBuilder();

                    sbSQLPaged.AppendLine("SELECT * FROM DocumentoModelo WHERE CompanyId = @CompanyId ");

                    var dataProc = _DBGetCommand(sbSQLPaged.ToString(), connection);
                    dataProc.CommandType = CommandType.Text;
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Id", MySqlDbType.Int32, companyid));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        var entidade = new DocumentoModeloItem();
                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["CompanyId"] != DBNull.Value)
                            entidade.CompanyId = Int32.Parse(dr["CompanyId"].ToString());

                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();

                        if (dr["Perfil"] != DBNull.Value)
                            entidade.Perfil = dr["Perfil"].ToString();

                        if (dr["ModeloCabecalho"] != DBNull.Value)
                            entidade.ModeloCabecalho = dr["ModeloCabecalho"].ToString();

                        if (dr["ModeloCorpo"] != DBNull.Value)
                            entidade.ModeloCorpo = dr["ModeloCorpo"].ToString();

                        if (dr["ModeloRodape"] != DBNull.Value)
                            entidade.ModeloRodape = dr["ModeloRodape"].ToString();

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


    }
}
