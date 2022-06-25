using PortalVet.App_Start;
using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Data.Interface;
using PortalVet.Data.Service;
using PortalVet.Helper;
using PortalVet.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PortalVet.Areas.Cadastro.Controllers
{
    public class ExameController : BaseController
    {
        private readonly IExameService _exameService;
        private readonly IAdminUserService _adminUserService;
        private readonly IContratoModeloService _contratoModeloService;
        private readonly IEmpresaService _empresaService;
        private readonly IAssinaturaService _assinaturaService;


        public string pathSaveFiles = "exames/";
        private List<ViewModelFoto> ListFiles
        {
            get
            {
                if (HttpContext.Session[string.Format("ListModelFotos_{0}", SessionsAdmin.UsuarioId)] == null)
                {
                    HttpContext.Session[string.Format("ListModelFotos_{0}", SessionsAdmin.UsuarioId)] = new List<ViewModelFoto>();
                }
                return (List<ViewModelFoto>)HttpContext.Session[string.Format("ListModelFotos_{0}", SessionsAdmin.UsuarioId)];
            }
            set
            {
                HttpContext.Session[string.Format("ListModelFotos_{0}", SessionsAdmin.UsuarioId)] = value;
            }
        }






        public ExameController(IAdminUserService adminUserService, IExameService exameService, IEmpresaService empresaService, IContratoModeloService contratoModeloService, IAssinaturaService assinaturaService)
        {
            _adminUserService = adminUserService ??
                 throw new ArgumentNullException(nameof(adminUserService));

            _exameService = exameService ??
                 throw new ArgumentNullException(nameof(exameService));

            _empresaService = empresaService ??
              throw new ArgumentNullException(nameof(empresaService));

            _contratoModeloService = contratoModeloService ??
             throw new ArgumentNullException(nameof(contratoModeloService));


            _assinaturaService = assinaturaService ??
                    throw new ArgumentNullException(nameof(assinaturaService));

        }

        public ActionResult Zoom()
        {
            ViewBag.PathImage = "";
            if (Request["p"] != null)
            {
                ViewBag.PathImage = Request["p"].ToString();
            }
            return View("Zoom");
        }
        // GET: Cadastro/Exame
        public ActionResult Index()
        {
            VMHome home = UtilAdmin.GetModelHome(this.RouteData, HttpContext.Session, Url);


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
            }


            ViewData["ListSituacao"] = statusFinal;

            var listCliente = _exameService.CarregarClientes(home.EmpresaId);
            var listClienteFinal = listCliente.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome} ({x.Email})" }).ToList();
            ViewData["ListCliente"] = listClienteFinal;

            var listLaudador = _exameService.CarregarLaudadores(home.EmpresaId);
            var listLaudadorFinal = listLaudador.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome} ({x.Email})" }).ToList();
            ViewData["ListLaudador"] = listLaudadorFinal;

            //var listRaca = _exameService.CarregarRacas(home.EmpresaId);
            //var listRacaFinal = listRaca.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome}" }).ToList();
            //ViewData["ListRaca"] = listRacaFinal;

            var listEspecie = _exameService.CarregarEspecies(home.EmpresaId);
            var listEspecieFinal = listEspecie.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome}" }).ToList();
            ViewData["ListEspecie"] = listEspecieFinal;

            return View("Index", home);
        }

        public LargeJsonResult CarregarRaca(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            var codigo = 0;
            if (!form["codigo"].Trim().Length.Equals(0))
            {
                codigo = Int32.Parse(form["codigo"].Trim());
            }
            var listagem = new List<SelectListItem>();
            if (codigo > 0)
            {
                var listRaca = _exameService.CarregarRacas(codigo);
                listagem = listRaca.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome}" }).ToList();
            }

            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = listagem;

            return new LargeJsonResult { Data = jsonResultado };
        }


        [ValidateInput(false)]
        public LargeJsonResult Load(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            if (form["hdnId"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "hdnId", Message = "Required" });
            }

            var entity = new ExameItem();
            if (!jsonResultado.Criticas.Any())
            {
                entity = _exameService.Get(Int32.Parse(form["hdnId"].ToString()));
                if (SessionsAdmin.PerfilId == EnumAdminProfile.Gerente.GetHashCode())
                {
                    entity.DuvidasLaudador = _exameService.ListHistoricoDuvidaLaudador(Int32.Parse(form["hdnId"].ToString()));
                    entity.DuvidasClinica = _exameService.ListHistoricoDuvidaClinica(Int32.Parse(form["hdnId"].ToString()));
                }

                if (SessionsAdmin.PerfilId == EnumAdminProfile.Laudador.GetHashCode())
                {
                    entity.DuvidasLaudador = _exameService.ListHistoricoDuvidaLaudador(Int32.Parse(form["hdnId"].ToString()));
                }

                if (SessionsAdmin.PerfilId == EnumAdminProfile.Clinica.GetHashCode())
                {
                    entity.DuvidasClinica = _exameService.ListHistoricoDuvidaClinica(Int32.Parse(form["hdnId"].ToString()));
                }

                var key = HttpUtility.UrlEncode(Data.Helper.Util.Encriptar(string.Format("{0}|{1}|{2}", entity.CompanyId, entity.Id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                var linkExame = $"{System.Configuration.ConfigurationManager.AppSettings["UrlRoot"].ToString()}viewExame?key={key}";
                entity.LinkExame = linkExame;
            }
            if (SessionsAdmin.EmpresaId == entity.CompanyId)
            {
                jsonResultado.Error = jsonResultado.Criticas.Any();
                jsonResultado.Data = entity;
            }
            else
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "Sem Acesso", Message = "Required" });
                jsonResultado.Error = jsonResultado.Criticas.Any();
                jsonResultado.Data = null;
            }
            return new LargeJsonResult { Data = jsonResultado };
        }



        [ValidateInput(false)]
        public LargeJsonResult LoadHistorico(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            if (form["hdnId"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "hdnId", Message = "Required" });
            }

            var entities = new List<ExameHistoricoItem>();
            if (!jsonResultado.Criticas.Any())
            {
                entities = _exameService.GetHistorico(Int32.Parse(form["hdnId"].ToString()));
            }

            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = entities;

            return new LargeJsonResult { Data = jsonResultado };
        }


        [ValidateInput(false)]
        public LargeJsonResult CarregarModelos(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            int _codigo = 0;
            if (form["codigo"] != null)
            {
                Int32.TryParse(form["codigo"].Trim(), out _codigo);
            }

            var listModelo = _exameService.CarregarModelos(_codigo, SessionsAdmin.EmpresaId);
            var listModeloFinal = listModelo.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome}" }).ToList();
            jsonResultado.Data = listModeloFinal;
            return new LargeJsonResult { Data = jsonResultado };
        }

        [ValidateInput(false)]
        public LargeJsonResult CarregarModelo(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            int _codigo = 0;
            var _descricao = "";
            var _rodape = "";
            if (form["codigo"] != null)
            {
                Int32.TryParse(form["codigo"].Trim(), out _codigo);
            }
            if (_codigo > 0)
            {
                var obj = _exameService.CarregarModelo(_codigo);
                if (obj != null)
                {
                    _descricao = obj.ModeloCorpo;
                    _rodape = obj.ModeloRodape;
                }
            }
            jsonResultado.Data = new { Descricao = _descricao, Rodape = _rodape };
            return new LargeJsonResult { Data = jsonResultado };
        }
        public LargeJsonResult List(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            int pageIndex = 1;
            int pageSize = 20;
            int pgTotal = 0;

            if (form["pageIndex"] != null)
            {
                Int32.TryParse(form["pageIndex"].Trim(), out pageIndex);
            }

            if (form["pageSize"] != null)
            {
                Int32.TryParse(form["pageSize"].Trim(), out pageSize);
            }


            var filterEmpresaID = SessionsAdmin.EmpresaId;
            var filterCodigo = "";
            var filterSituacaoId = -1;
            var filterLaudadorId = 0;
            var filterClienteId = 0;
            DateTime? filterDtExame = null;
            if (form["txtFilterCodigo"] != null)
            {
                filterCodigo = form["txtFilterCodigo"].ToString();
            }

            if (form["txtFilterDtExame"] != null)
            {
                if (DateTime.TryParse(form["txtFilterDtExame"].ToString(), out DateTime dtAux))
                {
                    filterDtExame = dtAux;
                }
            }

            if (form["drpFilterSituacao"] != null && !String.IsNullOrEmpty(form["drpFilterSituacao"]))
            {
                filterSituacaoId = Int32.Parse(form["drpFilterSituacao"].ToString());
            }
            if (form["drpFilterCliente"] != null && !String.IsNullOrEmpty(form["drpFilterCliente"]))
            {
                filterClienteId = Int32.Parse(form["drpFilterCliente"].ToString());
            }

            if (form["drpFilterLaudador"] != null && !String.IsNullOrEmpty(form["drpFilterLaudador"]))
            {
                filterLaudadorId = Int32.Parse(form["drpFilterLaudador"].ToString());
            }


            int pageOrderCol = 0;
            string pageOrderSort = "ASC";
            if (form["pageOrderCol"] != null)
            {
                Int32.TryParse(form["pageOrderCol"].Trim(), out pageOrderCol);
            }

            if (form["pageOrderSort"] != null)
            {
                if (form["pageOrderSort"].Trim() == "DESC")
                {
                    pageOrderSort = "DESC";
                }
            }

            var data = _exameService.List(out pgTotal, pageIndex, pageSize, pageOrderCol, pageOrderSort,
                 GetFilter(filterEmpresaID,
                            filterCodigo,
                            filterSituacaoId,
                            filterLaudadorId,
                            filterClienteId,
                            filterDtExame));

            jsonResultado.Data = data;
            jsonResultado.PageTotal = pgTotal;
            jsonResultado.PageIndex = pageIndex;
            jsonResultado.PageSize = pageSize;
            return new LargeJsonResult { Data = jsonResultado };
        }


        private Dictionary<string, object> GetFilter(int filterEmpresaID,
                            string filterCodigo,
                            int filterSituacaoId,
                            int filterLaudadorId,
                            int filterClienteId,
                           DateTime? filterDtExame)
        {
            Dictionary<string, object> filter = new Dictionary<string, object>();

            filter.Add("Exame.CompanyId", filterEmpresaID);

            if (SessionsAdmin.PerfilId == EnumAdminProfile.Cliente.GetHashCode())
            {
                filter.Add("Exame.SituacaoId", EnumExameSituacao.Concluido.GetHashCode().ToString());
            }

            if (SessionsAdmin.PerfilId == EnumAdminProfile.Clinica.GetHashCode())
            {
                filter.Add("Exame.SituacaoId!=", EnumExameSituacao.Criacao.GetHashCode().ToString());
            }

            if (SessionsAdmin.PerfilId == EnumAdminProfile.Laudador.GetHashCode())
            {
                List<string> listStatus = new List<string>();
                listStatus.Add(EnumExameSituacao.Concluido_Gerente.GetHashCode().ToString());
                listStatus.Add(EnumExameSituacao.Concluido.GetHashCode().ToString());
                // listStatus.Add(EnumExameSituacao.Em_Analise_Gerente.GetHashCode().ToString());
                listStatus.Add(EnumExameSituacao.Criacao.GetHashCode().ToString());
                filter.Add("Exame.SituacaoId!=", listStatus);

            }
            if (!String.IsNullOrEmpty(filterCodigo))
            {
                filter.Add("Exame.Id", filterCodigo);
            }

            if (filterSituacaoId >= 0)
            {
                filter.Add("Exame.SituacaoId", filterSituacaoId);
            }

            if (filterLaudadorId > 0)
            {
                filter.Add("Exame.LaudadorId", filterLaudadorId);
            }

            if (filterClienteId > 0)
            {
                filter.Add("Exame.ClienteId", filterClienteId);
            }
            if (filterDtExame.HasValue)
            {
                filter.Add("Year(Exame.DataExame)", filterDtExame.Value.Year);
                filter.Add("Month(Exame.DataExame)", filterDtExame.Value.Month);
                filter.Add("Day(Exame.DataExame)", filterDtExame.Value.Day);
            }

            return filter.Count() == 0 ? null : filter;
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

                    _exameService.Remove(id);
                }
            }
            catch (Exception ex)
            {
                jsonResultado.Error = true;
                jsonResultado.Message = ex.Message;
            }

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


            //var listRaca = _exameService.CarregarRacas(home.EmpresaId);
            //var listRacaFinal = listRaca.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome}" }).ToList();
            //ViewData["ListRaca"] = listRacaFinal;


            var listCliente = _exameService.CarregarClientes(home.EmpresaId);
            var listClienteFinal = listCliente.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome} ({x.Email})" }).ToList();
            ViewData["ListCliente"] = listClienteFinal;


            var listLaudador = _exameService.CarregarLaudadores(home.EmpresaId);
            var listLaudadorFinal = listLaudador.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome} ({x.Email})" }).ToList();
            ViewData["ListLaudador"] = listLaudadorFinal;


            var listEspecie = _exameService.CarregarEspecies(home.EmpresaId);
            var listEspecieFinal = listEspecie.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome}" }).ToList();
            ViewData["ListEspecie"] = listEspecieFinal;


            var listModelo = _exameService.CarregarModelos(Int32.Parse(id), home.EmpresaId);
            var listModeloFinal = listModelo.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome}" }).ToList();
            ViewData["ListModelo"] = listModeloFinal;


            var listClinica = _exameService.CarregarClinicas(home.EmpresaId);
            var listClinicaFinal = listClinica.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome} ({x.Email})" }).ToList();
            ViewData["ListClinica"] = listClinicaFinal;


            var listStatus = Enum.GetValues(typeof(EnumExameSituacao)).Cast<EnumExameSituacao>().ToList();

            List<SelectListItem> statusFinal = new List<SelectListItem>();

            if (SessionsAdmin.PerfilId == EnumAdminProfile.Gerente.GetHashCode())
            {
                foreach (var item in listStatus)
                {
                    var text = Enumeradores.GetDescription((EnumExameSituacao)item.GetHashCode());
                    statusFinal.Add(new SelectListItem() { Text = text, Value = item.GetHashCode().ToString() });
                }
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
                    if (item.GetHashCode() != EnumExameSituacao.Concluido_Gerente.GetHashCode() &&
                        item.GetHashCode() != EnumExameSituacao.Concluido.GetHashCode() &&
                        //item.GetHashCode() != EnumExameSituacao.Em_Analise_Gerente.GetHashCode() &&
                        item.GetHashCode() != EnumExameSituacao.Criacao.GetHashCode())
                    {
                        var text = Enumeradores.GetDescription((EnumExameSituacao)item.GetHashCode());
                        statusFinal.Add(new SelectListItem() { Text = text, Value = item.GetHashCode().ToString() });
                    }
                }
            }


            ViewData["ListSituacao"] = statusFinal;

            ViewData["hdnEmpresaId"] = home.EmpresaId;


            ViewData["ListRodape"] = new List<SelectListItem>();

            if (SessionsAdmin.PerfilId == EnumAdminProfile.Gerente.GetHashCode())
            {
                var listagem = _assinaturaService.ListarLaudadorCompany(SessionsAdmin.EmpresaId);
                var auxList = listagem.Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Nome }).ToList();
                ViewData["ListRodape"] = auxList;
            }

            if (SessionsAdmin.PerfilId == EnumAdminProfile.Laudador.GetHashCode())
            {
                var listagem = _assinaturaService.ListarLaudador(SessionsAdmin.UsuarioId);
                var auxList = listagem.Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Nome }).ToList();
                ViewData["ListRodape"] = auxList;
            }
            // ARQUIVOS
            ListFiles = new List<ViewModelFoto>();
            if (!String.IsNullOrEmpty(id))
            {
                var pathUploadCliente = Util.GetMapUpload();
                var pathCliente = string.Format("{0}exames\\{1}\\{2}", pathUploadCliente, SessionsAdmin.EmpresaId, id);
                if (Directory.Exists(pathCliente))
                {
                    var ct = 0;
                    foreach (var file in Directory.GetFiles(pathCliente, @"*.*", SearchOption.TopDirectoryOnly))
                    {
                        if (!file.EndsWith(".zip"))
                        {
                            var fileName = Path.GetFileName(file);
                            var relativo = string.Concat(Util.GetUrlUpload(), "exames/", SessionsAdmin.EmpresaId, "/", id, "/", fileName);
                            var fisico = string.Concat(Util.GetMapUpload(), "exames/", SessionsAdmin.EmpresaId, "/", id, "/", fileName);


                            Image img = Image.FromFile(fisico);
                            var orientacao = "";
                            if (img.Width > img.Height)
                                orientacao = "H";
                            else
                                orientacao = "V";
                            ListFiles.Add(new ViewModelFoto() { ID = ct, Orientacao = orientacao, Path = fisico, Name = relativo, OriginalFileName = fileName });
                            ct++;
                        }
                    }
                }
            }
            ViewData["listArquivos"] = ListFiles;

            return View("Save", home);
        }

        [ValidateInput(false)]
        public LargeJsonResult LoadClientes(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            int empresaId = Int32.Parse(form["hdnEmpresaId"].ToString());
            var list = _exameService.CarregarClientes(empresaId);
            var listFinal = list.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome} ({x.Email})" }).ToList();
            jsonResultado.Data = listFinal;

            return new LargeJsonResult { Data = jsonResultado };
        }

        [ValidateInput(false)]
        public LargeJsonResult LoadLaudadores(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            int empresaId = Int32.Parse(form["hdnEmpresaId"].ToString());
            var list = _exameService.CarregarLaudadores(empresaId);
            var listFinal = list.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome} ({x.Email})" }).ToList();
            jsonResultado.Data = listFinal;

            return new LargeJsonResult { Data = jsonResultado };
        }

        [ValidateInput(false)]
        public LargeJsonResult SaveLaudador(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            var entity = new AdminUserItem();


            var codigo = 0;


            if (form["txtModalNome"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalNome", Message = "Required" });
            }

            var emptyEmail = true;

            if (!form["txtModalEmail"].Trim().Length.Equals(0))
            {
                emptyEmail = false;
            }

            if (emptyEmail == true)
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalEmail", Message = "Required" });
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = "Informe o campo e-mail." });
            }

            if (!emptyEmail)
            {
                if (!Util.IsEmail(form["txtModalEmail"].Trim()))
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalEmail", Message = "Required" });
                    jsonResultado.Message = "E-mail incorreto!";
                }
            }
            var _senhaInformada = string.Empty;

            if (form["txtModalSenha"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalSenha", Message = "Required" });
            }

            if (form["txtModalSenhaCC"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalSenhaCC", Message = "Required" });
            }
            if (!jsonResultado.Criticas.Any())
            {
                if (form["txtModalSenhaCC"].ToString() != form["txtModalSenhaCC"].ToString())
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalSenha", Message = "Required" });
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalSenhaCC", Message = "Required" });
                }
                else
                {
                    _senhaInformada = form["txtModalSenha"].ToString().Trim();
                }
            }

            int empresaId = Int32.Parse(form["hdnCurrentEmpresaId"].ToString());


            int idexist = 0;

            if (!jsonResultado.Criticas.Any())
            {
                var message = "";
                int profileId = EnumAdminProfile.Laudador.GetHashCode();
                entity = _adminUserService.SaveUserPerfilDireto(new AdminUserItem()
                {
                    Id = codigo,
                    PerfilId = (EnumAdminProfile)profileId,
                    CompanyId = empresaId,
                    Nome = form["txtModalNome"].ToString(),
                    CPFCNPJ = string.Empty,// form["txtCPFCNPJ"].ToString().Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", ""),
                    Sobrenome = string.Empty,//form["txtSobreNome"].ToString(),
                    Password = _senhaInformada,
                    Telefone = string.Empty,//form["txtTelefone"].ToString(),
                    Telefone2 = string.Empty,//form["txtTelefone2"].ToString(),
                    Email = form["txtModalEmail"].ToString(),
                    Imagem = string.Empty,
                    Active = true
                }, out message, out idexist);


                if (!String.IsNullOrEmpty(message))
                {
                    if (message == "CPFCNPJ_CADASTRADO_EMPRESA")
                    {
                        //jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "AVISO_USUARIO", Message = "CPFCNPJ_CADASTRADO_EMPRESA", Auxiliar = idexist.ToString() });
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = "CPF/CNPJ já cadastrado." });
                    }
                    else if (message == "EMAIL_CADASTRADO_EMPRESA")
                    {
                        //jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "AVISO_USUARIO", Message = "EMAIL_CADASTRADO_EMPRESA", Auxiliar = idexist.ToString() });
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = "E-mail já cadastrado." });
                    }
                    else if (message == "CPFCNPJ_CADASTRADO")
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "CARREGAR_USUARIO", Message = "CPFCNPJ_CADASTRADO", Auxiliar = idexist.ToString() });
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = "CPF/CNPJ já cadastrado." });
                    }
                    else if (message == "EMAIL_CADASTRADO")
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "CARREGAR_USUARIO", Message = "EMAIL_CADASTRADO", Auxiliar = idexist.ToString() });
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = "E-mail já cadastrado." });
                    }
                    else
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = message });
                    }

                }
            }


            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = new { Data = entity, IDExist = idexist };

            return new LargeJsonResult { Data = jsonResultado };
        }

        [ValidateInput(false)]
        public LargeJsonResult SaveDuvida(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            var codigo = 0;
            if (!form["hdnId"].Trim().Length.Equals(0))
            {
                codigo = Int32.Parse(form["hdnId"].ToString());
            }
            if (codigo > 0)
            {
                var mensagem = form["mensagem"].ToString();
                var tipo = form["tipo"].ToString();
                var hist = new ExameHistoricoDuvidaItem();
                hist.ExameId = codigo;
                hist.UsuarioId = SessionsAdmin.UsuarioId;
                hist.UsuarioNome = SessionsAdmin.UsuarioNome;
                hist.UsuarioEmail = SessionsAdmin.UsuarioEmail;
                hist.Tipo = tipo;
                hist.Mensagem = mensagem;
                _exameService.SaveHistoricoDuvida(hist);

                if (tipo == "CLINICA")
                    jsonResultado.Data = _exameService.ListHistoricoDuvidaClinica(codigo);

                if (tipo == "LAUDADOR")
                    jsonResultado.Data = _exameService.ListHistoricoDuvidaLaudador(codigo);

                EnviarNotificacaoExame(codigo, hist, tipo);
            }
            else
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { Message = "Exame nao encontrado" });
            }

            return new LargeJsonResult { Data = jsonResultado };
        }

        private void EnviarNotificacaoExame(int exameId, ExameHistoricoDuvidaItem item, string tipo)
        {
            var exame = _exameService.Get(exameId);
            var emails = new List<SelectListWeb>();

            var _emailClinica = "";
            var _emailLaudador = "";
            var _emailGerente = "";
            if (tipo == "CLINICA")
            {
                var emailsClinica = _exameService.CarregarEmailsClinica(exame.ClinicaId);
                emails.AddRange(emailsClinica);

                 _emailClinica = String.Join(",", emailsClinica.Select(t=>t.Value).ToArray());
            }

            if (tipo == "LAUDADOR")
            {
                var emailsLaudador = _exameService.CarregarEmailsLaudador(exameId);
                if (!emailsLaudador.Any())
                {
                    emailsLaudador = _exameService.CarregarEmailsLaudadorGeral(exame.CompanyId);
                }
                emails.AddRange(emailsLaudador);

                _emailLaudador = String.Join(",", emailsLaudador.Select(t => t.Value).ToArray());
            }

            var emailsGerente = _exameService.CarregarEmailsGerente(exame.CompanyId);
            emails.AddRange(emailsGerente);

            _emailGerente = String.Join(",", emailsGerente.Select(t => t.Value).ToArray());

            StringBuilder sbText = new StringBuilder();
            sbText.AppendLine($"Exame: <strong>{exameId}</strong> <br/>");
            sbText.AppendLine("<br/><br/>");
            sbText.AppendLine("<strong>Mensagem:</strong><br/><br/>");
            sbText.AppendLine($"{item.Mensagem}<br/>");
            sbText.AppendLine($"<small><strong>{item.UsuarioNome} ({item.UsuarioEmail})</strong></small>");
            sbText.AppendLine("<br/>");
            sbText.AppendLine("<br/>");
            sbText.AppendLine($" EM CASO DE DÚVIDAS OU SUGESTÃO  REFERENTES AO EXAME <strong>{exameId}</strong><br/> ");
            sbText.AppendLine($" DEIXE SEU COMENTÁRIO ABAIXO.<br/> ");


            EnviarEmailMensagem(exame.CompanyId, "Mensagem de dúvidas/sugestões no exame", sbText.ToString(), emails);


            if (!String.IsNullOrEmpty(_emailGerente))
            {
                Util.SaveHistoricoEmail(
                                       exameId,
                                        (EnumExameSituacao)exame.SituacaoId,
                                        SessionsAdmin.UsuarioId,
                                        SessionsAdmin.UsuarioNome,
                                        SessionsAdmin.UsuarioEmail,
                                        $"Mensagem de dúvidas/sugestões no exame {exameId}' para o(s) gerente(s): {_emailGerente}");
            }

            if (!String.IsNullOrEmpty(_emailLaudador))
            {
                Util.SaveHistoricoEmail(
                                       exameId,
                                        (EnumExameSituacao)exame.SituacaoId,
                                        SessionsAdmin.UsuarioId,
                                        SessionsAdmin.UsuarioNome,
                                        SessionsAdmin.UsuarioEmail,
                                        $"Mensagem de dúvidas/sugestões no exame {exameId}' para o(s) laudador(es): {_emailLaudador}");
            }

            if (!String.IsNullOrEmpty(_emailClinica))
            {
                Util.SaveHistoricoEmail(
                                       exameId,
                                        (EnumExameSituacao)exame.SituacaoId,
                                        SessionsAdmin.UsuarioId,
                                        SessionsAdmin.UsuarioNome,
                                        SessionsAdmin.UsuarioEmail,
                                        $"Mensagem de dúvidas/sugestões no exame {exameId}' para o(s) clínica(s): {_emailClinica}");
            }

        }

        public void EnviarEmailMensagem(int empresaId, string assunto, string mensagem, List<SelectListWeb> emails)
        {
            var empresa = new EmpresaService().Carregar(empresaId);
            var body = Util.GetResourcesMensagemModelo();
            body = body.Replace("#MENSAGEM#", mensagem);
            var bodyMaster = Util.CarregaConteudoMaster(empresa, body);

            foreach (var item in emails)
            {
                Util.SendEmail($"{empresa.Nome} : {assunto}", item.Value, bodyMaster);
            }

        }

        [ValidateInput(false)]
        public LargeJsonResult SaveCliente(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            var entity = new AdminUserItem();


            var codigo = 0;

            if (form["txtModalCliNome"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalCliNome", Message = "Required" });
            }

            var emptyCPFCNPJ = false;

            var emptyEmail = false;
            if (form["txtModalCliCPFCNPJ"].Trim().Length.Equals(0))
            {
                emptyCPFCNPJ = true;
                //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtCPFCNPJ", Message = "Required" });
            }

            if (form["txtModalCliEmail"].Trim().Length.Equals(0))
            {
                emptyEmail = true;
            }

            if (emptyCPFCNPJ == true && emptyEmail == true)
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalCliEmail", Message = "Required" });
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalCliCPFCNPJ", Message = "Required" });
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = "Informe o campo e-mail e/ou CPF/CNPJ." });
            }

            if (!emptyEmail)
            {
                if (!Util.IsEmail(form["txtModalCliEmail"].Trim()))
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalCliEmail", Message = "Required" });
                    jsonResultado.Message = "E-mail incorreto!";
                }
            }

            //var alterarSenha = true;
            //var senhaInformada = "";
            //if (codigo > 0)
            //{
            //    alterarSenha = form["chkHabilita"] != null ? true : false;
            //}
            //else
            //{
            //    if (form["txtModalCliSenha"].Trim().Length.Equals(0) &&
            //        form["txtModalCliSenhaCC"].Trim().Length.Equals(0))
            //    {
            //        alterarSenha = false;
            //    }

            //}

            var _senhaInformada = string.Empty;
            if (emptyCPFCNPJ == true)
            {
                if (form["txtModalCliSenha"].Trim().Length.Equals(0))
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalCliSenha", Message = "Required" });
                }

                if (form["txtModalCliSenhaCC"].Trim().Length.Equals(0))
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalCliSenhaCC", Message = "Required" });
                }
                if (!jsonResultado.Criticas.Any())
                {
                    if (form["txtModalCliSenhaCC"].ToString() != form["txtModalCliSenhaCC"].ToString())
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalCliSenha", Message = "Required" });
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtModalCliSenhaCC", Message = "Required" });
                    }
                    else
                    {
                        _senhaInformada = form["txtModalCliSenha"].ToString().Trim();
                    }
                }
            }

            int empresaId = Int32.Parse(form["hdnCurrentEmpresaId"].ToString());


            int idexist = 0;

            if (!jsonResultado.Criticas.Any())
            {
                var message = "";
                int profileId = EnumAdminProfile.Cliente.GetHashCode();
                entity = _adminUserService.SaveUserPerfilDireto(new AdminUserItem()
                {
                    Id = codigo,
                    PerfilId = (EnumAdminProfile)profileId,
                    CompanyId = empresaId,
                    Nome = form["txtModalCliNome"].ToString(),
                    CPFCNPJ = form["txtModalCliCPFCNPJ"].ToString().Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", ""),
                    Sobrenome = form["txtModalCliSobreNome"].ToString(),
                    Password = _senhaInformada,
                    Telefone = string.Empty,
                    Telefone2 = string.Empty,
                    Email = form["txtModalCliEmail"].ToString(),
                    Imagem = string.Empty,
                    Active = true
                }, out message, out idexist);


                if (!String.IsNullOrEmpty(message))
                {
                    if (message == "CPFCNPJ_CADASTRADO_EMPRESA")
                    {
                        //jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "AVISO_USUARIO", Message = "CPFCNPJ_CADASTRADO_EMPRESA", Auxiliar = idexist.ToString() });
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = "CPF/CNPJ já cadastrado." });
                    }
                    else if (message == "EMAIL_CADASTRADO_EMPRESA")
                    {
                        //jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "AVISO_USUARIO", Message = "EMAIL_CADASTRADO_EMPRESA", Auxiliar = idexist.ToString() });
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = "E-mail já cadastrado." });
                    }
                    else if (message == "CPFCNPJ_CADASTRADO")
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "CARREGAR_USUARIO", Message = "CPFCNPJ_CADASTRADO", Auxiliar = idexist.ToString() });
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = "CPF/CNPJ já cadastrado." });
                    }
                    else if (message == "EMAIL_CADASTRADO")
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "CARREGAR_USUARIO", Message = "EMAIL_CADASTRADO", Auxiliar = idexist.ToString() });
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = "E-mail já cadastrado." });
                    }
                    else
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = message });
                    }

                }
            }

            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = new { Data = entity, IDExist = idexist };

            return new LargeJsonResult { Data = jsonResultado };
        }



        [HttpPost]
        [ValidateInput(false)]
        public LargeJsonResult VincularLaudador(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            var codigo = 0;
            if (!form["hdnId"].Trim().Length.Equals(0))
            {
                codigo = Int32.Parse(form["hdnId"].ToString());
            }

            var laudadorid = SessionsAdmin.UsuarioId;

            _exameService.VincularLaudador(new ExameItem { Id = codigo, LaudadorId = laudadorid });

            var hist = new ExameHistoricoItem();
            hist.ExameId = codigo;
            hist.UsuarioId = SessionsAdmin.UsuarioId;
            hist.UsuarioNome = SessionsAdmin.UsuarioNome;
            hist.UsuarioEmail = SessionsAdmin.UsuarioEmail;
            hist.Descricao = $"Laudador : {SessionsAdmin.UsuarioNome}, vinculado com exame : {codigo}.";
            hist.SituacaoId = EnumExameSituacao.Em_Analise_Laudador.GetHashCode();
            hist.Conteudo = string.Empty;
            _exameService.SaveHistorico(hist);


            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = new { Data = true };

            return new LargeJsonResult { Data = jsonResultado };

        }



        [HttpPost]
        [ValidateInput(false)]
        public LargeJsonResult DesvincularLaudador(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            var codigo = 0;
            if (!form["hdnId"].Trim().Length.Equals(0))
            {
                codigo = Int32.Parse(form["hdnId"].ToString());
            }

            _exameService.VincularLaudador(new ExameItem { Id = codigo, LaudadorId = 0 });

            var hist = new ExameHistoricoItem();
            hist.ExameId = codigo;
            hist.UsuarioId = SessionsAdmin.UsuarioId;
            hist.UsuarioNome = SessionsAdmin.UsuarioNome;
            hist.UsuarioEmail = SessionsAdmin.UsuarioEmail;
            hist.Descricao = $"Laudador : {SessionsAdmin.UsuarioNome}, desvinculado com exame : {codigo}.";
            hist.SituacaoId = EnumExameSituacao.Em_Analise_Laudador.GetHashCode();
            hist.Conteudo = string.Empty;
            _exameService.SaveHistorico(hist);


            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = new { Data = true };

            return new LargeJsonResult { Data = jsonResultado };
        }


        [HttpPost]
        [ValidateInput(false)]
        public LargeJsonResult DesvincularLaudadorConcorrente(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            var codigo = 0;
            if (!form["hdnId"].Trim().Length.Equals(0))
            {
                codigo = Int32.Parse(form["hdnId"].ToString());
            }

            _exameService.VincularLaudador(new ExameItem { Id = codigo, LaudadorId = 0 });

            var hist = new ExameHistoricoItem();
            hist.ExameId = codigo;
            hist.UsuarioId = SessionsAdmin.UsuarioId;
            hist.UsuarioNome = SessionsAdmin.UsuarioNome;
            hist.UsuarioEmail = SessionsAdmin.UsuarioEmail;
            hist.Descricao = $"Laudo expirou, ficou disponível.";
            hist.SituacaoId = EnumExameSituacao.Em_Analise_Laudador.GetHashCode();
            hist.Conteudo = string.Empty;
            _exameService.SaveHistorico(hist);


            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = new { Data = true };

            return new LargeJsonResult { Data = jsonResultado };
        }


        [HttpPost]
        [ValidateInput(false)]
        public LargeJsonResult DesvincularLaudadorExpirou(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            var codigo = 0;
            if (!form["hdnId"].Trim().Length.Equals(0))
            {
                codigo = Int32.Parse(form["hdnId"].ToString());
            }

            _exameService.VincularLaudador(new ExameItem { Id = codigo, LaudadorId = 0 });

            var hist = new ExameHistoricoItem();
            hist.ExameId = codigo;
            hist.UsuarioId = SessionsAdmin.UsuarioId;
            hist.UsuarioNome = SessionsAdmin.UsuarioNome;
            hist.UsuarioEmail = SessionsAdmin.UsuarioEmail;
            hist.Descricao = $"Laudador : {SessionsAdmin.UsuarioNome}, desvinculado com exame : {codigo}. Tempo expirado";
            hist.SituacaoId = EnumExameSituacao.Em_Analise_Laudador.GetHashCode();
            hist.Conteudo = string.Empty;
            _exameService.SaveHistorico(hist);


            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = new { Data = true };

            return new LargeJsonResult { Data = jsonResultado };
        }

        [HttpPost]
        [SecurityPages]
        [ValidateInput(false)]
        public LargeJsonResult Save(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            var entity = new ExameItem();

            var _clinicaId = 0;
            var codigo = 0;
            var option = 0;
            var _laudadorId = 0;
            if (!form["opt"].Trim().Length.Equals(0))
            {
                option = Int32.Parse(form["opt"].ToString());
            }


            if (!form["hdnId"].Trim().Length.Equals(0))
            {
                codigo = Int32.Parse(form["hdnId"].ToString());
            }

            if (!form["hdnClinicaId"].Trim().Length.Equals(0))
            {
                _clinicaId = Int32.Parse(form["hdnClinicaId"].ToString());
            }

            int _situacaoId = 0;
            //if (form["drpSituacao"] == null || form["drpSituacao"].Trim().Length.Equals(0))
            //{
            //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpSituacao", Message = "Required" });
            //}
            //else
            //{
            //    _situacaoId = Int32.Parse(form["drpSituacao"].Trim());
            //}
            if (!form["hdnSituacaoId"].Trim().Length.Equals(0))
            {
                _situacaoId = Int32.Parse(form["hdnSituacaoId"].Trim());
            }


            // GERENTE
            if (option == 2) // LAUDADOR
            {
                _situacaoId = EnumExameSituacao.Em_Analise_Laudador.GetHashCode();
            }

            if (option == 3) // CONCLUIR
            {
                _situacaoId = EnumExameSituacao.Concluido.GetHashCode();
            }


            // LAUDADOR
            //if (option == 4) // ANALISE GERENTE
            //{
            //    _situacaoId = EnumExameSituacao.Em_Analise_Gerente.GetHashCode();
            //}

            if (option == 5) // CONCLUIR GERENTE
            {
                _situacaoId = EnumExameSituacao.Concluido_Gerente.GetHashCode();
            }
            if (SessionsAdmin.PerfilId == EnumAdminProfile.Laudador.GetHashCode())
            {
                #region LAUDADOR




                var _editorTexto = "";
                if (!form["editorTexto"].Trim().Length.Equals(0))
                {
                    _editorTexto = form["editorTexto"].Trim();
                }


                var _editorRodape = "";
                var _assinaturaId = 0;
                if (!form["drpRodape"].Trim().Length.Equals(0))
                {
                    _assinaturaId = Int32.Parse(form["drpRodape"].Trim());
                }
                #endregion




                if (!jsonResultado.Criticas.Any())
                {
                    try
                    {
                        entity = _exameService.SaveDetailLaudador(new ExameItem()
                        {
                            Id = codigo,
                            Descricao = _editorTexto,
                            Rodape = _editorRodape,
                            AssinaturaId = _assinaturaId,
                            SituacaoId = _situacaoId,
                            CompanyId = SessionsAdmin.EmpresaId
                        });

                        if (entity.Id > 0)
                        {
                            var hist = new ExameHistoricoItem();
                            hist.ExameId = entity.Id;
                            hist.UsuarioId = SessionsAdmin.UsuarioId;
                            hist.UsuarioNome = SessionsAdmin.UsuarioNome;
                            hist.UsuarioEmail = SessionsAdmin.UsuarioEmail;
                            hist.Descricao = "Atualização dos dados (Laudador).";
                            hist.SituacaoId = _situacaoId;
                            hist.Conteudo = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                            _exameService.SaveHistorico(hist);


                            var acessoGerente = (SessionsAdmin.PerfilId == EnumAdminProfile.Gerente.GetHashCode() && (option == 2 || option == 3));
                            var acessoLaudador = (SessionsAdmin.PerfilId == EnumAdminProfile.Laudador.GetHashCode() && (option == 4 || option == 5));

                            if (acessoGerente || acessoLaudador)
                            {
                                EnviarEmailFluxo(_situacaoId, entity.Id, SessionsAdmin.EmpresaId, entity.ClinicaId);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = ex.Message });
                    }
                }
            }
            else
            {
                #region GERAL 


                DateTime? dataExame = null;
                if (form["txtDataExame"].Trim().Length.Equals(0))
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtDataExame", Message = "Required" });
                }
                else
                {
                    if (DateTime.TryParse(form["txtDataExame"].ToString(), out DateTime dtAux))
                    {
                        dataExame = dtAux;
                    }

                    if (!dataExame.HasValue)
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtDataExame", Message = "Required" });
                    }
                }

                int hour = 0;
                int min = 0;
                if (form["txtDataExameTime"].Trim().Length.Equals(0))
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtDataExameTime", Message = "Required" });
                }
                else
                {
                    try
                    {

                        var text = form["txtDataExameTime"].ToString();

                        hour = Int32.Parse(text.Split(':')[0]);
                        min = Int32.Parse(text.Split(':')[1]);

                        var dtNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, min, 0);
                    }
                    catch
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtDataExameTime", Message = "Required" });
                    }
                }

                var _racaSelecao = "";
                if (!form["txtRacaSelecao"].Trim().Length.Equals(0))
                {
                    _racaSelecao = form["txtRacaSelecao"].Trim();
                }

                var _especieSelecao = "";
                if (!form["txtEspecieSelecao"].Trim().Length.Equals(0))
                {
                    _especieSelecao = form["txtEspecieSelecao"].Trim();
                }


                var _formaPgto = "";
                var _valor = "";

                if (PortalVet.Helper.SessionsAdmin.PerfilId == PortalVet.Data.Helper.EnumAdminProfile.Administrador.GetHashCode()
                || PortalVet.Helper.SessionsAdmin.PerfilId == PortalVet.Data.Helper.EnumAdminProfile.Gerente.GetHashCode())
                {
                    if (!form["txtFormaPgto"].Trim().Length.Equals(0))
                    {
                        _formaPgto = form["txtFormaPgto"].Trim();
                    }

                    if (!form["txtValor"].Trim().Length.Equals(0))
                    {
                        _valor = form["txtValor"].Trim();
                    }
                }
                var _historico = "";
                if (!form["txtHistorico"].Trim().Length.Equals(0))
                {
                    _historico = form["txtHistorico"].Trim();
                }
                var _veterinario = "";
                if (!form["txtVeterinario"].Trim().Length.Equals(0))
                {
                    _veterinario = form["txtVeterinario"].Trim();
                }
                var _paciente = "";
                if (!form["txtPaciente"].Trim().Length.Equals(0))
                {

                    _paciente = form["txtPaciente"].Trim();
                }




                //int _situacaoId = 0;
                //if (form["drpSituacao"] == null || form["drpSituacao"].Trim().Length.Equals(0))
                //{
                //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpSituacao", Message = "Required" });
                //}
                //else
                //{
                //    _situacaoId = Int32.Parse(form["drpSituacao"].Trim());
                //}


                //var _laudadorId = 0;
                //if (form["drpLaudador"] == null || form["drpLaudador"].Trim().Length.Equals(0))
                //{
                //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpLaudador", Message = "Required" });
                //}
                //else
                //{
                //    _laudadorId = Int32.Parse(form["drpLaudador"].Trim());
                //}

                //var _clienteId = 0;
                //if (form["drpCliente"] == null || form["drpCliente"].Trim().Length.Equals(0))
                //{
                //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpCliente", Message = "Required" });
                //}
                //else
                //{
                //    _clienteId = Int32.Parse(form["drpCliente"].Trim());
                //}




                var _idade = "";
                if (!form["txtIdade"].Trim().Length.Equals(0))
                {
                    _idade = form["txtIdade"].Trim();
                }

                var _editorTexto = "";
                if (!form["editorTexto"].Trim().Length.Equals(0))
                {
                    _editorTexto = form["editorTexto"].Trim();
                }


                var _editorRodape = "";
                //if (!form["editorRodape"].Trim().Length.Equals(0))
                //{
                //    _editorRodape = form["editorRodape"].Trim();
                //}
                var _assinaturaId = 0;
                if (!form["drpRodape"].Trim().Length.Equals(0))
                {
                    _assinaturaId = Int32.Parse(form["drpRodape"].Trim());
                }
                #endregion

                if (SessionsAdmin.PerfilId == EnumAdminProfile.Gerente.GetHashCode())
                {

                    if (form["drpClinica"] == null || form["drpClinica"].Trim().Length.Equals(0))
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpClinica", Message = "Required" });
                    }
                    else
                    {
                        _clinicaId = Int32.Parse(form["drpClinica"].Trim());
                    }
                }

                if (!jsonResultado.Criticas.Any())
                {
                    try
                    {
                        var dataHoraExame = new DateTime(dataExame.Value.Year, dataExame.Value.Month, dataExame.Value.Day, hour, min, 0);

                        entity = _exameService.SaveSemProp(new ExameItem()
                        {
                            Id = codigo,
                            DataExame = dataHoraExame,
                            Veterinario = _veterinario,
                            Paciente = _paciente,
                            ClinicaId = _clinicaId,
                            //Proprietario = _proprietario,
                            AssinaturaId = _assinaturaId,
                            Descricao = _editorTexto,
                            Rodape = _editorRodape,
                            Idade = _idade,
                            EspecieId = 0,//_especieId,
                            EspecieOutros = "",//_especieOutros,
                            RacaId = 0,//_racaId,
                            RacaSelecao = _racaSelecao,
                            EspecieSelecao = _especieSelecao,
                            ClienteId = 0,
                            LaudadorId = _laudadorId,// TODO: verificar disparo p todos
                            SituacaoId = _situacaoId,
                            FormaPagamento = _formaPgto,
                            Valor = _valor,
                            Historico = _historico,
                            CompanyId = SessionsAdmin.EmpresaId,
                        });

                        if (entity.Id > 0)
                        {
                            var hist = new ExameHistoricoItem();
                            hist.ExameId = entity.Id;
                            hist.UsuarioId = SessionsAdmin.UsuarioId;
                            hist.UsuarioNome = SessionsAdmin.UsuarioNome;
                            hist.UsuarioEmail = SessionsAdmin.UsuarioEmail;
                            hist.Descricao = "Atualização dos dados.";
                            hist.SituacaoId = _situacaoId;
                            hist.Conteudo = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                            _exameService.SaveHistorico(hist);

                            var acessoGerente = (SessionsAdmin.PerfilId == EnumAdminProfile.Gerente.GetHashCode() && (option == 2 || option == 3));
                            var acessoLaudador = (SessionsAdmin.PerfilId == EnumAdminProfile.Laudador.GetHashCode() && (option == 4 || option == 5));

                            if (acessoGerente || acessoLaudador)
                            {
                                EnviarEmailFluxo(_situacaoId, entity.Id, SessionsAdmin.EmpresaId, entity.ClinicaId);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = ex.Message });
                    }
                }
            }



            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = new { Data = entity };

            return new LargeJsonResult { Data = jsonResultado };
        }


        public void EnviarEmailFluxo(int situacaoId, int exameId, int companyId, int clinicaId)
        {
            var empresa = _empresaService.Carregar(companyId);
            /*
            if (((EnumExameSituacao)situacaoId) == EnumExameSituacao.Em_Analise_Clinica)
            {
                // NOTIFICAR CLINICA(S) VINCULADAS COM EMPRESAID
                var emails = _exameService.CarregarEmailsClinica(clinicaId);
                var prop = _exameService.Get(exameId);

                // NOTIFICAR PROPRIETARIO
                if (prop != null && !String.IsNullOrEmpty(prop.ProprietarioEmail))
                {
                    emails.Add(new SelectListWeb() { Text = prop.Proprietario, Value = prop.ProprietarioEmail });
                }
                var exame = _exameService.Get(exameId);
                EnviarNotificacaoExame(empresa, exameId,
                    $"{empresa.Nome} : {exame.Paciente} : Exame {exameId} em análise",
                    exame.Paciente,
                    "Exame recebido com sucesso!<br/> Em breve será analisado pelo laudador!", emails);
            }
            */
            //if (((EnumExameSituacao)situacaoId) == EnumExameSituacao.Em_Analise_Gerente)
            //{
            //    // NOTIFICAR GERENTES(S) VINCULADAS COM EMPRESAID
            //    var emails = _exameService.CarregarEmailsGerente(companyId);
            //    var exame = _exameService.Get(exameId);
            //    EnviarNotificacaoExame(empresa, exameId,
            //           $"{empresa.Nome} : {exame.Paciente} : Exame {exameId} em análise com o(s) gerente(s)",
            //           exame.Paciente,
            //        "Laudo finalizado pelo laudador!<br/>Favor revisar e liberar para o gerente.", emails);
            //}

            if (((EnumExameSituacao)situacaoId) == EnumExameSituacao.Em_Analise_Laudador)
            {
                // NOTIFICAR LAUDADOR VINCULADO COM EMPRESAID E EXAMEID
                var emails = _exameService.CarregarEmailsLaudador(exameId);
                if (!emails.Any())
                {
                    emails = _exameService.CarregarEmailsLaudadorGeral(companyId);
                }

                var exame = _exameService.Get(exameId);
                EnviarNotificacaoExame(empresa, exameId,
                     $"{empresa.Nome} : {exame.Paciente} : Exame {exameId} em análise com o laudador",
                     exame.Paciente,
                    "VOCÊ ESTÁ RECEBENDO ESTE EXAME PARA LAUDAR EM ATÉ 24H!", emails);

            }

            if (((EnumExameSituacao)situacaoId) == EnumExameSituacao.Concluido_Gerente)
            {
                // NOTIFICAR GERENTES(S) VINCULADAS COM EMPRESAID
                var emails = _exameService.CarregarEmailsGerente(companyId);
                var exame = _exameService.Get(exameId);
                EnviarNotificacaoExame(empresa, exameId,
                     $"{empresa.Nome} : {exame.Paciente} : Exame {exameId} concluído e enviado para o(s) gerente(s).",
                     exame.Paciente,
                    "Laudo finalizado com sucesso!<br/>Favor revisar e liberar para o cliente.", emails);
            }

            if (((EnumExameSituacao)situacaoId) == EnumExameSituacao.Concluido)
            {
                // NOTIFICAR ????
                //var emails = _exameService.CarregarEmailsGerente(companyId);
                var emails = _exameService.CarregarEmailsCliente(exameId);
                //emails.AddRange(emails2);
                var exame = _exameService.Get(exameId);
                EnviarNotificacaoExame(empresa, exameId,
                     $"{empresa.Nome} : {exame.Paciente} : Exame {exameId} concluído.",
                     exame.Paciente,
                    "EXAME FINALIZADO DISPONÍVEL NO LINK ABAIXO!", emails);
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
                                    if (!file.EndsWith(".zip"))
                                    {
                                        var fileName = Path.GetFileName(file);
                                        var relativo = string.Concat(Util.GetUrlUpload(), "exames/", data.CompanyId, "/", id, "/", fileName);
                                        var fisico = string.Concat(Util.GetMapUpload(), "exames/", data.CompanyId, "/", id, "/", fileName);
                                        Image img = Image.FromFile(fisico);
                                        var orientacao = "";
                                        if (img.Width > img.Height)
                                            orientacao = "H";
                                        else
                                            orientacao = "V";
                                        listFiles.Add(new ViewModelFoto() { ID = ct, Orientacao = orientacao, Path = fisico, Name = relativo, OriginalFileName = fileName });
                                        ct++;
                                    }
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


        #region --- UPLOAD FILES ---

        public ActionResult UploadFileControl()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SubmitSaveFile(FormCollection form)
        {
            var _exameId = Request["codigo"].ToString();
            var resultJson = "{\"Status\":\"NOK\" }";
            if (this.Request.Files.Count > 0)
            {

                foreach (string upload in Request.Files)
                {
                    var fileName = "";
                    string ext = "";

                    ext = System.IO.Path.GetExtension(Request.Files[upload].FileName).ToLower();

                    if (!String.IsNullOrEmpty(ext))
                    {
                        var prefix = "file_";
                        fileName = string.Format("{0}{1}{2}", prefix, DateTime.Now.ToString("yyyyMMddHHmmssfff"), ext);
                        //fileName = Request.Files[upload].FileName;
                    }

                    var extOk = false;
                    var msgErro = "";
                    if (ext.ToLower().Contains("jpg") || ext.ToLower().Contains("jpeg") || ext.ToLower().Contains("png"))
                    {
                        extOk = true;
                    }
                    else
                    {
                        msgErro = $"Arquivo {Request.Files[upload].FileName} incorreto!";
                    }

                    var modelFile = new ViewModelFoto()
                    {
                        Name = fileName,
                        StrBytes = "",
                    };

                    if (extOk)
                    {
                        var diretorio = string.Concat(SessionsAdmin.EmpresaId.ToString(), "/", _exameId);// cal.GetWeekOfYear(DateTime.Now, dfi.CalendarWeekRule, dfi.FirstDayOfWeek).ToString();
                        var path = "";

                        if (modelFile != null && !String.IsNullOrEmpty(modelFile.Name))
                        {
                            var exception = "";
                            try
                            {
                                string location = string.Concat(Util.GetMapUpload(), pathSaveFiles, diretorio);
                                //Check if the directory exists
                                if (!System.IO.Directory.Exists(location))
                                {
                                    System.IO.Directory.CreateDirectory(location);
                                }
                                Request.Files[upload].SaveAs(string.Concat(location, @"\", modelFile.Name));
                                //var fileInfo = new FileInfo(string.Concat(location, @"\", modelFile.Name));
                                //Data.Helper.Util.SaveSistemaFile(string.Concat(pathSaveFiles, diretorio), SessionsAdmin.EmpresaId, Helper.Sessions.Cliente.Email, modelFile.Name, ext, fileInfo.Length, EnumTipoCadastro.SeguroFianca.ToString());

                                path = string.Concat(Util.GetUrlUpload(), pathSaveFiles, diretorio);
                            }
                            catch (Exception ex)
                            {
                                exception = ex.Message;
                            }
                            //var fn = string.Format("{0}/{1}", diretorio, modelFile.Name);
                            var fn = string.Format("{0}/{1}", diretorio, modelFile.Name);
                            resultJson = string.Concat("{\"Status\":\"OK\", \"Path\": \"", path, "\",\"Base64\": \"", modelFile.StrBytes, "\", \"FileName\": \"", fn, "\",\"OriginalFileName\":\"" + fileName + "\",\"Ex\":\"" + exception + "\"}");
                        }
                        else
                        {
                            resultJson = string.Concat("{\"Status\":\"NOK_FILE\", \"Message\": \"Arquivo não encontrado!\", \"FileName\": \"", fileName, "\"}");
                        }
                    }
                    else
                    {
                        resultJson = string.Concat("{\"Status\":\"NOK_FILE_ERRO\", \"Message\": \"", msgErro, "\", \"FileName\": \"", fileName, "\"}");
                    }
                }
            }
            return new JsonResult() { Data = resultJson };
        }

        public static System.Drawing.Image TratamentoImagem(System.Drawing.Image OriginalImage, Size ThumbSize)
        {
            Int32 thWidth = ThumbSize.Width;
            Int32 thHeight = ThumbSize.Height;
            System.Drawing.Image i = OriginalImage;
            Int32 w = i.Width;
            Int32 h = i.Height;
            Int32 th = thWidth;
            Int32 tw = thWidth;
            if (h > w)
            {
                Double ratio = (Double)w / (Double)h;
                th = thHeight < h ? thHeight : h;
                tw = thWidth < w ? (Int32)(ratio * thWidth) : w;
            }
            else
            {
                Double ratio = (Double)h / (Double)w;
                th = thHeight < h ? (Int32)(ratio * thHeight) : h;
                tw = thWidth < w ? thWidth : w;
            }
            Bitmap target = new Bitmap(tw, th);
            Graphics g = Graphics.FromImage(target);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.High;
            Rectangle rect = new Rectangle(0, 0, tw, th);
            g.DrawImage(i, rect, 0, 0, w, h, GraphicsUnit.Pixel);
            return (System.Drawing.Image)target;
        }

        private void TratamentoTamanhoImagem(System.Drawing.Image imageAux, int max, out int w, out int h)
        {
            if (imageAux.Width > imageAux.Height) // horizontal
            {
                if (imageAux.Width > max)
                {
                    w = max;
                    h = (w * imageAux.Height) / imageAux.Width;
                }
                else
                {
                    w = imageAux.Width;
                    h = imageAux.Height;
                }
            }
            else // vertical
            {
                if (imageAux.Height > max)
                {
                    h = max;
                    w = (h * imageAux.Width) / imageAux.Height;
                }
                else
                {
                    w = imageAux.Width;
                    h = imageAux.Height;
                }
            }


        }

        public JsonResult JsonRemoveFile(string index)
        {
            var resultStatus = false;


            var listfotos = new List<ViewModelFoto>();
            try
            {
                if (ListFiles == null)
                {
                    ListFiles = new List<ViewModelFoto>();
                }

                if (ListFiles != null)
                {
                    listfotos = (List<ViewModelFoto>)ListFiles;
                    if (!String.IsNullOrEmpty(index))
                    {
                        int indexVal = -1;
                        Int32.TryParse(index, out indexVal);

                        string fisicalPath = listfotos.ElementAt(indexVal).Path;

                        if (System.IO.File.Exists(fisicalPath))
                        {
                            try
                            {
                                System.IO.File.Delete(fisicalPath);
                            }
                            catch
                            {
                                //
                            }
                        }

                        listfotos.RemoveAt(indexVal);
                    }
                    ListFiles = listfotos;
                }
                resultStatus = true;
            }
            catch
            {

            }

            return new JsonResult
            {
                Data = new
                {
                    listImages = listfotos,
                    status = resultStatus ? "OK" : "NOK"
                }
            };
        }




        public JsonResult JsonSaveNewImage(int eid, int id, string filename, string filenametemp)
        {
            bool resultStatus = false;
            var _fileNameTemp = "";
            var fullFileNameOld = "";
            var fullFileNameRemoveOld = "";
            var mapPath = Util.GetMapUpload();
            try
            {


                var pathFileTemp = $"{mapPath}exames\\{eid}\\{id}\\temp\\{filenametemp}";
                using (StreamReader streamReader = new StreamReader(pathFileTemp))
                {
                    using (Bitmap originalBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream))
                    {
                        fullFileNameOld = $"{mapPath}exames\\{eid}\\{id}\\{filename}";
                        fullFileNameRemoveOld = $"{mapPath}remover\\{eid}\\{id}\\{filename}";
                        var idx = filenametemp.LastIndexOf(".");
                        var extTemp = filenametemp.Substring(idx);
                        var prefix = "file_";
                        var fileName = string.Format("{0}{1}{2}", prefix, DateTime.Now.ToString("yyyyMMddHHmmssfff"), extTemp);
                        var fullFileNameNew = $"{mapPath}exames\\{eid}\\{id}\\{fileName}";
                        _fileNameTemp = fileName;
                        originalBitmap.Save(fullFileNameNew);
                    }
                }
                resultStatus = true;

            }
            catch { }

            try
            {
                if (System.IO.File.Exists(fullFileNameOld))
                {
                    var location = $"{mapPath}remover\\{eid}\\{id}\\";
                    if (!System.IO.Directory.Exists(location))
                    {
                        System.IO.Directory.CreateDirectory(location);
                    }

                    System.IO.File.Move(fullFileNameOld, fullFileNameRemoveOld);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



            try
            {
                if (System.IO.File.Exists(fullFileNameRemoveOld))
                {
                    System.IO.File.Delete(fullFileNameRemoveOld);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new JsonResult()
            {
                Data = new
                {
                    Authenticated = true,
                    ErrorMessage = "",
                    Timestamp = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    FileName = _fileNameTemp,
                    Status = resultStatus ? "OK" : "NOK"
                }
            };
        }

        public JsonResult JsonEditImageBrilho(int valor, int eid, int id, string mapPath, string file)
        {
            bool resultStatus = false;
            var _fileNameTemp = "";
            try
            {
                var idx = file.LastIndexOf(".");
                var fileTemp = file.Substring(0, idx);
                var extTemp = file.Substring(idx);
                var pathFileTemp = $"{fileTemp}_temp{extTemp}";
                var fullFileName = $"{mapPath}exames/{eid}/{id}/{file}";
                using (StreamReader streamReader = new StreamReader(fullFileName))
                {
                    using (Image originalBitmap = (Image)Bitmap.FromStream(streamReader.BaseStream))
                    {
                        var v = (float)((valor / 100.0) + 1);
                        Image previewBitmap = originalBitmap.Brilho(v);
                        var location = $"{mapPath}exames/{eid}/{id}/temp/";
                        if (!System.IO.Directory.Exists(location))
                        {
                            System.IO.Directory.CreateDirectory(location);
                        }

                        var fullFileNameTemp = $"{mapPath}exames/{eid}/{id}/temp/{pathFileTemp}";
                        previewBitmap.Save(fullFileNameTemp);
                    }
                }
                resultStatus = true;
                _fileNameTemp = pathFileTemp;
            }
            catch { }
            return new JsonResult()
            {
                Data = new
                {
                    Authenticated = true,
                    ErrorMessage = "",
                    Timestamp = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    FileNameTemp = _fileNameTemp,
                    Status = resultStatus ? "OK" : "NOK"
                }
            };
        }

        public JsonResult JsonEditImage(int valor, int eid, int id, string mapPath, string file)
        {
            bool resultStatus = false;
            var _fileNameTemp = "";
            try
            {
                var idx = file.LastIndexOf(".");
                var fileTemp = file.Substring(0, idx);
                var extTemp = file.Substring(idx);
                var pathFileTemp = $"{fileTemp}_temp{extTemp}";
                var fullFileName = $"{mapPath}exames/{eid}/{id}/{file}";
                using (StreamReader streamReader = new StreamReader(fullFileName))
                {
                    using (Bitmap originalBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream))
                    {
                        Bitmap previewBitmap = originalBitmap.Contraste(valor);
                        var location = $"{mapPath}exames/{eid}/{id}/temp/";
                        if (!System.IO.Directory.Exists(location))
                        {
                            System.IO.Directory.CreateDirectory(location);
                        }

                        var fullFileNameTemp = $"{mapPath}exames/{eid}/{id}/temp/{pathFileTemp}";
                        previewBitmap.Save(fullFileNameTemp);
                    }
                }
                resultStatus = true;
                _fileNameTemp = pathFileTemp;
            }
            catch { }
            return new JsonResult()
            {
                Data = new
                {
                    Authenticated = true,
                    ErrorMessage = "",
                    Timestamp = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    FileNameTemp = _fileNameTemp,
                    Status = resultStatus ? "OK" : "NOK"
                }
            };
        }

        public JsonResult JsonEditZoom(int valor, int eid, int id, string mapPath, string file)
        {
            bool resultStatus = false;
            var _fileNameTemp = "";
            try
            {
                var idx = file.LastIndexOf(".");
                var fileTemp = file.Substring(0, idx);
                var extTemp = file.Substring(idx);
                var pathFileTemp = $"{fileTemp}_temp{extTemp}";
                var fullFileName = $"{mapPath}exames/{eid}/{id}/{file}";
                using (StreamReader streamReader = new StreamReader(fullFileName))
                {
                    using (Bitmap originalBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream))
                    {
                        var v = (float)((valor / 100.0) + 1);
                        var valorW = originalBitmap.Width * v;
                        var valorH = originalBitmap.Height * v;
                        Bitmap previewBitmap = originalBitmap.PictureBoxZoom(new Size(Convert.ToInt32(valorW), Convert.ToInt32(valorH)));
                        var location = $"{mapPath}exames/{eid}/{id}/temp/";
                        if (!System.IO.Directory.Exists(location))
                        {
                            System.IO.Directory.CreateDirectory(location);
                        }

                        var fullFileNameTemp = $"{mapPath}exames/{eid}/{id}/temp/{pathFileTemp}";
                        previewBitmap.Save(fullFileNameTemp);
                    }
                }
                resultStatus = true;
                _fileNameTemp = pathFileTemp;
            }
            catch { }
            return new JsonResult()
            {
                Data = new
                {
                    Authenticated = true,
                    ErrorMessage = "",
                    Timestamp = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    FileNameTemp = _fileNameTemp,
                    Status = resultStatus ? "OK" : "NOK"
                }
            };
        }

        public JsonResult JsonAddFile(string path, string name, string index, string base64)
        {
            var resultStatus = false;
            int indexVal = -1;
            var listfotos = new List<ViewModelFoto>();
            try
            {
                if (ListFiles == null)
                {
                    ListFiles = new List<ViewModelFoto>();
                }
                if (ListFiles != null)
                {
                    // listModelFotos.Clear(); // SEMPRE UM REGISTRO

                    listfotos = (List<ViewModelFoto>)ListFiles;

                    if (!String.IsNullOrEmpty(index))
                    {
                        indexVal = 0;
                        listfotos[indexVal].Path = path;
                        listfotos[indexVal].Name = name;
                        listfotos[indexVal].OriginalFileName = name.Substring(name.LastIndexOf("/") + 1);
                        listfotos[indexVal].StrBytes = base64;
                    }
                    else
                    {
                        if (!listfotos.Any(n => n.Name.Equals(name)))
                        {
                            listfotos.Add(new ViewModelFoto()
                            {
                                Path = path,
                                Name = name,
                                OriginalFileName = name.Substring(name.LastIndexOf("/") + 1),
                                StrBytes = base64
                            });
                        }
                    }
                    ListFiles = listfotos;
                }
                resultStatus = true;
            }
            catch
            {

            }


            return new JsonResult()
            {
                Data = new
                {
                    listImages = listfotos,
                    status = resultStatus ? "OK" : "NOK"
                }
            };
        }



        public JsonResult JsonRefresh(string id)
        {
            var resultStatus = false;
            var listfotos = new List<ViewModelFoto>();
            try
            {

                ListFiles = new List<ViewModelFoto>();
                if (!String.IsNullOrEmpty(id))
                {
                    var pathUploadCliente = Util.GetMapUpload();
                    var pathCliente = string.Format("{0}exames\\{1}\\{2}", pathUploadCliente, SessionsAdmin.EmpresaId, id);
                    if (Directory.Exists(pathCliente))
                    {
                        var ct = 0;
                        foreach (var file in Directory.GetFiles(pathCliente, @"*.*", SearchOption.TopDirectoryOnly))
                        {
                            if (!file.EndsWith(".zip"))
                            {
                                var fileName = Path.GetFileName(file);
                                var relativo = string.Concat(Util.GetUrlUpload(), "exames/", SessionsAdmin.EmpresaId, "/", id, "/", fileName);
                                var fisico = string.Concat(Util.GetMapUpload(), "exames/", SessionsAdmin.EmpresaId, "/", id, "/", fileName);

                                Image img = Image.FromFile(fisico);
                                var orientacao = "";
                                if (img.Width > img.Height)
                                    orientacao = "H";
                                else
                                    orientacao = "V";
                                ListFiles.Add(new ViewModelFoto() { ID = ct, Orientacao = orientacao, Path = fisico, Name = relativo, OriginalFileName = fileName });
                                ct++;
                            }
                        }
                    }
                    listfotos = ListFiles;
                }
                resultStatus = true;
            }
            catch
            {

            }

            return new JsonResult()
            {
                Data = new
                {
                    listImages = listfotos,
                    status = resultStatus ? "OK" : "NOK"
                }
            };
        }

        #endregion --- UPLOAD FILES ---

    }
}