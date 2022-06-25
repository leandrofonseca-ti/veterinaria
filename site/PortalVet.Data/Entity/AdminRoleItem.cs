using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Entity
{
    public class AdminRoleItem
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Chave { get; set; }
        public string Pagina { get; set; }

        public bool Ativo { get; set; }
    }
}
