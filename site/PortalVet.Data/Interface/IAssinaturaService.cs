using PortalVet.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Interface
{
    public interface IAssinaturaService
    {
        AssinaturaItem Salvar(AssinaturaItem entidade);

        List<AssinaturaItem> Listar(out int pgTotal, int pageIndex, int pageSize,int usuarioId, int  perfilId);

        List<AssinaturaItem> ListarLaudador(int laudadorid);
        List<AssinaturaItem> ListarLaudadorCompany(int companyid);

        void Remover(int id);

        AssinaturaItem Carregar(int id);

    }
}
