using PortalVet.Data.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalVet.Data.Entity
{
    public class DashReportItem
    {
        public int totalExamesEmAndamento { get; set; }
        public int totalExamesConcluidos { get; set; }
        public int totalExamesCancelados { get; set; }
        public int totalExamesAguardandoAtendimento { get; set; }
        public int totalClientes { get; set; }
        public int totalGerentes { get; set; }
    }

    public class DashReportAdminItem
    {
        public int ID { get; set; }
        public string NOME { get; set; }
        public string IMAGEM { get; set; }
        public int TOTAL_EM_ANALISE { get; set; }
        public int TOTAL_CRIACAO { get; set; }
        public int TOTAL_CONCLUIDO { get; set; }
    }
}
