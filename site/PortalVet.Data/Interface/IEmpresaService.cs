using PortalVet.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalVet.Data.Interface
{
    public interface IEmpresaService
    {
        EmpresaItem Carregar(int empresaId);
        List<EmpresaItem> ListEmpresaFull();
        List<EmpresaItem> ListEmpresa();
        List<AcoesRecentesItem> ListAcoesRecentes(out int pageTotal, int pageIndex, int pageSize, int usuarioId, int empresaid, int perfilId, Dictionary<string, object> dicFilter);
        List<ModeloMensagemItem> CarregarModelosMensagem(int empresaId, int usuarioId, int perfilId);
        void Salvar(ModeloMensagemItem modeloMensagem);
        List<DashReportAdminItem> CarregarDashboardAdmin();
        bool CarregarPorChave(string chave, out int unidadeId);
        int CarregarRacaPorNome(int unidadeId, string raca, int racaId);
    }
}
