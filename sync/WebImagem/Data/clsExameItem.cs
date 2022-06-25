using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebImagem.Data
{
    public class clsExameItem
    {
        public int Index { get; set; }
        public bool Status { get; set; }
        public int CodigoFromWeb { get; set; }
        public int ID { get; set; }
        public string NumeroExame { get; set; }
        public DateTime Data { get; set; }
        public string Clinica { get; set; }
        public int ClinicaId { get; set; }
        public string ClinicaTelefone { get; set; }
        public string ClinicaEmail { get; set; }
        public string Veterinario { get; set; }
        public string Proprietario { get; set; }
        public string ProprietarioTelefone { get; set; }
        public string ProprietarioCPFCNPJ { get; set; }
        public string ProprietarioEmail { get; set; }
        public string Paciente { get; set; }
        public string Raca { get; set; }
        public int RacaId { get; set; }
        public string Especie { get; set; }
        public string Idade { get; set; }
        public string Historico { get; set; }
        public string Valor { get; set; }
        public string FormaPagamento { get; set; }
        public int Laudo { get; set; }
        public int LinhaExcel { get; set; }
        public DateTime Hora { get; set; }
        public string Deletado { get; set; }
        public string Reserva5 { get; set; }

        public string LogMsg { get; set; }
    }
}
