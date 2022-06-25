using PortalVet.App_Start;
using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Data.Interface;
using PortalVet.Helper;
using PortalVet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalVet.Areas.Administracao.Controllers
{
    public class MenuController : BaseController
    {
        private readonly IAdminMenuService _adminMenuService;
        private readonly IEmpresaService _empresaService;
        public MenuController(IAdminMenuService adminMenuService, IEmpresaService empresaService)
        {
            _adminMenuService = adminMenuService ??
             throw new ArgumentNullException(nameof(adminMenuService));

            _empresaService = empresaService ??
                throw new ArgumentNullException(nameof(empresaService));
        }

        [SecurityPages]
        public ActionResult Index()
        {
            VMHome home = UtilAdmin.GetModelHome(this.RouteData, HttpContext.Session, Url);
            return View("Index", home);
        }


        private Dictionary<string, object> GetFilter()
        {
            Dictionary<string, object> filter = new Dictionary<string, object>();

            //if (!String.IsNullOrEmpty(filterNome.Text))
            //    filter.Add("M1.NAME", filterNome.Text);

            return filter.Count() == 0 ? null : filter;
        }


        public LargeJsonResult List(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            int pageIndex = 1;
            int pageSize = 10;
            int pgTotal = 0;

            if (form["pageIndex"] != null)
            {
                Int32.TryParse(form["pageIndex"].Trim(), out pageIndex);
            }

            if (form["pageSize"] != null)
            {
                Int32.TryParse(form["pageSize"].Trim(), out pageSize);
            }

            var data = _adminMenuService.List(out pgTotal, pageIndex, pageSize, GetFilter());

            jsonResultado.Data = data;
            jsonResultado.PageTotal = pgTotal;
            jsonResultado.PageIndex = pageIndex;
            jsonResultado.PageSize = pageSize;
            return new LargeJsonResult { Data = jsonResultado };
        }


        public LargeJsonResult Remove(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            int id = 0;

            try
            {
                if (form["id"] != null)
                {
                    Int32.TryParse(form["id"].Trim(), out id);

                    _adminMenuService.Remove(id);
                }
            }
            catch (Exception ex)
            {
                jsonResultado.Error = true;
                jsonResultado.Message = ex.Message;
            }

            return new LargeJsonResult { Data = jsonResultado };
        }



        #region SAVE



        public LargeJsonResult Load(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            if (form["hdnId"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "hdnId", Message = "Required" });
            }

            var entity = new AdminMenuItem();
            if (!jsonResultado.Criticas.Any())
            {
                entity = _adminMenuService.Get(Int32.Parse(form["hdnId"].ToString()));
            }

            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = entity;

            return new LargeJsonResult { Data = jsonResultado };
        }

        [HttpGet]
        [SecurityPages]
        public ActionResult Save(string id)
        {
            VMHome home = UtilAdmin.GetModelHome(this.RouteData, HttpContext.Session, Url);

            if (!String.IsNullOrEmpty(id))
            {
                home.ID = id;
            }

            var list = _adminMenuService.ListMenuPai();
            var listAll = list.Select(x => new SelectListItem() { Value = x.MenuId.ToString(), Text = x.Nome }).ToList();
            ViewData["ListMenuPai"] = listAll;

            var listEmpresa = _empresaService.ListEmpresa();
            var listEmpresaAll = listEmpresa.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nome }).ToList();
            ViewData["ListEmpresa"] = listEmpresaAll;

            return View("Save", home);
        }


        public LargeJsonResult Save(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            AdminMenuItem entity = new AdminMenuItem();
            if (form["txtNome"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtNome", Message = "Required" });
            }

            var caminho = "";
            var modulo = "";
            var icone = "";
            var area = "";
            var controller = "";
            var action = "";
            int ordem = 0;
            if (!String.IsNullOrEmpty(form["txtCaminho"]))
            {
                caminho = form["txtCaminho"].Trim();
            }

            if (!String.IsNullOrEmpty(form["txtModulo"]))
            {
                modulo = form["txtModulo"].Trim();
            }

            if (!String.IsNullOrEmpty(form["txtIcone"]))
            {
                icone = form["txtIcone"].Trim();
            }

            if (!String.IsNullOrEmpty(form["txtArea"]))
            {
                area = form["txtArea"].Trim();
            }


            if (!String.IsNullOrEmpty(form["txtController"]))
            {
                controller = form["txtController"].Trim();

            }

            if (!String.IsNullOrEmpty(form["txtAction"]))
            {
                action = form["txtAction"].Trim();

            }

            if (!String.IsNullOrEmpty(form["txtOrdem"]))
            {
                ordem = Int32.Parse(form["txtOrdem"].Trim());

            }

            var ativo = form["chkAtivo"] != null ? true : false;
            //var _empresasId = new List<AdminMenuTesteItem>();
            //if (!ativo)
            //{
            //    if (form["drpEmpresa"] != null)
            //    {
            //        var codes = form["drpEmpresa"].ToString().Split(',');
            //        var listIds = Array.ConvertAll(codes, int.Parse).ToList();
            //        listIds.ForEach(t =>
            //        {
            //            _empresasId.Add(new AdminMenuTesteItem()
            //            {
            //                EmpresaId = t
            //            });
            //        });
            //    }
            //}



            var drpMenuPai = 0;
            if (!String.IsNullOrEmpty(form["drpMenuPai"]))
            {
                drpMenuPai = Int32.Parse(form["drpMenuPai"].ToString());
            }
            if (!jsonResultado.Criticas.Any())
            {
                int id = 0;
                if (!form["hdnId"].Trim().Length.Equals(0))
                {
                    id = Int32.Parse(form["hdnId"].ToString());
                }


                entity = _adminMenuService.Save(new AdminMenuItem()
                {
                    MenuId = id,
                    Nome = form["txtNome"].ToString(),
                    Path = caminho,
                    Modulo = modulo,
                    IconeCss = icone,
                    AreaNome = area,
                    ControllerNome = controller,
                    ActionNome = action,
                    Ordem = ordem,
                    ParentId = drpMenuPai,
                    Ativo = form["chkAtivo"] != null ? true : false,
                    //ListEmpresasTeste = _empresasId
                });

            }
            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = entity;

            return new LargeJsonResult { Data = jsonResultado };
        }

        #endregion SAVE
    }
}