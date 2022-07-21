using PortalVet.App_Start;
using PortalVet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Web;
using PortalVet.Data.Interface;
using PortalVet.Helper;
using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using System.Web.Security;

namespace PortalVet.Controllers
{
    [RequireHttps]
    public class AdminController : BaseController
    {

        private readonly IAdminUserService _adminUserService;
        private readonly IEmpresaService _empresaService;
        //private readonly IClienteService _clienteService;

        public AdminController(IAdminUserService adminUserService, IEmpresaService empresaService)//, IClienteService clienteService)
        {
            _adminUserService = adminUserService ??
                throw new ArgumentNullException(nameof(adminUserService));

            _empresaService = empresaService ??
              throw new ArgumentNullException(nameof(empresaService));


            //_clienteService = clienteService ??
            // throw new ArgumentNullException(nameof(clienteService));
        }


        // GET: Admin
        
        public ActionResult Index()
        {
            //var empresa = new Data.Service.EmpresaService().CarragaPorSubdominio(Request.Url.Host);
            ViewBag.erroMensagem = "";
            ViewBag.erroEmail = false;
            ViewBag.erroPassword = false;
            ViewBag.erroTelefone = false;
            return View();
        }


        [WebGet(UriTemplate = "JsonLogin", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult JsonLogin(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(true);
            //var opcao = "";
            //if (!string.IsNullOrEmpty(form["opcao"]))
            //{
            //    opcao = form["opcao"].ToString();
            //}

            //if (opcao == "CPF_CNPJ")
            //{
            //    if (string.IsNullOrEmpty(form["txtCPFCNPJ"]))
            //    {
            //        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtCPFCNPJ", Message = "Informe um CPF ou CNPJ." });
            //    }
            //}
            //else if (opcao == "TELEFONE")
            //{
            //    if (string.IsNullOrEmpty(form["txtTelefone"]))
            //    {
            //        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtTelefone", Message = "Informe um CPF ou CNPJ." });
            //    }
            //}
            //else
            //{
            if (string.IsNullOrEmpty(form["txtEmail"]))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtEmail", Message = "Informe uma senha." });
            }
            if (string.IsNullOrEmpty(form["txtPassword"]))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtPassword", Message = "Informe uma senha." });
            }
            //}


            if (!jsonResultado.Criticas.Any())
            {
                var usuario = new AdminUserItem();
                //if (opcao == "CPF_CNPJ")
                //{
                //    var cpf_cnpj = form["txtCPFCNPJ"].ToString();
                //    usuario = _adminUserService.VerificaAdminUser(cpf_cnpj);
                //}
                //else if (opcao == "TELEFONE")
                //{
                //    var telefone = form["txtTelefone"].ToString();
                //    usuario = _adminUserService.VerificaAdminUserTelefone(telefone);
                //}
                //else
                //{
                var email = form["txtEmail"].ToString();
                var senha = form["txtPassword"].ToString();
                usuario = _adminUserService.VerificaAdminUser(email, senha);
                //}
                if (usuario != null && usuario.Id > 0)
                {
                    PreencheCredenciais(usuario);
                    jsonResultado.Data = new { UrlRedirect = Url.Action("Index", "Dashboard", new { area = "" }) };
                }
                else
                {
                    SessionsAdmin.UsuarioId = 0;
                    SessionsAdmin.UsuarioEmail = string.Empty;
                    SessionsAdmin.UsuarioNome = string.Empty;
                    //SessionsAdmin.EmpresaId = 0;
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MESSAGE", Message = "Não foi possível realizar o login." });

                }
            }

            return new LargeJsonResult { Data = jsonResultado };
        }

        public ActionResult ForceLogin(string pid, string id)
        {
            var user = _adminUserService.GetById(Int32.Parse(id));
            if (user.Id > 0)
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);


                if (Int32.Parse(pid) == user.Id)
                    SessionsAdmin.ForceId = "";
                else
                    SessionsAdmin.ForceId = pid;


                SessionsAdmin.UsuarioId = user.Id;
                SessionsAdmin.UsuarioNome = user.Nome;
                SessionsAdmin.UsuarioPicture = user.Imagem;
                SessionsAdmin.UsuarioEmail = user.Email;

                var listProfileId = _adminUserService.ListProfiles(user.Id).Select(r => r.Id).ToList();
                var perfilId = listProfileId.FirstOrDefault();

                if (SessionsAdmin.CurrentProfileId > 0)
                {
                    var pAuxId = listProfileId.Where(t => t == SessionsAdmin.CurrentProfileId).FirstOrDefault();
                    if (pAuxId > 0)
                        perfilId = pAuxId;
                }

                //SessionsAdmin.EmpresaId = user.Empresas.Any() ? user.Empresas.FirstOrDefault().Id : 0;
                SessionsAdmin.PerfilId = perfilId;
                SessionsAdmin.PerfilNome = Enumeradores.GetDescription((EnumAdminProfile)perfilId);
                SessionsAdmin.ListProfileId = new List<int>() { perfilId };//listProfileId;

                var _empresaId = 0;
                var _empresalogoadmin = "";
                var _empresalogo = "";
                var listEmpresa = new List<SelectListWeb>();
                if (user.Empresas != null && user.Empresas.Any())
                {
                    var first = user.Empresas.FirstOrDefault();
                    _empresaId = first.Id;
                    _empresalogoadmin = first.Imagem;
                    _empresalogo = first.Imagem;
                    user.Empresas.ForEach(r =>
                    {
                        listEmpresa.Add(new SelectListWeb()
                        {
                            Value = r.Id.ToString(),
                            Text = r.Nome                            
                        });
                    });
                }

                SessionsAdmin.ListEmpresa = listEmpresa;
                _empresalogoadmin = perfilId == EnumAdminProfile.Administrador.GetHashCode() ? string.Empty : _empresalogoadmin;
                _empresalogo = perfilId == EnumAdminProfile.Administrador.GetHashCode() ? string.Empty : _empresalogo;
                var logo = !String.IsNullOrEmpty(_empresalogoadmin) ? Util.GetUrlUpload() + "empresas/" + _empresalogoadmin : (!String.IsNullOrEmpty(_empresalogo) ? Util.GetUrlUpload() + "empresas/" + _empresalogo : "~/Content/img/logo.png");
                SessionsAdmin.EmpresaLogo = logo;
                ///SessionsAdmin.EmpresaId = _empresaId;
                SessionsAdmin.Acessos = UtilAdmin.GetAcesso(perfilId, _empresaId);
                //SessionsAdmin.EmpresaJson = UtilAdmin.GetEmpresaJson(_empresaId);

                // Add Cookie
                HttpCookie myCookie = new HttpCookie("admCookie");
                HttpContext.Response.Cookies.Remove("admCookie");
                HttpContext.Response.Cookies.Add(myCookie);

                if (Int32.Parse(pid) == user.Id)
                    myCookie.Values.Add("ForceId", "");
                else
                    myCookie.Values.Add("ForceId", pid);

                myCookie.Values.Add("UsuarioId", user.Id.ToString());
                myCookie.Values.Add("Logotipo", logo);
                myCookie.Values.Add("EmpresaId", _empresaId.ToString());
                myCookie.Values.Add("Nome", user.Nome);
                myCookie.Values.Add("Foto", user.Imagem);
                myCookie.Values.Add("Email", user.Email);
                myCookie.Values.Add("CurrentProfileId", perfilId.ToString());
                myCookie.Values.Add("PerfilNome", SessionsAdmin.PerfilNome);
                DateTime dtxpiry = DateTime.Now.AddDays(this.tempoExpirarCookie);
                myCookie.Expires = dtxpiry;


                if (SessionsAdmin.PerfilId == EnumAdminProfile.Administrador.GetHashCode())
                    return RedirectToAction("User", "Administracao");
                else
                    return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        public ActionResult Logout()
        {
            var empresaid = SessionsAdmin.EmpresaId;
            Response.Cookies.Remove("admCookie");
            HttpCookie myCookie = new HttpCookie("admCookie");
            myCookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(myCookie);
            Session.Clear();
            FormsAuthentication.SignOut();

            return Redirect(Url.Action("Index", "Admin", new { id = empresaid }));
        }

        public void PreencheCredenciais(AdminUserItem user)
        {
            SessionsAdmin.UsuarioId = user.Id;
            SessionsAdmin.UsuarioEmail = user.Email;
            SessionsAdmin.UsuarioNome = user.Nome;
            //SessionsAdmin.EmpresaId = user.Empresas.Any() ? user.Empresas.FirstOrDefault().Id : 0;
            if (SessionsAdmin.EmpresaId > 0)
            {
                var emp = _empresaService.Carregar(SessionsAdmin.EmpresaId);
                SessionsAdmin.NomeEmpresa = emp.Nome;
                SessionsAdmin.EmpresaLogo = emp.Imagem;
            }
            else
            {
                SessionsAdmin.NomeEmpresa = "";
                SessionsAdmin.EmpresaLogo = "";
            }
            SessionsAdmin.ListProfileId = user.Perfis.Select(r => r.Id).ToList();

            var perfilId = user.Perfis.FirstOrDefault().Id;
            if (perfilId > 0)
                SessionsAdmin.Acessos = UtilAdmin.GetAcesso(perfilId, SessionsAdmin.EmpresaId);

            HttpCookie myCookie = new HttpCookie("admCookie");
            HttpContext.Response.Cookies.Remove("admCookie");
            HttpContext.Response.Cookies.Add(myCookie);
            myCookie.Values.Add("ForceId", "");
            myCookie.Values.Add("UsuarioId", SessionsAdmin.UsuarioId.ToString());
            myCookie.Values.Add("EmpresaId", SessionsAdmin.EmpresaId.ToString());
            myCookie.Values.Add("Nome", SessionsAdmin.UsuarioNome);
            myCookie.Values.Add("Email", SessionsAdmin.UsuarioEmail);
            DateTime dtxpiry = DateTime.Now.AddDays(15);
            myCookie.Expires = dtxpiry;

        }

        public ActionResult ResetPassword()
        {
            return View();
        }


        [WebGet(UriTemplate = "JsonResetPassword", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult JsonResetPassword(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(false);
            var email = "";
            bool valido = false;
            if (form["txtEmail"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtEmail", Message = "Campo obrigatório." });
            }
            else
            {
                if (!Data.Helper.Util.IsEmail(form["txtEmail"].ToString()))
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtEmail", Message = "Campo incorreto." });
                }
            }


            if (!jsonResultado.Criticas.Any())
            {
                email = form["txtEmail"].Trim();
                var message = "";


                var user = _adminUserService.GetByEmail(email);
                if (user.Id > 0)
                {
                    var empresaId = 0;
                    if (user.Empresas.Any())
                    {
                        empresaId = user.Empresas.FirstOrDefault().Id;
                        var emp = _empresaService.Carregar(empresaId);
                        EnviarEmail(user, emp);
                    }
                    else
                    {
                        EnviarEmail(user, null);
                    }

                    valido = true;
                    message = "Solicitação enviada com sucesso.";
                }

                if (valido == false)
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtEmail", Message = "Campo obrigatório." });
                    message = "E-mail não encontrado";
                }

                jsonResultado.Message = message;

            }
            jsonResultado.Data = valido;
            return new LargeJsonResult { Data = jsonResultado };

        }


        public void EnviarEmail(AdminUserItem usuario, EmpresaItem empresa)
        {
            var body = Util.GetResourcesNovaSenha();
            body = body.Replace("#NOME#", usuario.Nome);
            var key = HttpUtility.UrlEncode(Data.Helper.Util.Encriptar(string.Format("{0}|{1}", usuario.Email, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
            body = body.Replace("#LINK#", $"{System.Configuration.ConfigurationManager.AppSettings["UrlRootAdmin"].ToString()}newpassword?key={key}");
            var bodyMaster = Util.CarregaConteudoMaster(empresa, body);
            Util.SendEmail("Recuperação de senha", usuario.Email, bodyMaster);
        }

        public ActionResult NewPassword(string key = "")
        {
            ViewData["Status"] = "";
            ViewData["Mensagem"] = "";
            ViewData["hdnEmail"] = "";
            if (!String.IsNullOrEmpty(key))
            {
                var parametros = Data.Helper.Util.Descriptar(HttpUtility.UrlDecode(key));
                string[] valores = parametros.Split('|');
                var email = valores[0]; // E-MAIL
                ViewData["hdnEmail"] = email;

                var user = _adminUserService.GetByEmail(email);
                if (user.Id == 0)
                {
                    ViewData["Status"] = "Erro";
                    ViewData["Mensagem"] = "Solicitação incorreta!";
                    return View();
                }

                var data = valores[1]; // DATA
                DateTime.TryParse(data, out DateTime dtEnvio);

                if (dtEnvio.AddHours(12).CompareTo(DateTime.Now) < 1)
                {
                    ViewData["Status"] = "Expirou";
                    ViewData["Mensagem"] = "Solicitação expirou!";
                    return View();
                }
            }
            return View();
        }


        [WebGet(UriTemplate = "JsonNewPassword", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult JsonNewPassword(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(false);
            var email = "";
            var mensagemErro = "";
            if (form["hdnEmail"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "hdnEmail", Message = "Campo obrigatório." });
            }

            if (form["txtPassword"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtPassword", Message = "Campo obrigatório." });
                mensagemErro = "Campo obrigatório.";
            }

            if (form["txtPasswordC"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtPasswordC", Message = "Campo obrigatório." });
                mensagemErro = "Campo obrigatório.";
            }


            if (!form["txtPassword"].Trim().Length.Equals(0) && !form["txtPasswordC"].Trim().Length.Equals(0))
            {
                if (form["txtPassword"].Trim() != form["txtPasswordC"].Trim())
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtPassword", Message = "Campo obrigatório." });
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtPasswordC", Message = "Campo obrigatório." });
                    mensagemErro = "Senhas incorretas!";
                }
            }

            if (!jsonResultado.Criticas.Any())
            {
                email = form["hdnEmail"].ToString().Trim();
                var password = form["txtPassword"].ToString().Trim();
                var message = "";
                bool valido = false;

                var isOk = _adminUserService.UpdatePasswordUser(email, password);
                if (isOk)
                {
                    valido = true;
                    message = "Solicitação enviada com sucesso.";
                }
                else
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "lblEmail", Message = "Campo obrigatório." });
                    message = "E-mail não encontrado";
                }

                jsonResultado.Message = message;
                jsonResultado.Data = valido;
            }
            else
            {
                jsonResultado.Message = mensagemErro;
            }
            return new LargeJsonResult { Data = jsonResultado };

        }

    }
}