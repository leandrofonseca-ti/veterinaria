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
using System.ServiceModel.Web;
using System.Web;
using System.Web.Mvc;

namespace PortalVet.Areas.Cadastro.Controllers
{
    [ValidateInput(false)]
    public class ContratoModeloController : BaseController
    {
        private readonly IContratoModeloService _contratoModeloService;

        private readonly IAssinaturaService _assinaturaService;

        public ContratoModeloController(IContratoModeloService contratoModeloService, IAssinaturaService assinaturaService)
        {
            _contratoModeloService = contratoModeloService ??
                    throw new ArgumentNullException(nameof(contratoModeloService));

            _assinaturaService = assinaturaService ??
                    throw new ArgumentNullException(nameof(assinaturaService));
        }

        [HttpGet]
        [SecurityPages]
        [ValidateInput(false)]
        public ActionResult Index()
        {
            VMHome home = UtilAdmin.GetModelHome(this.RouteData, HttpContext.Session, Url);

            return View("Index", home);
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

            var data = _contratoModeloService.List(out pgTotal, pageIndex, pageSize, SessionsAdmin.UsuarioId, SessionsAdmin.PerfilId);

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

                    _contratoModeloService.RemoveDocumento(id);

                    int usuarioId = SessionsAdmin.UsuarioId;
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


        [HttpGet]
        [SecurityPages]
        [ValidateInput(false)]
        public ActionResult Save(string id)
        {
            VMHome home = UtilAdmin.GetModelHome(this.RouteData, HttpContext.Session, Url);





            if (!string.IsNullOrEmpty(id))
            {
                int documentoId = int.Parse(id);

                home.ID = documentoId.ToString();
            }

            ViewData["ListRodape"] = new List<SelectListItem>();
            var listagem = _assinaturaService.ListarLaudador(SessionsAdmin.UsuarioId);
            var auxList = listagem.Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Nome }).ToList();
            ViewData["ListRodape"] = auxList;
            return View("Save", home);
        }

        [WebGet(UriTemplate = "CarregarVariaveis", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult CarregarVariaveis(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            var listaVariavel = _contratoModeloService.CarregarVariaveis();

            jsonResultado.Data = listaVariavel;

            return new LargeJsonResult { Data = jsonResultado };
        }



        [WebGet(UriTemplate = "UploadImageCkeditor", ResponseFormat = WebMessageFormat.Json)]
        public LargeJsonResult UploadImageCkeditor()
        {
            FileUploadCkeditor data = new FileUploadCkeditor();

            if (this.Request.Files.Count > 0)
            {
                if (this.Request.Files[0] != null)
                {
                    var modelFile = SaveFile(this.Request.Files[0]);

                    if (modelFile != null && !String.IsNullOrEmpty(modelFile.Name))
                    {
                        try
                        {
                            string location = Util.GetMapUpload();

                            location = string.Concat(location, "ckeditor_upload/");

                            if (!System.IO.Directory.Exists(location))
                            {
                                System.IO.Directory.CreateDirectory(location);
                            }

                            modelFile.Image.Save(string.Concat(location, @"\", modelFile.Name), ImageFormat.Jpeg);

                            string path = string.Concat(Util.GetUrlUpload(), "ckeditor_upload/");

                            data.url = path + modelFile.Name;
                            data.upload = true;
                        }
                        catch
                        {
                            data.upload = false;
                            data.error = new FileUploadCkeditorError
                            {
                                mensagem = "Erro ao realizar o upload"
                            };
                        }
                    }
                    else
                    {
                        data.upload = false;
                        data.error = new FileUploadCkeditorError
                        {
                            mensagem = "Erro ao realizar o upload"
                        };
                    }
                }
                else
                {
                    data.upload = false;
                    data.error = new FileUploadCkeditorError
                    {
                        mensagem = "Erro ao realizar o upload"
                    };
                }
            }
            else
            {
                data.upload = false;
                data.error = new FileUploadCkeditorError
                {
                    mensagem = "Erro ao realizar o upload"
                };
            }

            return new LargeJsonResult { Data = data };
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

        [HttpPost, ValidateInput(false)]
        public LargeJsonResult Save(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            DocumentoModeloItem entidade = new DocumentoModeloItem();

            if (!string.IsNullOrEmpty(form["hdnEmpresaId"]))
            {
                entidade.CompanyId = Int32.Parse(form["hdnEmpresaId"].ToString());
            }
            else
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "hdnEmpresaId", Message = "Required" });
            }

            if (!string.IsNullOrEmpty(form["txtNomeModelo"]))
            {
                entidade.Nome = form["txtNomeModelo"];
            }
            else
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "txtNomeModelo", Message = "Required" });
            }

            //if (!string.IsNullOrEmpty(form["drpModalidade"]))
            //{
            //    modalidades = form["drpModalidade"].Split(',').ToList();
            //}
            //else
            //{
            //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpModalidade", Message = "Required" });
            //}

            //if (!string.IsNullOrEmpty(form["drpPerfil"]))
            //{
            //    entidade.Perfil = form["drpPerfil"];
            //}
            //else
            //{
            //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpModalidade", Message = "Required" });
            //}

            //if (!string.IsNullOrEmpty(form["txtLocatorios"]))
            //{
            //    entidade.QuantidadeLocatorio = int.Parse(form["txtLocatorios"]);
            //}
            //else
            //{
            //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpModalidade", Message = "Required" });
            //}

            //entidade.QuantidadeFiador = null;


            //if (!string.IsNullOrEmpty(form["editorCabecalho"]))
            //{
            //    entidade.ModeloCabecalho = form["editorCabecalho"];
            //}
            //else
            //{
            //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "editorCabecalho", Message = "Required" });
            //}

            if (!string.IsNullOrEmpty(form["editorCorpo"]))
            {
                entidade.ModeloCorpo = form["editorCorpo"];
            }
            else
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "editorCorpo", Message = "Required" });
            }
            entidade.AssinaturaId = 0;
            //if (!string.IsNullOrEmpty(form["drpRodape"]))
            //{
            //    entidade.AssinaturaId = Int32.Parse(form["drpRodape"].ToString());
            //}
            //else
            //{
            //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "drpRodape", Message = "Required" });
            //}

            //if (!string.IsNullOrEmpty(form["editorRodape"]))
            //{
            //    entidade.ModeloRodape = form["editorRodape"];
            //}
            //else
            //{
            //    jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "editorCorpo", Message = "Required" });
            //}


            entidade.LaudadorId = SessionsAdmin.UsuarioId;

            if (!jsonResultado.Criticas.Any())
            {
                if (!form["hdnId"].Trim().Length.Equals(0))
                {
                    entidade.Id = Int32.Parse(form["hdnId"].ToString());
                }

                entidade = _contratoModeloService.SalvarDocumento(entidade);

                jsonResultado.MessageTipo = "success";
                jsonResultado.Message = "Operação realizada com sucesso.";
                jsonResultado.Status = "OK";
            }

            jsonResultado.Error = jsonResultado.Criticas.Any();
            jsonResultado.Data = entidade;

            return new LargeJsonResult { Data = jsonResultado };
        }

        public LargeJsonResult Load(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);

            if (form["hdnId"] != null)
            {
                int documentoId = int.Parse(form["hdnId"].Trim());

                DocumentoModeloItem documento = _contratoModeloService.CarregarDocumento(documentoId);

                jsonResultado.Data = documento;
            }
            else
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "code", Message = "Dados incorretos." });
            }

            return new LargeJsonResult { Data = jsonResultado };
        }




        public ActionResult ViewContratoModeloPDF(string id)
        {
            return new Rotativa.ActionAsPdf("ViewContratoModelo", new { id = id });
        }

        public ActionResult ViewContratoModelo(string id)
        {
            int _id = 0;
            if (!String.IsNullOrEmpty(id))
            {
                Int32.TryParse(id, out _id);
            }
            ViewData["imagemCompanyNome"] = "";
            ViewData["imagemCompany"] = "";

            ViewData["modeloCabecalho"] = "";
            ViewData["modeloCorpo"] = "";
            ViewData["modeloRodape"] = "";
            if (_id > 0)
            {
                try
                {
                    var data = _contratoModeloService.CarregarDocumento(_id);

                    var empresa = new Data.Service.EmpresaService().Carregar(data.CompanyId);
                    if (empresa != null)
                    {
                        if (!String.IsNullOrEmpty(empresa.Imagem))
                            ViewData["imagemCompany"] = $"{Util.GetUrlUpload()}empresas/{empresa.Imagem}";
                        else
                            ViewData["imagemCompanyNome"] = $"{empresa.Nome}";
                    }
                    //var data = _contratoModeloService.CarregarDocumentoVersao(_id);

                    ViewData["modeloCabecalho"] = data.ModeloCabecalho ?? string.Empty;
                    ViewData["modeloCorpo"] = data.ModeloCorpo ?? string.Empty;
                    ViewData["modeloRodape"] = data.ModeloRodape ?? string.Empty;
                }
                catch //(Exception ex)
                {
                }
            }

            return View("ViewContratoModelo");
        }
        #endregion SAVE
    }
}