using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Entity
{
    public class ModeloMensagemEnvio
    {
        public int ExameId { get; set; }
        public string ClienteNome { get; set; }
        public string ClienteEmail { get; set; }
        public string ClienteTelefone { get; set; }
        public string Mensagem { get; set; }
        public string CodigoImovel { get; set; }

        public int Codigo { get; set; }
        public int PrioridadeId { get; set; }
        public int ModalidadeId { get; set; }
        public bool EnvioPorEMail { get; set; }
        public bool EnvioPorWhats { get; set; }
    }

    public class ModeloMensagemItem
    {
        public long Id { get; set; }
        public int CompanyId { get; set; }
        public int UsuarioId { get; set; }
        public string Perfil { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public int PerfilId { get; set; }
    }
}
