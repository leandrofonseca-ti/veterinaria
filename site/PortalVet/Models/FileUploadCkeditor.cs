using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalVet.Models
{
    public class FileUploadCkeditor
    {
        public bool upload { get; set; }

        public string url { get; set; }

        public FileUploadCkeditorError error { get; set; }
    }

    public class FileUploadCkeditorError
    {
        public string mensagem { get; set; }
    }
}