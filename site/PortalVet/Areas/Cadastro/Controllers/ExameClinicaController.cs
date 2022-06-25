using PortalVet.App_Start;
using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Data.Interface;
using PortalVet.Data.Service;
using PortalVet.Helper;
using PortalVet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PortalVet.Areas.Cadastro.Controllers
{

    public class ExameClinicaController : BaseController
    {
        private readonly IExameService _exameService;
        private readonly IAdminUserService _adminUserService;
        public ExameClinicaController(IAdminUserService adminUserService, IExameService exameService)
        {
            _adminUserService = adminUserService ??
                 throw new ArgumentNullException(nameof(adminUserService));

            _exameService = exameService ??
                 throw new ArgumentNullException(nameof(exameService));
        }


        // GET: Cadastro/Exame
        public ActionResult Index()
        {
            VMHome home = UtilAdmin.GetModelHome(this.RouteData, HttpContext.Session, Url);


            var listStatus = Enum.GetValues(typeof(EnumExameSituacao)).Cast<EnumExameSituacao>().ToList();

            List<SelectListItem> statusFinal = new List<SelectListItem>();
            foreach (var item in listStatus)
            {
                if ((EnumExameSituacao)item.GetHashCode() != EnumExameSituacao.Criacao)
                {
                    var text = Enumeradores.GetDescription((EnumExameSituacao)item.GetHashCode());
                    statusFinal.Add(new SelectListItem() { Text = text, Value = item.GetHashCode().ToString() });
                }
            }
            ViewData["ListSituacao"] = statusFinal;

            var listCliente = _exameService.CarregarClientes(home.EmpresaId);
            var listClienteFinal = listCliente.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome} ({x.Email})" }).ToList();
            ViewData["ListCliente"] = listClienteFinal;

            var listLaudador = _exameService.CarregarLaudadores(home.EmpresaId);
            var listLaudadorFinal = listLaudador.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome} ({x.Email})" }).ToList();
            ViewData["ListLaudador"] = listLaudadorFinal;


            var listRaca = _exameService.CarregarRacas(home.EmpresaId);
            var listRacaFinal = listRaca.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome}" }).ToList();
            ViewData["ListRaca"] = listRacaFinal;


            return View("Index", home);
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
            }

            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = entity;

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

            if (!String.IsNullOrEmpty(filterCodigo))
            {
                filter.Add("Exame.Id", filterCodigo);
            }

            filter.Add("Exame.SituacaoId!=", EnumExameSituacao.Criacao.GetHashCode().ToString());

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


            var listRaca = _exameService.CarregarRacas(home.EmpresaId);
            var listRacaFinal = listRaca.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome}" }).ToList();
            ViewData["ListRaca"] = listRacaFinal;


            var listCliente = _exameService.CarregarClientes(home.EmpresaId);
            var listClienteFinal = listCliente.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome} ({x.Email})" }).ToList();
            ViewData["ListCliente"] = listClienteFinal;


            var listLaudador = _exameService.CarregarLaudadores(home.EmpresaId);
            var listLaudadorFinal = listLaudador.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = $"{x.Nome} ({x.Email})" }).ToList();
            ViewData["ListLaudador"] = listLaudadorFinal;


            var listStatus = Enum.GetValues(typeof(EnumExameSituacao)).Cast<EnumExameSituacao>().ToList();

            List<SelectListItem> statusFinal = new List<SelectListItem>();
            foreach (var item in listStatus)
            {
                if (//(EnumExameSituacao)item.GetHashCode() == EnumExameSituacao.Em_Analise_Clinica ||
                   // (EnumExameSituacao)item.GetHashCode() == EnumExameSituacao.Em_Analise_Gerente ||
                    (EnumExameSituacao)item.GetHashCode() == EnumExameSituacao.Em_Analise_Laudador)
                {
                    var text = Enumeradores.GetDescription((EnumExameSituacao)item.GetHashCode());
                    statusFinal.Add(new SelectListItem() { Text = text, Value = item.GetHashCode().ToString() });
                }
            }
            ViewData["ListSituacao"] = statusFinal;

            ViewData["hdnEmpresaId"] = home.EmpresaId;

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

            int empresaId = Int32.Parse(form["hdnEmpresaId"].ToString());


            int idexist = 0;

            if (!jsonResultado.Criticas.Any())
            {
                var message = "";
                int profileId = EnumAdminProfile.Laudador.GetHashCode();
                var registroLaudador = new AdminUserItem()
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
                };
                entity = _adminUserService.SaveUserPerfilDireto(registroLaudador, out message, out idexist);


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
                else
                {
                    if (codigo == 0)
                    {
                        var empresasNome = _adminUserService.CarregarPerfisEmpresas(registroLaudador.Id);
                        StringBuilder sbText = new StringBuilder();
                        sbText.AppendLine($"Olá, {registroLaudador.Nome}! <br/><br/>");
                        sbText.AppendLine($"ATENÇÃO CLÍNICA<br/><br/>");
                        sbText.AppendLine($"{empresasNome} <br/>");

                        sbText.AppendLine("Seja bem vindo ao portal WEBIMAGEM<br/>");
                        sbText.AppendLine("Local onde terá acesso às suas imagens e laudos dos exames realizados!");
                        sbText.AppendLine("<br/><br/><hr/>");

                        sbText.AppendLine("Seguem as credenciais<br/><br/>");
                        //sbText.AppendLine("Seguem os dados para primeiro acesso ao sistema<br/>");
                        sbText.AppendLine("E-mail: <strong>" + registroLaudador.Email + "</strong>");
                        sbText.AppendLine("Senha: <strong>" + registroLaudador.Password + "</strong>");
                        sbText.AppendLine("<br/><hr/>");
                        EnviarEmailMensagem(registroLaudador.CompanyId, "Credenciais", sbText.ToString(), registroLaudador.Email);




                        //var body = Util.GetResourcesBemVindoCredenciais();
                        //body = body.Replace("#CLINICA#", _paramClinica);
                        //body = body.Replace("#EMAIL#", _paramClinicaEmail);
                        //body = body.Replace("#SENHA#", _senha);

                        //body = body.Replace("#LINK#", $"{System.Configuration.ConfigurationManager.AppSettings["UrlRootAdmin"].ToString()}");

                        //var bodyMaster = Util.CarregaConteudoMaster(empresa, body);
                        //Util.SendEmail($"Credenciais", _paramClinicaEmail, bodyMaster);
                    }
                }
            }

            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = new { Data = entity, IDExist = idexist };

            return new LargeJsonResult { Data = jsonResultado };
        }


        public void EnviarEmailMensagem(int empresaId, string assunto,  string mensagem, string email)
        {
            var empresa = new EmpresaService().Carregar(empresaId);
            var body = Util.GetResourcesMensagemModeloBotao();
            body = body.Replace("#ALIAS#","Acessar");
            body = body.Replace("#LINK#", "https://webimagem.vet.br/admin");
            body = body.Replace("#MENSAGEM#", mensagem);
            var bodyMaster = Util.CarregaConteudoMaster(empresa, body);
            Util.SendEmail($"{empresa.Nome} : {assunto}", email, bodyMaster);

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

            int empresaId = Int32.Parse(form["hdnEmpresaId"].ToString());


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
        public LargeJsonResult Save(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            var entity = new ExameItem();

            var codigo = 0;
            if (!form["hdnId"].Trim().Length.Equals(0))
            {
                codigo = Int32.Parse(form["hdnId"].ToString());
            }

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

            var _veterinario = "";
            if (!form["txtVeterinario"].Trim().Length.Equals(0))
            {
                //jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtDataExame", Message = "Required" });
                //}
                //else
                //{
                _veterinario = form["txtVeterinario"].Trim();
            }
            var _proprietario = "";
            if (!form["txtProprietario"].Trim().Length.Equals(0))
            {
                //jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtDataExame", Message = "Required" });
                //}
                //else
                //{
                _proprietario = form["txtProprietario"].Trim();
            }



            var _paciente = "";
            if (!form["txtPaciente"].Trim().Length.Equals(0))
            {
                //jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtDataExame", Message = "Required" });
                //}
                //else
                //{
                _paciente = form["txtPaciente"].Trim();
            }

            var _especie = "";
            if (!form["txtEspecie"].Trim().Length.Equals(0))
            {
                //jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtDataExame", Message = "Required" });
                //}
                //else
                //{
                _especie = form["txtEspecie"].Trim();
            }


            int _situacaoId = 0;
            if (form["drpSituacao"] == null || form["drpSituacao"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpSituacao", Message = "Required" });
            }
            else
            {
                _situacaoId = Int32.Parse(form["drpSituacao"].Trim());
            }
            var _racaId = 0;
            if (form["drpRaca"] == null || form["drpRaca"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpRaca", Message = "Required" });
            }
            else
            {
                _racaId = Int32.Parse(form["drpRaca"].Trim());
            }

            var _laudadorId = 0;
            if (form["drpLaudador"] == null || form["drpLaudador"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpLaudador", Message = "Required" });
            }
            else
            {
                _laudadorId = Int32.Parse(form["drpLaudador"].Trim());
            }

            var _clienteId = 0;
            if (form["drpCliente"] == null || form["drpCliente"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpCliente", Message = "Required" });
            }
            else
            {
                _clienteId = Int32.Parse(form["drpCliente"].Trim());
            }




            var _idade = "";
            if (!form["txtIdade"].Trim().Length.Equals(0))
            {
                //jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtDataExame", Message = "Required" });
                //}
                //else
                //{
                _idade = form["txtIdade"].Trim();
            }

            var _editorTexto = "";
            if (!form["editorTexto"].Trim().Length.Equals(0))
            {
                //jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtDataExame", Message = "Required" });
                //}
                //else
                //{
                _editorTexto = form["editorTexto"].Trim();
            }


            if (!jsonResultado.Criticas.Any())
            {
                try
                {
                    var dataHoraExame = new DateTime(dataExame.Value.Year, dataExame.Value.Month, dataExame.Value.Day, hour, min, 0);

                    entity = _exameService.Save(new ExameItem()
                    {
                        Id = codigo,
                        DataExame = dataHoraExame,
                        Veterinario = _veterinario,
                        Paciente = _paciente,
                        Proprietario = _proprietario,
                        Descricao = _editorTexto,
                        Idade = _idade,
                        //Especie = _especie,
                        RacaId = _racaId,
                        ClienteId = _clienteId,
                        LaudadorId = _laudadorId,
                        SituacaoId = _situacaoId,
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

                        EnviarEmailFluxo(_situacaoId, entity.Id, SessionsAdmin.EmpresaId);
                    }
                }
                catch (Exception ex)
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "MENSAGEM", Message = ex.Message });
                }
            }
            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = new { Data = entity };

            return new LargeJsonResult { Data = jsonResultado };
        }


        public void EnviarEmailFluxo(int situacaoId, int exameId, int companyId)
        {
            //if (((EnumExameSituacao)situacaoId) == EnumExameSituacao.Em_Analise_Clinica)
            //{
            //    // NOTIFICAR CLINICA(S) VINCULADAS COM EMPRESAID
            //}

            //if (((EnumExameSituacao)situacaoId) == EnumExameSituacao.Em_Analise_Gerente)
            //{
            //    // NOTIFICAR GERENTES(S) VINCULADAS COM EMPRESAID
            //}

            if (((EnumExameSituacao)situacaoId) == EnumExameSituacao.Em_Analise_Laudador)
            {
                // NOTIFICAR LAUDADOR VINCULADO COM EMPRESAID E EXAMEID
            }

            if (((EnumExameSituacao)situacaoId) == EnumExameSituacao.Concluido_Gerente)
            {
                // NOTIFICAR GERENTES(S) VINCULADAS COM EMPRESAID
            }

            if (((EnumExameSituacao)situacaoId) == EnumExameSituacao.Concluido)
            {
                // NOTIFICAR ????
            }
        }

    }
}