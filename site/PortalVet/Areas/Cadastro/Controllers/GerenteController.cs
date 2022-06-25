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
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace PortalVet.Areas.Cadastro.Controllers
{
    public class GerenteController : BaseController
    {
        public string _pathFolderUser = "user";

        private readonly IAdminUserService _adminUserService;

        public GerenteController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService ??
             throw new ArgumentNullException(nameof(adminUserService));
        }


        [SecurityPages]
        public ActionResult Index()
        {

            VMHome home = UtilAdmin.GetModelHome(this.RouteData, HttpContext.Session, Url);

            ViewData["hdnEmpresaId"] = home.EmpresaId;
            //var listEmpresa = new EmpresaService().ListEmpresa();
            //var listEmp = listEmpresa.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nome }).ToList();
            //ViewData["ListEmpresa"] = listEmp;

            // var list = _service.ListPerfil();

            //var list = list.Where(x=>x.PerfilId == EnumAdminProfile.Imobiliaria);

            //var listAll = list.Select(x => new SelectListItem() { Value = x.PerfilId.ToString(), Text = x.Nome }).ToList();
            //ViewData["ListPerfil"] = listAll;

            return View("Index", home);
        }



        private Dictionary<string, object> GetFilter(string filterEmpresaID, EnumAdminProfile filterPerfilID, string filterEmail, string filterNome, string filterCPFCNPJ, string filterTelefone)
        {
            Dictionary<string, object> filter = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(filterEmail))
                filter.Add("adminuser.Email", filterEmail);

            if (!String.IsNullOrEmpty(filterNome))
                filter.Add("adminuser.Nome", filterNome);

            if (!String.IsNullOrEmpty(filterTelefone))
                filter.Add("adminuser.Telefone", filterTelefone);

            if (!String.IsNullOrEmpty(filterCPFCNPJ))
                filter.Add("adminuser.CPFCNPJ", filterCPFCNPJ);

            if (filterPerfilID != EnumAdminProfile.NULL)
                filter.Add("adminuserprofile.ProfileId", filterPerfilID.GetHashCode());


            if (!String.IsNullOrEmpty(filterEmpresaID))
                filter.Add("adminusercompany.CompanyId", Int32.Parse(filterEmpresaID));


            //filter.Add("AdminUser.Excluido", false);

            return filter.Count() == 0 ? null : filter;
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


            var filterEmpresaID = form["hdnEmpresaId"].ToString();

            var filterEmail = "";
            var filterNome = "";
            var filterCPFCNPJ = "";
            var filterTelefone = "";
            if (form["txtFilterEmail"] != null)
            {
                filterEmail = form["txtFilterEmail"].ToString();
            }
            if (form["txtFilterNome"] != null)
            {
                filterNome = form["txtFilterNome"].ToString();
            }
            if (form["txtFilterCPFCNPJ"] != null)
            {
                filterCPFCNPJ = form["txtFilterCPFCNPJ"].ToString().Replace(".", "").Replace("-", "").Replace("\\", "").Replace("/", "");
            }
            if (form["txtFilterTelefone"] != null)
            {
                filterTelefone = form["txtFilterTelefone"].ToString().Replace(" ", "");
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

            var filterPerfilID = EnumAdminProfile.Gerente;


            var data = _adminUserService.ListUserPerfil(out pgTotal, pageIndex, pageSize, pageOrderCol, pageOrderSort,
                GetFilter(filterEmpresaID, filterPerfilID, filterEmail, filterNome, filterCPFCNPJ, filterTelefone));

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

                    _adminUserService.DesvincularAdminUserProfile(id, EnumAdminProfile.Gerente, SessionsAdmin.EmpresaId);
                }
            }
            catch (Exception ex)
            {
                jsonResultado.Error = true;
                jsonResultado.Message = ex.Message;
            }

            return new LargeJsonResult { Data = jsonResultado };
        }


        //public LargeJsonResult SaveImage2(FormCollection form)
        //{
        //    JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

        //    int id = 0;

        //    try
        //    {
        //        if (form["id"] != null && form["image"] != null)
        //        {
        //            Int32.TryParse(form["id"].Trim(), out id);
        //            var picture = form["image"].ToString();
        //            // _service.SaveImageUser(id, picture);
        //            if (SessionsAdmin.UsuarioId == id)
        //            {
        //                SessionsAdmin.UsuarioPicture = picture;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonResultado.Error = true;
        //        jsonResultado.Message = ex.Message;
        //    }

        //    return new LargeJsonResult { Data = jsonResultado };
        //}


        public LargeJsonResult SaveImage(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            int id = 0;

            try
            {
                if (form["id"] != null && form["image"] != null)
                {
                    Int32.TryParse(form["id"].Trim(), out id);
                    var picture = form["image"].ToString();
                    _adminUserService.SaveImageUser(id, picture);
                }

            }
            catch (Exception ex)
            {
                jsonResultado.Error = true;
                jsonResultado.Message = ex.Message;
            }
            try
            {

                if (form["oldfile"] != null)
                {
                    try
                    {
                        var empresaid = SessionsAdmin.EmpresaId;
                        string folderFile = string.Concat(Util.GetMapUpload(), empresaid, form["oldfile"].ToString());
                        if (System.IO.File.Exists(folderFile))
                        {
                            System.IO.File.Delete(folderFile);
                        }
                    }
                    catch { }
                }
            }
            catch
            {

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

            var entity = new AdminUserItem();
            if (!jsonResultado.Criticas.Any())
            {
                entity = _adminUserService.GetById(Int32.Parse(form["hdnId"].ToString()), SessionsAdmin.EmpresaId);
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

            ViewData["hdnEmpresaId"] = home.EmpresaId;

            return View("Save", home);
        }
        public LargeJsonResult VincularExistente(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            try
            {
                var codigo = 0;
                if (!form["codigo"].Trim().Length.Equals(0))
                {
                    codigo = Int32.Parse(form["codigo"].ToString());
                }
                var result = _adminUserService.SaveAdminUserEmpresas(codigo, SessionsAdmin.EmpresaId);
                jsonResultado.Data = result;
            }
            catch { }
            return new LargeJsonResult { Data = jsonResultado };
        }

        public LargeJsonResult Save(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            var entity = new AdminUserItem();


            var codigo = 0;
            if (!form["hdnId"].Trim().Length.Equals(0))
            {
                codigo = Int32.Parse(form["hdnId"].ToString());
            }


            if (form["txtNome"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtNome", Message = "Required" });
            }

            if (form["txtEmail"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtNome", Message = "Required" });
            }
            else
            {
                if (!Util.IsEmail(form["txtEmail"].Trim()))
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtEmail", Message = "Required" });
                    jsonResultado.Message = "E-mail incorreto!";
                }
            }


            var alterarSenha = true;
            var senhaInformada = "";
            if (codigo > 0)
            {
                alterarSenha = form["chkHabilita"] != null ? true : false;
            }
            else
            {
                if (form["txtSenha"].Trim().Length.Equals(0) &&
                    form["txtSenhaConfirma"].Trim().Length.Equals(0))
                {
                    alterarSenha = false;
                }

            }

            if (alterarSenha)
            {
                if (form["txtSenha"].Trim().Length.Equals(0))
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtSenha", Message = "Required" });
                }

                if (form["txtSenhaConfirma"].Trim().Length.Equals(0))
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtSenhaConfirma", Message = "Required" });
                }
                if (!jsonResultado.Criticas.Any())
                {
                    if (form["txtSenhaConfirma"].ToString() != form["txtSenhaConfirma"].ToString())
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtSenha", Message = "Required" });
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtSenhaConfirma", Message = "Required" });
                    }
                    else
                    {
                        senhaInformada = form["txtSenha"].ToString().Trim();
                    }
                }
            }

            int empresaId = Int32.Parse(form["hdnEmpresaId"].ToString());


            int idexist = 0;

            if (!jsonResultado.Criticas.Any())
            {
                var message = "";
                int profileId = EnumAdminProfile.Gerente.GetHashCode();
                var registro = new AdminUserItem()
                {
                    Id = codigo,
                    PerfilId = (EnumAdminProfile)profileId,
                    CompanyId = empresaId,
                    Nome = form["txtNome"].ToString(),
                    CPFCNPJ = form["txtCPFCNPJ"].ToString().Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", ""),
                    Sobrenome = form["txtSobreNome"].ToString(),
                    Password = senhaInformada,
                    Telefone = form["txtTelefone"].ToString(),
                    Telefone2 = form["txtTelefone2"].ToString(),
                    Email = form["txtEmail"].ToString(),
                    Imagem = string.Empty,
                    Active = true
                };
                entity = _adminUserService.SaveUserPerfil(registro, out message, out idexist);


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
                        var empresasNome = _adminUserService.CarregarPerfisEmpresas(registro.Id);
                        StringBuilder sbText = new StringBuilder();
                        sbText.AppendLine($"Olá, {registro.Nome}! <br/><br/>");
                        sbText.AppendLine($"ATENÇÃO GERENTE<br/><br/>");
                        sbText.AppendLine($"{empresasNome} <br/>");

                        sbText.AppendLine("VOCÊ ESTÁ RECEBENDO SUAS SENHA DE ACESSO AO PORTAL WEBIMAGEM");
                        sbText.AppendLine("<br/><br/><hr/>");
                        sbText.AppendLine("Seguem as credenciais<br/><br/>");
                        sbText.AppendLine("E-mail: <strong>" + registro.Email + "</strong><br/>");
                        sbText.AppendLine("Senha: <strong>" + registro.Password + "</strong>");
                        sbText.AppendLine("<br/><hr/>");
                        EnviarEmailMensagem(registro.CompanyId, "Credenciais", sbText.ToString(), registro.Email);
                    }
                    else if (codigo > 0 && alterarSenha)
                    {
                        var empresasNome = _adminUserService.CarregarPerfisEmpresas(registro.Id);
                        StringBuilder sbText = new StringBuilder();
                        sbText.AppendLine($"Olá, {registro.Nome}! <br/><br/>");
                        sbText.AppendLine($"ATENÇÃO GERENTE<br/><br/>");
                        sbText.AppendLine($"{empresasNome} <br/>");
                        
                        sbText.AppendLine("VOCÊ ESTÁ RECEBENDO SUAS SENHA DE ACESSO AO PORTAL WEBIMAGEM");
                        sbText.AppendLine("<br/><br/><hr/>");

                        sbText.AppendLine("Seguem as credenciais<br/><br/>");
                        sbText.AppendLine("E-mail: <strong>" + registro.Email + "</strong><br/>");
                        sbText.AppendLine("Senha: <strong>" + registro.Password + "</strong>");
                        sbText.AppendLine("<br/><hr/>");
                        EnviarEmailMensagem(registro.CompanyId, "Credenciais", sbText.ToString(), registro.Email);

                    }
                }
            }


            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = new { Data = entity, IDExist = idexist };

            return new LargeJsonResult { Data = jsonResultado };
        }


        public void EnviarEmailMensagem(int empresaId, string assunto, string mensagem, string email)
        {
            var empresa = new EmpresaService().Carregar(empresaId);
            var body = Util.GetResourcesMensagemModeloBotao();
            body = body.Replace("#MENSAGEM#", mensagem);
            body = body.Replace("#LINK#", "https://webimagem.vet.br/Admin");
            body = body.Replace("#ALIAS#", "Acessar");
            var bodyMaster = Util.CarregaConteudoMaster(empresa, body);
            Util.SendEmail($"{empresa.Nome} : {assunto}", email, bodyMaster);

        }



        public void EnviarEmailMensagemComBotao(int empresaId, string assunto, string mensagem, string email)
        {
            var empresa = new EmpresaService().Carregar(empresaId);
            var body = Util.GetResourcesMensagemModelo();
            body = body.Replace("#MENSAGEM#", mensagem);
            var bodyMaster = Util.CarregaConteudoMaster(empresa, body);
            Util.SendEmail($"{empresa.Nome} : {assunto}", email, bodyMaster);

        }
        #endregion SAVE


        #region FILE SAVE



        public void FileSave()
        {
            if (this.Request.Files.Count > 0)
            {
                if (this.Request.Files[0] != null)
                {
                    var modelFile = SaveFile(this.Request.Files[0]);
                    var path = "";
                    if (modelFile != null && !String.IsNullOrEmpty(modelFile.Name))
                    {
                        try
                        {
                            string location = Util.GetMapUpload();
                            location = string.Concat(location, "user/");
                            if (!System.IO.Directory.Exists(location))
                            {
                                System.IO.Directory.CreateDirectory(location);
                            }

                            modelFile.Image.Save(string.Concat(location, @"\", modelFile.Name), ImageFormat.Jpeg);

                            //  Thumb
                            /*string locationThumb = string.Concat("~/files/thumb");
                            //Check if the directory exists
                            if (!System.IO.Directory.Exists(Server.MapPath(locationThumb)))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath(locationThumb));
                            }
                            */

                            // THUMB
                            //System.Drawing.Image imageThum = TratamentoImagem(modelFile.Image, new Size(100, 100));
                            //imageThum.Save(string.Concat(Server.MapPath(location), @"\thumb_", modelFile.Name), ImageFormat.Jpeg);

                            /*
                            if (_Domain_Admin.NAME_VALUE.EndsWith("/"))
                                path = string.Format("{0}upload/", _Domain_Admin.NAME_VALUE);
                            else
                                path = string.Format("{0}/upload/", _Domain_Admin.NAME_VALUE);*/

                            path = string.Concat(Util.GetUrlUpload(), "user/");
                        }
                        catch
                        {

                        }
                        Response.Write(string.Concat("{\"Status\":\"OK\", \"Path\": \"", path, "\", \"FileName\": \"", string.Format("{0}", modelFile.Name), "\", \"FileNameThumb\": \"", string.Format("thumb_{0}", modelFile.Name), "\"}"));
                    }
                    else
                    {
                        Response.Write("{\"Status\":\"NOK\" }");
                    }


                }
                else
                {
                    Response.Write("{\"Status\":\"NOK\" }");
                }
            }
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

        public ModelFoto SaveFile(HttpPostedFileBase httpPostedFile)
        {
            var strBytes = "";
            var fileName = "";
            var filePath = "";
            string ext = "";

            var imageAux = System.Drawing.Image.FromStream(httpPostedFile.InputStream);
            int w = 0;
            int h = 0;
            //TratamentoTamanhoImagem(imageAux, 1600, out w, out h);
            w = imageAux.Width;
            h = imageAux.Height;
            var image = imageAux; // TratamentoImagem(imageAux, new Size(w, h));

            switch (System.IO.Path.GetExtension(httpPostedFile.FileName).ToLower())
            {
                case ".bmp":
                    ext = ".bmp";
                    strBytes = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                case ".gif":
                    ext = ".gif";
                    strBytes = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case ".jpg":
                case ".jpeg":
                    ext = ".jpeg";
                    strBytes = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case ".png":
                    ext = ".png";
                    strBytes = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);
                    break;
            }


            if (!String.IsNullOrEmpty(ext))
            {
                var prefix = "file";
                fileName = string.Format("{0}{1}{2}", prefix, DateTime.Now.ToString("yyyyMMddHHmmssfff"), ext);
            }
            return new ModelFoto()
            {
                Path = filePath,
                Name = fileName,
                StrBytes = strBytes,
                Image = image
            };
        }

        public string ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        #endregion


        #region MENUS




        #endregion
    }
}