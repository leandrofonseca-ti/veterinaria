using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Entity
{
    public class ApiExameItem
    {
        public int ID { get; set; }
        public string NumeroExame { get; set; }
        public DateTime Data { get; set; }
        public string Clinica { get; set; }
        public int ClinicaId { get; set; }
        public string Veterinario { get; set; }
        public string Proprietario { get; set; }
        public string Paciente { get; set; }
        public string Raca { get; set; }
        public string Especie { get; set; }
        public string Idade { get; set; }
        public string Historico { get; set; }
        public string Valor { get; set; }
        public string FormaPagamento { get; set; }
        public int Laudo { get; set; }
        public int LinhaExcel { get; set; }
        public DateTime Hora { get; set; }

    }



    [Serializable]
    public class ApiExameImagemItem
    {
        public int ExameId { get; set; }
        public string Base64Imagem { get; set; }
        public string NomeImagem { get; set; }
    }
}
