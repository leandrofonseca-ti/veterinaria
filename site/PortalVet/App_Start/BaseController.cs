using Microsoft.AspNet.SignalR;
using PortalVet.Data.Helper;
using PortalVet.Data.Service;
using PortalVet.Helper;
using PortalVet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PortalVet.App_Start
{
    [RequireSSL]
    public class BaseController : Controller
    {
        public int tempoExpirarCookie = 15;
        public readonly IHubContext _hubContext = GlobalHost.ConnectionManager.GetHubContext<HubUsuarios>();
        public string rootUrl = string.Empty;
        public bool IsAuthenticated
        {
            get
            {
                if (SessionsAdmin.UsuarioId > 0)
                {
                    return true;
                }
                else
                {
                    if (IsValidateCookie(this.HttpContext))
                    {
                        if (SessionsAdmin.UsuarioId > 0)
                        {
                            return true;
                        }
                    }

                }
                return false;
            }
        }



        private bool IsValidateCookie(System.Web.HttpContextBase httpContext)
        {
            try
            {
                HttpCookie getCookie = httpContext.Request.Cookies.Get("admCookie");

                if (getCookie != null && getCookie.Values["Email"] != null)
                {
                    string email = getCookie.Values["Email"].ToString();

                    if (String.IsNullOrEmpty(email))
                    {
                        return false;
                    }
                    else
                    {

                        var user = new AdminUserService().GetById(Int32.Parse(getCookie.Values["UsuarioId"].ToString()));
                        if (user.Id > 0)
                        {
                            FormsAuthentication.SetAuthCookie(user.Email, false);


                            if (String.IsNullOrEmpty(getCookie.Values["ForceId"].ToString()))
                                httpContext.Session.Add("objForceId", "");
                            else
                                httpContext.Session.Add("objForceId", getCookie.Values["ForceId"].ToString());

                            SessionsAdmin.UsuarioId = user.Id;
                            SessionsAdmin.UsuarioNome = user.Nome;
                            SessionsAdmin.UsuarioPicture = user.Imagem;
                            SessionsAdmin.UsuarioEmail = user.Email;
                            var listProfileId = new AdminUserService().GetProfiles(user.Id);
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
                            SessionsAdmin.ListProfileId = new List<int>() { perfilId }; 

                            var _empresaId = 0;
                            var _empresalogoadmin = "";
                            var _empresalogo = "";
                            var listEmpresa = new List<SelectListWeb>();
                            if (user.Empresas != null && user.Empresas.Any())
                            {
                                if (SessionsAdmin.EmpresaId == 0)
                                {
                                    var first = user.Empresas.FirstOrDefault();
                                    _empresaId = first.Id;
                                    _empresalogoadmin = first.Imagem;
                                    _empresalogo = first.Imagem;
                                }
                                else
                                {
                                    var first = user.Empresas.Where(r => r.Id == SessionsAdmin.EmpresaId).FirstOrDefault();
                                    _empresaId = first.Id;
                                    _empresalogoadmin = first.Imagem;
                                    _empresalogo = first.Imagem;
                                }
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

                            var logo = !String.IsNullOrEmpty(_empresalogoadmin) ? UtilAdmin.GetUrlUpload() + "empresas/" + _empresalogoadmin : (!String.IsNullOrEmpty(_empresalogo) ? Helper.UtilAdmin.GetUrlUpload() + "empresas/" + _empresalogo : "~/Content/img/logo.png");
                            SessionsAdmin.EmpresaLogo = logo;
                            //SessionsAdmin.EmpresaId = _empresaId;
                            SessionsAdmin.Acessos = UtilAdmin.GetAcesso(perfilId, _empresaId);
                            //SessionsAdmin.EmpresaJson = UtilAdmin.GetEmpresaJson(_empresaId);
                            return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            ViewData["rootUrl"] = "";
            ViewData["rootUrlDominio"] = Helper.AppSettings.UrlRoot;

            var context = requestContext.HttpContext;

            if (context != null)
            {
                if (context.Request.IsLocal)
                {
                    rootUrl = string.Format("{0}://{1}{2}{3}",
                                               "https",
                                               context.Request.Url.Host,
                                               context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port,
                                               context.Request.ApplicationPath);

                    rootUrl = Helper.AppSettings.UrlRoot;
                }
                else
                {
                    rootUrl = string.Format("{0}://{1}{2}{3}",
                                           "https",
                                           context.Request.Url.Host,
                                           context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port,
                                           context.Request.ApplicationPath);
                }
                if (!rootUrl.EndsWith("/"))
                {
                    rootUrl += "/";
                }

                ViewData["rootUrl"] = rootUrl;
            }

            base.Initialize(requestContext);
        }



        public JsonResult VerificaSessao(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(true);

            if (SessionsAdmin.EmpresaId == 0 || SessionsAdmin.UsuarioId == 0)
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "SessionTimeOut", Message = "Required" });
            }

            return Json(jsonResultado);
        }
    }
}