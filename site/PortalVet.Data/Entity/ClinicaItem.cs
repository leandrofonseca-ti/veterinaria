using PortalVet.Data.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PortalVet.Data.Entity
{
    public class ClinicaItem
    {
        public int Id { get; set; }
        public int UnidadeId { get; set; }
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Responsavel { get; set; }
        public string Responsavel2 { get; set; }
        public string Endereco { get; set; }
    }
}
