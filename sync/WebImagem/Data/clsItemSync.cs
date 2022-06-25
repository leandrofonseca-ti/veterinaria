using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebImagem.Data
{
    public class clsItemLogSync
    {
        public int ID { get; set; }
        public bool Syncronizado { get; set; }
        public DateTime Data_syncronizado { get; set; }
        public string Log { get; set; }
    }
}
