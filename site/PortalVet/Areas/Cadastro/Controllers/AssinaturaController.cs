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
    public class AssinaturaController : BaseController
    {

        private Dictionary<int, ViewModelFoto> listModelFotoSingle
        {
            get
            {
                if (HttpContext.Session[string.Format("ListModelFotosSingle_{0}", SessionsAdmin.UsuarioId)] == null)
                {
                    HttpContext.Session[string.Format("ListModelFotosSingle_{0}", SessionsAdmin.UsuarioId)] = new Dictionary<int, ViewModelFoto>();
                }
                return (Dictionary<int, ViewModelFoto>)HttpContext.Session[string.Format("ListModelFotosSingle_{0}", SessionsAdmin.UsuarioId)];
            }
            set
            {
                HttpContext.Session[string.Format("ListModelFotosSingle_{0}", SessionsAdmin.UsuarioId)] = value;
            }
        }


        private readonly IAssinaturaService _assinaturaService;

        public AssinaturaController(IAssinaturaService assinaturaService)
        {
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

            var data = _assinaturaService.Listar(out pgTotal, pageIndex, pageSize, SessionsAdmin.UsuarioId, SessionsAdmin.PerfilId);

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

                    _assinaturaService.Remover(id);

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



            return View("Save", home);
        }



        [HttpPost, ValidateInput(false)]
        public LargeJsonResult Save(FormCollection form)
        {
            JsonReturnJS jsonResultado = new JsonReturnJS(this.IsAuthenticated);
            AssinaturaItem entidade = new AssinaturaItem();

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



            if (!string.IsNullOrEmpty(form["txtNomeAssinatura"]))
            {
                entidade.AssinaturaNome = form["txtNomeAssinatura"];
            }

            if (!string.IsNullOrEmpty(form["txtCRMAssinatura"]))
            {
                entidade.AssinaturaCRM = form["txtCRMAssinatura"];
            }

            if (!string.IsNullOrEmpty(form["txtProfissaoAssinatura"]))
            {
                entidade.AssinaturaProfissao = form["txtProfissaoAssinatura"];
            }

            if (!string.IsNullOrEmpty(form["hdnArquivo1"]))
            {
                entidade.AssinaturaImagem = form["hdnArquivo1"];
            }

            entidade.LaudadorId = SessionsAdmin.UsuarioId;

            if (!jsonResultado.Criticas.Any())
            {
                if (!form["hdnId"].Trim().Length.Equals(0))
                {
                    entidade.Id = Int32.Parse(form["hdnId"].ToString());
                }

                entidade = _assinaturaService.Salvar(entidade);

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

                AssinaturaItem documento = _assinaturaService.Carregar(documentoId);

                jsonResultado.Data = documento;
            }
            else
            {
                jsonResultado.Criticas.Add(new JsonCriticaJS() { FieldId = "code", Message = "Dados incorretos." });
            }

            return new LargeJsonResult { Data = jsonResultado };
        }






        #endregion SAVE


        #region --- UPLOAD FILES ---


        public ActionResult UploadFileControl(string tid, string ismultiple, string email, string empresaid)
        {

            ViewData["hdnTipoId"] = tid;

            return View(new ViewModelUpload() { IsMultiple = ismultiple == "1", ClienteEmail = email, EmpresaId = empresaid });
        }


        [HttpPost]
        public JsonResult SubmitSaveFile(FormCollection form)
        {
            var resultJson = "{\"Status\":\"NOK\" }";
            if (this.Request.Files.Count > 0)
            {

                foreach (string upload in Request.Files)
                {
                    //var modelFile = SaveFile(Request.Files[upload]);
                    var fileName = "";
                    string ext = "";

                    switch (System.IO.Path.GetExtension(Request.Files[upload].FileName).ToLower())
                    {
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                        //case ".svg":
                            ext = System.IO.Path.GetExtension(Request.Files[upload].FileName).ToLower();

                            break;
                        default:
                            break;
                    }
                    if (!String.IsNullOrEmpty(ext))
                    {
                        var prefix = "file";
                        fileName = string.Format("{0}{1}{2}", prefix, DateTime.Now.ToString("yyyyMMddHHmmssfff"), ext);
                    }
                    var modelFile = new ViewModelFoto()
                    {
                        Name = fileName,
                        StrBytes = "",
                    };


                    var path = "";
                    var pathServer = "";
                    var nomeArquivo = "";
                    if (modelFile != null && !String.IsNullOrEmpty(modelFile.Name))
                    {
                        try
                        {

                            nomeArquivo = $"{Guid.NewGuid().ToString()}{ext}";
                            path = Url.Content($"~/Upload/assinaturas/{nomeArquivo}");
                            pathServer = Server.MapPath(path);
                            Request.Files[upload].SaveAs(pathServer);


                            //var urlAzure = "";

                            //byte[] bArray = null;
                            //using (var memoryStream = new MemoryStream())
                            //{
                            //    Request.Files[upload].InputStream.CopyTo(memoryStream);
                            //    bArray = memoryStream.ToArray();
                            //}

                            //urlAzure = new Data.Service.AzureService().UploadFile(fileName, bArray, EnumAzurePath.LOGO_EMPRESA);


                            //var guid = System.IO.Path.GetFileNameWithoutExtension(urlAzure).ToLower();
                            //ArquivoPropriedades arquivoPropriedades = new ArquivoPropriedades();
                            //arquivoPropriedades.Id = guid;
                            //arquivoPropriedades.Nome = Request.Files[upload].FileName;
                            //arquivoPropriedades.Data = DateTime.Now.ToString("dd/MM/yyyy");
                            //arquivoPropriedades.Path = urlAzure;
                            //arquivoPropriedades.ValidadoImob = false;
                            //path = urlAzure;

                        }
                        catch
                        {

                        }

                        var jsonResult = new { Status = "OK", Path = path, PathServer = pathServer, FileName = nomeArquivo };
                        //resultJson = string.Concat("{\"Status\":\"OK\", \"Path\": \"", path, "\", \"PathServer\": \"", pathServer, "\",\"Base64\": \"", modelFile.StrBytes, "\", \"FileName\": \"", string.Format("{0}", path), "\"}");
                        return new JsonResult() { Data = jsonResult };
                    }
                    else
                    {
                        var jsonResult = new { Status = "NOK_FILE"};
                        return new JsonResult() { Data = jsonResult };
                        //resultJson = "{\"Status\":\"NOK_FILE\" }";
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



        public JsonResult JsonRemoveFile(string index, string key, string ismultiple)
        {
            var resultStatus = false;
            int KEY = Int32.Parse(key);
            var listfotos = new Dictionary<int, ViewModelFoto>();

            var listResult = new List<ViewModelFoto>();
            try
            {
                if (ismultiple == "1")
                {
                    //if (listModelFotosMulti == null)
                    //{
                    //    listModelFotosMulti = new Dictionary<int, List<ViewModelFoto>>();
                    //}

                    //if (listModelFotosMulti.ContainsKey(KEY))
                    //{
                    //    int idx = Int32.Parse(index);
                    //    try
                    //    {
                    //        var path = listModelFotosMulti[KEY][idx].PathServer;
                    //        if (path != null && System.IO.File.Exists(path))
                    //        {
                    //            System.IO.File.Delete(path);
                    //        }
                    //        //System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(path);
                    //        //UtilAdmin.RemoveFiles(dInfo);
                    //    }
                    //    catch { }

                    //    try
                    //    {
                    //        listModelFotosMulti[KEY].RemoveAt(idx);
                    //        listResult = listModelFotosMulti[KEY];
                    //    }
                    //    catch { }
                    //}


                }
                else
                {
                    if (listModelFotoSingle == null)
                    {
                        listModelFotoSingle = new Dictionary<int, ViewModelFoto>();
                    }
                    else
                    {

                        try
                        {
                            var path = listModelFotoSingle[KEY].PathServer;
                            if (path != null && System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            //System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(path);
                            //UtilAdmin.RemoveFiles(dInfo);
                        }
                        catch { }

                        listModelFotoSingle.Remove(KEY);
                    }
                    var item = listfotos.Any(x => x.Key == KEY) ? listfotos[KEY] : null;

                    if (item != null)
                        listResult.Add(item);
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
                    listImages = listResult,
                    status = resultStatus ? "OK" : "NOK"
                }
            };
        }


        public JsonResult JsonAddFile(string path, string name, string index, string base64, string key, string ismultiple, string pathServer)
        {
            var resultStatus = false;
            int KEY = Int32.Parse(key);
            var listfotos = new Dictionary<int, ViewModelFoto>();
            pathServer = pathServer.Replace("_", "\\");
            var listResult = new List<ViewModelFoto>();
            try
            {
                if (ismultiple == "1")
                {
                    //if (listModelFotosMulti == null)
                    //{
                    //    listModelFotosMulti = new Dictionary<int, List<ViewModelFoto>>();
                    //}

                    //if (listModelFotosMulti.ContainsKey(KEY))
                    //{
                    //    if (listModelFotosMulti[KEY].Any(z => z.Index == index))
                    //    {
                    //        var indexNum = listModelFotosMulti[KEY].FindIndex(z => z.Index == index);

                    //        listModelFotosMulti[KEY][indexNum].Index = index;
                    //        listModelFotosMulti[KEY][indexNum].Path = path;
                    //        listModelFotosMulti[KEY][indexNum].PathServer = pathServer;
                    //        listModelFotosMulti[KEY][indexNum].Name = name;
                    //        listModelFotosMulti[KEY][indexNum].StrBytes = base64;
                    //    }
                    //    else
                    //    {

                    //        listModelFotosMulti[KEY].Add(new ViewModelFoto()
                    //        {
                    //            Index = listModelFotosMulti[KEY].Count.ToString(),
                    //            Path = path,
                    //            PathServer = pathServer,
                    //            Name = name,
                    //            StrBytes = base64
                    //        });
                    //    }
                    //}
                    //else
                    //{
                    //    listModelFotosMulti.Add(KEY, new List<ViewModelFoto>());
                    //    listModelFotosMulti[KEY].Add(new ViewModelFoto()
                    //    {
                    //        Index = listModelFotosMulti[KEY].Count.ToString(),
                    //        Path = path,
                    //        PathServer = pathServer,
                    //        Name = name,
                    //        StrBytes = base64
                    //    });
                    //}


                    //listResult = listModelFotosMulti[KEY];
                }
                else
                {

                    if (listModelFotoSingle == null)
                    {
                        listModelFotoSingle = new Dictionary<int, ViewModelFoto>();
                    }

                    if (listModelFotoSingle.ContainsKey(KEY))
                    {
                        listfotos[KEY].Path = path;
                        listfotos[KEY].PathServer = pathServer;
                        listfotos[KEY].Name = name;
                        listfotos[KEY].StrBytes = base64;
                    }
                    else
                    {
                        listfotos.Add(KEY, new ViewModelFoto()
                        {
                            Path = path,
                            PathServer = pathServer,
                            Name = name,
                            StrBytes = base64
                        });
                    }

                    var item = listfotos.Any(x => x.Key == KEY) ? listfotos[KEY] : null;

                    if (item != null)
                        listResult.Add(item);
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
                    listImages = listResult,
                    status = resultStatus ? "OK" : "NOK"
                }
            };
        }



        public JsonResult JsonListFile(string key, string ismultiple)
        {
            var resultStatus = false;
            int KEY = Int32.Parse(key);
            var listfotos = new Dictionary<int, ViewModelFoto>();
            List<ViewModelFoto> listResult = new List<ViewModelFoto>();
            try
            {
                if (ismultiple == "1")
                {
                    //if (listModelFotosMulti == null)
                    //{
                    //    listModelFotosMulti = new Dictionary<int, List<ViewModelFoto>>();
                    //}

                    //if (!listModelFotosMulti.ContainsKey(KEY))
                    //{
                    //    listModelFotosMulti.Add(KEY, new List<ViewModelFoto>());
                    //}

                    //listResult = listModelFotosMulti[KEY];
                }
                else
                {

                    if (listModelFotoSingle == null)
                    {
                        listModelFotoSingle = new Dictionary<int, ViewModelFoto>();
                    }

                    if (!listModelFotoSingle.ContainsKey(KEY))
                    {
                        listfotos.Add(KEY, new ViewModelFoto());
                    }


                    var item = listfotos.Any(x => x.Key == KEY) ? listfotos[KEY] : null;

                    if (item != null)
                        listResult.Add(item);
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
                    listImages = listResult,
                    status = resultStatus ? "OK" : "NOK"
                }
            };
        }



        #endregion --- UPLOAD FILES ---


    }
}