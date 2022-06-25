using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace PortalVet
{
    [Serializable]
    public class WSResult
    {
        public WSResult()
        {
            ErrorMessage = string.Empty;
            Message = string.Empty;
            Criticas = new List<WSCritica>();
            Error = "0";
        }

        public object Data { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string Error { get; set; }
        public List<WSCritica> Criticas { get; set; }
    }

    [Serializable]
    public class WSCritica
    {
        public WSCritica()
        {
            FieldId = string.Empty;
            AbaId = string.Empty;
            Message = string.Empty;
        }

        public string FieldId { get; set; }
        public string Message { get; set; }
        public string AbaId { get; set; }
    }

    /// <summary>
    /// Summary description for app
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class app : System.Web.Services.WebService
    {

        [WebMethod]
        public void ConfigParametros()
        {
            var entityResult = new WSResult();
            try
            {
                var parametros = new
                {
                    ConnectAppleIOS = false,
                    ConnectAppleAndroid = false,

                    ConnectFaceIOS = false, //true,
                    ConnectFaceAndroid = false,

                    ConnectInstaIOS = false,
                    ConnectInstaAndroid = false,
                };
                entityResult.Data = parametros;

            }
            catch (Exception ex)
            {
                entityResult.Error = "1";
                entityResult.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer js = new JavaScriptSerializer();

            Context.Response.ContentType = "application/x-javascript";
            Context.Response.Charset = "utf-8";
            Context.Response.ExpiresAbsolute = DateTime.UtcNow.AddYears(1);
            Context.Response.Cache.SetLastModified(DateTime.UtcNow);
            Context.Response.Cache.SetCacheability(HttpCacheability.Public);

            Context.Response.Write(js.Serialize(entityResult));
        }
    }
}
