using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace PortalVet.Models
{
    public class ViewModelUpload
    {
        public string ID { get; set; }
        public string Status { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string StrBytes { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public bool IsMultiple { get; set; }
        public string ClienteEmail { get; set; }
        public string EmpresaId { get; set; }
        public Image ImagemFile { get; set; }



        public int Width { get; set; }

        public int Height { get; set; }
    }
    public class ViewModelFoto
    {
        public int ID { get; set; }
        public string Index { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Localidade { get; set; }
        public string Data { get; set; }
        public string Path { get; set; }
        public string PathServer { get; set; }
        public int Ordem { get; set; }
        public string Orientacao { get; set; }
        public string Name { get; set; }
        public string OriginalFileName { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public string StrBytes { get; set; }
    }

    public class ModelFoto
    {
       // public MemoryStream Stream { get; set; }
        public int ID { get; set; }
        public string Index { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Localidade { get; set; }
        public string Data { get; set; }
        public string Path { get; set; }

        public int Ordem { get; set; }
        public string NameInit { get; set; }
        public string Name { get; set; }
        public string NameThumb
        {
            get
            {
                if (!String.IsNullOrEmpty(Name))
                {
                    return Name.Replace("/file_", "/thumb_file_");
                }
                else { return ""; }

            }
        }
        public string StrBytes { get; set; }
        public System.Drawing.Image Image { get; set; }
    }
}