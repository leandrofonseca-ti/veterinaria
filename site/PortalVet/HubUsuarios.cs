using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PortalVet
{
    public class HubUsuarios : Hub
    {
        private static readonly List<Data.Entity.UsuarioOnlineHelper> usersOnline = new List<Data.Entity.UsuarioOnlineHelper>();
        private Data.Service.EmpresaService empresaService = new Data.Service.EmpresaService();

        public override Task OnConnected()
        {
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Data.Entity.UsuarioOnlineHelper usuario = usersOnline.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (usuario != null)
            {
                usersOnline.Remove(usuario);
                Clients.All.usersOnline(usersOnline.OrderBy(n => n.Nome).ToList());
            }

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            //Clients.Client(Context.ConnectionId).reconectar();
            Clients.All.reconectar();

            return base.OnReconnected();
        }

        public void Connect(string email, string profileName, int empresaId)
        {
            string connectionId = Context.ConnectionId;

            if (usersOnline.Count(x => x.ConnectionId == connectionId) == 0)
            {
                var usuarioCorrente = new Data.Service.AdminUserService().GetByEmail(email);

                if (usuarioCorrente.Id > 0)
                {
                    Data.Entity.UsuarioOnlineHelper usuarioOnline = new Data.Entity.UsuarioOnlineHelper();
                    usuarioOnline.ConnectionId = connectionId;
                    usuarioOnline.ProfileName = profileName;
                    usuarioOnline.Nome = usuarioCorrente.Nome;
                    usuarioOnline.Email = usuarioCorrente.Email;

                    if (empresaId.Equals(0))
                    {
                        usuarioOnline.EmpresaNome = "PORTALVET";
                    }
                    else
                    {
                        var empresa = empresaService.Carregar(empresaId);
                        usuarioOnline.EmpresaNome = empresa.Nome.ToUpper();
                    }

                    usersOnline.Add(usuarioOnline);

                    Clients.All.usersOnline(usersOnline.OrderBy(n => n.Nome).ToList());
                }
            }
        }
    }
}