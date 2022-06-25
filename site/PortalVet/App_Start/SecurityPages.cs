using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Data.Service;
using PortalVet.Helper;
using PortalVet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace PortalVet.App_Start
{
    public class SecurityPages : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase req = filterContext.HttpContext.Request;
            HttpResponseBase res = filterContext.HttpContext.Response;

            if (HttpContext.Current.Session == null || HttpContext.Current.Session["objUsuarioId"] == null)
            {
                if (!IsValidateCookie(HttpContext.Current))
                {

                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"area", ""},
                            {"controller", "Home"},
                            {"action", "Index"}
                        }
                    );

                }
                else
                {
                    var AreaName = filterContext.RouteData.DataTokens["area"] != null ? filterContext.RouteData.DataTokens["area"].ToString() : "";
                    var ControllerName = filterContext.RouteData.Values["controller"] != null ? filterContext.RouteData.Values["controller"].ToString() : "";
                    var ActionName = filterContext.RouteData.Values["action"] != null ? filterContext.RouteData.Values["action"].ToString() : "";
                    var idName = filterContext.RouteData.Values["id"] != null ? filterContext.RouteData.Values["id"].ToString() : "";


                    if (!String.IsNullOrEmpty(AreaName) &&
                        !String.IsNullOrEmpty(ControllerName) &&
                         !String.IsNullOrEmpty(ActionName) &&
                         String.IsNullOrEmpty(idName))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                       new RouteValueDictionary
                       {
                                {"area", AreaName},
                                {"controller", ControllerName},
                                {"action", ActionName}
                       }
                   );

                    }
                    else if (!String.IsNullOrEmpty(AreaName) &&
                      !String.IsNullOrEmpty(ControllerName) &&
                       !String.IsNullOrEmpty(ActionName) &&
                       !String.IsNullOrEmpty(idName))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                       new RouteValueDictionary
                       {
                                {"area", AreaName},
                                {"controller", ControllerName},
                                {"action", ActionName},
                                {"id", idName}
                       }
                   );

                    }
                    else if (String.IsNullOrEmpty(AreaName) &&
                     !String.IsNullOrEmpty(ControllerName) &&
                      !String.IsNullOrEmpty(ActionName) &&
                      !String.IsNullOrEmpty(idName))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                       new RouteValueDictionary
                       {
                                {"area", ""},
                                {"controller", ControllerName},
                                {"action", ActionName},
                                {"id", idName}
                       }
                   );

                    }
                    else if (String.IsNullOrEmpty(AreaName) &&
                      !String.IsNullOrEmpty(ControllerName) &&
                       !String.IsNullOrEmpty(ActionName) &&
                       String.IsNullOrEmpty(idName))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                            {
                                    {"area", ""},
                                    {"controller", ControllerName},
                                    {"action", ActionName},
                            });

                    }

                    else
                    {

                        filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary
                                {
                                                    {"area", ""},
                                                    {"controller", "Dashboard"},
                                                    {"action", "Index"}
                                }
                            );
                    }
                }
            }
            else
            {
                if (!IsValidateCookie(HttpContext.Current))
                {

                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"area", ""},
                            {"controller", "Home"},
                            {"action", "Index"}
                        }
                    );

                }
                else
                {

                    var AreaName = filterContext.RouteData.DataTokens["area"] != null ? filterContext.RouteData.DataTokens["area"].ToString() : "";
                    var ControllerName = filterContext.RouteData.Values["controller"] != null ? filterContext.RouteData.Values["controller"].ToString() : "";
                    var ActionName = filterContext.RouteData.Values["action"] != null ? filterContext.RouteData.Values["action"].ToString() : "";

                    if (!ControllerName.ToLower().Equals("chamados") && !ControllerName.ToLower().Equals("gerarconvite"))
                    {
                        if (SessionsAdmin.Acessos != null)
                        {
                            List<AdminAcesso> acessos = SessionsAdmin.Acessos;

                            if (String.IsNullOrEmpty(AreaName) || ControllerName.Equals("dashboard", StringComparison.InvariantCultureIgnoreCase))
                            {
                                // NIVEL SEM AREA - SOMENTE DASHBOARD 
                                bool exist = acessos.Any(x =>
                                  !String.IsNullOrEmpty(x.Controller) && x.Controller.Equals(ControllerName, StringComparison.InvariantCultureIgnoreCase) &&
                                  !String.IsNullOrEmpty(x.Pagina) && x.Pagina.Equals(ActionName, StringComparison.InvariantCultureIgnoreCase)
                              );

                                if (!exist)
                                {
                                    HttpContext.Current.Response.Redirect("~/Ops");
                                }
                                //else
                                //{
                                //    ModuleName = "Dashboard";
                                //}
                            }
                            else
                            {
                                // NIVEL AREA
                                bool exist = acessos.Any(x =>
                                    !String.IsNullOrEmpty(x.Area) && x.Area.Equals(AreaName, StringComparison.InvariantCultureIgnoreCase) &&
                                    !String.IsNullOrEmpty(x.Controller) && x.Controller.Equals(ControllerName, StringComparison.InvariantCultureIgnoreCase) &&
                                    !String.IsNullOrEmpty(x.Pagina) && x.Pagina.Equals(ActionName, StringComparison.InvariantCultureIgnoreCase)
                                    && x.Ativo
                                );

                                var listAcesso = UtilAdmin.PopulateMenu(filterContext.Controller.ControllerContext.RouteData, acessos, out string an, out string cn);
                                var validacao01 = listAcesso.Any(e => e.SubMenus.Any(t => t.AreaName.ToUpper() == AreaName.ToUpper() && t.ControllerName == ControllerName && t.Active));
                                var validacao02 = listAcesso.Any(e => (e.AreaName == null || e.AreaName == "") && e.ControllerName == ControllerName && e.Active);
                                var validacao03 = listAcesso.Any(e => (e.AreaName == null || e.AreaName == "") &&
                                 e.SubMenus.Any(t => (t.AreaName == null || t.AreaName == "") && t.SubMenus.Any(u => u.AreaName.ToUpper() == AreaName.ToUpper() && u.ControllerName == ControllerName && u.Active)));
                                var validacao04 = listAcesso.Any(r => r.SubMenus.Any(e => e.SubMenus.Any(t => t.AreaName.ToUpper() == AreaName.ToUpper() && t.ControllerName == ControllerName && t.Active)));

                                if (validacao01 == false && validacao02 == false && validacao03 == false && validacao04 == false)
                                {
                                    HttpContext.Current.Response.Redirect("~/Ops");
                                }
                                else
                                {
                                    var item = acessos.FirstOrDefault(x =>
                                     !String.IsNullOrEmpty(x.Area) && x.Area.Equals(AreaName, StringComparison.InvariantCultureIgnoreCase) &&
                                     !String.IsNullOrEmpty(x.Controller) && x.Controller.Equals(ControllerName, StringComparison.InvariantCultureIgnoreCase) &&
                                     !String.IsNullOrEmpty(x.Pagina) && x.Pagina.Equals(ActionName, StringComparison.InvariantCultureIgnoreCase)
                                );

                                    // ModuleName = item.Modulo;
                                    //ModuleTitle = item.Nome;
                                    //ModuleParentTitle = item.ParentNome;
                                    //IconeCss = item.IconeCss;
                                }
                            }
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect("~/Home/Index");
                        }
                    }

                }

            }

            base.OnActionExecuting(filterContext);
        }



        private bool IsValidateCookie(HttpContext httpContext)
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
                            SessionsAdmin.ForceId = "";// httpContext.Session.Add("objForceId", "");
                        else
                            SessionsAdmin.ForceId = getCookie.Values["ForceId"].ToString();// httpContext.Session.Add("objForceId", getCookie.Values["ForceId"].ToString());


                        if (getCookie.Values["CurrentProfileId"] == null || String.IsNullOrEmpty(getCookie.Values["CurrentProfileId"].ToString()))
                            SessionsAdmin.CurrentProfileId = 0;
                        else
                        {
                            if (SessionsAdmin.CurrentProfileId != Int32.Parse(getCookie.Values["CurrentProfileId"].ToString()))
                            {
                                SessionsAdmin.PerfilId = SessionsAdmin.CurrentProfileId;
                            }
                        }

                        SessionsAdmin.UsuarioId = user.Id;
                        SessionsAdmin.UsuarioNome = user.Nome;
                        SessionsAdmin.UsuarioPicture = user.Imagem;
                        SessionsAdmin.UsuarioEmail = user.Email;

                        var listProfileId = new AdminUserService().GetProfiles(user.Id);

                        var perfilId = SessionsAdmin.PerfilId;
                        if (!listProfileId.Any(t=>t == SessionsAdmin.PerfilId))
                        {
                            perfilId = listProfileId.FirstOrDefault(); 
                        }
                        
                        if (SessionsAdmin.CurrentProfileId > 0)
                        {
                            var pAuxId = listProfileId.Where(t => t == SessionsAdmin.CurrentProfileId).FirstOrDefault();
                            if (pAuxId > 0)
                                perfilId = pAuxId;
                        }
                        //SessionsAdmin.EmpresaId = user.Empresas.Any() ? user.Empresas.FirstOrDefault().Id : 0;
                        SessionsAdmin.PerfilId = perfilId;
                        SessionsAdmin.PerfilNome = Enumeradores.GetDescription((EnumAdminProfile)perfilId);
                        SessionsAdmin.ListProfileId = new List<int>() { perfilId };// listProfileId;

                        var _empresaId = 0;
                        var _empresalogoadmin = "";
                        var _empresalogo = "";
                        var listEmpresa = new List<SelectListWeb>();
                        if (user.Empresas != null && user.Empresas.Any())
                        {
                            if (SessionsAdmin.EmpresaId == 0 || !user.Empresas.Any(r => r.Id == SessionsAdmin.EmpresaId))
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
                        var logo = !String.IsNullOrEmpty(_empresalogoadmin) ? Util.GetUrlUpload() + "empresas/" + _empresalogoadmin : (!String.IsNullOrEmpty(_empresalogo) ? Util.GetUrlUpload() + "empresas/" + _empresalogo : "~/Content/img/logo.png");
                        SessionsAdmin.EmpresaLogo = logo;
                        //SessionsAdmin.EmpresaId = _empresaId;
                        SessionsAdmin.Acessos = UtilAdmin.GetAcesso(perfilId, _empresaId);
                        //SessionsAdmin.EmpresaJson = UtilAdmin.GetEmpresaJson(_empresaId);

                        return true;

                    }



                }
            }
            return false;
        }
    }
}