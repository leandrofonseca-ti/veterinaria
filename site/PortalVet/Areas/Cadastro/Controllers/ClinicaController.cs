using PortalVet.App_Start;
using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Data.Interface;
using PortalVet.Helper;
using PortalVet.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalVet.Areas.Cadastro.Controllers
{   
    public class ClinicaController : BaseController
    {
        private readonly IAdminCompanyService _adminCompanyService;
        public string _pathFolder = "empresas";

        public ClinicaController(IAdminCompanyService adminCompanyService)
        {
            _adminCompanyService = adminCompanyService ??
             throw new ArgumentNullException(nameof(adminCompanyService));
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



        private Dictionary<string, object> GetFilter()
        {
            Dictionary<string, object> filter = new Dictionary<string, object>();


            if(SessionsAdmin.PerfilId == EnumAdminProfile.Gerente.GetHashCode())
            {
                    filter.Add("company.Id", SessionsAdmin.EmpresaId);
            }
            filter.Add("company.Ativo", true);

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


            var data = _adminCompanyService.List(out pgTotal, pageIndex, pageSize, pageOrderCol, pageOrderSort,
                 GetFilter());

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

                    _adminCompanyService.Remove(id);
                }
            }
            catch (Exception ex)
            {
                jsonResultado.Error = true;
                jsonResultado.Message = ex.Message;
            }

            return new LargeJsonResult { Data = jsonResultado };
        }

         
        public LargeJsonResult Load(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            if (form["hdnId"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "hdnId", Message = "Required" });
            }

            var entity = new AdminCompanyItem();
            if (!jsonResultado.Criticas.Any())
            {
                entity = _adminCompanyService.Get(Int32.Parse(form["hdnId"].ToString()));
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
            home.PathUrlUser = String.Concat(Util.GetUrlUpload(), _pathFolder, "/");
            ViewData["hdnEmpresaId"] = home.EmpresaId;

            return View("Save", home);
        }



        public LargeJsonResult Save(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            var entity = new AdminCompanyItem();

            var imagem = "";
            var codigo = 0;
            if (!form["hdnId"].Trim().Length.Equals(0))
            {
                codigo = Int32.Parse(form["hdnId"].ToString());
            }

            if (!form["hdnPicture"].Trim().Length.Equals(0))
            {
                imagem = form["hdnPicture"].Trim();
            }

            if (form["txtNome"].Trim().Length.Equals(0))
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtNome", Message = "Required" });
            }

            var whatsapp = "";
            var email = "";
            var url = "";
            if (!form["txtWhatsapp"].Trim().Length.Equals(0))
            {
                whatsapp = form["txtWhatsapp"].Trim();
            }

            if (!form["txtEmailCompany"].Trim().Length.Equals(0))
            {
                email = form["txtEmailCompany"].Trim();
            }
            if (!form["txtUrl"].Trim().Length.Equals(0))
            {
                url = form["txtUrl"].Trim();
            }

            var _texto = "";
            if (!form["txtTexto"].Trim().Length.Equals(0))
            {
                _texto = form["txtTexto"].Trim();
            }

            if (!jsonResultado.Criticas.Any())
            {
                try
                {
                    entity = _adminCompanyService.Save(new AdminCompanyItem()
                    {
                        Id = codigo,
                        Nome = form["txtNome"].ToString(),
                        Texto = _texto,
                        Imagem = imagem,
                        Whatsapp = whatsapp,
                        Email = email,
                        Url = url,
                        Ativo = true
                    });
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


        #region FILE SAVE


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
                   _adminCompanyService.SaveImageUser(id, picture);
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
                        //string folderFile = string.Format("{0}/{1}/{2}", Url.Content(_PathVirtual), empresaid, form["oldfile"].ToString());
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
                        //var empresaid = SessionsAdmin.EmpresaId;
                        //string folderImob = string.Format("{0}", empresaid);
                        var fileName = modelFile.Name;
                        try
                        {
                            string location = Util.GetMapUpload();
                            location = string.Concat(location, _pathFolder);
                            if (!System.IO.Directory.Exists(location))
                            {
                                System.IO.Directory.CreateDirectory(location);
                            }

                            //this.Request.Files[0].SaveAs(string.Concat(location, @"\", modelFile.NameInit));
                            var pathServer = string.Concat(location, @"\", modelFile.Name);

                            this.Request.Files[0].SaveAs(pathServer);
                            //GerarProporcao(string.Concat(location, @"\", modelFile.NameInit), pathServer, 128, 128, Color.Transparent);

                            //if (System.IO.File.Exists(string.Concat(location, @"\", modelFile.NameInit)))
                            //{
                            //    System.IO.File.Delete(string.Concat(location, @"\", modelFile.NameInit));
                            //}

                            path = string.Concat(Util.GetUrlUpload(), _pathFolder);
                            Response.Write(string.Concat("{\"Status\":\"OK\", \"Path\": \"", path, "\", \"FileName\": \"", string.Format("{0}", fileName), "\"}")); //, \"FileNameThumb\": \"", string.Format("thumb_{0}", fileName), "\"
                        }
                        catch
                        {
                            Response.Write("{\"Status\":\"NOK\" }");
                        }

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

        private void GerarProporcao(string pathOrigem, string pathDestino, int height, int width, System.Drawing.Color color)
        {
            //E cria o thumb dinamicamente
            if (System.IO.File.Exists(pathOrigem))
            {
                int posX = 0;
                int posY = 0;
                int vWidth = 0;
                int vHeight = 0;
                System.Drawing.Image imgPhotoVert = System.Drawing.Image.FromFile(pathOrigem);
                System.Drawing.Bitmap bit = new System.Drawing.Bitmap(width, height);
                if (imgPhotoVert.Height > imgPhotoVert.Width)
                {
                    vHeight = height;
                    vWidth = (imgPhotoVert.Width * height) / imgPhotoVert.Height;


                    while (vWidth > width)
                    {
                        vHeight -= 1;
                        vWidth = (imgPhotoVert.Width * vHeight) / imgPhotoVert.Height;
                    }

                }
                else
                {

                    vWidth = width;
                    vHeight = (imgPhotoVert.Height * width) / imgPhotoVert.Width;


                    while (vHeight > height)
                    {
                        vWidth -= 1;
                        vHeight = (imgPhotoVert.Height * vWidth) / imgPhotoVert.Width;
                    }

                }

                posX = width - vWidth;
                posX = posX / 2;
                posY = height - vHeight;
                posY = posY / 2;

                // Add image generation logic here and return an instance of ImageInfo
                System.Drawing.Graphics gra = System.Drawing.Graphics.FromImage(bit);

                gra.Clear(color);
                gra.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gra.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gra.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                System.Drawing.Rectangle oRectangle = new System.Drawing.Rectangle(posX, posY, vWidth, vHeight);
                gra.DrawImage(imgPhotoVert, oRectangle);

                bit.Save(pathDestino, System.Drawing.Imaging.ImageFormat.Png);
                imgPhotoVert.Dispose();
                bit.Dispose();
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
            var fileNameInit = "";
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
                    //strBytes = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                case ".gif":
                    ext = ".gif";
                    //strBytes = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case ".jpg":
                case ".jpeg":
                    ext = ".jpeg";
                    //strBytes = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case ".png":
                    ext = ".png";
                    //strBytes = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);


                    break;
            }


            if (!String.IsNullOrEmpty(ext))
            {
                var prefix = "file";
                var dt = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                fileNameInit = string.Format("{0}Init{1}{2}", prefix, dt, ext);
                fileName = string.Format("{0}{1}{2}", prefix, dt, ext);
            }
            return new ModelFoto()
            {
                Path = filePath,
                NameInit = fileNameInit,
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