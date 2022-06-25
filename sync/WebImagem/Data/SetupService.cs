using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WebImagem.Data
{
    public class SetupService
    {
        public string pathXml = $"{Application.StartupPath}\\";
        public clsSetup Carregar()
        {
            var setup = new clsSetup();
            XmlSerializer ser = new XmlSerializer(typeof(clsSetup));
            try
            {
                FileStream fs = new FileStream($"{pathXml}XML\\clsSetup.xml", FileMode.OpenOrCreate);
                try
                {
                    setup = ser.Deserialize(fs) as clsSetup;
                }
                catch (InvalidOperationException ex)
                {
                    ser.Serialize(fs, setup);
                }
                finally
                {
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return setup;
        }


        public void Salvar(clsSetup setup)
        {
            XmlSerializer ser = new XmlSerializer(typeof(clsSetup));

            if(!System.IO.Directory.Exists($"{pathXml}XML\\"))
            {
                System.IO.Directory.CreateDirectory($"{pathXml}XML\\");
            }
            FileStream fs = new FileStream($"{pathXml}XML\\clsSetup.xml", FileMode.Create);
            ser.Serialize(fs, setup);
            fs.Close();
        }
    }
}
