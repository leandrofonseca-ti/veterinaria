using PortalVet.Data.Entity;
using PortalVet.Data.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalVet.Data.Interface
{
    public interface IExameService
    {
        ExameItem Get(int Id);
        List<ExameItem> List(out int pgTotal, int pageIndex, int pageSize, int pageOrderCol, string pageOrderSort, Dictionary<string, object> dictionaries);
        void Remove(int id);

        bool ArquivarExame(int exameid, int perfilid, int usuarioid);
        List<SelectListWeb> CarregarExamesPendentesLaudador(int laudadorId);

        List<SelectListWeb> CarregarExamesPendentesGerente(int laudadorId);
        ExameItem Save(ExameItem entidade);

        ExameItem VincularLaudador(ExameItem entidade);
        ExameItem SaveSemProp(ExameItem entidade);
        ExameItem CriarExame(ExameItem entidade);
        void SaveHistorico(ExameHistoricoItem entidade);
        List<ExameHistoricoDuvidaItem> ListHistoricoDuvidaClinica(int codigo);
        List<ExameHistoricoDuvidaItem> ListHistoricoDuvidaLaudador(int codigo);
        void SaveHistoricoDuvida(ExameHistoricoDuvidaItem entidade);
        List<AdminUserItem> CarregarClientes(int empresaId);
        List<AdminUserItem> CarregarClinicas(int empresaId);
        List<AdminUserItem> CarregarLaudadores(int empresaId);
        List<RacaItem> CarregarRacas(int empresaId);

        List<EspecieItem> CarregarEspecies(int empresaId);

        
        List<ExameHistoricoItem> GetHistorico(int exameId);
        List<DocumentoModeloItem> CarregarModelos(int exameid, int empresaId);
        DocumentoModeloItem CarregarModelo(int codigo);
        List<SelectListWeb> CarregarEmailsClinica(int clinicaId);
        List<SelectListWeb> CarregarEmailsGerente(int companyId);
        List<SelectListWeb> CarregarEmailsLaudador(int exameId);
        List<SelectListWeb> CarregarEmailsLaudadorGeral(int companyId);
        
        List<SelectListWeb> CarregarEmailsCliente(int exameId);
        ExameItem SaveDetailLaudador(ExameItem exameItem);
 
    }
}
