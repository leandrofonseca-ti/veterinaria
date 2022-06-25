using PortalVet.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Interface
{
    public interface IContratoModeloService
    {
        List<DocumentoVariavelItem> CarregarVariaveis();

        DocumentoModeloItem SalvarDocumento(DocumentoModeloItem entidade);

        //void SalvarModalidade(List<string> modalidades, int documentoId);

        List<DocumentoModeloItem> List(out int pgTotal, int pageIndex, int pageSize,int usuarioId, int  perfilId);

        void RemoveDocumento(int id);

        //void ClearDocumentoModalidade(int documentoId);


        //DocumentoModeloVersaoItem SalvarDocumentoVersao(int modeloid, int moduloid, int tipomoduloid, int empresaid, string texto);

        DocumentoModeloVersaoItem CarregarDocumentoVersao(int modeloid, int moduloid, int tipomoduloid, int empresaid);

        DocumentoModeloVersaoItem CarregarDocumentoVersaoUltima(int modeloid, int empresaid, int tipomoduloid);
       // DocumentoModeloVersaoItem CarregarDocumentoVersao(int id);

        DocumentoModeloItem CarregarDocumento(int id);
        List<DocumentoModeloItem> CarregarDocumentos(int empresaid, int ct);

        //List<DocumentoModeloItem> CarregarDocumentosModalidade(int empresaid,  string tipoPessoa, int locatarioct, int fiadorct);
       // string ReplaceDocumentoModelo(int modeloid, int moduloid, int empresaid, string texto);
        //bool AtualizarCodigoImovelSave(int empresaid, int tipomoduloid, int moduloid, string codigo);

       // bool AtualizarCodigoAltImovelSave(int empresaid, int tipomoduloid, int moduloid, string codigo);
    }
}
