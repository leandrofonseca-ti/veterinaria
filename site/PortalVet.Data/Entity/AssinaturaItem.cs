using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Entity
{
    public class AssinaturaItem
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int LaudadorId { get; set; }

        public string Nome { get; set; }

        public string AssinaturaNome { get; set; }

        public string AssinaturaCRM{ get; set; }

        public string AssinaturaProfissao { get; set; }

        public string AssinaturaImagem { get; set; }

        public DateTime DtCadastro { get; set; }

    }
     
}
