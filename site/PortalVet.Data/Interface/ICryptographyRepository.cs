using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Interface
{
    public interface ICryptographyRepository
    {
        string Encrypt(string text);

        string Decrypt(string text);


    }
}
