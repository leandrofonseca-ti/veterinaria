using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System;
using PortalVet.App_Start;
using PortalVet.Data.Helper;
using PortalVet.Helper;
using PortalVet.Models;
using PortalVet.Data.Entity;
using PortalVet.Data.Interface;

namespace PortalVet.Areas.Administracao.Controllers
{
    public class PerfilController : BaseController
    {

        private readonly IAdminProfileService _adminProfileService;

        public PerfilController(IAdminProfileService adminProfileService)
        {
            _adminProfileService = adminProfileService ??
             throw new ArgumentNullException(nameof(adminProfileService));

        }


        // GET: Perfil
        [HttpGet]
        [SecurityPages]
        public ActionResult Index()
        {

            VMHome home = UtilAdmin.GetModelHome(this.RouteData, HttpContext.Session, Url);
            return View("Index", home);
        }




        private Dictionary<string, object> GetFilter()
        {
            Dictionary<string, object> filter = new Dictionary<string, object>();

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

            var data = _adminProfileService.List(out pgTotal, pageIndex, pageSize, GetFilter());

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

                    _adminProfileService.Remove(id);
                }
            }
            catch (Exception ex)
            {
                jsonResultado.Error = true;
                jsonResultado.Message = ex.Message;
            }

            return new LargeJsonResult { Data = jsonResultado };
        }



        public LargeJsonResult ListMenus(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            try
            {

                jsonResultado.Data = _adminProfileService.GetMenus();
            }
            catch (Exception ex)
            {
                jsonResultado.Error = true;
                jsonResultado.Message = ex.Message;
            }
            return new LargeJsonResult { Data = jsonResultado };
        }

        public LargeJsonResult ListRegras(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            int id = 0;
            try
            {
                if (form["drpMenu"] != null)
                {
                    Int32.TryParse(form["drpMenu"].Trim(), out id);

                    jsonResultado.Data = _adminProfileService.GetRegras(id);
                }
            }
            catch (Exception ex)
            {
                jsonResultado.Error = true;
                jsonResultado.Message = ex.Message;
            }

            return new LargeJsonResult { Data = jsonResultado };
        }



        public LargeJsonResult ListPermissoes(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            int id = 0;
            try
            {
                if (form["hdnId"] != null)
                {
                    Int32.TryParse(form["hdnId"].Trim(), out id);

                    jsonResultado.Data = _adminProfileService.GetPermissao(id);
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

            AdminProfileItem entity = new AdminProfileItem();
            if (!jsonResultado.Criticas.Any())
            {
                entity = _adminProfileService.Get(Int32.Parse(form["hdnId"].ToString()));
            }

            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = entity;

            return new LargeJsonResult { Data = jsonResultado };
        }



        // GET: Perfil
        [HttpGet]
        [SecurityPages]
        public ActionResult Save(string id)
        {
            VMHome home = UtilAdmin.GetModelHome(this.RouteData, HttpContext.Session, Url);

            if (!String.IsNullOrEmpty(id))
            {
                home.ID = id;
            }

            return View("Save", home);
        }


        public LargeJsonResult SaveRole(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            try
            {
                var _menuid = 0;
                var _chave = "";
                var _nome = "";
                var _action = "";
                if (!form["drpMenu"].Trim().Length.Equals(0))
                {
                    _menuid = Int32.Parse(form["drpMenu"].ToString());
                }
                if (!form["chave"].Trim().Length.Equals(0))
                {
                    _chave = form["chave"].ToString();
                }
                if (!form["nome"].Trim().Length.Equals(0))
                {
                    _nome = form["nome"].ToString();
                }
                if (!form["action"].Trim().Length.Equals(0))
                {
                    _action = form["action"].ToString();
                }

                _adminProfileService.SaveAction(_menuid, _nome, _chave, _action);
                jsonResultado.Error = false;
            }
            catch
            {
                jsonResultado.Error = true;
            }


            return new LargeJsonResult { Data = jsonResultado };
        }

        public LargeJsonResult RemoveRole(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            try
            {
                var _regraid = 0;
                var _perfilid = 0;
                if (!form["regraid"].Trim().Length.Equals(0))
                {
                    _regraid = Int32.Parse(form["regraid"].ToString());
                }

                if (!form["perfilid"].Trim().Length.Equals(0))
                {
                    _perfilid = Int32.Parse(form["perfilid"].ToString());
                }

                _adminProfileService.RemoveRegra(_regraid, _perfilid);
                jsonResultado.Error = false;
            }
            catch
            {
                jsonResultado.Error = true;
            }


            return new LargeJsonResult { Data = jsonResultado };
        }

        public LargeJsonResult SavePermission(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            try
            {
                int profileid = 0;
                int roleid = 0;
                if (!form["drpRegra"].Trim().Length.Equals(0))
                {
                    roleid = Int32.Parse(form["drpRegra"].ToString());
                }

                if (!form["hdnId"].Trim().Length.Equals(0))
                {
                    profileid = Int32.Parse(form["hdnId"].ToString());
                }

                _adminProfileService.SavePermissao(profileid, roleid);
                jsonResultado.Error = false;
            }
            catch
            {
                jsonResultado.Error = true;
            }


            return new LargeJsonResult { Data = jsonResultado };
        }

        public LargeJsonResult Save(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            var entity = new AdminProfileItem();
            if (form["txtNome"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtNome", Message = "Required" });
            }

            if (!jsonResultado.Criticas.Any())
            {
                int id = 0;
                if (!form["hdnId"].Trim().Length.Equals(0))
                {
                    id = Int32.Parse(form["hdnId"].ToString());
                }

                entity = _adminProfileService.Save(new AdminProfileItem()
                {
                    Id = id,
                    Nome = form["txtNome"].ToString()
                });

            }
            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = entity;

            return new LargeJsonResult { Data = jsonResultado };
        }

        #endregion SAVE
    }
}