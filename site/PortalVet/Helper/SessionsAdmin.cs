using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using PortalVet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalVet.Helper
{
    public static class SessionsAdmin
    {
        //public static int EmpresaId
        //{
        //    get
        //    {
        //        return ListEmpresa.Any() ? Int32.Parse(ListEmpresa.FirstOrDefault().Value) : 0;
        //    }
        //}

        public static int EmpresaId
        {
            get
            {
                if (HttpContext.Current.Session["objEmpresaId"] == null)
                {
                    return 0;
                }

                return (int)HttpContext.Current.Session["objEmpresaId"];
            }
            set
            {
                HttpContext.Current.Session["objEmpresaId"] = value;
            }
        }

        public static List<AdminAcesso> Acessos
        {
            get
            {
                if (HttpContext.Current.Session["objAcessosAdmin"] == null)
                {
                    return new List<AdminAcesso>();
                }

                return (List<AdminAcesso>)HttpContext.Current.Session["objAcessosAdmin"];
            }
            set
            {
                HttpContext.Current.Session["objAcessosAdmin"] = value;
            }


        }

        public static string ForceId
        {
            get
            {
                if (HttpContext.Current.Session["objForceId"] == null)
                {
                    return "";
                }

                return (string)HttpContext.Current.Session["objForceId"];
            }
            set
            {
                HttpContext.Current.Session["objForceId"] = value;
            }
        }
        public static int CurrentProfileId
        {
            get
            {
                if (HttpContext.Current.Session["objCurrentProfileId"] == null)
                {
                    return 0;
                }

                return (int)HttpContext.Current.Session["objCurrentProfileId"];
            }
            set
            {
                HttpContext.Current.Session["objCurrentProfileId"] = value;
            }
        }
        public static int UsuarioId
        {
            get
            {
                if (HttpContext.Current.Session["objUsuarioId"] == null)
                {
                    return 0;
                }

                return (int)HttpContext.Current.Session["objUsuarioId"];
            }
            set
            {
                HttpContext.Current.Session["objUsuarioId"] = value;
            }
        }
        public static int PerfilId
        {
            get
            {
                if (HttpContext.Current.Session["objPerfilId"] == null)
                {
                    return 0;
                }

                return (int)HttpContext.Current.Session["objPerfilId"];
            }
            set
            {
                HttpContext.Current.Session["objPerfilId"] = value;
            }
        }

        public static string NomeEmpresa
        {
            get
            {
                if (HttpContext.Current.Session["objNomeEmpresa"] == null)
                {
                    return string.Empty;
                }

                return HttpContext.Current.Session["objNomeEmpresa"].ToString();
            }
            set
            {
                HttpContext.Current.Session["objNomeEmpresa"] = value;
            }
        }
        public static string EmpresaLogo
        {
            get
            {
                if (HttpContext.Current.Session["objEmpresaLogo"] == null)
                {
                    return string.Empty;
                }

                return HttpContext.Current.Session["objEmpresaLogo"].ToString();
            }
            set
            {
                HttpContext.Current.Session["objEmpresaLogo"] = value;
            }
        }


        public static string UsuarioNome
        {
            get
            {
                if (HttpContext.Current.Session["objUsuarioNome"] == null)
                {
                    return string.Empty;
                }

                return HttpContext.Current.Session["objUsuarioNome"].ToString();
            }
            set
            {
                HttpContext.Current.Session["objUsuarioNome"] = value;
            }
        }

        public static string PerfilNome
        {
            get
            {
                if (HttpContext.Current.Session["objPerfilNome"] == null)
                {
                    return string.Empty;
                }

                return HttpContext.Current.Session["objPerfilNome"].ToString();
            }
            set
            {
                HttpContext.Current.Session["objPerfilNome"] = value;
            }
        }

        public static List<int> ListProfileId
        {
            get
            {
                if (HttpContext.Current.Session["objListProfileIdAdmin"] == null)
                {
                    return new List<int>();
                }

                return (List<int>)HttpContext.Current.Session["objListProfileIdAdmin"];
            }
            set
            {
                HttpContext.Current.Session["objListProfileIdAdmin"] = value;
            }
        }

        public static List<SelectListWeb> ListEmpresa
        {
            get
            {
                if (HttpContext.Current.Session["objListEmpresaAdmin"] == null)
                {
                    return new List<SelectListWeb>();
                }

                return (List<SelectListWeb>)HttpContext.Current.Session["objListEmpresaAdmin"];
            }
            set
            {
                HttpContext.Current.Session["objListEmpresaAdmin"] = value;
            }
        }
        public static string UsuarioPicture
        {
            get
            {
                if (HttpContext.Current.Session["objUsuarioPicture"] == null)
                {
                    return string.Empty;
                }

                return HttpContext.Current.Session["objUsuarioPicture"].ToString();
            }
            set
            {
                HttpContext.Current.Session["objUsuarioPicture"] = value;
            }
        }
        public static string UsuarioEmail
        {
            get
            {
                if (HttpContext.Current.Session["objUsuarioEmail"] == null)
                {
                    return string.Empty;
                }

                return HttpContext.Current.Session["objUsuarioEmail"].ToString();
            }
            set
            {
                HttpContext.Current.Session["objUsuarioEmail"] = value;
            }
        }

    }
}