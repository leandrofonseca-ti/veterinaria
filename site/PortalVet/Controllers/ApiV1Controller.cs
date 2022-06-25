using Newtonsoft.Json;
using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Data.Interface;
using PortalVet.Data.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace PortalVet.Controllers
{
    public class ApiV1Controller : ApiController
    {
     
        [HttpGet]
        [Route("~/api/IsLogged")]
        public ApiRetornoPadrao IsLogged()
        {
            ApiRetornoPadrao ret = new ApiRetornoPadrao();
            try
            {
                System.Net.Http.Headers.HttpRequestHeaders headers = Request.Headers;
                string chave = headers.Authorization.Scheme;
                var _empresaService = new EmpresaService();
                if (_empresaService.CarregarPorChave(chave, out int unidadeId))
                {
                    ret.Code = 0;
                    ret.Data = "OK";
                    ret.Message = "Success";
                }
                else
                {
                    ret.Code = 1;
                    ret.Data = null;
                    ret.Message = "Invalid Token";

                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.Code = 1;
                ret.Data = null;
                ret.Message = $"Internal Error ({ex.Message})";
                return ret;
            }
        }




        [HttpPost]
        [Route("~/api/SalvarExame")]
        public ApiRetornoPadrao SalvarExame([FromBody] dynamic data)
        {
            ApiRetornoPadrao ret = new ApiRetornoPadrao();

            try
            {
                System.Net.Http.Headers.HttpRequestHeaders headers = Request.Headers;
                string chave = headers.Authorization.Scheme;
                var _empresaService = new EmpresaService();
                if (_empresaService.CarregarPorChave(chave, out int unidadeId))
                {
                    #region
                    var _paramEspecie = data.Especie.Value;
                    var _especieOutros = "";
                    var _especieId = _empresaService.CarregarEspeciePorNome(data.Especie.Value, out bool outros);
                    if (outros)
                    {
                        _especieOutros = data.Especie.Value;
                    }

                    string _paramRaca = data.Raca.Value;
                    int _paramRacaId = Int32.Parse(data.RacaId.Value.ToString());

                    var _racaId = _empresaService.CarregarRacaPorNome(unidadeId, _paramRaca, _paramRacaId);
                    var _racaOutros = string.Empty;
                    if (data.ClinicaId > 0)
                    {
                        var _paramClinicaTelefone = data.ClinicaTelefone.Value;
                        var _paramClinicaEmail = data.ClinicaEmail.Value;
                        string _paramClinica = data.Clinica.Value;
                        int _paramClinicaId = Int32.Parse(data.ClinicaId.Value.ToString());
                        var _clinicaId = _empresaService.CarregarClinica(unidadeId, _paramClinicaId, _paramClinica, _paramClinicaEmail, _paramClinicaTelefone, out bool _novoRegistro, out string _senha);
                        if (_racaId == 0)
                        {
                            _racaId = 6;// tipo OUTROS
                            _racaOutros = data.Raca.Value;
                        }


                        var _proprietario = (string)data.Proprietario.Value;
                        var _proprietarioEmail = (string)data.ProprietarioEmail.Value;
                        var _proprietarioTelefone = (string)data.ProprietarioTelefone.Value;
                        var _proprietarioCPFCNPJ = (string)data.ProprietarioCPFCNPJ.Value;
                        //var _clienteId = _empresaService.CarregarClientePorNome(unidadeId, _proprietario, _proprietarioEmail, _proprietarioTelefone, _proprietarioCPFCNPJ);

                        if (_clinicaId > 0)
                        {
                            var _exameService = new ExameService();
                            var entity = new ExameItem();
                            var _auxDtHora = (DateTime)data.Hora.Value;
                            var _auxDtData = (DateTime)data.Data.Value;
                            var _dataHoraExame = new DateTime(_auxDtData.Year, _auxDtData.Month, _auxDtData.Day, _auxDtHora.Hour, _auxDtData.Minute, 0);
                            var _codigoOffline = Int32.Parse(data.ID.Value.ToString());
                            bool naoExisteCodigo = _exameService.naoExisteCodigo(_codigoOffline, unidadeId, out int codigoExistente);


                            var registro = new ExameItem()
                            {
                                SituacaoId = EnumExameSituacao.Em_Analise_Laudador.GetHashCode(),
                                Codigo = _codigoOffline,
                                CompanyId = unidadeId,
                                DataExame = _dataHoraExame,
                                ClinicaId = _clinicaId,
                                Proprietario = _proprietario,
                                ProprietarioEmail = _proprietarioEmail,
                                ProprietarioTelefone = _proprietarioTelefone,
                                ClienteId = 0,//_clienteId,
                                LaudadorId = 0,
                                RacaId = _racaId,
                                RacaSelecao = _paramRaca,
                                EspecieSelecao = _paramEspecie,
                                RacaOutros = _racaOutros,
                                Historico = (string)data.Historico.Value,
                                Valor = (string)data.Valor.Value,
                                FormaPagamento = (string)data.FormaPagamento.Value,
                                EmailCliente = string.Empty,
                                EspecieOutros = _especieOutros,
                                EspecieId = _especieId,
                                Idade = (string)data.Idade.Value,
                                NomeCliente = string.Empty,
                                Paciente = (string)data.Paciente.Value,
                                Veterinario = (string)data.Veterinario.Value
                            };

                            //if (!naoExisteCodigo)
                            //{
                            //    naoExisteCodigo = true;
                            //    registro.Id = codigoExistente;
                            //}

                            if (naoExisteCodigo)
                            {
                                entity = _exameService.Save(
                                           registro
                                          );

                                if (entity.Id > 0)
                                {
                                    var empresa = _empresaService.Carregar(entity.CompanyId);
                                    var hist = new ExameHistoricoItem();
                                    hist.ExameId = entity.Id;
                                    hist.UsuarioId = 0;
                                    hist.UsuarioNome = "API";
                                    hist.UsuarioEmail = string.Empty;
                                    hist.Descricao = "Cadastro dos dados.";
                                    hist.SituacaoId = EnumExameSituacao.Criacao.GetHashCode();
                                    hist.Conteudo = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                                    _exameService.SaveHistorico(hist);

                                    // NOTIFICAR GERENTES(S) VINCULADAS COM EMPRESAID
                                    //var emails = _exameService.CarregarEmailsGerente(unidadeId);
                                    //EnviarNotificacaoExame(entity.Id, unidadeId, "Novo exame criado!", emails);

                                    // NOTIFICAR CLINICA
                                    var emailsClinica = new List<SelectListWeb>();
                                    emailsClinica.Add(new SelectListWeb() { Text = _paramClinica, Value = _paramClinicaEmail });

                                    StringBuilder sbText = new StringBuilder();
                                    if (_novoRegistro)
                                    {
                                        var body = Util.GetResourcesBemVindoCredenciais();
                                        body = body.Replace("#CLINICA#", _paramClinica);
                                        body = body.Replace("#EMAIL#", _paramClinicaEmail);
                                        body = body.Replace("#SENHA#", _senha);

                                        body = body.Replace("#LINK#", $"{System.Configuration.ConfigurationManager.AppSettings["UrlRootAdmin"].ToString()}");

                                        var bodyMaster = Util.CarregaConteudoMaster(empresa, body);
                                        
                                        Util.SaveHistoricoEmail(
                                            hist.ExameId, 
                                            (EnumExameSituacao) hist.SituacaoId, 
                                            hist.UsuarioId, 
                                            hist.UsuarioNome,
                                            hist.UsuarioEmail,
                                            $"Envio email com credenciais para a clinica: {_paramClinicaEmail}");

                                        Util.SendEmail($"Credenciais", _paramClinicaEmail, bodyMaster);
                                    }

                                    Util.SaveHistoricoEmail(
                                           hist.ExameId,
                                           (EnumExameSituacao)hist.SituacaoId,
                                           hist.UsuarioId,
                                           hist.UsuarioNome,
                                           hist.UsuarioEmail,
                                           $"Envio email 'Novo exame criado! {hist.ExameId}' para a clinica: {_paramClinicaEmail}");

                                    EnviarNotificacaoExame(entity.Id, unidadeId, "<center><strong>Novo exame criado!</strong></center>", emailsClinica);

                                    var prop = _exameService.Get(entity.Id);
                                    // NOTIFICAR PROPRIETARIO
                                    if (prop != null && !String.IsNullOrEmpty(prop.ProprietarioEmail))
                                    {
                                        var emails = new List<SelectListWeb>();
                                        emails.Add(new SelectListWeb() { Text = prop.Proprietario, Value = prop.ProprietarioEmail });
                                        EnviarNotificacaoExame(empresa, entity.Id,
                                          $"{empresa.Nome} : {entity.Paciente} : Notificação Exame {entity.Id}",
                                           entity.Paciente,
                                           "<center><strong>Novo exame criado!</strong></center>", emails);


                                        Util.SaveHistoricoEmail(
                                           hist.ExameId,
                                           (EnumExameSituacao)hist.SituacaoId,
                                           hist.UsuarioId,
                                           hist.UsuarioNome,
                                           hist.UsuarioEmail,
                                           $"Envio email 'Novo exame criado! {hist.ExameId}' para o proprietário: {prop.ProprietarioEmail }");

                                    }


                                    ret.Code = 0;
                                    ret.Message = "OK";
                                    ret.Data = entity.Id;
                                    ret.Action = "INSERTED";
                                }
                            }
                            else
                            {
                                registro.Id = codigoExistente;
                                entity = _exameService.Save(registro);

                                var empresa = _empresaService.Carregar(entity.CompanyId);
                                var hist = new ExameHistoricoItem();
                                hist.ExameId = entity.Id;
                                hist.UsuarioId = 0;
                                hist.UsuarioNome = "API";
                                hist.UsuarioEmail = string.Empty;
                                hist.Descricao = "Atualização dos dados [2].";
                                hist.SituacaoId = EnumExameSituacao.Criacao.GetHashCode();
                                hist.Conteudo = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                                _exameService.SaveHistorico(hist);

                                ret.Code = 0;
                                ret.Message = "OK";
                                ret.Data = codigoExistente;
                                ret.Action = "UPDATED";
                            }
                        }
                        else
                        {
                            ret.Code = 1;
                            ret.Data = null;
                            ret.Message = "ClinicaId incorreto";
                        }
                    }
                    else
                    {
                        ret.Code = 1;
                        ret.Data = null;
                        ret.Message = "ClinicaId incorreto";
                    }
                    #endregion
                }
                else
                {
                    ret.Code = 1;
                    ret.Data = null;
                    ret.Message = "Invalid Token";

                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.Code = 1;
                ret.Data = null;
                ret.Message = $"Internal Error ({ex.Message})";
                return ret;
            }

        }



        public void EnviarNotificacaoExame(EmpresaItem empresa, int exameId, string assunto, string paciente, string mensagem, List<SelectListWeb> emails)
        {
            foreach (var item in emails)
            {
                var body = Util.GetResourcesNotificacaoExame();
                body = body.Replace("#NUMERO_EXAME#", exameId.ToString());
                body = body.Replace("#PACIENTE#", paciente);
                body = body.Replace("#MENSAGEM#", mensagem);
                var key = HttpUtility.UrlEncode(Data.Helper.Util.Encriptar(string.Format("{0}|{1}|{2}", empresa.Id, exameId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                body = body.Replace("#LINKEXAME#", $"{System.Configuration.ConfigurationManager.AppSettings["UrlRoot"].ToString()}viewExame?key={key}");
                var bodyMaster = Util.CarregaConteudoMaster(empresa, body);
                Util.SendEmail($"{assunto} : {paciente}", item.Value, bodyMaster);
            }

        }
       
        public void EnviarEmailMensagem(int empresaId, string assunto, string mensagem, string email)
        {
            var empresa = new EmpresaService().Carregar(empresaId);
            var body = Util.GetResourcesMensagemModelo();
            body = body.Replace("#MENSAGEM#", mensagem);
            body = body.Replace("#LINK#", "https://webimagem.vet.br/Admin");
            body = body.Replace("#ALIAS#", "Acessar");
            var bodyMaster = Util.CarregaConteudoMaster(empresa, body);
            Util.SendEmail($"{empresa.Nome} : {assunto}", email, bodyMaster);

        }

        public void EnviarNotificacaoExame(int exameId, int empresaId, string mensagem, List<SelectListWeb> emails)
        {
            var empresa = new EmpresaService().Carregar(empresaId);
            var exame = new ExameService().Get(exameId);
            foreach (var item in emails)
            {
                var body = Util.GetResourcesNotificacaoExame();
                body = body.Replace("#NUMERO_EXAME#", exameId.ToString());
                body = body.Replace("#PACIENTE#", exame.Paciente);
                body = body.Replace("#MENSAGEM#", mensagem);
                var key = HttpUtility.UrlEncode(Data.Helper.Util.Encriptar(string.Format("{0}|{1}|{2}", empresaId, exameId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                body = body.Replace("#LINKEXAME#", $"{System.Configuration.ConfigurationManager.AppSettings["UrlRoot"].ToString()}viewExame?key={key}");
                var bodyMaster = Util.CarregaConteudoMaster(empresa, body);
                Util.SendEmail($"{empresa.Nome} : {exame.Paciente} : Notificação Exame {exameId}", item.Value, bodyMaster);
            }

        }

        [HttpPost]
        [Route("~/api/SalvarImagemExame")]
        public ApiRetornoPadrao SalvarImagemExame([FromBody] dynamic data)
        {
            ApiRetornoPadrao ret = new ApiRetornoPadrao();
            int _paramExameId = 0;
            string ext = "";
            string fileName = "";
            string location = "";
            string exceptionMessage = "";
            int unidadeId = 0;
            try
            {
                System.Net.Http.Headers.HttpRequestHeaders headers = Request.Headers;
                string chave = headers.Authorization.Scheme;

                ret.Code = 1;
                ret.Message = "NOK";

                var _empresaService = new EmpresaService();
                if (_empresaService.CarregarPorChave(chave, out unidadeId))
                {
                    string pathSaveFiles = "exames\\";
                    _paramExameId = Int32.Parse(data.ExameId.Value.ToString());
                    string _fileBase64 = data.FileBase64.Value;
                    var diretorio = string.Concat(unidadeId, "\\", _paramExameId);

                    if (data != null && !String.IsNullOrEmpty(data.FileNome.Value) && _paramExameId > 0)
                    {
                        try
                        {
                            location = string.Concat(Util.GetMapUpload(), pathSaveFiles, diretorio);
                            //Check if the directory exists
                            if (!System.IO.Directory.Exists(location))
                            {
                                System.IO.Directory.CreateDirectory(location);
                            }


                            ext = System.IO.Path.GetExtension(data.FileNome.Value).ToLower();

                            if (!String.IsNullOrEmpty(ext))
                            {
                                var prefix = "file_";
                                fileName = string.Format("{0}{1}{2}", prefix, DateTime.Now.ToString("yyyyMMddHHmmssfff"), ext);
                                System.Drawing.Imaging.ImageFormat formatImg = System.Drawing.Imaging.ImageFormat.Png;
                                switch (ext)
                                {
                                    //case ".png":break;
                                    case ".bmp":
                                        formatImg = System.Drawing.Imaging.ImageFormat.Bmp;
                                        break;
                                    case ".jpg":
                                    case ".jpeg":
                                        formatImg = System.Drawing.Imaging.ImageFormat.Jpeg;
                                        break;
                                    case ".tiff":
                                        formatImg = System.Drawing.Imaging.ImageFormat.Tiff;
                                        break;
                                }
                                var result = SaveByteArrayAsImage(string.Concat(location, @"\", fileName), _fileBase64, formatImg, out exceptionMessage);


                                if (!result)
                                {
                                    ret.Code = 1;
                                    ret.Message = "NOK";
                                }
                                else
                                {
                                    ret.Code = 0;
                                    ret.Message = "OK";

                                    var hist = new ExameHistoricoItem();
                                    hist.ExameId = _paramExameId;
                                    hist.UsuarioId = 0;
                                    hist.UsuarioNome = "API";
                                    hist.UsuarioEmail = string.Empty;
                                    hist.Descricao = "Atualização dos dados (Imagens).";
                                    hist.SituacaoId = EnumExameSituacao.Criacao.GetHashCode();
                                    hist.Conteudo = "";
                                    new ExameService().SaveHistorico(hist);
                                    //path = string.Concat(Util.GetUrlUpload(), pathSaveFiles, diretorio, "/", fileName);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ret.Code = 2;
                            ret.Message = ex.Message;
                        }
                    }
                }
                new ExameService().SaveImageHistorico(ret.Code == 0, fileName, ext, location, unidadeId, _paramExameId, exceptionMessage);

                return ret;
            }
            catch (Exception ex)
            {
                ret.Code = 1;
                ret.Data = null;
                ret.Message = $"Internal Error ({ex.Message})";
                new ExameService().SaveImageHistorico(ret.Code == 0, fileName, ext, location, unidadeId, _paramExameId, ret.Message);

                return ret;
            }

        }


        private bool SaveByteArrayAsImage(string fullOutputPath, string base64String, System.Drawing.Imaging.ImageFormat formatImg, out string exception)
        {
            var result = false;
            exception = string.Empty;
            try
            {

                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true, false);
                image.Save(fullOutputPath, formatImg);


                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                exception = ex.Message + " | " + ex.StackTrace;
            }
            return result;
        }
    }
}
