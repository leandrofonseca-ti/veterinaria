using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebImagem.Data;

namespace WebImagem
{
    public partial class Form1 : Form
    {
        public DateTime _dataCorte = new DateTime(2022, 3, 1, 0, 0, 0);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new SyncService().SalvarLog(null);
            var setup = new SetupService().Carregar();
            txtPath.Text = setup.String_de_conexao;
            txtChave.Text = setup.API_CHAVE;
            if (String.IsNullOrEmpty(setup.API_URL))
            {
                setup.API_URL = "https://webimagem.vet.br/Api";
            }
            txtUrlAPI.Text = setup.API_URL;
            txtCaminhoExames.Text = setup.Caminho_Imagens_Exame;
            List<clsItemLogSync> logs = new SyncService().CarregarLog();
            StringBuilder sbText = new StringBuilder();

            logs.ForEach(r =>
            {
                sbText.AppendLine("-------------------------------------------------------------");
                sbText.AppendLine($"{r.Log} = {r.Data_syncronizado.ToString("dd/MM/yyyy HH:mm")}");
                //sbText.AppendLine($"{r.ExameId}|{r.Log}|{r.Data_syncronizado.ToString("dd/MM/yyyy HH:mm")}|{r.Syncronizado}");
            });

            txtLog.Text = sbText.ToString();

            //var auth = new clsAutenticacaoItem()
            //{
            //    API_URL_EXAME = setup.API_URL,
            //    API_CHAVE = setup.API_CHAVE
            //};
            //new SyncService().SyncExameArquivo(auth, 18, @"C:\Users\leand\Downloads\VET\Cadastro imagemovel teste\SISTEMA DE LAUDO\SISTEMA DE LAUDOS 2\IMAGENS RX\2021\06.JUNHO\010621\DR DE BICHO\CHIQUINHO\", "20210501114213_O_jasmin.JPG");
        }

        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtPath.Text))
                CheckConexao(txtPath.Text);
            else
                MessageBox.Show("Informa o caminho correto para a conexão com MySQL!");
        }


        public bool IsOkConexao(string path)
        {
            var result = false;
            string dbConnectionString = $@"Data Source={path};Cache=Shared;";
            try
            {
                SqliteConnection sqlite_con = new SqliteConnection(dbConnectionString);
                sqlite_con.Open();
                string query = "select count(*) as Total from tb_pacientes;";
                SqliteCommand sqlite_cmd = new SqliteCommand(query, sqlite_con);
                SqliteDataReader dr = sqlite_cmd.ExecuteReader();
                if (dr.Read())
                {
                    result = true;
                }
                sqlite_con.Close();
            }
            catch (Exception ex)
            {
            }
            return result;
        }


        public void CheckConexao(string path)
        {

            string dbConnectionString = $@"Data Source={path};Cache=Shared;";
            try
            {
                SqliteConnection sqlite_con = new SqliteConnection(dbConnectionString);
                sqlite_con.Open();
                string query = "select count(*) as Total from tb_pacientes;";
                SqliteCommand sqlite_cmd = new SqliteCommand(query, sqlite_con);
                SqliteDataReader dr = sqlite_cmd.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("Conexão com MySQL com sucesso!");
                    //MessageBox.Show(dr.GetString(0));
                }

                sqlite_con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnSalvarSetup_Click(object sender, EventArgs e)
        {
            var setup = new clsSetup();
            setup.String_de_conexao = txtPath.Text;
            setup.API_CHAVE = txtChave.Text;
            setup.API_URL = txtUrlAPI.Text;
            setup.Caminho_Imagens_Exame = txtCaminhoExames.Text;
            new SetupService().Salvar(setup);
            MessageBox.Show("Dados atualizados com sucesso!");

        }

        private void btnSync_Click(object sender, EventArgs e)
        {

            var total = 0;
            lblMensagem.Text = "";
            lblPercent.Text = "[ 0% ]";
            progressBar1.Value = 0;
            txtLog.Text = "";
            var check = ValidarConexao();

            if (check)
            {
                //List<clsItemLogSync> logs = new SyncService().CarregarLog();
                //StringBuilder sbText = new StringBuilder();


                //logs.ForEach(r =>
                //{
                //    sbText.AppendLine("-------------------------------------------------------------");
                //    sbText.AppendLine($"{r.Log} = {r.Data_syncronizado.ToString("dd/MM/yyyy HH:mm")}");
                //    //sbText.AppendLine($"{r.ExameId}|{r.Log}|{r.Data_syncronizado.ToString("dd/MM/yyyy HH:mm")}|{r.Syncronizado}");
                //});

                //txtLog.Text = sbText.ToString();
                //var codes = string.Empty;
                var setup = new SetupService().Carregar();
                string dbConnectionString = $@"Data Source={setup.String_de_conexao};Cache=Shared;";

                try
                {

                    // var listItensSync = new SyncService().CarregarLogSincronizados();

                    //codes = string.Join(",", listItensSync.Where(r => r.ID > 0).Select(t => t.ID).ToArray());

                    using (SqliteConnection sqlite_con = new SqliteConnection(dbConnectionString))
                    {
                        sqlite_con.Open();
                        string query = "select count(*) as Total from tb_pacientes;";
                        /*
                        if (!String.IsNullOrEmpty(codes))
                        {
                            query = $"select count(*) as Total from tb_pacientes WHERE ID not in ({codes});";
                        }*/

                        SqliteCommand sqlite_cmd = new SqliteCommand(query, sqlite_con);
                        SqliteDataReader dr = sqlite_cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            total = dr.GetInt32(0);
                        }

                        sqlite_con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                if (total > 0)
                {
                    List<clsExameItem> exames = new List<clsExameItem>();
                    if (MessageBox.Show($"Deseja sincronizar com o banco de dados o(s) {total} registro(s) ?", "Confirmação",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //MessageBox.Show("OK");

                        using (SqliteConnection sqlite_con = new SqliteConnection(dbConnectionString))
                        {
                            sqlite_con.Open();
                            string query = "select * from tb_pacientes;";

                            //if (String.IsNullOrEmpty(codes))
                            //{
                            //    query = $"select * from tb_pacientes WHERE ID not in ({codes});";
                            //}

                            SqliteCommand sqlite_cmd = new SqliteCommand(query, sqlite_con);
                            SqliteDataReader dr = sqlite_cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                var item = new clsExameItem();

                                if (dr["ID"] != DBNull.Value)
                                    item.ID = Int32.Parse(dr["ID"].ToString());

                                if (dr["NumeroExame"] != DBNull.Value)
                                    item.NumeroExame = dr["NumeroExame"].ToString();

                                if (dr["Data"] != DBNull.Value)
                                    item.Data = DateTime.Parse(dr["Data"].ToString());

                                if (dr["Clinica"] != DBNull.Value)
                                    item.Clinica = dr["Clinica"].ToString();

                                if (dr["Veterinario"] != DBNull.Value)
                                    item.Veterinario = dr["Veterinario"].ToString();

                                if (dr["Proprietario"] != DBNull.Value)
                                    item.Proprietario = dr["Proprietario"].ToString();

                                if (dr["ProprietarioEmail"] != DBNull.Value)
                                    item.ProprietarioEmail = dr["ProprietarioEmail"].ToString();

                                if (dr["ProprietarioTelefone"] != DBNull.Value)
                                    item.ProprietarioTelefone = dr["ProprietarioTelefone"].ToString();

                                if (dr["ProprietarioCnpj"] != DBNull.Value)
                                    item.ProprietarioCPFCNPJ = dr["ProprietarioCnpj"].ToString();

                                if (dr["Paciente"] != DBNull.Value)
                                    item.Paciente = dr["Paciente"].ToString();

                                if (dr["Raca"] != DBNull.Value)
                                    item.Raca = dr["Raca"].ToString();


                                if (dr["Especie"] != DBNull.Value)
                                    item.Especie = dr["Especie"].ToString();


                                if (dr["Idade"] != DBNull.Value)
                                    item.Idade = dr["Idade"].ToString();

                                if (dr["Historico"] != DBNull.Value)
                                    item.Historico = dr["Historico"].ToString();


                                if (dr["Valor"] != DBNull.Value)
                                    item.Valor = dr["Valor"].ToString();


                                if (dr["FormaPagamento"] != DBNull.Value)
                                    item.FormaPagamento = dr["FormaPagamento"].ToString();

                                if (dr["Laudo"] != DBNull.Value)
                                    item.Laudo = Int32.Parse(dr["Laudo"].ToString());

                                if (dr["LinhaExcel"] != DBNull.Value)
                                    item.LinhaExcel = Int32.Parse(dr["LinhaExcel"].ToString());

                                if (dr["Hora"] != DBNull.Value)
                                    item.Hora = DateTime.Parse(dr["Hora"].ToString());

                                if (dr["Deletado"] != DBNull.Value)
                                    item.Deletado = dr["Deletado"].ToString();

                                if (dr["Reserva5"] != DBNull.Value)
                                    item.Reserva5 = dr["Reserva5"].ToString();


                                if (item.Data >= _dataCorte)
                                {
                                    exames.Add(item);
                                }
                            }

                            sqlite_con.Close();
                        }

                    }
                    //else
                    //{
                    //    MessageBox.Show("NOK");
                    //}

                    if (exames.Any())
                    {
                        progressBar1.Maximum = 100;// total;
                        progressBar1.Step = 1;

                        //desabilita os botões enquanto a tarefa é executada
                        btnSync.Enabled = false;

                        //define o stilo padrao do progressbar
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Value = 0;

                        var ct = 0;
                        //Calculate(exame);
                        //executa o processo de forma assincrona.
                        backgroundWorker1.RunWorkerAsync(exames);
                        progressBar1.PerformStep();
                    }
                }
            }
        }

        private bool ValidarConexao()
        {
            var check01 = false;
            if (!String.IsNullOrEmpty(txtPath.Text))
                check01 = IsOkConexao(txtPath.Text);


            var setup = new SetupService().Carregar();


            if (String.IsNullOrEmpty(setup.String_de_conexao))
            {
                MessageBox.Show("Informe o Caminho do Banco Local");
                return false;
            }

            if (String.IsNullOrEmpty(setup.API_URL))
            {
                MessageBox.Show("Informe a URL Api");
                return false;
            }

            if (String.IsNullOrEmpty(setup.API_CHAVE))
            {
                MessageBox.Show("Informe a Chave de Integração");
                return false;
            }

            if (String.IsNullOrEmpty(setup.Caminho_Imagens_Exame))
            {
                MessageBox.Show("Informe o Caminho das Imagens dos Exames");
                return false;
            }



            var auth = new clsAutenticacaoItem()
            {
                API_URL_EXAME = setup.API_URL,
                API_CHAVE = setup.API_CHAVE
            };

            var check02 = false;
            var result = new SyncService().IsLogged(auth);
            if (result != null && result.Data != null && result.Data.ToString() == "OK")
                check02 = true;

            if (check01 == false && check02 == false)
                MessageBox.Show("Sem acesso ao banco local e sem conexão com API!");

            if (check01 == false && check02 == true)
                MessageBox.Show("Sem acesso ao banco local!");

            if (check01 == true && check02 == false)
                MessageBox.Show("Sem conexão com API!");

            return check01 == true && check02 == true;

        }

        private bool Calculate(clsExameItem exame)
        {
            var setup = new SetupService().Carregar();
            var auth = new clsAutenticacaoItem()
            {
                API_URL_EXAME = setup.API_URL,
                API_CHAVE = setup.API_CHAVE
            };
            
            var result = new SyncService().SyncExame(auth, exame);
            if (result != null && result.Code == 0 && result.Message == "OK" && result.Data != null)
            {
                var exameSyncImagesCt = 0;
                var exameSyncImagesCtErro = 0;
                CultureInfo idioma = new CultureInfo("pt-BR");
                var codigoFromWeb = Int32.Parse(result.Data.ToString());
                if (codigoFromWeb > 0)
                {
                    if (!String.IsNullOrEmpty(exame.NumeroExame) && exame.NumeroExame.Length > 6)
                    {
                        var codigo = exame.NumeroExame.Substring(0, 6);
                        var dia = codigo.Substring(0, 2);
                        var mes = codigo.Substring(2, 2);
                        var ano = codigo.Substring(4, 2);
                        var textMes = new DateTime(DateTime.Now.Year, Int32.Parse(mes), 1).ToString("MMMM", idioma).ToUpper();

                        textMes = textMes.Replace("Ç", "C");

                        var auxPath = setup.Caminho_Imagens_Exame.EndsWith("\\") ? setup.Caminho_Imagens_Exame : setup.Caminho_Imagens_Exame + "\\";
                        var caminho = $"{auxPath}20{ano}\\{mes}.{textMes}\\{dia}{mes}{ano}";
                        //caminho = "C:\\Users\\leand\\Downloads\\VET\\Cadastro imagemovel teste\\SISTEMA DE LAUDO\\SISTEMA DE LAUDOS 2\\IMAGENS RX\\2021\\06.JUNHO\\010621";
                        if (System.IO.Directory.Exists(caminho))
                        {
                            var dirClinicas = System.IO.Directory.GetDirectories(caminho);

                            foreach (var dir in dirClinicas)
                            {
                                var dirNomeClinica = dir.Substring(dir.LastIndexOf("\\") + 1);
                                if (exame.Clinica == dirNomeClinica)
                                {
                                    var dirPacientes = System.IO.Directory.GetDirectories(dir);
                                    foreach (var dir2 in dirPacientes)
                                    {
                                        var dirNomePaciente = dir2.Substring(dir2.LastIndexOf("\\") + 1);
                                        if (exame.Paciente == dirNomePaciente)
                                        {
                                            var files = System.IO.Directory.GetFiles(dir2, "*.*", System.IO.SearchOption.AllDirectories);
                                            foreach (var f in files.Where(r => r.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase)
                                            || r.EndsWith(".jpeg", StringComparison.InvariantCultureIgnoreCase)
                                            || r.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase)))
                                            {
                                                var rst = new SyncService().SyncExameArquivo(auth, codigoFromWeb, dir2, f);

                                                if (rst.Code == 0)
                                                {
                                                    exameSyncImagesCt++;
                                                }
                                                else
                                                {
                                                    exameSyncImagesCtErro++;
                                                }
                                            }
                                        }
                                    }

                                }
                            }


                        }
                    }
                }


                var msgImgs = "Sucesso [nenhuma imagem]";

                if (exameSyncImagesCt > 0 && exameSyncImagesCtErro > 0)
                    msgImgs = $"Sucesso [{(exameSyncImagesCt > 0 ? $"{exameSyncImagesCt} imagen(s) OK" : "")} e {(exameSyncImagesCtErro > 0 ? $"{exameSyncImagesCtErro} imagen(s) com ERRO" : "")}]";

                if (exameSyncImagesCt > 0 && exameSyncImagesCtErro == 0)
                    msgImgs = $"Sucesso [{(exameSyncImagesCt > 0 ? $"{exameSyncImagesCt} imagen(s) OK" : "")}]";

                if (exameSyncImagesCt == 0 && exameSyncImagesCtErro > 0)
                    msgImgs = $"Sucesso [{(exameSyncImagesCtErro > 0 ? $"{exameSyncImagesCtErro} imagen(s) com ERRO" : "")}]";


                exame.LogMsg = $"{msgImgs} = {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}";
                new SyncService().SalvarLog(new clsItemLogSync()
                {
                    ID = exame.ID,
                    Data_syncronizado = DateTime.Now,
                    Syncronizado = true,
                    Log = msgImgs //Newtonsoft.Json.JsonConvert.SerializeObject(exame)
                });

                //List<clsItemLogSync> logs = new SyncService().CarregarLog();
                //StringBuilder sbText = new StringBuilder();

                //logs.ForEach(r =>
                //{
                //    sbText.AppendLine("-------------------------------------------------------------");
                //    sbText.AppendLine($"{r.Log} = {r.Data_syncronizado.ToString("dd/MM/yyyy HH:mm")}");
                //    //sbText.AppendLine($"{r.ExameId}|{r.Log}|{r.Data_syncronizado.ToString("dd/MM/yyyy HH:mm")}|{r.Syncronizado}");
                //});
                //txtLog.Text = sbText.ToString();
                return result.Code == 0;
            }
            else if (result != null)
            {

                exame.LogMsg = $"{result.Message} = {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}";
                new SyncService().SalvarLog(new clsItemLogSync()
                {
                    ID = exame.ID,
                    Data_syncronizado = DateTime.Now,
                    Syncronizado = false,
                    Log = result.Message //Newtonsoft.Json.JsonConvert.SerializeObject(exame)
                });

            }
            return false;

        }


        /// <summary>
        /// //Aqui chamamos os nossos metodos com as tarefas demoradas.
        /// </summary>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var exames = (List<clsExameItem>)e.Argument;
            var ct = 0;

            var total = exames.Count;

            List<int> listagem = new List<int>();
            foreach (var exame in exames)
            {
                ct++;
                //Executa o método longo 100 vezes.
                //TarefaLonga(20);
                var isok = Calculate(exame);

                listagem.Add(isok ? 1 : 0);
                //incrementa o progresso do backgroundWorker
                //a cada passagem do loop.

                var aux = (ct * 100) / total;
                this.backgroundWorker1.ReportProgress(aux, exame);

                //Verifica se houve uma requisição para cancelar a operação.
                if (backgroundWorker1.CancellationPending)
                {
                    //se sim, define a propriedade Cancel para true
                    //para que o evento WorkerCompleted saiba que a tarefa foi cancelada.
                    e.Cancel = true;

                    //zera o percentual de progresso do backgroundWorker1.
                    backgroundWorker1.ReportProgress(0);
                    return;
                }
            }
            //Finalmente, caso tudo esteja ok, finaliza
            //o progresso em 100%.
            backgroundWorker1.ReportProgress(100);

            var ctSucesso = listagem.Where(t => t == 1).Count();
            var ctErro = listagem.Where(t => t == 0).Count();

            var log = "";

            if (ctSucesso > 0 && ctErro == 0)
            {
                log = $"{ctSucesso} registro(s) sincronizado(s)";
            }
            else if (ctSucesso == 0 && ctErro > 0)
            {
                log = $"{ctErro} registro(s) com erro";
            }
            else
            {
                log = $"{ctSucesso} registro(s) sincronizado(s) e {ctErro} registro(s) com erro";
            }

            new SyncService().SalvarLog(new clsItemLogSync()
            {
                ID = 0,
                Data_syncronizado = DateTime.Now,
                Syncronizado = true,
                Log = log
            });
        }

        /// <summary>
        /// Aqui implementamos o que desejamos fazer enquanto o progresso
        /// da tarefa é modificado,[incrementado].
        /// </summary>
        private void backgroundWorker1_ProgressChanged(object sender,
        ProgressChangedEventArgs e)
        {
            //Incrementa o valor da progressbar com o valor
            //atual do progresso da tarefa.
            progressBar1.Value = e.ProgressPercentage;

            //informa o percentual na forma de texto.
            lblPercent.Text = "[ " + e.ProgressPercentage.ToString() + "% ]";



            //List<clsItemLogSync> logs = new SyncService().CarregarLog();
            //StringBuilder sbText = new StringBuilder();
            //logs.ForEach(r =>
            //{
            //    sbText.AppendLine("-------------------------------------------------------------");
            //    sbText.AppendLine($"{r.Log} = {r.Data_syncronizado.ToString("dd/MM/yyyy HH:mm")}");
            //    //sbText.AppendLine($"{r.ExameId}|{r.Log}|{r.Data_syncronizado.ToString("dd/MM/yyyy HH:mm")}|{r.Syncronizado}");
            //});
            //var aux = sbText.ToString();

            var aux = "";// txtLog.Text;
            if (e.UserState != null)
            {
                var lg = $"{((clsExameItem)e.UserState).LogMsg}{Environment.NewLine}";
                aux = $"Nº: {((clsExameItem)e.UserState).NumeroExame}{Environment.NewLine}" + lg;
            }
            txtLog.Text += aux;
        }

        /// <summary>
        /// Após a tarefa ser concluida, esse metodo e chamado para
        /// implementar o que deve ser feito imediatamente após a conclusão da tarefa.
        /// </summary>
        private void backgroundWorker1_RunWorkerCompleted(object sender,
        RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //caso a operação seja cancelada, informa ao usuario.
                lblMensagem.Text = "Operação Cancelada pelo Usuário!";

                //habilita o Botao cancelar
                btnCancelar.Enabled = true;
                //limpa a label
                lblMensagem.Text = string.Empty;
            }
            else if (e.Error != null)
            {
                //informa ao usuario do acontecimento de algum erro.
                lblMensagem.Text = "Aconteceu um erro durante a execução do processo!";
            }
            else
            {
                //informa que a tarefa foi concluida com sucesso.
                lblMensagem.Text = "Tarefa Concluida com sucesso!";

                var aux = txtLog.Text;
                StringBuilder sbText = new StringBuilder();
                sbText.AppendLine("-------------------------------------------------------------");
                //sbText.AppendLine($"{r.Log} = {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}");
                sbText.AppendLine(aux);
                txtLog.Text = sbText.ToString();
            }
            //habilita os botões.
            btnSync.Enabled = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //Cancelamento da tarefa com fim determinado [backgroundWorker1]
            if (backgroundWorker1.IsBusy)//se o backgroundWorker1 estiver ocupado
            {
                // notifica a thread que o cancelamento foi solicitado.
                // Cancela a tarefa DoWork
                backgroundWorker1.CancelAsync();
            }
            //desabilita o botão cancelar.
            btnCancelar.Enabled = false;
            lblPercent.Text = "Cancelando...";
        }
    }
}
