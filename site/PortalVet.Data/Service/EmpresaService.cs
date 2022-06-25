using MySql.Data.MySqlClient;
using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PortalVet.Data.Service
{
    public class EmpresaService : ProviderConnection, IEmpresaService
    {
        public EmpresaItem Carregar(int empresaid)
        {
            EmpresaItem entidade = new EmpresaItem();

            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine(" SELECT * FROM Company WHERE ID = @ID ");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, empresaid));
                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        entidade = new EmpresaItem();

                        entidade.Id = Int32.Parse(dr["Id"].ToString());
                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();


                        if (dr["Imagem"] != DBNull.Value)
                            entidade.Imagem = dr["Imagem"].ToString();

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


        public List<EmpresaItem> Carregar()
        {
            List<EmpresaItem> entidades = new List<EmpresaItem>();

            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();

                    sbSQL.AppendLine(" SELECT * FROM Company WHERE Ativo = 1 ORDER BY ID ");

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        var entidade = new EmpresaItem();

                        entidade.Id = Int32.Parse(dr["Id"].ToString());
                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();

                        if (dr["Texto"] != DBNull.Value)
                            entidade.Texto = dr["Texto"].ToString();

                        if (dr["Imagem"] != DBNull.Value)
                            entidade.Imagem = dr["Imagem"].ToString();

                        entidades.Add(entidade);
                    }


                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entidades;
        }



        private ModeloMensagemItem CarregarModelo(long id)
        {
            ModeloMensagemItem entidade = new ModeloMensagemItem();

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(" SELECT * FROM MensagemModelo Where Id = @ID ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, id));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        entidade = new ModeloMensagemItem();

                        entidade.Id = Int64.Parse(dr["Id"].ToString());
                        if (dr["CompanyId"] != DBNull.Value)
                            entidade.CompanyId = Int32.Parse(dr["CompanyId"].ToString());

                        if (dr["UsuarioId"] != DBNull.Value)
                            entidade.UsuarioId = Int32.Parse(dr["UsuarioId"].ToString());

                        if (dr["PerfilId"] != DBNull.Value)
                            entidade.PerfilId = Int32.Parse(dr["PerfilId"].ToString());

                        if (dr["Perfil"] != DBNull.Value)
                            entidade.Perfil = dr["Perfil"].ToString();

                        if (dr["Titulo"] != DBNull.Value)
                            entidade.Titulo = dr["Titulo"].ToString();

                        if (dr["Mensagem"] != DBNull.Value)
                            entidade.Mensagem = dr["Mensagem"].ToString();


                    }




                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); 
                throw ex;
            }

            return entidade;
        }

        public int CarregarClinica(int companyId, int codigoClinicaOfflineID, string nome, string email, string telefone, out bool novoRegistro,out string senha)
        {
            int result = 0;
            novoRegistro = false;
            senha = string.Empty;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(@" SELECT ID FROM
                                        (
                                            SELECT au.ID FROM adminuser au
                                            join adminuserprofile aup ON aup.UserId = au.Id
                                            JOIN adminprofile ap ON ap.Id = aup.ProfileId
                                            JOIN adminusercompany auc ON auc.UserId = au.Id
                                            WHERE ap.Nome = 'Clinica'
                                            AND auc.CompanyId = @COMPANYID
                                            AND au.Email = @EMAIL
                                            UNION
                                            SELECT au.ID FROM adminuser au WHERE CodigoClinicaOfflineID = @CODIGO
                                        ) as TB LIMIT 0, 1 ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, companyId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CODIGO", MySqlDbType.Int32, codigoClinicaOfflineID));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EMAIL", MySqlDbType.String, email));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        result = Int32.Parse(dr["ID"].ToString());
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); 
                throw ex;
            }


            if (result == 0)
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {
                        StringBuilder sbSQL = new StringBuilder();
                        sbSQL.AppendLine("INSERT INTO adminuser (Nome, Email, Senha, CodigoClinicaOfflineID, Telefone, Ativo, DtCriacao) VALUES ( @Nome, @Email, @Senha, @Codigo, @Telefone, 1, NOW()); SELECT LAST_INSERT_ID(); ");
                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Codigo", MySqlDbType.Int32, codigoClinicaOfflineID));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Nome", MySqlDbType.String, nome));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Email", MySqlDbType.String, email));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Telefone", MySqlDbType.String, telefone));
                        senha = $"{codigoClinicaOfflineID}{DateTime.Now.Second}";
                        var senhaHash = Util.GerarHashMd5(senha);
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Senha", MySqlDbType.String, senha));
                        // executa comando.                    
                        result = Convert.ToInt32(dataProc.ExecuteScalar());
                        dataProc.Dispose();
                    }
                    using (var connection = _DBGetConnection())
                    {
                        StringBuilder sbSQL = new StringBuilder();

                        sbSQL.AppendLine("INSERT INTO adminuserprofile (ProfileId, UserId) VALUES ( @ProfileId, @UserId); ");
                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ProfileId", MySqlDbType.Int32, 3)); // 3 = CLINICA
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UserId", MySqlDbType.Int32, result));
                        // executa comando.                    
                        dataProc.ExecuteNonQuery();
                        dataProc.Dispose();
                    }
                    using (var connection = _DBGetConnection())
                    {
                        StringBuilder sbSQL = new StringBuilder();

                        sbSQL.AppendLine("INSERT INTO adminusercompany (CompanyId, UserId) VALUES ( @CompanyId, @UserId); ");
                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, companyId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UserId", MySqlDbType.Int32, result));
                        // executa comando.                    
                        dataProc.ExecuteNonQuery();
                        dataProc.Dispose();
                    }
                    novoRegistro = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {
                        StringBuilder sbSQL = new StringBuilder();
                        sbSQL.AppendLine("UPDATE adminuser SET Email = @Email, Telefone = @Telefone WHERE ID = @ID; ");
                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, result));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Email", MySqlDbType.String, email));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Telefone", MySqlDbType.String, telefone));
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
            return result;
        }


        public int CarregarClinicaPorNomeBKP(int unidadeId, string clinica, int clinicaId, string email, string telefone)
        {
            int result = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(@" SELECT ID FROM
                                        (
                                        SELECT ID FROM Clinica Where Codigo = @CODIGO and UnidadeId = @UNIDADEID
                                        union
                                        SELECT ID FROM Clinica Where Nome = @NOME
                                        ) as TB LIMIT 0, 1 ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UNIDADEID", MySqlDbType.Int32, unidadeId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CODIGO", MySqlDbType.Int32, clinicaId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@NOME", MySqlDbType.String, clinica));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        result = Int32.Parse(dr["ID"].ToString());
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); 
                throw ex;
            }


            if (result == 0)
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {

                        StringBuilder sbSQL = new StringBuilder();

                        sbSQL.AppendLine("INSERT INTO Clinica (UnidadeId, Codigo, Nome, Email, Telefone) VALUES ( @UnidadeId, @Codigo, @Nome, @Email, @Telefone); SELECT LAST_INSERT_ID(); ");

                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Codigo", MySqlDbType.Int32, clinicaId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UnidadeId", MySqlDbType.Int32, unidadeId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Nome", MySqlDbType.String, clinica));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Email", MySqlDbType.String, email));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Telefone", MySqlDbType.String, telefone));
                        // executa comando.                    
                        result = Convert.ToInt32(dataProc.ExecuteScalar());
                        dataProc.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {

                        StringBuilder sbSQL = new StringBuilder();

                        sbSQL.AppendLine("UPDATE Clinica SET Email = @Email, Telefone = @Telefone WHERE ID = @ID; ");
                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ID", MySqlDbType.Int32, result));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Email", MySqlDbType.String, email));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Telefone", MySqlDbType.String, telefone));
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
            return result;
        }

        public int CarregarClientePorNome(int unidadeId, string proprietarioNome, string proprietarioEmail, string proprietarioTelefone, string proprietarioCPFCNPJ)
        {
            int result = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(@" SELECT ID FROM
                                        (
                                        SELECT AU.ID FROM Cliente AU 
                                        JOIN ClienteCompany AUC on AUC.UserId = AU.Id  
                                        Where AUC.CompanyId = @COMPANYID and AU.Email = @EMAIL 
                                        union
                                        SELECT AU.ID FROM Cliente AU 
                                        JOIN ClienteCompany AUC on AUC.UserId = AU.Id  
                                        Where AUC.CompanyId = @COMPANYID and AU.CPFCNPJ = @CPFCNPJ
                                        union
                                        SELECT AU.ID FROM Cliente AU 
                                        JOIN ClienteCompany AUC on AUC.UserId = AU.Id  
                                        Where AUC.CompanyId = @COMPANYID and AU.Nome = @NOME
                                        ) as TB LIMIT 0, 1 ");


                    //sbSQL.AppendLine(@" SELECT ID FROM
                    //                    (
                    //                    SELECT AU.ID FROM AdminUser AU 
                    //                    JOIN AdminUserCompany AUC on AUC.UserId = AU.Id  
                    //                    Where AUC.CompanyId = @COMPANYID and AU.Email = @EMAIL 
                    //                    union
                    //                    SELECT AU.ID FROM AdminUser AU 
                    //                    JOIN AdminUserCompany AUC on AUC.UserId = AU.Id  
                    //                    Where AUC.CompanyId = @COMPANYID and AU.CPFCNPJ = @CPFCNPJ
                    //                    union
                    //                    SELECT AU.ID FROM AdminUser AU 
                    //                    JOIN AdminUserCompany AUC on AUC.UserId = AU.Id  
                    //                    Where AUC.CompanyId = @COMPANYID and AU.Nome = @NOME
                    //                    ) as TB LIMIT 0, 1 ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, unidadeId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@EMAIL", MySqlDbType.String, proprietarioEmail));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@NOME", MySqlDbType.String, proprietarioNome));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CPFCNPJ", MySqlDbType.String, proprietarioCPFCNPJ));
                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        result = Int32.Parse(dr["ID"].ToString());
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); 
                throw ex;
            }


            if (result == 0)
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {

                        StringBuilder sbSQL = new StringBuilder();
                        sbSQL.AppendLine("INSERT INTO Cliente (CPFCNPJ, Nome, Email, Telefone) VALUES ( @CPFCNPJ, @Nome, @Email, @Telefone); SELECT LAST_INSERT_ID(); ");

                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CPFCNPJ", MySqlDbType.String, proprietarioCPFCNPJ ?? ""));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Email", MySqlDbType.String, proprietarioEmail ?? ""));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Nome", MySqlDbType.String, proprietarioNome));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Telefone", MySqlDbType.String, proprietarioTelefone ?? ""));
                        // executa comando.                    
                        result = Convert.ToInt32(dataProc.ExecuteScalar());
                        dataProc.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            if (result > 0)
            {
                //VerificaCriacaoUnidadeCliente(result, unidadeId);
            }
            return result;
        }

        //private void VerificaCriacaoPerfilCliente(int userId)
        //{

        //    int result = 0;
        //    try
        //    {
        //        using (var connection = _DBGetConnection())
        //        {

        //            StringBuilder sbSQL = new StringBuilder();
        //            sbSQL.AppendLine(@" SELECT ID FROM AdminUserProfile WHERE UserId = @UserId and ProfileId = 4 ");
        //            var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
        //            dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UserId", MySqlDbType.Int32, userId));
        //            // executa comando.
        //            IDataReader dr = dataProc.ExecuteReader();

        //            if (dr.Read())
        //            {
        //                result = Int32.Parse(dr["ID"].ToString());
        //            }

        //            dataProc.Dispose();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //new SOH.Data.Helper.Util().EventLog(ex); 
        //        throw ex;
        //    }

        //    if (result == 0)
        //    {

        //        try
        //        {
        //            using (var connection = _DBGetConnection())
        //            {

        //                StringBuilder sbSQL = new StringBuilder();
        //                sbSQL.AppendLine("INSERT INTO AdminUserProfile (ProfileId, UserId) VALUES ( 4, @UserId ); ");

        //                var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

        //                dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UserId", MySqlDbType.Int32, userId));
        //                // executa comando.           
        //                dataProc.ExecuteNonQuery();
        //                dataProc.Dispose();
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}


        private void VerificaCriacaoUnidadeCliente_OLD(int userId, int unidadeId)
        {
            int result = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.AppendLine(@" SELECT ID FROM ClienteCompany WHERE UserId = @UserId and CompanyId = @CompanyId ");
                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UserId", MySqlDbType.Int32, userId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, unidadeId));
                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        result = Int32.Parse(dr["ID"].ToString());
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); 
                throw ex;
            }

            if (result == 0)
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {

                        StringBuilder sbSQL = new StringBuilder();
                        sbSQL.AppendLine("INSERT INTO ClienteCompany (UserId, CompanyId) VALUES ( @UserId, @CompanyId); ");

                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UserId", MySqlDbType.Int32, userId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, unidadeId));
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



        private void VerificaCriacaoUnidadeCliente(int userId, int unidadeId, int exameid)
        {
            int result = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {
                    StringBuilder sbSQL = new StringBuilder();
                    sbSQL.AppendLine(@" SELECT ID FROM ClienteCompany WHERE UserId = @UserId and CompanyId = @CompanyId  and CompanyId = @ExameId ");
                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UserId", MySqlDbType.Int32, userId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, unidadeId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ExameId", MySqlDbType.Int32, exameid));
                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        result = Int32.Parse(dr["ID"].ToString());
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); 
                throw ex;
            }

            if (result == 0)
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {

                        StringBuilder sbSQL = new StringBuilder();
                        sbSQL.AppendLine("INSERT INTO ClienteCompany (UserId, CompanyId, ExameId) VALUES ( @UserId, @CompanyId, @ExameId); ");

                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UserId", MySqlDbType.Int32, userId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, unidadeId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@ExameId", MySqlDbType.Int32, exameid));
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


        private List<ModeloMensagemItem> CarregarModelos(int companyid, int perfilid)
        {
            List<ModeloMensagemItem> listagem = new List<ModeloMensagemItem>();

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(" SELECT * FROM MensagemModelo Where CompanyId = @COMPANYID and PerfilId = @PERFILID  ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PERFILID", MySqlDbType.Int32, perfilid));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@COMPANYID", MySqlDbType.Int32, companyid));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        ModeloMensagemItem entidade = new ModeloMensagemItem();

                        entidade.Id = Int64.Parse(dr["Id"].ToString());
                        if (dr["CompanyId"] != DBNull.Value)
                            entidade.CompanyId = Int32.Parse(dr["CompanyId"].ToString());

                        if (dr["UsuarioId"] != DBNull.Value)
                            entidade.UsuarioId = Int32.Parse(dr["UsuarioId"].ToString());

                        if (dr["PerfilId"] != DBNull.Value)
                            entidade.PerfilId = Int32.Parse(dr["PerfilId"].ToString());

                        if (dr["Perfil"] != DBNull.Value)
                            entidade.Perfil = dr["Perfil"].ToString();

                        if (dr["Titulo"] != DBNull.Value)
                            entidade.Titulo = dr["Titulo"].ToString();

                        if (dr["Mensagem"] != DBNull.Value)
                            entidade.Mensagem = dr["Mensagem"].ToString();


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

        public List<ModeloMensagemItem> CarregarModelosMensagem(int empresaId, int userId, int perfilId)
        {
            List<ModeloMensagemItem> itens = new List<ModeloMensagemItem>();

            List<ModeloMensagemItem> itensFiltrados = new List<ModeloMensagemItem>();

            itensFiltrados = CarregarModelos(empresaId, perfilId);

            foreach (ModeloMensagemItem item in itensFiltrados)
            {
                itens.Add(new Entity.ModeloMensagemItem()
                {
                    Id = item.Id,
                    DataCriacao = item.DataCriacao,
                    CompanyId = item.CompanyId,
                    Mensagem = item.Mensagem,
                    Titulo = item.Titulo,
                    UsuarioId = item.UsuarioId
                });
            }

            return itens;
        }



        public void Salvar(ModeloMensagemItem entidade)
        {
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();

                    if (entidade.Id > 0)
                    {
                        sbSQL.AppendLine("UPDATE MensagemModelo SET  Titulo = @Titulo, Mensagem = @Mensagem ");
                        sbSQL.AppendLine("WHERE ID = @Id ");
                    }
                    else
                    {
                        sbSQL.AppendLine("INSERT INTO MensagemModelo (CompanyId, UsuarioId, PerfilId, Perfil, Titulo, Mensagem, DataCriacao) VALUES ( @CompanyId, @UsuarioId, @PerfilId, @Perfil, @Titulo, @Mensagem, NOW()); SELECT LAST_INSERT_ID(); ");
                    }

                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    if (entidade.Id > 0)
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Id", MySqlDbType.Int32, entidade.Id));

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Titulo", MySqlDbType.String, entidade.Titulo));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Mensagem", MySqlDbType.String, entidade.Mensagem));
                    }
                    else
                    {
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CompanyId", MySqlDbType.Int32, entidade.CompanyId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UsuarioId", MySqlDbType.Int32, entidade.UsuarioId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@PerfilId", MySqlDbType.Int32, entidade.PerfilId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Perfil", MySqlDbType.String, entidade.Perfil));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Titulo", MySqlDbType.String, entidade.Titulo));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Mensagem", MySqlDbType.String, entidade.Mensagem));
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


        }



        public int CarregarEspeciePorNome(string especie, out bool outros)
        {
            int result = 0;
            outros = false;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(@" SELECT ID FROM Especie Where Nome = @NOME ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@NOME", MySqlDbType.String, especie));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        result = Int32.Parse(dr["ID"].ToString());
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (result == 0)
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {

                        StringBuilder sbSQL = new StringBuilder();

                        sbSQL.AppendLine(@" SELECT ID FROM Especie Where Nome = @NOME ");

                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@NOME", MySqlDbType.String, "Outros"));

                        // executa comando.
                        IDataReader dr = dataProc.ExecuteReader();

                        if (dr.Read())
                        {
                            result = Int32.Parse(dr["ID"].ToString());
                            outros = true;
                        }

                        dataProc.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }

        public int CarregarRacaPorNome(int unidadeId, string raca, int racaId)
        {
            int result = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(@" SELECT ID FROM
                                        (
                                        SELECT ID FROM Raca Where Codigo = @CODIGO and UnidadeId = @UNIDADEID
                                        union
                                        SELECT ID FROM Raca Where Nome = @NOME
                                        ) as TB LIMIT 0, 1 ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UNIDADEID", MySqlDbType.Int32, unidadeId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CODIGO", MySqlDbType.Int32, racaId));
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@NOME", MySqlDbType.String, raca));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        result = Int32.Parse(dr["ID"].ToString());
                    }

                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); 
                throw ex;
            }


            if (result == 0)
            {
                try
                {
                    using (var connection = _DBGetConnection())
                    {

                        StringBuilder sbSQL = new StringBuilder();

                        sbSQL.AppendLine("INSERT INTO Raca (UnidadeId, Codigo, Nome) VALUES ( @UnidadeId, @Codigo, @Nome); SELECT LAST_INSERT_ID(); ");

                        var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Codigo", MySqlDbType.Int32, racaId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@UnidadeId", MySqlDbType.Int32, unidadeId));
                        dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@Nome", MySqlDbType.String, raca));
                        // executa comando.                    
                        result = Convert.ToInt32(dataProc.ExecuteScalar());
                        dataProc.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }
        public bool CarregarPorChave(string chave, out int unidadeId)
        {

            List<DashReportAdminItem> entidades = new List<DashReportAdminItem>();
            unidadeId = 0;
            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(@" SELECT ID 
                            FROM Company
                            WHERE ATIVO = 1 and CHAVE = @CHAVE ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);
                    dataProc.Parameters.Add(_DBBuildParameter(dataProc, "@CHAVE", MySqlDbType.String, chave));

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr["ID"] != DBNull.Value)
                        {
                            unidadeId = Int32.Parse(dr["ID"].ToString());

                        }
                    }


                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return unidadeId > 0;
        }
        public List<DashReportAdminItem> CarregarDashboardAdmin()
        {
            List<DashReportAdminItem> entidades = new List<DashReportAdminItem>();

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(@" SELECT ID, NOME, IMAGEM, 
                            (SELECT COUNT(*) AS TOTAL FROM Exame WHERE exame.CompanyId = company.Id AND exame.SituacaoId IN (2,3,4)) AS TOTAL_EM_ANALISE,
                            (SELECT COUNT(*) AS TOTAL FROM Exame WHERE exame.CompanyId = company.Id AND exame.SituacaoId = 1) AS TOTAL_CRIACAO,
                            (SELECT COUNT(*) AS TOTAL FROM Exame WHERE exame.CompanyId = company.Id AND exame.SituacaoId = 6) AS TOTAL_CONCLUIDO
                            FROM Company
                            WHERE Company.ATIVO = 1 ");


                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        var entidade = new DashReportAdminItem();

                        if (dr["ID"] != DBNull.Value)
                            entidade.ID = Int32.Parse(dr["ID"].ToString());

                        if (dr["NOME"] != DBNull.Value)
                            entidade.NOME = dr["NOME"].ToString();

                        if (dr["IMAGEM"] != DBNull.Value)
                            entidade.IMAGEM = dr["IMAGEM"].ToString();

                        if (dr["TOTAL_EM_ANALISE"] != DBNull.Value)
                            entidade.TOTAL_EM_ANALISE = Int32.Parse(dr["TOTAL_EM_ANALISE"].ToString());

                        if (dr["TOTAL_CRIACAO"] != DBNull.Value)
                            entidade.TOTAL_CRIACAO = Int32.Parse(dr["TOTAL_CRIACAO"].ToString());

                        if (dr["TOTAL_CONCLUIDO"] != DBNull.Value)
                            entidade.TOTAL_CONCLUIDO = Int32.Parse(dr["TOTAL_CONCLUIDO"].ToString());

                        entidades.Add(entidade);
                    }


                    dataProc.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entidades;
        }
        public List<AcoesRecentesItem> ListAcoesRecentes(out int pageTotal, int pageIndex, int pageSize, int usuarioId, int empresaid, int perfilId, Dictionary<string, object> dicFilter)
        {
            var listagem = new List<AcoesRecentesItem>();
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

                                    if (item.Value.GetType() == typeof(List<DateTime>))
                                    {
                                        DateTime dtIni = ((List<DateTime>)item.Value).FirstOrDefault();
                                        DateTime dtFim = ((List<DateTime>)item.Value).LastOrDefault();
                                        clauseWhere.AppendFormat(" AND {0} between '{1}' and '{2}'", item.Key, dtIni.ToString("yyyy-MM-dd 00:00:00"), dtFim.ToString("yyyy-MM-dd 23:59:59"));
                                    }

                                    if (item.Value.GetType() == typeof(int))
                                    {
                                        if (item.Key.Equals("[ORD_SITUACAO_LAUDADORID_LAU]"))
                                        {
                                            clauseWhere.AppendFormat(" AND (LaudadorId = {0} or LaudadorId = 0)", item.Value);
                                        }
                                        else
                                        {
                                            clauseWhere.AppendFormat(" AND {0} = {1}", item.Key, item.Value);
                                        }

                                    }
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
                                            else if (item.Key.Equals("[ORD_SITUACAO_LAUDADORID]"))
                                            {
                                               var filterSituacao = ((List<string>)item.Value).FirstOrDefault();
                                               var filterLaudadorId = ((List<string>)item.Value).LastOrDefault();                                                
                                               clauseWhere.AppendFormat(" AND (SituacaoId = {0} and LaudadorId = {1})", filterSituacao, filterLaudadorId);
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
                                    if (item.Value.GetType() == typeof(List<DateTime>))
                                    {
                                        DateTime dtIni = ((List<DateTime>)item.Value).FirstOrDefault();
                                        DateTime dtFim = ((List<DateTime>)item.Value).LastOrDefault();
                                        clauseWhere.AppendFormat(" {0} between '{1}' and '{2}'", item.Key, dtIni.ToString("yyyy-MM-dd 00:00:00"), dtFim.ToString("yyyy-MM-dd 23:59:59"));
                                    }

                                    if (item.Value.GetType() == typeof(int))
                                    {
                                        if (item.Key.Equals("[ORD_SITUACAO_LAUDADORID_LAU]"))
                                        {                                            
                                            clauseWhere.AppendFormat(" (LaudadorId = {0} or LaudadorId = 0)", item.Value);
                                        }
                                        else
                                        {
                                            clauseWhere.AppendFormat(" {0} = {1}", item.Key, item.Value);
                                        }
                                    }

                                    if (item.Value.GetType() == typeof(string))
                                    {
                                        if (item.Key.EndsWith("=="))
                                        {
                                            var codeKey = item.Key.Replace("==", "");


                                            if (clauseWhere.Length > 0)
                                                clauseWhere.AppendFormat(" and {0} = '{1}'", codeKey, item.Value);
                                            else
                                                clauseWhere.AppendFormat(" {0} = '{1}'", codeKey, item.Value);
                                        }
                                        else if (item.Key.EndsWith("!="))
                                        {
                                            var codeKey = item.Key.Replace("!=", "");


                                            if (clauseWhere.Length > 0)
                                                clauseWhere.AppendFormat(" and {0} != '{1}'", codeKey, item.Value);
                                            else
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

                                                if (clauseWhere.Length > 0)
                                                    clauseWhere.AppendFormat(" and {0} = '{1}'", codeKey, itemValue);
                                                else
                                                    clauseWhere.AppendFormat(" {0} = '{1}'", codeKey, itemValue);
                                            }
                                            else if (item.Key.EndsWith("!="))
                                            {
                                                var codeKey = item.Key.Replace("!=", "");

                                                if (clauseWhere.Length > 0)
                                                    clauseWhere.AppendFormat(" and {0} != '{1}'", codeKey, itemValue);
                                                else
                                                    clauseWhere.AppendFormat(" {0} != '{1}'", codeKey, itemValue);

                                            }
                                            else if (item.Key.Equals("[ORD_SITUACAO_LAUDADORID]"))
                                            {
                                                var filterSituacao = ((List<string>)item.Value).FirstOrDefault();
                                                var filterLaudadorId = ((List<string>)item.Value).LastOrDefault();
                                                clauseWhere.AppendFormat(" (SituacaoId = {0} and LaudadorId = {1})", filterSituacao, filterLaudadorId);
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

                    var strOrder = "DataAtualizacao DESC";
                    var strColumnsJoin = "Exame.*, CLIN.Nome as NomeClinica, AUC.Telefone as TelefoneCliente, AUC.Nome as NomeCliente,   AUC.Email as EmailCliente, AUL.Telefone as TelefoneLaudador, AUL.Nome as NomeLaudador, AUL.Email as EmailLaudador ";
                    var strTable = @" Exame 
                                    left join cliente AUC on AUC.Id = Exame.ClienteId 
                                    left join adminUser AUL on AUL.Id = Exame.LaudadorId
                                    left join adminUser CLIN on CLIN.Id = Exame.ClinicaId ";

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

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        var entidade = new AcoesRecentesItem();
                        if (dr["Id"] != DBNull.Value)
                            entidade.Id = Int32.Parse(dr["Id"].ToString());

                        if (dr["DataExame"] != DBNull.Value)
                            entidade.DataExame = DateTime.Parse(dr["DataExame"].ToString());

                        if (dr["DataCadastro"] != DBNull.Value)
                            entidade.DataCadastro = DateTime.Parse(dr["DataCadastro"].ToString());

                        if (dr["SituacaoId"] != DBNull.Value)
                            entidade.SituacaoId = Int32.Parse(dr["SituacaoId"].ToString());

                        //if (dr["CPFCNPJ"] != DBNull.Value)
                            //entidade.CPFCNPJ = dr["CPFCNPJ"].ToString();
                        
                        if (dr["ClienteId"] != DBNull.Value)
                            entidade.ClienteId = Int32.Parse(dr["ClienteId"].ToString());

                        if (dr["LaudadorId"] != DBNull.Value)
                            entidade.LaudadorId = Int32.Parse(dr["LaudadorId"].ToString());

                        if (dr["CompanyId"] != DBNull.Value)
                            entidade.CompanyId = Int32.Parse(dr["CompanyId"].ToString());

                        if (dr["Veterinario"] != DBNull.Value)
                            entidade.Veterinario = dr["Veterinario"].ToString();

                        if (dr["Paciente"] != DBNull.Value)
                            entidade.Paciente = dr["Paciente"].ToString();

                        if (dr["Proprietario"] != DBNull.Value)
                            entidade.Proprietario = dr["Proprietario"].ToString();

                        if (dr["Idade"] != DBNull.Value)
                            entidade.Idade = dr["Idade"].ToString();

                        if (dr["EspecieId"] != DBNull.Value)
                            entidade.EspecieId = Int32.Parse(dr["EspecieId"].ToString());

                        if (dr["EspecieOutros"] != DBNull.Value)
                            entidade.EspecieOutros = dr["EspecieOutros"].ToString();

                        if (dr["NomeClinica"] != DBNull.Value)
                            entidade.NomeClinica = dr["NomeClinica"].ToString();
                        else
                            entidade.NomeClinica = string.Empty;

                        if (dr["NomeCliente"] != DBNull.Value)
                            entidade.NomeCliente = dr["NomeCliente"].ToString();
                        else
                            entidade.NomeCliente = string.Empty;

                        if (dr["EmailCliente"] != DBNull.Value)
                            entidade.EmailCliente = dr["EmailCliente"].ToString();
                        else
                            entidade.EmailCliente = string.Empty;

                        if (dr["NomeLaudador"] != DBNull.Value)
                            entidade.NomeLaudador = dr["NomeLaudador"].ToString();
                        else
                            entidade.NomeLaudador = string.Empty;

                        if (dr["EmailLaudador"] != DBNull.Value)
                            entidade.EmailLaudador = dr["EmailLaudador"].ToString();
                        else
                            entidade.EmailLaudador = string.Empty;

                        if (dr["TelefoneCliente"] != DBNull.Value)
                            entidade.TelefoneCliente = dr["TelefoneCliente"].ToString();
                        else
                            entidade.TelefoneCliente = string.Empty;

                        if (dr["TelefoneLaudador"] != DBNull.Value)
                            entidade.TelefoneLaudador = dr["TelefoneLaudador"].ToString();
                        else
                            entidade.TelefoneLaudador = string.Empty;



                        if (dr["RacaId"] != DBNull.Value)
                            entidade.RacaId = Int32.Parse(dr["RacaId"].ToString());

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

        public List<EmpresaItem> ListEmpresa()
        {
            List<EmpresaItem> listagem = new List<EmpresaItem>();

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(" SELECT * FROM Company WHERE Ativo = 1 ");



                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        EmpresaItem entidade = new EmpresaItem();

                        entidade.Id = Int32.Parse(dr["Id"].ToString());
                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();


                        if (dr["Imagem"] != DBNull.Value)
                            entidade.Imagem = dr["Imagem"].ToString();

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

        public List<EmpresaItem> ListEmpresaFull()
        {
            List<EmpresaItem> listagem = new List<EmpresaItem>();

            try
            {
                using (var connection = _DBGetConnection())
                {

                    StringBuilder sbSQL = new StringBuilder();


                    sbSQL.AppendLine(" SELECT * FROM Company Where Ativo = 1 ORDER BY Nome ");



                    var dataProc = _DBGetCommand(sbSQL.ToString(), connection);

                    // executa comando.
                    IDataReader dr = dataProc.ExecuteReader();

                    while (dr.Read())
                    {
                        EmpresaItem entidade = new EmpresaItem();

                        entidade.Id = Int32.Parse(dr["Id"].ToString());
                        if (dr["Nome"] != DBNull.Value)
                            entidade.Nome = dr["Nome"].ToString();


                        if (dr["Imagem"] != DBNull.Value)
                            entidade.Imagem = dr["Imagem"].ToString();
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


    }
}
