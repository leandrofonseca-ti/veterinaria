using PortalVet.App_Start;
using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Data.Interface;
using PortalVet.Data.Service;
using PortalVet.Helper;
using PortalVet.Models;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PortalVet.Controllers
{
    public class DashboardController : BaseController
    {
        public string _pathFolderUser = "user";

        private readonly IAdminUserService _adminUserService;
        private readonly IAdminCompanyService _adminCompanyService;
        private readonly IEmpresaService _empresaService;
        private readonly IExameService _exameService;
        public DashboardController(IAdminCompanyService adminCompanyService, IExameService exameService, IAdminUserService adminUserService, IEmpresaService empresaService)//, IClienteService clienteService)
        {
            _exameService = exameService ??
                throw new ArgumentNullException(nameof(exameService));

            _adminCompanyService = adminCompanyService ??
                throw new ArgumentNullException(nameof(adminCompanyService));

            _adminUserService = adminUserService ??
                throw new ArgumentNullException(nameof(adminUserService));

            _empresaService = empresaService ??
              throw new ArgumentNullException(nameof(empresaService));

        }
        // GET: Dashboard
        [SecurityPages]
        public ActionResult Index()
        {
            //_hubContext.Clients.All.moduloName(new LogModulo() { Nome = "Dashboard", Data = DateTime.Now });

            VMHome home = UtilAdmin.GetModelHome(RouteData, HttpContext.Session, Url);

            var profileid = (EnumAdminProfile)SessionsAdmin.PerfilId;

            ViewData["listUsuarios"] = new List<SelectListItem>();
            ViewData["listImoveis"] = new List<SelectListItem>();
            ViewData["hdnAdminDashboard"] = (home.ProfileName.ToLower() == "administrador").ToString().ToLower();

            SessionsAdmin.ListEmpresa.ForEach(r =>
            {
                r.Total = 0;
            });


            var listStatus = Enum.GetValues(typeof(EnumExameSituacao)).Cast<EnumExameSituacao>().ToList();

            List<SelectListItem> statusFinal = new List<SelectListItem>();

            if (SessionsAdmin.PerfilId == EnumAdminProfile.Cliente.GetHashCode())
            {
                foreach (var item in listStatus)
                {
                    if (item.GetHashCode() == EnumExameSituacao.Concluido.GetHashCode())
                    {
                        var text = Enumeradores.GetDescription((EnumExameSituacao)item.GetHashCode());
                        statusFinal.Add(new SelectListItem() { Text = text, Value = item.GetHashCode().ToString() });
                    }
                }
            }


            if (SessionsAdmin.PerfilId == EnumAdminProfile.Gerente.GetHashCode())
            {
                foreach (var item in listStatus)
                {
                    var text = Enumeradores.GetDescription((EnumExameSituacao)item.GetHashCode());
                    statusFinal.Add(new SelectListItem() { Text = text, Value = item.GetHashCode().ToString() });
                }

                var listagem = _exameService.CarregarExamesPendentesGerente(SessionsAdmin.UsuarioId);

                SessionsAdmin.ListEmpresa.ForEach(r =>
                {
                    var ent = listagem.Where(y => y.ID == r.Value).FirstOrDefault();
                    if (ent != null)
                        r.Total = ent.Total;
                });
            }


            if (SessionsAdmin.PerfilId == EnumAdminProfile.Clinica.GetHashCode())
            {
                foreach (var item in listStatus)
                {
                    if (item.GetHashCode() != EnumExameSituacao.Criacao.GetHashCode())
                    {
                        var text = Enumeradores.GetDescription((EnumExameSituacao)item.GetHashCode());
                        statusFinal.Add(new SelectListItem() { Text = text, Value = item.GetHashCode().ToString() });
                    }
                }
            }

            if (SessionsAdmin.PerfilId == EnumAdminProfile.Laudador.GetHashCode())
            {
                foreach (var item in listStatus)
                {
                    if (//item.GetHashCode() != EnumExameSituacao.Concluido_Gerente.GetHashCode() &&
                        //item.GetHashCode() != EnumExameSituacao.Concluido.GetHashCode() &&
                        //item.GetHashCode() != EnumExameSituacao.Em_Analise_Gerente.GetHashCode() &&
                        item.GetHashCode() != EnumExameSituacao.Criacao.GetHashCode())
                    {
                        var text = Enumeradores.GetDescription((EnumExameSituacao)item.GetHashCode());
                        statusFinal.Add(new SelectListItem() { Text = text, Value = item.GetHashCode().ToString() });
                    }
                }
                var listagem = _exameService.CarregarExamesPendentesLaudador(SessionsAdmin.UsuarioId);

                SessionsAdmin.ListEmpresa.ForEach(r =>
                {
                    var ent = listagem.Where(y => y.ID == r.Value).FirstOrDefault();
                    if (ent != null)
                        r.Total = ent.Total;
                });

            }


            ViewData["ListSituacao"] = statusFinal;
            return View("Index", home);
        }

        [WebGet(UriTemplate = "ListProfiles", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult ListProfiles(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(true);

            var usuarioId = Int32.Parse(form["uid"].ToString());

            var list = _adminUserService.ListProfiles(usuarioId);

            jsonResultado.Data = list;

            return new LargeJsonResult { Data = jsonResultado };
        }


        [WebGet(UriTemplate = "ArquivarExame", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult ArquivarExame(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(true);

            var exameid = Int32.Parse(form["exameid"].ToString());

            var result = _exameService.ArquivarExame(exameid, SessionsAdmin.PerfilId, SessionsAdmin.UsuarioId);

            jsonResultado.Data = result;

            return new LargeJsonResult { Data = jsonResultado };
        }

        [WebGet(UriTemplate = "CarregarEmailExame", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult CarregarEmailExame(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(true);

            var exameId = Int32.Parse(form["exameid"].ToString());

            var exame = _exameService.Get(exameId);

            jsonResultado.Data = exame.EmailCliente;

            return new LargeJsonResult { Data = jsonResultado };
        }


        [WebGet(UriTemplate = "EnviarEmailExame", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult EnviarEmailExame(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(true);

            var exameId = Int32.Parse(form["exameid"].ToString());
            var email = form["email"].ToString();
            var result = false;
            try
            {
                MemoryStream pdfStream = new MemoryStream(GenerateExamePDF(exameId));
                Attachment pdf = new Attachment(pdfStream, "Exame_" + exameId + ".pdf", "application/pdf");

                var body = Data.Properties.Resources.MensagemModelo;
                body = body.Replace("#MENSAGEM#", $"Segue em anexo o exame {exameId}");
                //var key = HttpUtility.UrlEncode(Data.Helper.Util.Encriptar(string.Format("{0}|{1}", usuario.Email, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                //body = body.Replace("#LINK#", $"{System.Configuration.ConfigurationManager.AppSettings["UrlRootAdmin"].ToString()}newpassword?key={key}");
                var exame = _exameService.Get(exameId);
                var company = new AdminUserService().CarregarCompany(exame.CompanyId);

                var bodyMaster = Util.CarregaConteudoMaster(company, body);

                Util.SendEmail($"{company.Nome} - Exame {exameId}", email, bodyMaster, pdf);
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            jsonResultado.Data = result;

            return new LargeJsonResult { Data = jsonResultado };
        }

        public Byte[] GenerateExamePDF(int id)
        {
            byte[] pdfBytes = new byte[] { };

            System.Web.Mvc.ControllerContext context = this.ControllerContext;

            //route.Values.Add("area", "DashboardController");
            // var controller = DependencyResolver.Current.GetService<DashboardController>();
            // ControllerContext newContext = new ControllerContext(new HttpContextWrapper(System.Web.HttpContext.Current), route, controller);

            // controller.ControllerContext = newContext;
            //controller.ViewContratoModeloPDF(id.ToString());
            //var actionPDF = new ViewAsPdf("ViewContratoModeloPDF", new { id = id })
            //{
            //    PageSize = Size.A4,
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,
            //    PageMargins = { Left = 1, Right = 1 }
            //};
            //var actionPDF  = new Rotativa.ActionAsPdf("ViewContratoModeloPDF", new { id = id })
            //{
            //    PageSize = Size.A4,
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,
            //    PageMargins = { Left = 1, Right = 1 },
            //    CustomSwitches = "--load-error-handling ignore --load-media-error-handling ignore",
            //};

            var actionPDF = new Rotativa.UrlAsPdf(Url.Action("ViewContratoModelo", "Dashboard", new { id = id }))
            {
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },
                CustomSwitches = "--load-error-handling ignore --load-media-error-handling ignore",
            };

            pdfBytes = actionPDF.BuildFile(context);


            return pdfBytes;
        }


        public ActionResult ViewContratoModeloPDF(string id)
        {
            return new Rotativa.ActionAsPdf("ViewContratoModelo", new { id = id });
        }

        public ActionResult ViewContratoModelo(string id)
        {
            var data = new ExameItem();
            int _id = 0;
            var listFiles = new List<ViewModelFoto>();
            if (!String.IsNullOrEmpty(id))
            {
                Int32.TryParse(id, out _id);
            }
            ViewData["imagemCompanyNome"] = "";
            ViewData["imagemCompany"] = "";
            ViewData["modeloCabecalho"] = "";
            ViewData["modeloCorpo"] = "";
            ViewData["modeloRodape"] = "";
            ViewData["viewArquivos"] = new List<ViewModelFoto>();

            if (_id > 0)
            {
                try
                {

                    data = _exameService.Get(_id);
                    var empresa = new Data.Service.EmpresaService().Carregar(data.CompanyId);
                    if (empresa != null)
                    {
                        data.CompanyNome = empresa.Nome;
                        if (!String.IsNullOrEmpty(empresa.Imagem))
                            ViewData["imagemCompany"] = $"{Util.GetUrlUpload()}empresas/{empresa.Imagem}";
                        else
                            ViewData["imagemCompanyNome"] = $"{empresa.Nome}";






                        if (!String.IsNullOrEmpty(id))
                        {
                            var pathUploadCliente = Util.GetMapUpload();
                            var pathCliente = string.Format("{0}exames\\{1}\\{2}", pathUploadCliente, data.CompanyId, id);
                            if (Directory.Exists(pathCliente))
                            {
                                var ct = 0;
                                foreach (var file in Directory.GetFiles(pathCliente, @"*.*", SearchOption.TopDirectoryOnly))
                                {
                                    var fileName = Path.GetFileName(file);
                                    var relativo = string.Concat(Util.GetUrlUpload(), "exames/", data.CompanyId, "/", id, "/", fileName);
                                    var fisico = string.Concat(Util.GetMapUpload(), "exames/", data.CompanyId, "/", id, "/", fileName);
                                    listFiles.Add(new ViewModelFoto() { ID = ct, Path = fisico, Name = relativo, OriginalFileName = fileName });
                                    ct++;
                                }
                            }
                        }
                    }
                    //var data = _contratoModeloService.CarregarDocumentoVersao(_id);

                    ViewData["modeloCabecalho"] = "";//?? string.Empty;
                    ViewData["modeloCorpo"] = data.Descricao ?? string.Empty;
                    ViewData["modeloRodape"] = data.Rodape ?? string.Empty;
                    ViewData["viewArquivos"] = listFiles;




                }
                catch //(Exception ex)
                {
                }
            }

            return View("ViewContratoModelo", data);
        }

        [ValidateInput(false)]
        [WebGet(UriTemplate = "SetProfile", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult SetProfile(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(true);

            var profileId = Int32.Parse(form["pid"].ToString());

            HttpCookie getCookie = HttpContext.Request.Cookies.Get("admCookie");

            if (getCookie != null)// && getCookie.Values["CurrentProfileId"] != null)
            {
                getCookie.Values.Add("CurrentProfileId", profileId.ToString());
            }
            SessionsAdmin.CurrentProfileId = profileId;
            SessionsAdmin.PerfilId = profileId;
            SessionsAdmin.Acessos = UtilAdmin.GetAcesso(profileId, SessionsAdmin.EmpresaId);
            jsonResultado.Data = true;

            return new LargeJsonResult { Data = jsonResultado };
        }




        [WebGet(UriTemplate = "SetProfileEmp", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult SetProfileEmp(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(true);

            var userId = Int32.Parse(form["uid"].ToString());

            var profileId = Int32.Parse(form["pid"].ToString());

            var empresaId = Int32.Parse(form["eid"].ToString());

            HttpCookie getCookie = HttpContext.Request.Cookies.Get("admCookie");

            if (getCookie != null)// && getCookie.Values["CurrentProfileId"] != null)
            {
                getCookie.Values.Add("CurrentProfileId", profileId.ToString());
            }
            SessionsAdmin.CurrentProfileId = profileId;
            SessionsAdmin.PerfilId = profileId;

            var _empresaId = 0;
            var _empresalogoadmin = "";
            var _empresalogo = "";
            var listEmpresa = new List<SelectListWeb>();
            if (empresaId > 0 && userId > 0)
            {
                var first = _adminCompanyService.CarregarEmpresaAdmin(userId, empresaId);
                _empresaId = first.Id;
                _empresalogoadmin = first.Imagem;
                _empresalogo = first.Imagem;

            }

            _empresalogoadmin = profileId == EnumAdminProfile.Administrador.GetHashCode() ? string.Empty : _empresalogoadmin;
            _empresalogo = profileId == EnumAdminProfile.Administrador.GetHashCode() ? string.Empty : _empresalogo;

            var logo = !String.IsNullOrEmpty(_empresalogoadmin) ? Util.GetUrlUpload() + "empresas/" + _empresalogoadmin : (!String.IsNullOrEmpty(_empresalogo) ? Util.GetUrlUpload() + "empresas/" + _empresalogo : "~/Content/img/logo.png");
            SessionsAdmin.EmpresaLogo = logo;
            SessionsAdmin.EmpresaId = _empresaId;

            SessionsAdmin.Acessos = UtilAdmin.GetAcesso(profileId, _empresaId);

            SessionsAdmin.PerfilNome = Enumeradores.GetDescription((EnumAdminProfile)profileId);

            var user = _adminUserService.ValidateUser(userId);
            // Add Cookie
            HttpCookie myCookie = new HttpCookie("admCookie");
            HttpContext.Response.Cookies.Remove("admCookie");
            HttpContext.Response.Cookies.Add(myCookie);
            myCookie.Values.Add("ForceId", "");
            myCookie.Values.Add("UsuarioId", user.Id.ToString());
            myCookie.Values.Add("Logotipo", logo);
            myCookie.Values.Add("EmpresaId", _empresaId.ToString());
            myCookie.Values.Add("Nome", user.Nome);
            myCookie.Values.Add("Foto", user.Imagem);
            myCookie.Values.Add("Email", user.Email);
            myCookie.Values.Add("CurrentProfileId", profileId.ToString());
            myCookie.Values.Add("PerfilNome", SessionsAdmin.PerfilNome);
            DateTime dtxpiry = DateTime.Now.AddDays(this.tempoExpirarCookie);
            myCookie.Expires = dtxpiry;


            jsonResultado.Data = true;

            return new LargeJsonResult { Data = jsonResultado };
        }


        [WebGet(UriTemplate = "DashReport", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult DashReport(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(true);

            var report = _adminUserService.DashReport(SessionsAdmin.PerfilId, SessionsAdmin.EmpresaId, SessionsAdmin.UsuarioId);

            jsonResultado.Data = new
            {
                totalClientes = report.totalClientes,
                totalExamesEmAndamento = report.totalExamesEmAndamento,
                totalExamesConcluidos = report.totalExamesConcluidos,
                totalExamesCancelados = report.totalExamesCancelados,
                totalGerentes = report.totalGerentes
            };

            return new LargeJsonResult { Data = jsonResultado };
        }



        [WebGet(UriTemplate = "DashReportAdmin", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult DashReportAdmin(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            var data = _empresaService.CarregarDashboardAdmin();

            jsonResultado.Data = data;
            return new LargeJsonResult { Data = jsonResultado };
        }



        [WebGet(UriTemplate = "ListAcoes", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult ListAcoes(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            int pageIndex = 1;
            int pageSize = 10;
            int pgTotal = 0;
            DateTime? dtIni = null;
            DateTime? dtFim = null;
            var filterSituacao = "";
            //var filterCPFCNPJ = "";
            var filterCodigo = "";
            if (form["pageIndex"] != null)
            {
                Int32.TryParse(form["pageIndex"].Trim(), out pageIndex);
            }

            if (form["pageSize"] != null)
            {
                Int32.TryParse(form["pageSize"].Trim(), out pageSize);
            }

            if (form["txtFilterCodigo"] != null)
            {
                filterCodigo = form["txtFilterCodigo"].ToString();
            }

            //if (form["txtFilterCPFCNPJ"] != null)
            //{
            //    filterCPFCNPJ = form["txtFilterCPFCNPJ"].ToString();
            //}


            var filterLaudadorId = 0;

            if (SessionsAdmin.PerfilId == EnumAdminProfile.Laudador.GetHashCode())
            {
                filterLaudadorId = SessionsAdmin.UsuarioId;
            }

            if (form["drpFilterSituacao"] != null)
            {
                filterSituacao = form["drpFilterSituacao"].ToString();
            }


            if (form["txtFilterDtInicio"] != null && form["txtFilterDtFim"] != null)
            {
                if (form["txtFilterDtInicio"] != null && !String.IsNullOrEmpty(form["txtFilterDtInicio"].ToString()))
                {
                    if (DateTime.TryParse(form["txtFilterDtInicio"].ToString(), out DateTime auxDtIni))
                    {
                        dtIni = new DateTime(auxDtIni.Year, auxDtIni.Month, auxDtIni.Day, 0, 0, 0);
                    }
                }

                if (form["txtFilterDtFim"] != null && !String.IsNullOrEmpty(form["txtFilterDtFim"].ToString()))
                {
                    if (DateTime.TryParse(form["txtFilterDtFim"].ToString(), out DateTime auxDtFim))
                    {
                        dtFim = new DateTime(auxDtFim.Year, auxDtFim.Month, auxDtFim.Day, 23, 59, 59);
                    }
                }
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                DateTime dtFinish = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 59);
                DateTime dtInitAux = dtFinish.AddDays(-7);
                dtIni = new DateTime(dtInitAux.Year, dtInitAux.Month, dtInitAux.Day, 0, 0, 0);
                dtFim = dtFinish;
            }
            int empresaid = SessionsAdmin.EmpresaId;
            int usuarioId = SessionsAdmin.UsuarioId;
            int perfilId = SessionsAdmin.PerfilId;

            var data = _empresaService.ListAcoesRecentes(out pgTotal, pageIndex, pageSize, usuarioId, empresaid, perfilId, GetFilter(perfilId, usuarioId, empresaid, dtIni, dtFim, filterSituacao, "", filterCodigo, filterLaudadorId));

            jsonResultado.Data = data;
            jsonResultado.PageTotal = pgTotal;
            jsonResultado.PageIndex = pageIndex;
            jsonResultado.PageSize = pageSize;
            return new LargeJsonResult { Data = jsonResultado };
        }

        public JsonResult CarregarModelosMensagem()
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(true);

            jsonResultado.Data = new
            {
                Modelos = _empresaService.CarregarModelosMensagem(Helper.SessionsAdmin.EmpresaId, Helper.SessionsAdmin.UsuarioId, Helper.SessionsAdmin.PerfilId),
                EmailTo = Helper.SessionsAdmin.UsuarioEmail
            };

            return Json(jsonResultado);
        }


        public JsonResult EnviarMensagemModelo(ModeloMensagemEnvio modeloMensagemEnvio)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(true);
            try
            {
                if (string.IsNullOrEmpty(modeloMensagemEnvio.Mensagem))
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "TextoModelo", Message = "Campo Obrigatório" });
                }

                if (!modeloMensagemEnvio.EnvioPorEMail && !modeloMensagemEnvio.EnvioPorWhats)
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "lblEmail", Message = "Campo Obrigatório" });
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "lblWhats", Message = "Campo Obrigatório" });
                }

                if (jsonResultado.Criticas.Any())
                {
                    jsonResultado.MessageTipo = "error";
                    jsonResultado.Message = "Verifique as críticas em vermelho.";
                    return Json(jsonResultado);
                }

                if (modeloMensagemEnvio.EnvioPorEMail)
                {
                    string emailTo = "";

                    if (SessionsAdmin.UsuarioEmail != null)
                    {
                        emailTo = SessionsAdmin.UsuarioEmail;
                    }

                    var company = new AdminUserService().CarregarCompany(SessionsAdmin.EmpresaId);
                    var _clinicaNome = company.Nome;
                    var msg = modeloMensagemEnvio.Mensagem.Replace("\n", "<br>");
                    //var chave = new Cryptography().Encrypt($"eid={modeloMensagemEnvio.ExameId.ToString()}&uid={SessionsAdmin.UsuarioId}&cid={SessionsAdmin.EmpresaId}");
                    //var _urlExame = $"{AppSettings.UrlRootAdmin}AcessoExame?chave={chave}";

                    var key = HttpUtility.UrlEncode(Data.Helper.Util.Encriptar(string.Format("{0}|{1}|{2}", SessionsAdmin.EmpresaId, modeloMensagemEnvio.ExameId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                    var _urlExame = $"{AppSettings.UrlRoot}viewExame?key={key}";


                    var body = PortalVet.Data.Properties.Resources.MensagemModelo;
                    body = body.Replace("#MENSAGEM#", msg);
                    body = body.Replace("{CLIENTE}", modeloMensagemEnvio.ClienteNome);
                    body = body.Replace("{URL_EXAME}", _urlExame);
                    body = body.Replace("{CLINICA}", _clinicaNome);
                    body = body.Replace("{USUARIO_LOGADO}", SessionsAdmin.UsuarioNome);

                    var bodyMaster = Util.CarregaConteudoMaster(company, body);

                    Util.SendEmail(company.Nome, modeloMensagemEnvio.ClienteEmail, bodyMaster);
                }
                if (modeloMensagemEnvio.EnvioPorWhats)
                {
                    var company = new AdminUserService().CarregarCompany(SessionsAdmin.EmpresaId);
                    var _clinicaNome = company.Nome;
                    //var chave = new Cryptography().Encrypt($"eid={modeloMensagemEnvio.ExameId.ToString()}&uid={SessionsAdmin.UsuarioId}&cid={SessionsAdmin.EmpresaId}");
                    //var _urlExame = $"{AppSettings.UrlRootAdmin}AcessoExame?chave={chave}";

                    var key = HttpUtility.UrlEncode(Data.Helper.Util.Encriptar(string.Format("{0}|{1}|{2}", SessionsAdmin.EmpresaId, modeloMensagemEnvio.ExameId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                    var _urlExame = $"{AppSettings.UrlRoot}viewExame?key={key}";

                    var msgWhats = modeloMensagemEnvio.Mensagem.Replace("\n", "%0a");
                    msgWhats = msgWhats.Replace("{CLIENTE}", modeloMensagemEnvio.ClienteNome);
                    msgWhats = msgWhats.Replace("{URL_EXAME}", _urlExame);
                    msgWhats = msgWhats.Replace("{CLINICA}", _clinicaNome);
                    msgWhats = msgWhats.Replace("{USUARIO_LOGADO}", SessionsAdmin.UsuarioNome);

                    jsonResultado.Data = $"https://api.whatsapp.com/send?phone=55{modeloMensagemEnvio.ClienteTelefone}&text={msgWhats}";
                }
                //TODO: ENVIAR PARA ESTAGIOS
                jsonResultado.MessageTipo = "success";
                jsonResultado.Message = "Operação efetuada com sucesso.";
                return Json(jsonResultado);
            }
            catch (Exception ex)
            {
                jsonResultado.MessageTipo = "error";
                jsonResultado.Message = "Ocorreu um erro no sistema, favor tentar novamente.";
                return Json(jsonResultado);
            }
        }


        public JsonResult SalvarModeloMensagem(ModeloMensagemItem modeloMensagem)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(true);

            if (string.IsNullOrEmpty(modeloMensagem.Titulo))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "TituloEditModelo", Message = "Campo Obrigatório" });
            }
            if (string.IsNullOrEmpty(modeloMensagem.Mensagem))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "TextoEditModelo", Message = "Campo Obrigatório" });
            }

            if (jsonResultado.Criticas.Any())
            {
                jsonResultado.MessageTipo = "error";
                jsonResultado.Message = "Verifique as críticas em vermelho.";
                return Json(jsonResultado);
            }


            modeloMensagem.CompanyId = Helper.SessionsAdmin.EmpresaId;
            modeloMensagem.UsuarioId = Helper.SessionsAdmin.UsuarioId;
            modeloMensagem.Perfil = Helper.SessionsAdmin.PerfilNome;
            modeloMensagem.PerfilId = Helper.SessionsAdmin.PerfilId;

            _empresaService.Salvar(modeloMensagem);

            jsonResultado.MessageTipo = "success";
            jsonResultado.Message = "Operação efetuada com sucesso.";

            return Json(jsonResultado);
        }


        private Dictionary<string, object> GetFilter(int perfilId, int usuarioId, int empresaid, DateTime? dtIni,
        DateTime? dtFim,
        string filterSituacao,
        string filterCPFCNPJ,
        string filterCodigo,
        int filterLaudadorId)
        {
            Dictionary<string, object> filter = new Dictionary<string, object>();

            if (EnumAdminProfile.Clinica.GetHashCode() == perfilId)
            {
                // filter.Add("Exame.SituacaoId!=", EnumExameSituacao.Criacao.GetHashCode().ToString());

                filter.Add("Exame.ClinicaId", usuarioId);
            }


            if (EnumAdminProfile.Cliente.GetHashCode() == perfilId)
            {
                filter.Add("Exame.SituacaoId!=", EnumExameSituacao.Criacao.GetHashCode().ToString());
                filter.Add("Exame.ClienteId", usuarioId);
            }

            if (!String.IsNullOrEmpty(filterCodigo))
            {
                filter.Add("Exame.Id", filterCodigo);
            }

            if (!String.IsNullOrEmpty(filterCPFCNPJ))
            {
                filter.Add("AUC.CPFCNPJ", filterCPFCNPJ);
            }


            if (EnumAdminProfile.Laudador.GetHashCode() == perfilId)
            {
                List<string> listStatus = new List<string>();
                listStatus.Add(EnumExameSituacao.Criacao.GetHashCode().ToString());
                filter.Add("Exame.SituacaoId!=", listStatus);

                if (filterSituacao == "ARQUIVADO")
                {
                    filter.Add("Exame.ArquivadoLaudador", 1);

                }
                else
                {
                    filter.Add("Exame.ArquivadoLaudador", 0);
                    if (filterLaudadorId > 0)
                    {
                        if (!String.IsNullOrEmpty(filterSituacao))
                        {
                            filter.Add("[ORD_SITUACAO_LAUDADORID]", new List<string>() { filterSituacao, filterLaudadorId.ToString() });
                        }
                        else
                        {
                            if (filterLaudadorId > 0)
                                filter.Add("[ORD_SITUACAO_LAUDADORID_LAU]", filterLaudadorId);
                        }
                    }
                }
            }
            else
            {
                if (filterSituacao == "ARQUIVADO")
                {
                    if (EnumAdminProfile.Gerente.GetHashCode() == perfilId)
                    {
                        filter.Add("Exame.ArquivadoGerente", 1);
                    }
                    if (EnumAdminProfile.Clinica.GetHashCode() == perfilId)
                    {
                        filter.Add("Exame.ArquivadoClinica", 1);
                    }
                }
                else
                {
                    if (EnumAdminProfile.Gerente.GetHashCode() == perfilId)
                    {
                        filter.Add("Exame.ArquivadoGerente", 0);
                    }
                    if (EnumAdminProfile.Clinica.GetHashCode() == perfilId)
                    {
                        filter.Add("Exame.ArquivadoClinica", 0);
                    }

                    if (!String.IsNullOrEmpty(filterSituacao))
                    {
                        filter.Add("Exame.SituacaoId", filterSituacao);
                    }
                }
            }

            if (dtIni.HasValue && dtFim.HasValue)
            {
                List<DateTime> dates = new List<DateTime>();
                dates.Add(dtIni.Value);
                dates.Add(dtFim.Value);
                filter.Add("Exame.DataExame", dates);
            }

            filter.Add("Exame.CompanyId", empresaid);

            return filter.Count() == 0 ? null : filter;
        }

    }
}