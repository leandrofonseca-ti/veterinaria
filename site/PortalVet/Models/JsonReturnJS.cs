using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalVet.Models
{
    public class JsonReturnJS
    {
        public JsonReturnJS(bool auth)
        {
            ErrorMessage = string.Empty;
            Message = string.Empty;
            Criticas = new List<JsonCriticaJS>();
            Error = false;
            Authenticated = auth;
        }
        public bool Authenticated { get; set; }
        public object Data { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageTotal { get; set; }
        public string MessageTipo { get; set; }
        public string Message { get; set; }
        public string RedirectUrl { get; set; }
        public string ErrorMessage { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool Error { get; set; }
        public List<JsonCriticaJS> Criticas { get; set; }
        public Object Object { get; set; }
    }
}