using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Entity
{
    public class AdminMenuItem
    {
        public AdminMenuItem()
        {
        }
        public int MenuId { get; set; }
        public string ParentNome { get; set; }
        public string AreaNome { get; set; }
        public string ControllerNome { get; set; }
        public string ActionNome { get; set; }
        public int ParentId { get; set; }
        public string Nome { get; set; }
        public string Path { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; }
        public int Tipo { get; set; }
        public string IconeCss { get; set; }
        public string Modulo { get; set; }
    }

}
