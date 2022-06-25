using MySql.Data.MySqlClient;
using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace PortalVet.Data.Service
{
    public class ExameService : ProviderConnection, IExameService
    {

        private int _tempoExpirarLaudadorHoras = 24;

        public List<SelectListWeb> CarregarExamesPendentesLaudador(int laudadorId)
        {
            List<SelectListWeb> listagem = new List<SelectListWeb>();
            try
            {

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(" SELECT CompanyId, COUNT(*) AS Total  ");
                sbSQL.AppendLine(" FROM exame ");
                sbSQL.AppendLine(" WHERE LaudadorId = 0 OR (LaudadorId = @ID AND SituacaoId = 3) ");
                sbSQL.AppendLine(" GROUP BY CompanyId ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, laudadorId));

                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            SelectListWeb entidade = new SelectListWeb();

                            if (dr["CompanyId"] != DBNull.Value)
                                entidade.ID = dr["CompanyId"].ToString();

                            if (dr["Total"] != DBNull.Value)
                                entidade.Total = Int32.Parse(dr["Total"].ToString());

                            listagem.Add(entidade);

                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }
            return listagem;
        }

        public List<SelectListWeb> CarregarExamesPendentesGerente(int gerenteId)
        {
            List<SelectListWeb> listagem = new List<SelectListWeb>();
            try
            {

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(" SELECT CompanyId, COUNT(*) AS Total  ");
                sbSQL.AppendLine(" FROM exame ");
                sbSQL.AppendLine(" WHERE SituacaoId = 5 ");
                sbSQL.AppendLine(" GROUP BY CompanyId ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        //cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, laudadorId));

                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            SelectListWeb entidade = new SelectListWeb();

                            if (dr["CompanyId"] != DBNull.Value)
                                entidade.ID = dr["CompanyId"].ToString();

                            if (dr["Total"] != DBNull.Value)
                                entidade.Total = Int32.Parse(dr["Total"].ToString());

                            listagem.Add(entidade);

                        }
                        dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }
            return listagem;
        }
        public ExameItem Get(int Id)
        {
            ExameItem entidade = new ExameItem();
            try
            {

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.AppendLine(" SELECT Exame.*, AUCLI.Email as EmailCliente FROM   ");
                sbSQL.AppendLine(" Exame ");
                sbSQL.AppendLine(" LEFT JOIN AdminUser as AUCLI on AUCLI.Id = Exame.ClienteId");
                sbSQL.AppendLine(" WHERE Exame.Id = @ID ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, Id));

                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["AssinaturaId"] != DBNull.Value)
                                entidade.AssinaturaId = Int32.Parse(dr["AssinaturaId"].ToString());

                            if (dr["DataExame"] != DBNull.Value)
                                entidade.DataExame = DateTime.Parse(dr["DataExame"].ToString());

                            if (dr["DataCadastro"] != DBNull.Value)
                                entidade.DataCadastro = DateTime.Parse(dr["DataCadastro"].ToString());

                            if (dr["SituacaoId"] != DBNull.Value)
                                entidade.SituacaoId = Int32.Parse(dr["SituacaoId"].ToString());

                            if (dr["ClienteId"] != DBNull.Value)
                                entidade.ClienteId = Int32.Parse(dr["ClienteId"].ToString());

                            if (dr["EmailCliente"] != DBNull.Value)
                                entidade.EmailCliente = dr["EmailCliente"].ToString();

                            if (dr["LaudadorId"] != DBNull.Value)
                                entidade.LaudadorId = Int32.Parse(dr["LaudadorId"].ToString());

                            if (dr["CompanyId"] != DBNull.Value)
                                entidade.CompanyId = Int32.Parse(dr["CompanyId"].ToString());

                            if (dr["Veterinario"] != DBNull.Value)
                                entidade.Veterinario = dr["Veterinario"].ToString();

                            if (dr["Paciente"] != DBNull.Value)
                                entidade.Paciente = dr["Paciente"].ToString();

                            //if (dr["Proprietario"] != DBNull.Value)
                            //    entidade.Proprietario = dr["Proprietario"].ToString();

                            if (dr["Idade"] != DBNull.Value)
                                entidade.Idade = dr["Idade"].ToString();



                            if (dr["Proprietario"] != DBNull.Value)
                                entidade.Proprietario = dr["Proprietario"].ToString();

                            if (dr["ProprietarioEmail"] != DBNull.Value)
                                entidade.ProprietarioEmail = dr["ProprietarioEmail"].ToString();

                            if (dr["ProprietarioTelefone"] != DBNull.Value)
                                entidade.ProprietarioTelefone = dr["ProprietarioTelefone"].ToString();

                            //if (dr["EspecieNome"] != DBNull.Value)
                            //  entidade.EspecieNome = dr["EspecieNome"].ToString();

                            if (dr["EspecieOutros"] != DBNull.Value)
                                entidade.EspecieOutros = dr["EspecieOutros"].ToString();

                            if (dr["EspecieId"] != DBNull.Value)
                                entidade.EspecieId = Int32.Parse(dr["EspecieId"].ToString());

                            if (dr["Descricao"] != DBNull.Value)
                                entidade.Descricao = dr["Descricao"].ToString();

                            if (dr["Rodape"] != DBNull.Value)
                                entidade.Rodape = dr["Rodape"].ToString();

                            if (dr["RacaId"] != DBNull.Value)
                                entidade.RacaId = Int32.Parse(dr["RacaId"].ToString());

                            //if (dr["RacaNome"] != DBNull.Value)
                            //    entidade.RacaNome = dr["RacaNome"].ToString();

                            if (dr["Historico"] != DBNull.Value)
                                entidade.Historico = dr["Historico"].ToString();

                            if (dr["FormaPagamento"] != DBNull.Value)
                                entidade.FormaPagamento = dr["FormaPagamento"].ToString();

                            if (dr["Valor"] != DBNull.Value)
                                entidade.Valor = dr["Valor"].ToString();

                            if (dr["RacaOutros"] != DBNull.Value)
                                entidade.RacaOutros = dr["RacaOutros"].ToString();


                            if (dr["RacaSelecao"] != DBNull.Value)
                                entidade.RacaSelecao = dr["RacaSelecao"].ToString();


                            if (dr["EspecieSelecao"] != DBNull.Value)
                                entidade.EspecieSelecao = dr["EspecieSelecao"].ToString();

                            if (dr["ClinicaId"] != DBNull.Value)
                                entidade.ClinicaId = Int32.Parse(dr["ClinicaId"].ToString());


                            if (dr["DataVinculoLaudador"] != DBNull.Value)
                                entidade.DataVinculoLaudador = DateTime.Parse(dr["DataVinculoLaudador"].ToString());
                            else
                                entidade.DataVinculoLaudador = null;

                            if (entidade.DataVinculoLaudador.HasValue)
                            {
                                System.TimeSpan diffResult = DateTime.Now.Subtract(entidade.DataVinculoLaudador.Value.AddHours(_tempoExpirarLaudadorHoras));
                                if (diffResult.Minutes < 0)
                                {
                                    entidade.PeriodoTermino = $"{(-1 * diffResult.Hours).ToString()}:{(-1 * diffResult.Minutes).ToString()} restantes";

                                    var mes = entidade.DataVinculoLaudador.Value.ToString("MMM", CultureInfo.CurrentCulture);
                                    var dia = entidade.DataVinculoLaudador.Value.Day;
                                    var ano = entidade.DataVinculoLaudador.Value.Year;
                                    var time = entidade.DataVinculoLaudador.Value.ToString("HH:mm:ss");
                                    entidade.PeriodoTerminoFmt = $"{mes} {dia}, {ano} {time}";

                                    var dt2 = entidade.DataVinculoLaudador.Value.AddHours(_tempoExpirarLaudadorHoras);
                                    var mes2 = dt2.ToString("MMM", CultureInfo.CurrentCulture);
                                    var dia2 = dt2.Day;
                                    var ano2 = dt2.Year;
                                    var time2 = dt2.ToString("HH:mm:ss");

                                    entidade.PeriodoTermino2Fmt = $"{mes2} {dia2}, {ano2} {time2}";
                                }
                                else
                                {
                                    var mes = entidade.DataVinculoLaudador.Value.ToString("MMM", CultureInfo.CurrentCulture);
                                    var dia = entidade.DataVinculoLaudador.Value.Day;
                                    var ano = entidade.DataVinculoLaudador.Value.Year;
                                    var time = entidade.DataVinculoLaudador.Value.ToString("HH:mm:ss");
                                    entidade.PeriodoTerminoFmt = $"{mes} {dia}, {ano} {time}";
                                    var dt2 = entidade.DataVinculoLaudador.Value.AddHours(_tempoExpirarLaudadorHoras);
                                    var mes2 = dt2.ToString("MMM", CultureInfo.CurrentCulture);
                                    var dia2 = dt2.Day;
                                    var ano2 = dt2.Year;
                                    var time2 = dt2.ToString("HH:mm:ss");

                                    entidade.PeriodoTermino2Fmt = $"{mes2} {dia2}, {ano2} {time2}";
                                    entidade.PeriodoTermino = "";
                                }
                            }
                            else
                            {
                                entidade.PeriodoTermino = "";
                                entidade.PeriodoTerminoFmt = "";
                                entidade.PeriodoTermino2Fmt = "";
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




        public void SaveHistorico(ExameHistoricoItem entidade)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine("INSERT INTO ExameHistorico (ExameId, UsuarioId, UsuarioNome, UsuarioEmail, Descricao, SituacaoId, Conteudo, DataCadastro) VALUES ( @ExameId, @UsuarioId, @UsuarioNome, @UsuarioEmail, @Descricao, @SituacaoId, @Conteudo, NOW());  ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ExameId", MySqlDbType.Int32, entidade.ExameId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UsuarioId", MySqlDbType.Int32, entidade.UsuarioId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UsuarioNome", MySqlDbType.String, entidade.UsuarioNome));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UsuarioEmail", MySqlDbType.String, entidade.UsuarioEmail));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Descricao", MySqlDbType.String, entidade.Descricao));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SituacaoId", MySqlDbType.Int32, entidade.SituacaoId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Conteudo", MySqlDbType.String, entidade.Conteudo));

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




        public void SaveImageHistorico(bool result, string fileName, string formatImg, string location, int unidade, int exameid, string exception)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine("INSERT INTO ExameImageHistorico (ExameId, Resultado, UnidadeId, FileName, Ext, Location, Exception, DataCadastro) VALUES ( @ExameId, @Resultado, @UnidadeId, @FileName, @Ext, @Location, @Exception, NOW());  ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ExameId", MySqlDbType.Int32, exameid));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UnidadeId", MySqlDbType.Int32, unidade));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Resultado", MySqlDbType.Bit, result));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@FileName", MySqlDbType.String, fileName));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Exception", MySqlDbType.String, exception));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Ext", MySqlDbType.String, formatImg));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Location", MySqlDbType.String, location));

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


        public List<ExameHistoricoItem> GetHistorico(int exameId)
        {
            List<ExameHistoricoItem> entidades = new List<ExameHistoricoItem>();

            try
            {

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(" SELECT * FROM   ");
                sbSQL.AppendLine(" ExameHistorico ");
                sbSQL.AppendLine(" WHERE ExameId = @ID ORDER BY DataCadastro DESC ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, exameId));

                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            ExameHistoricoItem entidade = new ExameHistoricoItem();

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["ExameId"] != DBNull.Value)
                                entidade.ExameId = Int32.Parse(dr["ExameId"].ToString());

                            if (dr["UsuarioId"] != DBNull.Value)
                                entidade.UsuarioId = Int32.Parse(dr["UsuarioId"].ToString());

                            if (dr["SituacaoId"] != DBNull.Value)
                                entidade.SituacaoId = Int32.Parse(dr["SituacaoId"].ToString());

                            if (dr["UsuarioNome"] != DBNull.Value)
                                entidade.UsuarioNome = dr["UsuarioNome"].ToString();

                            if (dr["UsuarioEmail"] != DBNull.Value)
                                entidade.UsuarioEmail = dr["UsuarioEmail"].ToString();

                            if (dr["Descricao"] != DBNull.Value)
                                entidade.Descricao = dr["Descricao"].ToString();

                            if (dr["DataCadastro"] != DBNull.Value)
                                entidade.DataCadastro = DateTime.Parse(dr["DataCadastro"].ToString());

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

        public bool naoExisteCodigo(int codigoOffline, int unidadeId, out int codigoExistente)
        {
            codigoExistente = 0;
            var result = true;
            List<ExameHistoricoItem> entidades = new List<ExameHistoricoItem>();
            try
            {

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(" SELECT ID FROM  ");
                sbSQL.AppendLine(" Exame ");
                sbSQL.AppendLine(" WHERE CompanyId = @ID and Codigo = @CODIGO ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, unidadeId));
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@CODIGO", MySqlDbType.Int32, codigoOffline));

                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            if (dr["Id"] != DBNull.Value)
                            {
                                codigoExistente = Int32.Parse(dr["Id"].ToString());
                                result = false;
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



            return result;
        }

        public List<ExameItem> List(out int pageTotal, int pageIndex, int pageSize, int pageOrderCol, string pageOrderSort, Dictionary<string, object> dicFilter)
        {
            List<ExameItem> listagem = new List<ExameItem>();
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

                                    //if (item.Value.GetType() == typeof(string))
                                    //    clauseWhere.AppendFormat(" AND {0} like '%{1}%'", item.Key, item.Value);

                                    if (item.Value.GetType() == typeof(string))
                                    {
                                        if (item.Key.EndsWith("=="))
                                        {
                                            var codeKey = item.Key.Replace("==", "");
                                            clauseWhere.AppendFormat(" AND {0} = '{1}'", codeKey, item.Value);
                                        }
                                        else if (item.Key.EndsWith("!="))
                                        {
                                            var codeKey = item.Key.Replace("!=", "");

                                            clauseWhere.AppendFormat(" AND {0} != '{1}'", codeKey, item.Value);
                                        }
                                        else
                                        {
                                            clauseWhere.AppendFormat(" AND {0} like '%{1}%'", item.Key, item.Value);
                                        }
                                    }


                                    if (item.Value.GetType() == typeof(List<string>))
                                    {
                                        var list = (List<string>)item.Value;
                                        foreach (var itemValue in list)
                                        {
                                            if (item.Key.EndsWith("=="))
                                            {
                                                var codeKey = item.Key.Replace("==", "");
                                                clauseWhere.AppendFormat(" AND {0} = '{1}'", codeKey, itemValue);
                                            }
                                            else if (item.Key.EndsWith("!="))
                                            {
                                                var codeKey = item.Key.Replace("!=", "");

                                                clauseWhere.AppendFormat(" AND {0} != '{1}'", codeKey, itemValue);
                                            }
                                            else
                                            {
                                                clauseWhere.AppendFormat(" AND {0} like '%{1}%'", item.Key, itemValue);
                                            }
                                        }


                                    }

                                }
                                else
                                {
                                    if (item.Value.GetType() == typeof(int))
                                        clauseWhere.AppendFormat(" {0} = {1}", item.Key, item.Value);

                                    //if (item.Value.GetType() == typeof(string))
                                    //    clauseWhere.AppendFormat(" {0} like '%{1}%'", item.Key, item.Value);


                                    if (item.Value.GetType() == typeof(string))
                                    {
                                        if (item.Key.EndsWith("=="))
                                        {
                                            var codeKey = item.Key.Replace("==", "");
                                            clauseWhere.AppendFormat(" {0} = '{1}'", codeKey, item.Value);
                                        }
                                        else if (item.Key.EndsWith("!="))
                                        {
                                            var codeKey = item.Key.Replace("!=", "");

                                            clauseWhere.AppendFormat(" {0} != '{1}'", codeKey, item.Value);
                                        }
                                        else
                                        {
                                            clauseWhere.AppendFormat(" {0} like '%{1}%'", item.Key, item.Value);
                                        }
                                    }


                                    if (item.Value.GetType() == typeof(List<string>))
                                    {
                                        var list = (List<string>)item.Value;
                                        foreach (var itemValue in list)
                                        {
                                            if (item.Key.EndsWith("=="))
                                            {
                                                var codeKey = item.Key.Replace("==", "");
                                                clauseWhere.AppendFormat(" {0} = '{1}'", codeKey, itemValue);
                                            }
                                            else if (item.Key.EndsWith("!="))
                                            {
                                                var codeKey = item.Key.Replace("!=", "");

                                                clauseWhere.AppendFormat(" {0} != '{1}'", codeKey, itemValue);
                                            }
                                            else
                                            {
                                                clauseWhere.AppendFormat(" {0} like '%{1}%'", item.Key, itemValue);
                                            }
                                        }


                                    }
                                }
                            }
                        }


                    StringBuilder sbSQLPaged = new StringBuilder();


                    var strOrder = "DataExame DESC";
                    var strColumnsJoin = "Exame.*, AUC.Nome as NomeCliente, AUL.Nome as NomeLaudador ";
                    var strTable = @" Exame 
                                    left join adminUser AUC on AUC.Id = Exame.ClienteId 
                                    left join adminUser AUL on AUL.Id = Exame.LaudadorId ";

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
                        ExameItem entidade = new ExameItem();

                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());


                        if (dr["DataExame"] != DBNull.Value)
                            entidade.DataExame = DateTime.Parse(dr["DataExame"].ToString());

                        if (dr["DataCadastro"] != DBNull.Value)
                            entidade.DataCadastro = DateTime.Parse(dr["DataCadastro"].ToString());

                        if (dr["SituacaoId"] != DBNull.Value)
                            entidade.SituacaoId = Int32.Parse(dr["SituacaoId"].ToString());

                        if (dr["LaudadorSituacaoId"] != DBNull.Value)
                            entidade.LaudadorSituacaoId = Int32.Parse(dr["LaudadorSituacaoId"].ToString());

                        if (dr["ClienteId"] != DBNull.Value)
                            entidade.ClienteId = Int32.Parse(dr["ClienteId"].ToString());

                        if (dr["NomeCliente"] != DBNull.Value)
                            entidade.NomeCliente = dr["NomeCliente"].ToString();
                        else
                            entidade.NomeCliente = string.Empty;

                        if (dr["NomeLaudador"] != DBNull.Value)
                            entidade.NomeLaudador = dr["NomeLaudador"].ToString();
                        else
                            entidade.NomeLaudador = string.Empty;

                        if (dr["LaudadorId"] != DBNull.Value)
                            entidade.LaudadorId = Int32.Parse(dr["LaudadorId"].ToString());

                        if (dr["CompanyId"] != DBNull.Value)
                            entidade.CompanyId = Int32.Parse(dr["CompanyId"].ToString());


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



        public bool ArquivarExame(int exameid, int perfilid, int usuarioid)
        {
            var result = false;
            if (perfilid == EnumAdminProfile.Gerente.GetHashCode())
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {
                        StringBuilder sbSQL = new StringBuilder();

                        sbSQL.AppendLine($"UPDATE Exame SET ArquivadoGerente = 1, UsuarioArquivadoGerente = {usuarioid} WHERE Id = {exameid}");
                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                        dataProc.CommandType = CommandType.Text;

                        // executa comando.
                        dataProc.ExecuteNonQuery();

                        dataProc.Dispose();

                        result = true;
                    }

                }
                catch (Exception ex)
                {
                    result = false;
                }

            }

            if (perfilid == EnumAdminProfile.Clinica.GetHashCode())
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {
                        StringBuilder sbSQL = new StringBuilder();

                        sbSQL.AppendLine($"UPDATE Exame SET ArquivadoClinica = 1, UsuarioArquivadoClinica = {usuarioid} WHERE Id = {exameid}");
                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                        dataProc.CommandType = CommandType.Text;

                        // executa comando.
                        dataProc.ExecuteNonQuery();

                        dataProc.Dispose();

                        result = true;
                    }

                }
                catch (Exception ex)
                {
                    result = false;
                }

            }

            if (perfilid == EnumAdminProfile.Laudador.GetHashCode())
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {
                        StringBuilder sbSQL = new StringBuilder();

                        sbSQL.AppendLine($"UPDATE Exame SET ArquivadoLaudador = 1, UsuarioArquivadoLaudador = {usuarioid} WHERE Id = {exameid}");
                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                        dataProc.CommandType = CommandType.Text;

                        // executa comando.
                        dataProc.ExecuteNonQuery();

                        dataProc.Dispose();

                        result = true;
                    }

                }
                catch (Exception ex)
                {
                    result = false;
                }
            }

            return result;
        }


        public void Remove(int id)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine($"DELETE FROM ExameHistorico WHERE ExameId in ({id});DELETE FROM Exame WHERE Id in ({id})");

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


        private int GetExameDateCount(DateTime dataExame, int companyId)
        {
            var total = 0;
            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@" SELECT COUNT(*) AS Total FROM exame
                                    WHERE CompanyId = @COMPANYID and DAY(DataExame) = @DIA && Month(DataExame) = @MES && YEAR(DataExame) = @ANO;");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Cliente.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@COMPANYID", MySqlDbType.Int32, companyId));
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@DIA", MySqlDbType.Int32, dataExame.Day));
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@MES", MySqlDbType.Int32, dataExame.Month));
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ANO", MySqlDbType.Int32, dataExame.Year));
                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            if (dr["Total"] != DBNull.Value)
                                total = Int32.Parse(dr["Total"].ToString());
                        }
                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }
            return total;

        }
        public ExameItem CriarExame(ExameItem entidade)
        {
            try
            {
                var contador = GetExameDateCount(entidade.DataExame, entidade.CompanyId);
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.AppendLine("INSERT INTO Exame (DataExame, SituacaoId, SituacaoNome, CompanyId, DataCadastro) VALUES ( @DataExame, @SituacaoId, @SituacaoNome, @CompanyId, NOW()); SELECT LAST_INSERT_ID(); ");
                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@DataExame", MySqlDbType.DateTime, entidade.DataExame));
                    var situacaoId = EnumExameSituacao.Criacao.GetHashCode();
                    var situacaoNome = Enumeradores.GetDescription(EnumExameSituacao.Criacao);
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SituacaoId", MySqlDbType.Int32, situacaoId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SituacaoNome", MySqlDbType.String, situacaoNome));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, entidade.CompanyId));


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



        public ExameItem SaveDetailLaudador(ExameItem entidade)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    if (entidade.Id > 0)
                    {
                        sbSQL.AppendLine("UPDATE Exame SET  Descricao = @Descricao, Rodape = @Rodape, SituacaoId = @SituacaoId, AssinaturaId = @AssinaturaId, SituacaoNome = @SituacaoNome ");
                        sbSQL.AppendLine("WHERE ID = @Id ");
                    }

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Id", MySqlDbType.Int32, entidade.Id));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@AssinaturaId", MySqlDbType.Int32, entidade.AssinaturaId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SituacaoId", MySqlDbType.Int32, entidade.SituacaoId));
                    var _situacaoNome = Enumeradores.GetDescription((EnumExameSituacao)entidade.SituacaoId);
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SituacaoNome", MySqlDbType.String, _situacaoNome));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Descricao", MySqlDbType.String, entidade.Descricao));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Rodape", MySqlDbType.String, entidade.Rodape));


                    // executa comando.
                    dataProc.ExecuteNonQuery();
                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return entidade;
        }


        public ExameItem Save(ExameItem entidade)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    if (String.IsNullOrEmpty(entidade.Valor) &&
                        String.IsNullOrEmpty(entidade.FormaPagamento))
                    {
                        if (entidade.Id > 0)
                        {
                            sbSQL.AppendLine("UPDATE Exame SET  Codigo = @Codigo, Veterinario = @Veterinario, Idade = @Idade, EspecieSelecao = @EspecieSelecao, RacaSelecao = @RacaSelecao, EspecieOutros = @EspecieOutros, EspecieId = @EspecieId, Paciente = @Paciente, Descricao = @Descricao, Rodape = @Rodape, RacaId = @RacaId, DataExame = @DataExame, SituacaoId = @SituacaoId, SituacaoNome = @SituacaoNome, ClienteId = @ClienteId, LaudadorId = @LaudadorId, CompanyId = @CompanyId, Historico = @Historico, RacaOutros = @RacaOutros, ClinicaId = @ClinicaId, Proprietario = @Proprietario, ProprietarioEmail = @ProprietarioEmail, ProprietarioTelefone = @ProprietarioTelefone  ");
                            sbSQL.AppendLine("WHERE ID = @Id ");
                        }
                        else
                        {
                            sbSQL.AppendLine("INSERT INTO Exame (Codigo, DataExame, SituacaoId, SituacaoNome, ClienteId, LaudadorId, CompanyId, DataCadastro, DataAtualizacao, Idade, Veterinario, EspecieSelecao, RacaSelecao, EspecieOutros, EspecieId, Paciente, Descricao, Rodape, RacaId, Historico, RacaOutros, ClinicaId, Proprietario, ProprietarioEmail, ProprietarioTelefone, UsuarioArquivadoLaudador, UsuarioArquivadoGerente, UsuarioArquivadoClinica, ArquivadoLaudador, ArquivadoGerente, ArquivadoClinica) VALUES ( @Codigo, @DataExame, @SituacaoId, @SituacaoNome, @ClienteId, @LaudadorId, @CompanyId, NOW(),  NOW(), @Idade, @Veterinario, @EspecieSelecao, @RacaSelecao, @EspecieOutros, @EspecieId, @Paciente, @Descricao, @Rodape, @RacaId, @Historico, @RacaOutros, @ClinicaId, @Proprietario, @ProprietarioEmail, @ProprietarioTelefone, @UsuarioArquivadoLaudador, @UsuarioArquivadoGerente, @UsuarioArquivadoClinica, @ArquivadoLaudador, @ArquivadoGerente, @ArquivadoClinica); SELECT LAST_INSERT_ID(); ");
                        }
                    }
                    else
                    {
                        if (entidade.Id > 0)
                        {
                            sbSQL.AppendLine("UPDATE Exame SET  Codigo = @Codigo, Veterinario = @Veterinario, Idade = @Idade, EspecieSelecao = @EspecieSelecao, RacaSelecao = @RacaSelecao, EspecieOutros = @EspecieOutros, EspecieId = @EspecieId, Paciente = @Paciente, Descricao = @Descricao, Rodape = @Rodape, RacaId = @RacaId, DataExame = @DataExame, SituacaoId = @SituacaoId, SituacaoNome = @SituacaoNome, ClienteId = @ClienteId, LaudadorId = @LaudadorId, CompanyId = @CompanyId, Historico = @Historico, Valor = @Valor, FormaPagamento = @FormaPagamento, RacaOutros = @RacaOutros, ClinicaId = @ClinicaId, Proprietario = @Proprietario, ProprietarioEmail = @ProprietarioEmail, ProprietarioTelefone = @ProprietarioTelefone ");
                            sbSQL.AppendLine("WHERE ID = @Id ");
                        }
                        else
                        {
                            sbSQL.AppendLine("INSERT INTO Exame (Codigo, DataExame, SituacaoId, SituacaoNome, ClienteId, LaudadorId, CompanyId, DataCadastro, DataAtualizacao, Idade, Veterinario, EspecieSelecao, RacaSelecao, EspecieOutros, EspecieId, Paciente, Descricao, Rodape, RacaId, Historico, Valor, FormaPagamento, RacaOutros, ClinicaId, Proprietario, ProprietarioEmail, ProprietarioTelefone, UsuarioArquivadoLaudador, UsuarioArquivadoGerente, UsuarioArquivadoClinica, ArquivadoLaudador, ArquivadoGerente, ArquivadoClinica) VALUES ( @Codigo, @DataExame, @SituacaoId, @SituacaoNome, @ClienteId, @LaudadorId, @CompanyId, NOW(), NOW(), @Idade, @Veterinario, @EspecieSelecao, @RacaSelecao, @EspecieOutros, @EspecieId, @Paciente, @Descricao, @Rodape, @RacaId, @Historico, @Valor, @FormaPagamento, @RacaOutros, @ClinicaId, @Proprietario, @ProprietarioEmail, @ProprietarioTelefone, @UsuarioArquivadoLaudador, @UsuarioArquivadoGerente, @UsuarioArquivadoClinica, @ArquivadoLaudador, @ArquivadoGerente, @ArquivadoClinica); SELECT LAST_INSERT_ID(); ");
                        }
                    }



                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    if (entidade.Id > 0)
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Id", MySqlDbType.Int32, entidade.Id));
                    }

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Codigo", MySqlDbType.Int32, entidade.Codigo));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@DataExame", MySqlDbType.DateTime, entidade.DataExame));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SituacaoId", MySqlDbType.Int32, entidade.SituacaoId));
                    var _situacaoNome = Enumeradores.GetDescription((EnumExameSituacao)entidade.SituacaoId);
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SituacaoNome", MySqlDbType.String, _situacaoNome));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ClienteId", MySqlDbType.Int32, entidade.ClienteId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@LaudadorId", MySqlDbType.Int32, entidade.LaudadorId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, entidade.CompanyId));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Idade", MySqlDbType.String, entidade.Idade));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Veterinario", MySqlDbType.String, entidade.Veterinario));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Paciente", MySqlDbType.String, entidade.Paciente));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EspecieOutros", MySqlDbType.String, entidade.EspecieOutros));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EspecieSelecao", MySqlDbType.String, entidade.EspecieSelecao));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@RacaSelecao", MySqlDbType.String, entidade.RacaSelecao));


                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EspecieId", MySqlDbType.Int32, entidade.EspecieId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Proprietario", MySqlDbType.String, entidade.Proprietario));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ProprietarioEmail", MySqlDbType.String, entidade.ProprietarioEmail));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ProprietarioTelefone", MySqlDbType.String, entidade.ProprietarioTelefone));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Descricao", MySqlDbType.String, entidade.Descricao));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Rodape", MySqlDbType.String, entidade.Rodape));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@RacaId", MySqlDbType.Int32, entidade.RacaId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@RacaOutros", MySqlDbType.String, entidade.RacaOutros));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ClinicaId", MySqlDbType.Int32, entidade.ClinicaId));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UsuarioArquivadoLaudador", MySqlDbType.Int32, 0));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UsuarioArquivadoGerente", MySqlDbType.Int32, 0));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UsuarioArquivadoClinica", MySqlDbType.Int32, 0));


                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ArquivadoLaudador", MySqlDbType.Bit, false));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ArquivadoGerente", MySqlDbType.Bit, false));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ArquivadoClinica", MySqlDbType.Bit, false));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Historico", MySqlDbType.String, entidade.Historico));

                    if (!String.IsNullOrEmpty(entidade.Valor) ||
                        !String.IsNullOrEmpty(entidade.FormaPagamento))
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Valor", MySqlDbType.String, entidade.Valor));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@FormaPagamento", MySqlDbType.String, entidade.FormaPagamento));
                    }

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

        public ExameItem VincularLaudador(ExameItem entidade)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    if (entidade.Id > 0)
                    {
                        sbSQL.AppendLine("UPDATE Exame SET  LaudadorId = @LaudadorId, DataVinculoLaudador = @DataVinculoLaudador ");
                        sbSQL.AppendLine("WHERE ID = @Id ");
                    }



                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    if (entidade.Id > 0)
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Id", MySqlDbType.Int32, entidade.Id));

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@LaudadorId", MySqlDbType.Int32, entidade.LaudadorId));

                        if (entidade.LaudadorId > 0)
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@DataVinculoLaudador", MySqlDbType.DateTime, DateTime.Now));
                        else
                            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@DataVinculoLaudador", MySqlDbType.DateTime, DBNull.Value));

                    }



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


        public ExameItem SaveSemProp(ExameItem entidade)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    if (String.IsNullOrEmpty(entidade.Valor) &&
                        String.IsNullOrEmpty(entidade.FormaPagamento))
                    {
                        if (entidade.Id > 0)
                        {
                            sbSQL.AppendLine("UPDATE Exame SET  Codigo = @Codigo, AssinaturaId = @AssinaturaId, Veterinario = @Veterinario, DataAtualizacao = @DataAtualizacao, Idade = @Idade, EspecieSelecao = @EspecieSelecao, RacaSelecao = @RacaSelecao, EspecieOutros = @EspecieOutros, EspecieId = @EspecieId, Paciente = @Paciente, Descricao = @Descricao, Rodape = @Rodape, RacaId = @RacaId, DataExame = @DataExame, SituacaoId = @SituacaoId, SituacaoNome = @SituacaoNome, ClienteId = @ClienteId,  CompanyId = @CompanyId, Historico = @Historico, RacaOutros = @RacaOutros, ClinicaId = @ClinicaId ");
                            sbSQL.AppendLine("WHERE ID = @Id ");
                        }
                        else
                        {
                            sbSQL.AppendLine("INSERT INTO Exame (Codigo, AssinaturaId, DataExame, SituacaoId, SituacaoNome, ClienteId, CompanyId, DataCadastro, DataAtualizacao, Idade, Veterinario, EspecieSelecao, RacaSelecao, EspecieOutros, EspecieId, Paciente, Descricao, Rodape, RacaId, Historico, RacaOutros, ClinicaId ) VALUES ( @Codigo, @AssinaturaId, @DataExame, @SituacaoId, @SituacaoNome, @ClienteId, @CompanyId, NOW(), @Idade, @Veterinario, @EspecieSelecao, @RacaSelecao, @EspecieOutros, @EspecieId, @Paciente, @Descricao, @Rodape, @RacaId, @Historico, @RacaOutros, @ClinicaId); SELECT LAST_INSERT_ID(); ");
                        }
                    }
                    else
                    {
                        if (entidade.Id > 0)
                        {
                            sbSQL.AppendLine("UPDATE Exame SET  Codigo = @Codigo, AssinaturaId = @AssinaturaId, Veterinario = @Veterinario,  DataAtualizacao = @DataAtualizacao, Idade = @Idade, EspecieSelecao = @EspecieSelecao, RacaSelecao = @RacaSelecao, EspecieOutros = @EspecieOutros, EspecieId = @EspecieId, Paciente = @Paciente, Descricao = @Descricao, Rodape = @Rodape, RacaId = @RacaId, DataExame = @DataExame, SituacaoId = @SituacaoId, SituacaoNome = @SituacaoNome, ClienteId = @ClienteId, CompanyId = @CompanyId, Historico = @Historico, Valor = @Valor, FormaPagamento = @FormaPagamento, RacaOutros = @RacaOutros, ClinicaId = @ClinicaId ");
                            sbSQL.AppendLine("WHERE ID = @Id ");
                        }
                        else
                        {
                            sbSQL.AppendLine("INSERT INTO Exame (Codigo, AssinaturaId, DataExame, SituacaoId, SituacaoNome, ClienteId, CompanyId, DataCadastro, DataAtualizacao, Idade, Veterinario, EspecieSelecao, RacaSelecao, EspecieOutros, EspecieId, Paciente, Descricao, Rodape, RacaId, Historico, Valor, FormaPagamento, RacaOutros, ClinicaId) VALUES ( @Codigo, @AssinaturaId, @DataExame, @SituacaoId, @SituacaoNome, @ClienteId, @CompanyId, NOW(), @Idade, @Veterinario, @EspecieSelecao, @RacaSelecao, @EspecieOutros, @EspecieId, @Paciente, @Descricao, @Rodape, @RacaId, @Historico, @Valor, @FormaPagamento, @RacaOutros, @ClinicaId); SELECT LAST_INSERT_ID(); ");
                        }
                    }


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    if (entidade.Id > 0)
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Id", MySqlDbType.Int32, entidade.Id));
                    }

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Codigo", MySqlDbType.Int32, entidade.Codigo));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@AssinaturaId", MySqlDbType.Int32, entidade.AssinaturaId)); 
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@DataAtualizacao", MySqlDbType.DateTime, DateTime.Now));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@DataExame", MySqlDbType.DateTime, entidade.DataExame));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SituacaoId", MySqlDbType.Int32, entidade.SituacaoId));
                    var _situacaoNome = Enumeradores.GetDescription((EnumExameSituacao)entidade.SituacaoId);
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@SituacaoNome", MySqlDbType.String, _situacaoNome));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ClienteId", MySqlDbType.Int32, entidade.ClienteId));
                    //dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@LaudadorId", MySqlDbType.Int32, entidade.LaudadorId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, entidade.CompanyId));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Idade", MySqlDbType.String, entidade.Idade));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Veterinario", MySqlDbType.String, entidade.Veterinario));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Paciente", MySqlDbType.String, entidade.Paciente));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EspecieOutros", MySqlDbType.String, entidade.EspecieOutros));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EspecieSelecao", MySqlDbType.String, entidade.EspecieSelecao));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@RacaSelecao", MySqlDbType.String, entidade.RacaSelecao));


                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EspecieId", MySqlDbType.Int32, entidade.EspecieId));
                    //dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Proprietario", MySqlDbType.String, entidade.Proprietario));
                    //dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ProprietarioEmail", MySqlDbType.String, entidade.ProprietarioEmail));
                    //dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ProprietarioTelefone", MySqlDbType.String, entidade.ProprietarioTelefone));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Descricao", MySqlDbType.String, entidade.Descricao));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Rodape", MySqlDbType.String, entidade.Rodape));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@RacaId", MySqlDbType.Int32, entidade.RacaId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@RacaOutros", MySqlDbType.String, entidade.RacaOutros));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ClinicaId", MySqlDbType.Int32, entidade.ClinicaId));

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Historico", MySqlDbType.String, entidade.Historico));

                    if (!String.IsNullOrEmpty(entidade.Valor) ||
                        !String.IsNullOrEmpty(entidade.FormaPagamento))
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Valor", MySqlDbType.String, entidade.Valor));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@FormaPagamento", MySqlDbType.String, entidade.FormaPagamento));
                    }

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

        public List<AdminUserItem> CarregarClientes(int empresaId)
        {
            List<AdminUserItem> entidades = new List<AdminUserItem>();

            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@" SELECT cliente.Id, cliente.Nome, cliente.Email FROM cliente
                        JOIN clientecompany ON clientecompany.UserId = cliente.Id
                        WHERE  clientecompany.CompanyId = @COMPANYID
                        ORDER BY cliente.Nome    ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Cliente.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@COMPANYID", MySqlDbType.Int32, empresaId));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            AdminUserItem entidade = new AdminUserItem();

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Email = dr["Email"].ToString();


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



        public List<AdminUserItem> CarregarClinicas(int empresaId)
        {
            List<AdminUserItem> entidades = new List<AdminUserItem>();

            try
            {

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@" SELECT adminuser.Id, adminuser.Nome, adminuser.Email FROM adminuser
                        JOIN adminuserprofile ON adminuserprofile.UserId = adminuser.Id
                        JOIN adminusercompany ON adminusercompany.UserId = adminuser.Id
                        WHERE adminuserprofile.ProfileId = @PROFILEID AND adminusercompany.CompanyId = @COMPANYID
                        ORDER BY adminuser.Nome    ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Clinica.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@PROFILEID", MySqlDbType.Int32, code));
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@COMPANYID", MySqlDbType.Int32, empresaId));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            AdminUserItem entidade = new AdminUserItem();

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Email = dr["Email"].ToString();


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

        public List<AdminUserItem> CarregarLaudadores(int empresaId)
        {
            List<AdminUserItem> entidades = new List<AdminUserItem>();

            try
            {

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@" SELECT adminuser.Id, adminuser.Nome, adminuser.Email FROM adminuser
                        JOIN adminuserprofile ON adminuserprofile.UserId = adminuser.Id
                        JOIN adminusercompany ON adminusercompany.UserId = adminuser.Id
                        WHERE adminuserprofile.ProfileId = @PROFILEID AND adminusercompany.CompanyId = @COMPANYID
                        ORDER BY adminuser.Nome    ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Laudador.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@PROFILEID", MySqlDbType.Int32, code));
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@COMPANYID", MySqlDbType.Int32, empresaId));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            AdminUserItem entidade = new AdminUserItem();

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Email = dr["Email"].ToString();


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


        public List<DocumentoModeloItem> CarregarModelos(int exameid, int empresaId)
        {
            List<DocumentoModeloItem> entidades = new List<DocumentoModeloItem>();


            try
            {
                var exame = Get(exameid);
                if (exame.LaudadorId > 0)
                {
                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.AppendLine(@" SELECT Id, Nome FROM documentomodelo
                        WHERE CompanyId = @COMPANYID and LaudadorId = @LAUDADORID
                        ORDER BY Nome  ");

                    using (var connection = _DBGetConnection())
                    {

                        using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                        {
                            var code = EnumAdminProfile.Laudador.GetHashCode();
                            cmd.Parameters.Add(_DBBuildParameter(cmd, "@COMPANYID", MySqlDbType.Int32, empresaId));
                            cmd.Parameters.Add(_DBBuildParameter(cmd, "@LAUDADORID", MySqlDbType.Int32, exame.LaudadorId));

                            IDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                DocumentoModeloItem entidade = new DocumentoModeloItem();

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
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); throw ex;
            }

            return entidades;
        }


        /*
        public List<SelectListWeb> CarregarEmailsClinica(int clinicaId)
        {

            List<SelectListWeb> entidades = new List<SelectListWeb>();

            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@"SELECT C.Nome, C.Email FROM clinica C
                            WHERE C.Id = @ID ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Laudador.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, clinicaId));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            var entidade = new SelectListWeb();

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Text = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Value = dr["Email"].ToString();

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

        */
        public List<SelectListWeb> CarregarEmailsClinica(int clinicaId)
        {

            List<SelectListWeb> entidades = new List<SelectListWeb>();

            try
            {

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@"SELECT AU.Nome, AU.Email   FROM adminuser AU
                            WHERE AU.ID = @ID ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Laudador.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, clinicaId));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            var entidade = new SelectListWeb();

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Text = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Value = dr["Email"].ToString();

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


        public List<SelectListWeb> CarregarEmailsGerente(int companyId)
        {


            List<SelectListWeb> entidades = new List<SelectListWeb>();

            try
            {

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@"SELECT AU.Nome, AU.Email   FROM adminuser AU
                            JOIN adminuserprofile A ON A.UserId = AU.ID
                            JOIN adminusercompany C ON C.UserId = AU.ID
                            WHERE A.ProfileId = 2 AND C.CompanyId = @ID AND AU.Email IS NOT NULL ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Laudador.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, companyId));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            var entidade = new SelectListWeb();

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Text = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Value = dr["Email"].ToString();

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



        public List<SelectListWeb> CarregarEmailsLaudadorGeral(int empresaId)
        {

            List<SelectListWeb> entidades = new List<SelectListWeb>();

            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@" SELECT AU2.Nome,  AU2.Email FROM adminuser AU2
                            JOIN adminuserprofile AU3 ON AU3.UserId = AU2.Id
                            JOIN adminprofile AU4 ON AU4.Id = AU3.ProfileId
                            JOIN adminusercompany AU5 ON AU5.UserId = AU2.Id
                            WHERE AU4.Nome = 'Laudador' AND AU5.CompanyId = @EMPRESAID ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Laudador.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@EMPRESAID", MySqlDbType.Int32, empresaId));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            var entidade = new SelectListWeb();

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Text = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Value = dr["Email"].ToString();

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

        public List<SelectListWeb> CarregarEmailsLaudador(int exameId)
        {

            List<SelectListWeb> entidades = new List<SelectListWeb>();

            try
            {

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@"SELECT AU2.Nome,  AU2.Email   FROM exame E
                                    LEFT JOIN adminuser AU2 ON AU2.Id = E.LaudadorId
                                    WHERE E.ID = @ID
                                    AND AU2.Email IS NOT NULL ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Laudador.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, exameId));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            var entidade = new SelectListWeb();

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Text = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Value = dr["Email"].ToString();

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
        public List<SelectListWeb> CarregarEmailsCliente(int exameId)
        {

            List<SelectListWeb> entidades = new List<SelectListWeb>();

            try
            {

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@"SELECT AU2.Nome,  AU2.Email   FROM exame E
                                    LEFT JOIN cliente AU2 ON AU2.Id = E.ClienteId
                                    WHERE E.ID = @ID
                                    AND AU2.Email IS NOT NULL ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Laudador.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, exameId));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            var entidade = new SelectListWeb();

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Text = dr["Nome"].ToString();

                            if (dr["Email"] != DBNull.Value)
                                entidade.Value = dr["Email"].ToString();

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

        public DocumentoModeloItem CarregarModelo(int codigo)
        {
            DocumentoModeloItem entidade = new DocumentoModeloItem();

            try
            {

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@" SELECT Id, Nome, ModeloCorpo, ModeloRodape FROM documentomodelo
                        WHERE Id = @ID
                        ORDER BY Nome    ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Laudador.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ID", MySqlDbType.Int32, codigo));
                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            entidade = new DocumentoModeloItem();

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["Nome"] != DBNull.Value)
                                entidade.Nome = dr["Nome"].ToString();

                            if (dr["ModeloRodape"] != DBNull.Value)
                                entidade.ModeloRodape = dr["ModeloRodape"].ToString();

                            if (dr["ModeloCorpo"] != DBNull.Value)
                                entidade.ModeloCorpo = dr["ModeloCorpo"].ToString();

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
        public List<EspecieItem> CarregarEspecies(int empresaId)
        {
            List<EspecieItem> entidades = new List<EspecieItem>();

            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@" SELECT * FROM especie ORDER BY Ordenacao ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Laudador.GetHashCode();
                        ///cmd.Parameters.Add(_DBBuildParameter(cmd, "@COMPANYID", MySqlDbType.Int32, empresaId));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            EspecieItem entidade = new EspecieItem();

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


        public List<RacaItem> CarregarRacas(int especieId)
        {
            List<RacaItem> entidades = new List<RacaItem>();

            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@" SELECT * FROM raca WHERE EspecieId = @EspecieId ORDER BY Nome ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Laudador.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@EspecieId", MySqlDbType.Int32, especieId));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            RacaItem entidade = new RacaItem();

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

        public List<ExameHistoricoDuvidaItem> ListHistoricoDuvidaClinica(int codigo)
        {
            List<ExameHistoricoDuvidaItem> entidades = new List<ExameHistoricoDuvidaItem>();

            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@" SELECT * FROM ExameHistoricoDuvida WHERE Tipo = 'CLINICA' and ExameId = @ExameId ORDER BY DataCadastro Desc ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Laudador.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ExameId", MySqlDbType.Int32, codigo));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            ExameHistoricoDuvidaItem entidade = new ExameHistoricoDuvidaItem();

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["DataCadastro"] != DBNull.Value)
                                entidade.DataCadastro = DateTime.Parse(dr["DataCadastro"].ToString());

                            if (dr["ExameId"] != DBNull.Value)
                                entidade.ExameId = Int32.Parse(dr["ExameId"].ToString());

                            if (dr["UsuarioId"] != DBNull.Value)
                                entidade.UsuarioId = Int32.Parse(dr["UsuarioId"].ToString());

                            if (dr["UsuarioNome"] != DBNull.Value)
                                entidade.UsuarioNome = dr["UsuarioNome"].ToString();

                            if (dr["UsuarioEmail"] != DBNull.Value)
                                entidade.UsuarioEmail = dr["UsuarioEmail"].ToString();

                            if (dr["Mensagem"] != DBNull.Value)
                                entidade.Mensagem = dr["Mensagem"].ToString();

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


        public List<ExameHistoricoDuvidaItem> ListHistoricoDuvidaLaudador(int codigo)
        {
            List<ExameHistoricoDuvidaItem> entidades = new List<ExameHistoricoDuvidaItem>();

            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine(@" SELECT * FROM ExameHistoricoDuvida WHERE Tipo = 'LAUDADOR' and ExameId = @ExameId ORDER BY DataCadastro Desc ");

                using (var connection = _DBGetConnection())
                {

                    using (var cmd = _DBGetCommand(sbSQL.ToString(), connection))
                    {
                        var code = EnumAdminProfile.Laudador.GetHashCode();
                        cmd.Parameters.Add(_DBBuildParameter(cmd, "@ExameId", MySqlDbType.Int32, codigo));
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            ExameHistoricoDuvidaItem entidade = new ExameHistoricoDuvidaItem();

                            if (dr["Id"] != DBNull.Value)
                                entidade.Id = Int32.Parse(dr["Id"].ToString());

                            if (dr["DataCadastro"] != DBNull.Value)
                                entidade.DataCadastro = DateTime.Parse(dr["DataCadastro"].ToString());

                            if (dr["ExameId"] != DBNull.Value)
                                entidade.ExameId = Int32.Parse(dr["ExameId"].ToString());

                            if (dr["UsuarioId"] != DBNull.Value)
                                entidade.UsuarioId = Int32.Parse(dr["UsuarioId"].ToString());

                            if (dr["UsuarioNome"] != DBNull.Value)
                                entidade.UsuarioNome = dr["UsuarioNome"].ToString();

                            if (dr["UsuarioEmail"] != DBNull.Value)
                                entidade.UsuarioEmail = dr["UsuarioEmail"].ToString();

                            if (dr["Mensagem"] != DBNull.Value)
                                entidade.Mensagem = dr["Mensagem"].ToString();

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

        public void SaveHistoricoDuvida(ExameHistoricoDuvidaItem entidade)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine("INSERT INTO ExameHistoricoDuvida (ExameId, UsuarioId, UsuarioNome, UsuarioEmail, Mensagem, DataCadastro, Tipo) VALUES ( @ExameId, @UsuarioId, @UsuarioNome, @UsuarioEmail, @Mensagem, NOW(), @Tipo);  ");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ExameId", MySqlDbType.Int32, entidade.ExameId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UsuarioId", MySqlDbType.Int32, entidade.UsuarioId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UsuarioNome", MySqlDbType.String, entidade.UsuarioNome));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Tipo", MySqlDbType.String, entidade.Tipo));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UsuarioEmail", MySqlDbType.String, entidade.UsuarioEmail));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Mensagem", MySqlDbType.String, entidade.Mensagem));

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
