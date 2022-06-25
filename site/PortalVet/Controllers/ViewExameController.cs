using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Data.Interface;
using PortalVet.Data.Service;
using PortalVet.Models;
using Rotativa.Options;

namespace PortalVet.Controllers
{
    public class ViewExameController : Controller
    {
        private readonly IExameService _exameService;
        private readonly IAdminCompanyService _adminCompanyService;
        private readonly IAssinaturaService _assinaturaService;
        public ViewExameController(IExameService exameService, IAdminCompanyService adminCompanyService, IAssinaturaService assinaturaService)
        {

            _assinaturaService = assinaturaService ??
                 throw new ArgumentNullException(nameof(assinaturaService));

            _exameService = exameService ??
                 throw new ArgumentNullException(nameof(exameService));


            _adminCompanyService = adminCompanyService ??
                 throw new ArgumentNullException(nameof(adminCompanyService));

        }
        // GET: ViewExame
        public ActionResult Index(string key)
        {
            string dadosQueryString = Data.Helper.Util.Descriptar(key);
            int empresaid = dadosQueryString.Split('|')[0].ToInteger();
            int exameid = dadosQueryString.Split('|')[1].ToInteger();
            string dataenvio = dadosQueryString.Split('|')[2].ToString();



            var exame = new ExameService().Get(exameid);
            List<string> lista = new List<string>();



            ViewData["lista"] = lista;



            if (exame.SituacaoId == 6)
            {
                ViewData["exameid"] = $"{exameid} - Concluído";
                lista.Add($"<a target='_blank' href='{Url.Action("PDFExame", "ViewExame", new { key = key })}'>Acesso ao laudo</a>");
                lista.Add($"<a target='_blank' href='{Url.Action("ListImages", "ViewExame", new { key = key })}'>Visualizar imagens</a>");
                lista.Add($"<a target='_blank' href='{Url.Action("ZipImages", "ViewExame", new { key = key })}'>Download das imagens</a>");
                ViewData["lista"] = lista;
            }
            else
            {
                ViewData["exameid"] = $"{exameid} - Pendente";
                lista.Add($"<a target='_blank' href='{Url.Action("PDFExame", "ViewExame", new { key = key })}'>Acesso ao laudo</a>");
                lista.Add($"<a target='_blank' href='{Url.Action("ListImages", "ViewExame", new { key = key })}'>Visualizar imagens</a>");
                lista.Add($"<a target='_blank' href='{Url.Action("ZipImages", "ViewExame", new { key = key })}'>Download das imagens</a>");
                ViewData["lista"] = lista;
            }

            ViewData["DADOS_EMPRESA"] = "";
            ViewData["logoOuNome"] = "";


            string logoOuNome = string.Empty;
            var empresa = new EmpresaService().Carregar(empresaid);
            StringBuilder sbHtml = new StringBuilder();

            if (empresa != null)
            {
                if (!string.IsNullOrEmpty(empresa.Imagem))
                {
                    logoOuNome = $"<img src=\"{string.Concat(Util.GetUrlUpload(), "empresas/", empresa.Imagem)}\" style=\"width:150px;\" width=\"150\" />";
                }
                else
                {
                    logoOuNome = $"<span style=\"color: #ffffff;\">{empresa.Nome}</span>";
                }

                if (!string.IsNullOrEmpty(empresa.Nome))
                {
                    sbHtml.AppendLine($" • {empresa.Nome} • ");
                }
            }
            else
            {
                logoOuNome = $"<span style=\"color: #ffffff;\">Portal</span>";
            }

            ViewData["logoOuNome"] = logoOuNome;
            ViewData["DADOS_EMPRESA"] = sbHtml.ToString();

            return View();
        }

        public ActionResult ListImages(string key)
        {

            string dadosQueryString = Data.Helper.Util.Descriptar(key);
            int empresaid = dadosQueryString.Split('|')[0].ToInteger();
            int exameid = dadosQueryString.Split('|')[1].ToInteger();
            string id = exameid.ToString();
            List<string> arquivos = new List<string>();
            ViewBag.ListAqruivos = arquivos;
            var data = new ExameItem();
            int _id = 0;
            //var listFiles = new List<ViewModelFoto>();

            List<FileInfo> filesToArchive = new List<FileInfo>();
            if (!String.IsNullOrEmpty(id))
            {
                Int32.TryParse(id, out _id);
            }

            if (_id > 0)
            {
                try
                {
                    data = _exameService.Get(_id);
                    var empresa = new Data.Service.EmpresaService().Carregar(data.CompanyId);
                    if (empresa != null)
                    {

                        var pathUploadCliente = Util.GetMapUpload();
                        var pathCliente = string.Format("{0}exames\\{1}\\{2}", pathUploadCliente, data.CompanyId, id);


                        if (Directory.Exists(pathCliente))
                        {
                            var ct = 0;
                            foreach (var file in Directory.GetFiles(pathCliente, @"*.*", SearchOption.TopDirectoryOnly))
                            {
                                var fileName = Path.GetFileName(file);
                                var caminhoUrl = string.Concat(Util.GetUrlUpload(), "exames/", data.CompanyId, "/", id, "/", fileName);
                                ct++;
                                arquivos.Add(caminhoUrl);

                            }
                        }
                        ViewBag.ListAqruivos = arquivos;
                    }
                }
                catch
                {
                }
            }

            if (arquivos.Count > 0)
                return View("ListImages");
            else
            {
                ViewData["exameid"] = id;
                return View("NoImage");
            }
        }

        public ActionResult ZipImages(string key)
        {

            string dadosQueryString = Data.Helper.Util.Descriptar(key);
            int empresaid = dadosQueryString.Split('|')[0].ToInteger();
            int exameid = dadosQueryString.Split('|')[1].ToInteger();
            string id = exameid.ToString();


            var data = new ExameItem();
            int _id = 0;
            string pathUrlZip = "";
            //var listFiles = new List<ViewModelFoto>();

            List<FileInfo> filesToArchive = new List<FileInfo>();
            if (!String.IsNullOrEmpty(id))
            {
                Int32.TryParse(id, out _id);
            }

            if (_id > 0)
            {
                try
                {
                    data = _exameService.Get(_id);
                    var empresa = new Data.Service.EmpresaService().Carregar(data.CompanyId);
                    if (empresa != null)
                    {

                        var pathUploadCliente = Util.GetMapUpload();
                        var pathCliente = string.Format("{0}exames\\{1}\\{2}", pathUploadCliente, data.CompanyId, id);
                        try
                        {
                            foreach (var file in Directory.GetFiles(pathCliente, @"*.zip", SearchOption.TopDirectoryOnly))
                            {
                                System.IO.File.Delete(file);
                            }
                        }
                        catch { }

                        if (Directory.Exists(pathCliente))
                        {
                            var ct = 0;
                            foreach (var file in Directory.GetFiles(pathCliente, @"*.*", SearchOption.TopDirectoryOnly))
                            {
                                var fileName = Path.GetFileName(file);
                                //var relativo = string.Concat(Util.GetUrlUpload(), "exames/", data.CompanyId, "/", id, "/", fileName);
                                var fisico = string.Concat(Util.GetMapUpload(), "exames/", data.CompanyId, "/", id, "/", fileName);
                                //listFiles.Add(new ViewModelFoto() { ID = ct, Path = fisico, Name = relativo, OriginalFileName = fileName });
                                ct++;

                                filesToArchive.Add(new FileInfo(fisico));
                            }
                        }

                        //provide the path and name for the zip file to create
                        //string zipFile = @"c:\Temp\ZipSampleOutput\MyZippedDocuments2.zip";
                        var fileZipName = DateTime.Now.ToString("ddMMyyyyHHmmss");
                        pathUrlZip = string.Concat(Util.GetUrlUpload(), "exames/", data.CompanyId, "/", id, "/", fileZipName, ".zip");
                        string zipFile = string.Concat(Util.GetMapUpload(), "exames/", data.CompanyId, "/", id, "/", $"{fileZipName}", ".zip");
                        using (ZipArchive zipArchive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
                        {
                            //defensive code always checks for null BEFORE executing
                            if (filesToArchive != null && filesToArchive.Count > 0)
                            {
                                //iterate the filesToArchive string array
                                foreach (FileInfo fileToArchive in filesToArchive)
                                {
                                    zipArchive.CreateEntryFromFile(fileToArchive.FullName, fileToArchive.Name, CompressionLevel.Optimal);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            if (!String.IsNullOrEmpty(pathUrlZip))
                return Redirect(pathUrlZip);
            else
            {
                ViewData["exameid"] = id;
                return View("NoImage");
            }
        }

        public ActionResult PDFExame(string key)
        {

            string customSwitches = string.Format("--print-media-type --header-spacing 4 --allow {0} --footer-html {0} --footer-spacing 4 ",
              Url.Action("ViewContratoPDFooter", "ViewExame", new { area = "", key = key }, "https"));


            //string customSwitches1 = string.Format("--print-media-type --allow {0} --header-html {0} --header-spacing 4 --allow {1} --footer-html {1} --footer-spacing 4 ",
            //  Url.Action("ViewContratoPDFHeader", "ViewExame", new { area = "", key = key }, "https"),
            //  Url.Action("ViewContratoPDFooter", "ViewExame", new { area = "", key = key }, "https"));

            var PDFResult = new Rotativa.ActionAsPdf("ViewContratoPDF", new { key = key })
            {
                PageOrientation = Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = { Left = 5, Bottom = 25, Right = 5, Top = 5 },
                CustomSwitches = customSwitches,
            };

            return PDFResult;
        }




         
        public ActionResult ViewContratoPDFHeader(string key)
        {
            string dadosQueryString = Data.Helper.Util.Descriptar(key);
            int empresaid = dadosQueryString.Split('|')[0].ToInteger();
            int exameid = dadosQueryString.Split('|')[1].ToInteger();
            string id = exameid.ToString();
            var data = new ExameItem();
            int _id = 0;
            var listFiles = new List<ViewModelFoto>();
            if (!String.IsNullOrEmpty(id))
            {
                Int32.TryParse(id, out _id);
            }
            ViewData["imagemCompanyNome"] = "";
            ViewData["imagemCompany"] = "";


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
                    }
                }
                catch //(Exception ex)
                {
                }
            }

            return View("ViewContratoPDFHeader", data);
        }


        public ActionResult ViewContratoPDFooter(string key)
        {
            string dadosQueryString = Data.Helper.Util.Descriptar(key);
            int empresaid = dadosQueryString.Split('|')[0].ToInteger();
            int exameid = dadosQueryString.Split('|')[1].ToInteger();
            string id = exameid.ToString();
            var data = new ExameItem();
            int _id = 0;
            var listFiles = new List<ViewModelFoto>();
            if (!String.IsNullOrEmpty(id))
            {
                Int32.TryParse(id, out _id);
            }
            ViewData["modeloRodape"] = "";
            ViewData["infoEmail"] = "";
            ViewData["infoWhats"] = "";
            ViewData["infoUrl"] = "";
            if (_id > 0)
            {
                try
                {
                    data = _exameService.Get(_id);
                    var company = _adminCompanyService.Get(data.CompanyId);
                    ViewData["infoEmail"] = company.Email ?? string.Empty;
                    ViewData["infoWhats"] = company.Whatsapp ?? string.Empty;
                    ViewData["infoUrl"] = company.Url ?? string.Empty;
                    ViewData["modeloRodape"] = data.Rodape ?? string.Empty;

                }
                catch //(Exception ex)
                {
                }
            }

            return View("ViewContratoPDFooter", data);
        }
        public ActionResult ViewContratoPDF(string key)
        {
            string dadosQueryString = Data.Helper.Util.Descriptar(key);
            int empresaid = dadosQueryString.Split('|')[0].ToInteger();
            int exameid = dadosQueryString.Split('|')[1].ToInteger();
            string id = exameid.ToString();
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
            //ViewData["modeloRodape"] = "";
            ViewData["viewArquivos"] = new List<ViewModelFoto>();

            ViewData["AssinaturaNome"] = "";
            ViewData["AssinaturaProfissao"] = "";
            ViewData["AssinaturaImagem"] = "";
            ViewData["AssinaturaCRM"] = "";
            ViewData["AssinaturaData"] = "";
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
                                        listFiles.Add(new ViewModelFoto() { ID = ct, Orientacao = orientacao, Path = fisico, Name = relativo, OriginalFileName = fileName, Width = img.Width, Height = img.Height });
                                        ct++;
                                    }
                                }
                            }
                        }
                    }

                    listFiles = listFiles.OrderBy(t => t.Orientacao).ToList();

                    //var data = _contratoModeloService.CarregarDocumentoVersao(_id);
                    var _assinatura = _assinaturaService.Carregar(data.AssinaturaId);
                    if(_assinatura != null && _assinatura.Id > 0)
                    {

                        ViewData["AssinaturaNome"] = _assinatura.AssinaturaNome;
                        ViewData["AssinaturaProfissao"] = _assinatura.AssinaturaProfissao;
                        ViewData["AssinaturaImagem"] = _assinatura.AssinaturaImagem;
                        ViewData["AssinaturaCRM"] = _assinatura.AssinaturaCRM;
                    }
                    ViewData["AssinaturaData"] = $"{data.DataExame.Day} de {data.DataExame.ToString("MMMM")} de {data.DataExame.Year}";
                    ViewData["modeloCabecalho"] = "";//?? string.Empty;
                    ViewData["modeloCorpo"] = data.Descricao ?? string.Empty;
                    //ViewData["modeloRodape"] = data.Rodape ?? string.Empty;
                    ViewData["viewArquivos"] = listFiles;




                }
                catch //(Exception ex)
                {
                }
            }

            return View("ViewContratoPDF", data);
        }

    }
}