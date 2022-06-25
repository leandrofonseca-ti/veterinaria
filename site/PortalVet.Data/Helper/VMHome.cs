using PortalVet.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Helper
{
    public class VMHome
    {
        public VMHome()
        {
            Menus = new List<VMMenu>();
            Empresa = new EmpresaItem();
            UrlPathUpload = string.Empty;
            MenuIdSelected = new List<int>();
            ListProfileId = new List<int>();
            ListEmpresa = new List<SelectListWeb>();
        }
        public List<VMMenu> Menus { get; set; }
        public bool HasAccessPageAnalise { get; set; }
        public bool HasAccessPageVisitasCadastrosLocacoes { get; set; }
        public bool HasAccessPageVisitasCadastrosVendas { get; set; }
        public bool HasAccessPageLocacoes { get; set; }
        public bool HasInicialAccess { get; set; }
        public bool HasCurrentAccess { get; set; }
        public bool HasAccessPageVendas { get; set; }
        public bool HasAccessPagePropostas { get; set; }
        public bool podeCadastrar { get; set; }

        public bool podeAtualizar { get; set; }

        public bool podeVerMenu { get; set; }

        public bool podeExcluir { get; set; }

        public List<int> MenuIdSelected { get; set; }

        public string MenuNameSelected { get; set; }
        public bool podeListar { get; set; }
        public string ID { get; set; }

        public int TipoModuloId { get; set; }

        public int ContratoStatusId { get; set; }
        public int BookMasterID { get; set; }
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string AreaNameUrl { get; set; }
        public string ControllerNameUrl { get; set; }
        //public string MapPath { get; set; }
        public string UrlPathUpload { get; set; }


        public string PathUrlUser { get; set; }
        public string UrlImageUser { get; set; }
        public string FirstNameTop { get; set; }
        public string FirstName { get; set; }
        public string UsuarioEmail { get; set; }
        public string ProfileName { get; set; }
        public string EmpresaLogo { get; set; }

        public int ProfileId { get; set; }
        public int EmpresaId { get; set; }
        public EmpresaItem Empresa { get; set; }
        public int UserId { get; set; }

        public bool AtualizacaoExibir { get; set; }
        public bool VisualizarProximoConsultor { get; set; }



        public List<int> ListProfileId { get; set; }
        public string CurrentProfileName
        {
            get
            {
                if (ProfileId > 0)
                    return Enumeradores.GetDescription((EnumAdminProfile)ProfileId);
                else
                    return string.Empty;
            }
        }

        public List<SelectListWeb> ListEmpresa { get; set; }
    }
}
