
using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Data.Service;
using System.Collections.Generic;
using System.Web.Routing;
using System.Linq;
using System;
using System.Web;
using System.Web.Mvc;

namespace PortalVet.Helper
{
    public static class UtilAdmin
    {
        public static string GetUrlUpload()
        {
            return System.Configuration.ConfigurationManager.AppSettings["UrlRootUpload"].ToString();
        }


        public static List<AdminAcesso> GetAcesso(int perfilId, int empresaId)
        {
            List<AdminAcesso> obj = new AdminUserService().GetAllByName(perfilId, empresaId);
            return obj;
        }



        public static List<VMMenu> PopulateMenu(RouteData obj, List<AdminAcesso> acessos, out string areaNome, out string controllerNome)
        {
            areaNome = "";
            controllerNome = "";
            string AreaName = obj.DataTokens["area"] != null ? obj.DataTokens["area"].ToString() : "";
            string ControllerName = obj.Values["controller"] != null ? obj.Values["controller"].ToString() : "";
            string ActionName = obj.Values["action"] != null ? obj.Values["action"].ToString() : "";


            List<VMMenu> listParent = new List<VMMenu>();

            if (acessos.Any())
            {
                VMMenu itemsUnique = (from context
                            in acessos
                                      where
                                      (context.Controller != null && context.Controller.Equals(ControllerName, StringComparison.OrdinalIgnoreCase)) &&
                                      (context.Pagina != null && context.Pagina.Equals(ActionName, StringComparison.OrdinalIgnoreCase))
                                      select new VMMenu() { Name = context.Nome, AreaName = context.Area, ControllerName = context.Controller }).FirstOrDefault();

                areaNome = itemsUnique != null ? itemsUnique.AreaName : string.Empty;
                controllerNome = itemsUnique != null ? itemsUnique.ControllerName : string.Empty;

                List<VMMenu> listParentTEMP = (from context
                                      in acessos
                                               where context.ParentId.Equals(0) &&
                                               context.Chave.Equals("VER_MENU")
                                               select new VMMenu() { MenuId = context.MenuId.Value, Name = context.Nome, Page = context.Path, FontIcon = context.IconeCss, AreaName = context.Area, ControllerName = context.Controller, ActionName = context.Action }).Distinct().ToList();

                foreach (VMMenu item in listParentTEMP)
                {
                    List<VMMenu> subMenus = RecursiveMenus(item, acessos, AreaName, ControllerName, out bool selected);

                    subMenus.ForEach(e =>
                    {
                        List<AdminAcesso> acc = acessos.Where(r => r.Controller == e.ControllerName && r.Area == e.AreaName).ToList();
                        e.Acessos = acc;
                    });

                    listParent.Add(new VMMenu()
                    {
                        MenuId = item.MenuId,
                        Selected = selected,
                        SubMenus = subMenus,
                        Name = item.Name,
                        Page = item.Page,
                        AreaName = item.AreaName,
                        ControllerName = item.ControllerName,
                        ActionName = item.ActionName,
                        FontIcon = item.FontIcon
                    });
                }

            }
            return listParent;
        }

        public static List<VMMenu> RecursiveMenus(VMMenu item, List<AdminAcesso> acessos, string AreaName, string ControllerName, out bool selected)
        {
            List<VMMenu> subMenus = (from context
                               in acessos
                                     where context.Ativo == true && context.ParentId.Equals(item.MenuId) &&
                                     context.Chave.Equals("VER_MENU")
                                     select new VMMenu() { MenuId = context.MenuId, FontIcon = context.IconeCss, Page = context.Path, Name = context.Nome, AreaName = context.Area, ControllerName = context.Controller, ActionName = context.Action }).Distinct().ToList();

            selected = false;
            if (string.IsNullOrEmpty(AreaName) && string.IsNullOrEmpty(item.AreaName))
            {
                if (item.ControllerName != null && item.ControllerName.Equals(ControllerName, StringComparison.InvariantCultureIgnoreCase))
                {
                    selected = true;
                }
            }
            else
            {

                if (subMenus.Exists(x => x.AreaName != null && x.AreaName.Equals(AreaName, StringComparison.InvariantCultureIgnoreCase) &&
                             x.ControllerName != null && x.ControllerName.Equals(ControllerName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    selected = true;

                    int index = subMenus.FindIndex(x => x.AreaName.Equals(AreaName, StringComparison.InvariantCultureIgnoreCase) &&
                                x.ControllerName.Equals(ControllerName, StringComparison.InvariantCultureIgnoreCase));
                    subMenus[index].Selected = true;

                }
            }

            foreach (VMMenu w in subMenus)
            {
                if (w.MenuId.HasValue)// && IsParentMenu(w.MenuId.Value))
                {
                    w.SubMenus = RecursiveMenus(w, acessos, AreaName, ControllerName, out bool select2);
                    if (selected == false)
                    {
                        selected = select2;
                        w.Selected = select2;
                    }
                }
            }

            return subMenus;
        }


        public static VMHome GetModelHome(RouteData routeData, HttpSessionStateBase session, UrlHelper url)
        {
            VMHome home = new VMHome();
            List<AdminAcesso> acessos = new List<AdminAcesso>();
            string areaName = "";
            string controllerName = "";
            if (SessionsAdmin.Acessos != null)
            {
                acessos = SessionsAdmin.Acessos;
                home.Menus = PopulateMenu(routeData, acessos, out areaName, out controllerName);
            }

            string picture = SessionsAdmin.UsuarioPicture;
            string foto = string.IsNullOrEmpty(picture) ? url.Content("~/Content/img/avatar-1-64.png") : url.Content(string.Concat(Util.GetUrlUpload(), "user/", picture));
            home.UrlImageUser = foto;

            string usuarioEmail = SessionsAdmin.UsuarioEmail;


            string primeiroNome = SessionsAdmin.UsuarioNome;
            primeiroNome = primeiroNome.Trim();
            string[] arrayNome = primeiroNome.Split(' ');
            if (arrayNome.Length > 1)
            {
                primeiroNome = arrayNome[0];
            }
            home.FirstNameTop = HttpUtility.HtmlEncode(string.Format("Olá, {0}", primeiroNome));// ;
            home.FirstName = primeiroNome;
            home.UsuarioEmail = usuarioEmail.Trim();
            home.ProfileName = SessionsAdmin.PerfilNome;
            home.ProfileId = SessionsAdmin.PerfilId;

            if (SessionsAdmin.PerfilId == EnumAdminProfile.Administrador.GetHashCode())
            {
                home.EmpresaId = 0;
            }
            else
            {
                home.EmpresaId = SessionsAdmin.EmpresaId;
            }
 

            home.ListProfileId = SessionsAdmin.ListProfileId;
            home.ListEmpresa = SessionsAdmin.ListEmpresa;
            home.UserId = SessionsAdmin.UsuarioId;

            home.EmpresaLogo = SessionsAdmin.EmpresaLogo;
             

            string AreaName = routeData.DataTokens["area"] != null ? routeData.DataTokens["area"].ToString() : "";
            string ControllerName = routeData.Values["controller"] != null ? routeData.Values["controller"].ToString() : "";

            home.AreaNameUrl = AreaName;
            home.ControllerNameUrl = ControllerName;

            home.AreaName = areaName;
            home.ControllerName = controllerName;

         
            if (SessionsAdmin.Acessos != null)
            {
                acessos = SessionsAdmin.Acessos;
                List<AdminAcesso> acessosPage = acessos.Where(t => (t.Area != null && t.Area.ToUpper() == AreaName.ToUpper()) && t.Controller == ControllerName).ToList();

                home.podeCadastrar = acessosPage.Any(t => t.Chave == "CADASTRAR");

                home.podeAtualizar = acessosPage.Any(t => t.Chave == "ATUALIZAR");

                home.podeExcluir = acessosPage.Any(t => t.Chave == "EXCLUIR");

                home.podeVerMenu = acessosPage.Any(t => t.Chave == "VER_MENU");

                AdminAcesso itemSelected = acessosPage.FirstOrDefault();
                if (itemSelected != null)
                {
                    List<int> itemsSelected = acessosPage.Where(y => y.MenuId.HasValue).Select(t => t.MenuId.Value).ToList();

                    home.MenuIdSelected = itemsSelected;
                    home.MenuNameSelected = itemSelected.Nome;
                }
            }
            //home.MapPath = Util.GetMapUpload(HttpContext.Current.Server.MapPath("~/"));
            /*
            home.UrlPathUpload = Util.GetUrlUpload();
            if (!String.IsNullOrEmpty(SessionsAdmin.EmpresaJson) && home.EmpresaId > 0)
            {
                //List<MarketPlaceProdutoItem> apps = null;
                EmpresaModel empresa = GetEmpresaObj(SessionsAdmin.EmpresaJson);
                if (empresa != null)
                {
                    home.Empresa = empresa;
                    home.Empresa2 = new Data.Service.EmpresaService().Carregar(new EmpresaItem() { Id = home.Empresa.Id });

                    // apps = new MarketPlaceService().ListProdutos(empresa.Id);
                }

                List<CustomizacaoMenuSetupItem> listCustomMenu = new List<CustomizacaoMenuSetupItem>();
                List<CustomizacaoPerfilMenuItem> menusPermitidos = null;
                if (home.Empresa.Id > 0)
                {
                    listCustomMenu = new CustomizacaoService().CarregarMenuSetup(empresa.Id);
                    listCustomMenu = listCustomMenu.Where(r => r.VALOR != "" && r.VALOR != null).ToList();


                    home.Menus.ForEach(e =>
                    {

                        if (e.MenuId.HasValue)
                        {
                            TratamentoItemMenu(e.MenuId.Value, e.Name, home.Empresa, listCustomMenu);
                        }

                        if (e.SubMenus.Any())
                        {
                            e.SubMenus.ForEach(g =>
                            {
                                if (g.MenuId.HasValue)
                                {
                                    TratamentoItemMenu(g.MenuId.Value, g.Name, home.Empresa, listCustomMenu);
                                }

                                if (g.SubMenus.Any())
                                {
                                    g.SubMenus.ForEach(h =>
                                    {
                                        if (h.MenuId.HasValue)
                                        {
                                            TratamentoItemMenu(h.MenuId.Value, h.Name, home.Empresa, listCustomMenu);
                                        }


                                    });
                                }
                            });
                        }
                    });


                    // VERIFICA TRATAMENTO DE ACESSO ../Cadastro/Consultor/Save/
                    //if (home.ProfileId == EnumAdminProfile.Imobiliaria.GetHashCode() ||
                    //    home.ProfileId == EnumAdminProfile.Consultor.GetHashCode() ||
                    //    home.ProfileId == EnumAdminProfile.ConsultorTotal.GetHashCode())
                    //{
                    //    int perfilid = new CustomizacaoService().CarregarPerfil($"{empresa.Id}_{home.UserId}");
                    //    if (perfilid > 0)
                    //    {
                    //        menusPermitidos = new CustomizacaoService().CarregarMenuLogado(empresa.Id, home.UserId);


                    //        // HOTFIX: FC ANALISE - Forçar Ativo SEMPRE
                    //        if (!menusPermitidos.Any(t => t.MENUID == 122))
                    //        {
                    //            menusPermitidos.Add(new CustomizacaoPerfilMenuItem()
                    //            {
                    //                EMPRESAID = empresa.Id,
                    //                MENUID = 122,
                    //            });
                    //        }
                    //    }
                    //}
                }

                // VERIFICA TRATAMENTO DE ACESSO ../Cadastro/Consultor/Save/
                //  if (home.ProfileId == EnumAdminProfile.Consultor.GetHashCode() ||
                //   home.ProfileId == EnumAdminProfile.ConsultorTotal.GetHashCode())
                //  {
                Util.TratamentoMenus((EnumAdminProfile)home.ProfileId, home.Menus, home.Empresa, listCustomMenu, menusPermitidos);
                //}
                if (home.Empresa.Id > 0)
                {
                    CustomizacaoMenuSetupItem itemCustom = listCustomMenu.FirstOrDefault(r => home.MenuIdSelected.Contains(r.MENUID));
                    if (itemCustom != null)
                    {
                        home.MenuNameSelected = itemCustom.VALOR;
                    }
                }
            }

             

            if (home.Empresa.Id > 0)
            {
                var check = false;
                if (String.IsNullOrEmpty(home.AreaName))
                {
                    check = TemAcessoCorrente(home.Menus, 1, home.ControllerName);
                }
                else
                {
                    //if (!String.IsNullOrEmpty(home.AreaName))
                    //{
                    check = TemAcessoCorrente(home.Menus, 2, home.AreaName, home.ControllerName);
                    //}
                    //else
                    //{
                    //    check = TemAcessoCorrente(home.Menus, 3, home.AreaName, home.ControllerName);
                    //}
                }


                home.HasInicialAccess = check;
                home.HasCurrentAccess = check;
                if (String.IsNullOrEmpty(home.AreaName) && home.ControllerName.Equals("Dashboard", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (home.Menus.FirstOrDefault(w => w.ControllerName == "Dashboard") != null)
                    {
                        home.Menus.FirstOrDefault(w => w.ControllerName == "Dashboard").Active = true;
                    }
                    home.HasCurrentAccess = true;
                }
            }
            else
            {
                home.HasInicialAccess = true;
                home.HasCurrentAccess = true;
            }
            */
            //if (home.HasCurrentAccess == false)
            //{
            //    if (home.Menus.FirstOrDefault(w => w.ControllerName == "Dashboard") != null)
            //    {
            //        home.Menus.FirstOrDefault(w => w.ControllerName == "Dashboard").Active = true;
            //    }
            //    HttpContext.Current.Response.Redirect("~/Dashboard");
            //}
            return home;
        }

    }

}