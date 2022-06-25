using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using PortalVet.App_Start;
using PortalVet.Helper;
using PortalVet.Data.Helper;
using PortalVet.Models;
using PortalVet.Data.Interface;
using PortalVet.Data.Entity;
using System.Text;

namespace PortalVet.Areas.Administracao.Controllers
{
    public class UserController : BaseController
    {

        private readonly IAdminUserService _adminUserService;
        private readonly IEmpresaService _empresaService;
        public string _pathFolderUser = "user";

        public UserController(IAdminUserService adminUserService, IEmpresaService empresaService)
        {
            _adminUserService = adminUserService ??
             throw new ArgumentNullException(nameof(adminUserService));

            _empresaService = empresaService ??
                throw new ArgumentNullException(nameof(empresaService));
        }

        [SecurityPages]
        public ActionResult Index()
        {

            VMHome home = UtilAdmin.GetModelHome(this.RouteData, HttpContext.Session, Url);
            ViewData["hdnFilterEmpresaId"] = "";
            if (Request["eid"] != null)
            {
                ViewData["hdnFilterEmpresaId"] = Request["eid"].ToString();
            }

            var listEmpresa = _empresaService.ListEmpresaFull();
            var listEmp = listEmpresa.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nome }).ToList();
            ViewData["ListEmpresa"] = listEmp;

            var list = _adminUserService.ListPerfil();

            var listAll = list.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nome }).ToList();
            ViewData["ListPerfil"] = listAll;

            return View("Index", home);
        }



        private Dictionary<string, object> GetFilter(string filterEmpresaID, string filterPerfilID, string filterEmail)
        {
            Dictionary<string, object> filter = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(filterEmail))
                filter.Add("AdminUser.Email", filterEmail);


            if (!String.IsNullOrEmpty(filterPerfilID))
                filter.Add("AdminUserProfile.PROFILEID", Int32.Parse(filterPerfilID));


            if (!String.IsNullOrEmpty(filterEmpresaID))
                filter.Add("AdminUserCompany.CompanyId", Int32.Parse(filterEmpresaID));

            return filter.Count() == 0 ? null : filter;
        }


        public LargeJsonResult List(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            int pageIndex = 1;
            int pageSize = 20;
            int pgTotal = 0;
            int pageOrderCol = 0;
            string pageOrderSort = "ASC";
            if (form["pageIndex"] != null)
            {
                Int32.TryParse(form["pageIndex"].Trim(), out pageIndex);
            }

            if (form["pageSize"] != null)
            {
                Int32.TryParse(form["pageSize"].Trim(), out pageSize);
            }


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


            var filterEmpresaID = "";

            if (form["drpFilterEmpresa"] != null)
            {
                filterEmpresaID = form["drpFilterEmpresa"].ToString();
            }


            var filterPerfilID = "";

            if (form["drpFilterPerfil"] != null)
            {
                filterPerfilID = form["drpFilterPerfil"].ToString();
            }


            var filterEmail = "";

            if (form["txtFilterEmail"] != null)
            {
                filterEmail = form["txtFilterEmail"].ToString();
            }


            var data = _adminUserService.List(out pgTotal, pageIndex, pageSize, pageOrderCol, pageOrderSort, GetFilter(filterEmpresaID, filterPerfilID, filterEmail));

            jsonResultado.Data = data;
            jsonResultado.PageTotal = pgTotal;
            jsonResultado.PageIndex = pageIndex;
            jsonResultado.PageSize = pageSize;
            return new LargeJsonResult { Data = jsonResultado };
        }

        public LargeJsonResult EnviarCredenciais(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            int id = 0;
            int empresaId = SessionsAdmin.EmpresaId;
            try
            {
                if (form["userid"] != null)
                {
                    Int32.TryParse(form["userid"].Trim(), out id);
                    var user = _adminUserService.GetById(id);

                    var empresa = _empresaService.Carregar(empresaId);

                    var body = Util.GetResourcesNovaSenha();
                    body = body.Replace("#NOME#", user.Nome);
                    var key = HttpUtility.UrlEncode(Data.Helper.Util.Encriptar(string.Format("{0}|{1}", user.Email, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                    body = body.Replace("#LINK#", $"{System.Configuration.ConfigurationManager.AppSettings["UrlRootAdmin"].ToString()}newpassword?key={key}");
                    var bodyMaster = Util.CarregaConteudoMaster(empresa, body);
                    Util.SendEmail("Recuperação de senha", user.Email, bodyMaster);
                    jsonResultado.Data = true;
                }
            }
            catch (Exception ex)
            {
                jsonResultado.Data = false;
                jsonResultado.Message = ex.Message;
            }

            return new LargeJsonResult { Data = jsonResultado };
        }

        public LargeJsonResult LoginUser(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            int id = 0;
            jsonResultado.Data = id;
            try
            {
                if (form["id"] != null)
                {
                    Int32.TryParse(form["id"].Trim(), out id);
                    var user = _adminUserService.GetById(id);
                    jsonResultado.Data = user.Id;
                }
            }
            catch (Exception ex)
            {
                jsonResultado.Error = true;
                jsonResultado.Message = ex.Message;
            }

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
                    _adminUserService.RemoveUser(id);
                }
            }
            catch (Exception ex)
            {
                jsonResultado.Error = true;
                jsonResultado.Message = ex.Message;
            }

            return new LargeJsonResult { Data = jsonResultado };
        }


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
                    if (SessionsAdmin.UsuarioId == id)
                    {
                        SessionsAdmin.UsuarioPicture = picture;
                    }
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

            var entity = new AdminUserItem();
            if (!jsonResultado.Criticas.Any())
            {
                entity = _adminUserService.GetById(Int32.Parse(form["hdnId"].ToString()));
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

            var list = _adminUserService.ListPerfil();


            var listAll = list.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = Enumeradores.GetDescription((EnumAdminProfile)x.Id) }).ToList();
            ViewData["ListPerfil"] = listAll;


            var listEmpresa = _empresaService.ListEmpresaFull();
            var listEmpresaAll = listEmpresa.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nome }).ToList();
            ViewData["ListEmpresa"] = listEmpresaAll;


            home.PathUrlUser = string.Concat(Util.GetUrlUpload(), "user/");
            return View("Save", home);
        }


        public LargeJsonResult Save(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            AdminUserItem entity = new AdminUserItem();

            var _senha = "";
            if (form["txtNome"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtNome", Message = "Required" });
            }

            if (form["txtSobreNome"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtSobreNome", Message = "Required" });
            }

            if (form["txtEmail"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtEmail", Message = "Required" });
            }

            /*if (form["txtUsuario"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtUsuario", Message = "Required" });
            }*/

            //if (form["txtTelefone"].Trim().Length.Equals(0))
            //{
            //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtTelefone", Message = "Required" });
            //}

            //if (form["drpPerfil"].Trim().Length.Equals(0))
            //{
            //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpPerfil", Message = "Required" });
            //}
            /*
            int empresaId = 0;

            if (!form["drpEmpresa"].Trim().Length.Equals(0))
            {
                empresaId = Int32.Parse(form["drpEmpresa"].ToString());
            }*/

            if (form["hdnId"].Trim().Length.Equals(0))
            {
                if (form["txtSenha"].Trim().Length.Equals(0))
                {
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtSenha", Message = "Required" });
                }
                else
                {
                    _senha = form["txtSenha"].Trim();
                }
            }
            else
            {
                if (form["chkAlterarSenha"] != null)
                {
                    if (form["txtSenha"].Trim().Length.Equals(0))
                    {
                        jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtSenha", Message = "Required" });
                    }
                    else
                    {
                        _senha = form["txtSenha"].Trim();
                    }
                }

            }

            var _perfis = new List<AdminProfileItem>();
            if (form["drpPerfil"] == null || form["drpPerfil"].Trim().Length.Equals(0))
            {
                //jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpPerfil", Message = "Required" });
            }
            else
            {
                string[] codesStr = form["drpPerfil"].ToString().Split(',');
                var list = Array.ConvertAll(codesStr, int.Parse).ToList();
                list.ForEach(t =>
                {
                    _perfis.Add(new AdminProfileItem() { Id = t });
                });
            }



            var _empresas = new List<AdminCompanyItem>();
            if (form["drpEmpresas"] == null || form["drpEmpresas"].Trim().Length.Equals(0))
            {
                //jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpEmpresas", Message = "Required" });
            }
            else
            {
                string[] codesStr = form["drpEmpresas"].ToString().Split(',');
                var list = Array.ConvertAll(codesStr, int.Parse).ToList();
                list.ForEach(t =>
                {
                    _empresas.Add(new AdminCompanyItem() { Id = t });
                });
            }


            if (!jsonResultado.Criticas.Any())
            {
                int id = 0;
                if (!form["hdnId"].Trim().Length.Equals(0))
                {
                    id = Int32.Parse(form["hdnId"].ToString());
                }
                var message = "";
                entity = _adminUserService.SaveMultiPerfis(new AdminUserItem()
                {
                    Id = id,
                    Nome = form["txtNome"].ToString(),
                    Sobrenome = form["txtSobreNome"].ToString(),
                    Password = _senha,
                    Telefone = form["txtTelefone"].ToString(),
                    Email = form["txtEmail"].ToString(),
                    Perfis = _perfis,
                    Empresas = _empresas,
                    Active = form["chkAtivo"] != null ? true : false
                }, out message);


                if (!String.IsNullOrEmpty(message))
                {
                    jsonResultado.Message = message;
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtEmail", Message = "Required" });
                    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtTelefone", Message = "Required" });
                }
            }


            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = entity;

            return new LargeJsonResult { Data = jsonResultado };
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
    }
}