using PortalVet.Data.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PortalVet.Data.Entity
{
    [Serializable]
    public class ExameItem
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public DateTime DataExame { get; set; }
        public string DataExameFmt { get { return DataExame.ToString("dd/MM/yyyy"); } }
        public string DataExameHH { get { return DataExame.ToString("HH"); } }
        public string DataExameMM { get { return DataExame.ToString("mm"); } }
        public int SituacaoId { get; set; }
        public int AssinaturaId { get; set; }
        public string SituacaoNome { get { return Enumeradores.GetDescription((EnumExameSituacao)SituacaoId); } }
        public string EmailCliente { get; set; }
        public int ClienteId { get; set; }
        public int ClinicaId { get; set; }
        public int LaudadorId { get; set; }
        public string NomeCliente { get; set; }
        public string NomeLaudador { get; set; }
        public int LaudadorSituacaoId { get; set; }
        public string LaudadorSituacaoNome { get { return Enumeradores.GetDescription((EnumExameSituacaoLaudador)LaudadorSituacaoId); } }
        public int CompanyId { get; set; }

        public string CompanyNome { get; set; }
        public DateTime DataCadastro { get; set; }

        public DateTime? DataVinculoLaudador { get; set; }

        public string PeriodoTermino { get; set; }
        public string PeriodoTerminoFmt { get; set; }

        public string PeriodoTermino2Fmt { get; set; }

        public int RacaId { get; set; }
        public string RacaNome { get; set; }
        public string RacaOutros { get; set; }
        public string Veterinario { get; set; }
        public string Idade { get; set; }
        public string Paciente { get; set; }

        public string EspecieNome { get; set; }
        public string EspecieOutros { get; set; }
        public int EspecieId { get; set; }
        public string Proprietario { get; set; }
        public string ProprietarioEmail { get; set; }
        public string ProprietarioTelefone { get; set; }
        public string Descricao { get; set; }
        public string Rodape { get; set; }

        public string Historico { get; set; }
        public string Valor { get; set; }
        public string FormaPagamento { get; set; }

        public string EspecieSelecao { get; set; }
        public string RacaSelecao { get; set; }
        public List<ExameHistoricoItem> ListHistorico { get; set; } = new List<ExameHistoricoItem>();
        public string LinkExame { get; set; }
        public List<ExameHistoricoDuvidaItem> DuvidasClinica { get; set; } = new List<ExameHistoricoDuvidaItem>();

        public List<ExameHistoricoDuvidaItem> DuvidasLaudador { get; set; } = new List<ExameHistoricoDuvidaItem>();
    }

    public class ExameHistoricoItem
    {
        public int Id { get; set; }
        public int ExameId { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioEmail { get; set; }
        public string Conteudo { get; set; }
        public string Descricao { get; set; }


        public int SituacaoId { get; set; }
        public string SituacaoNome { get { return Enumeradores.GetDescription((EnumExameSituacao)SituacaoId); } }

        public DateTime DataCadastro { get; set; }

        public string DataCadastroFmt { get { return DataCadastro.ToString("dd/MM/yyyy HH:mm"); } }
    }




    public class ExameHistoricoDuvidaItem
    {
        public int Id { get; set; }
        public int ExameId { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; }
        public string Tipo { get; set; }
        public string UsuarioEmail { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataCadastro { get; set; }
        public string DataCadastroFmt { get { return DataCadastro.ToString("dd/MM/yyyy HH:mm"); } }
    }


}
