using PortalVet.Data.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PortalVet.Data.Entity
{
    public class AcoesRecentesItem
    {
        public int Id { get; set; }
        public DateTime DataExame { get; set; }
        public string DataExameFmt { get { return DataExame.ToString("dd/MM/yyyy"); } }
        public string DataExameHH { get { return DataExame.ToString("HH"); } }
        public string DataExameMM { get { return DataExame.ToString("mm"); } }
        public int SituacaoId { get; set; }

        public string CPFCNPJ { get; set; }
        public string SituacaoNome { get { return Enumeradores.GetDescription((EnumExameSituacao)SituacaoId);  } }
        public int ClienteId { get; set; }
        public int LaudadorId { get; set; }
        public string NomeCliente { get; set; }

        public string NomeClinica { get; set; }
        public string EmailCliente { get; set; }
        public string NomeLaudador { get; set; }
        public string EmailLaudador { get; set; }
        public string TelefoneCliente { get; set; }
        public string TelefoneLaudador { get; set; }
        public int LaudadorSituacaoId { get; set; }
        public string LaudadorSituacaoNome { get { return Enumeradores.GetDescription((EnumExameSituacaoLaudador)LaudadorSituacaoId); } }
        public int CompanyId { get; set; }
        public DateTime DataCadastro { get; set; }

        public int RacaId { get; set; }
        public string Veterinario { get; set; }
        public string Idade { get; set; }
        public string Paciente { get; set; }

        public int EspecieId { get; set; }
        public string EspecieOutros { get; set; }
        public string Proprietario { get; set; }
        public string Descricao { get; set; }

    }
}
