using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Entity
{
    public class DocumentoModeloItem
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public int LaudadorId { get; set; }

        public string Nome { get; set; }

        public string Perfil { get; set; }

        public string ModeloCabecalho { get; set; }

        public string ModeloCorpo { get; set; }

        public int AssinaturaId { get; set; }

        public string ModeloRodape { get; set; }

        public DateTime DtCadastro { get; set; }

    }

    public class DocumentoVariavelItem
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }
    }

    public class DocumentoModeloVersaoItem
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int ModuloId { get; set; }
        public int TipoModuloId { get; set; }
        public int ModeloId { get; set; }
        public string ModeloCorpo { get; set; }
        public DateTime DtCadastro { get; set; }
    }
}
