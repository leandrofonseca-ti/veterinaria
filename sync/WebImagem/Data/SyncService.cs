using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WebImagem.Data
{
    public class SyncService
    {
        public string pathXml = $"{Application.StartupPath}\\";

        public List<clsItemLogSync> listLog = new List<clsItemLogSync>();

        public List<clsItemLogSync> CarregarLog()
        {
            var list = new List<clsItemLogSync>();
            XmlSerializer ser = new XmlSerializer(typeof(List<clsItemLogSync>));
            try
            {
                using (FileStream fs = new FileStream($"{pathXml}XML\\clsSyncLog.xml", FileMode.OpenOrCreate))
                {
                    try
                    {
                        list = ser.Deserialize(fs) as List<clsItemLogSync>;
                    }
                    catch (InvalidOperationException ex)
                    {
                        ser.Serialize(fs, list);
                    }
                    finally
                    {
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        /*

        public List<clsItemLogSync> CarregarLogSincronizados()
        {
            var list = new List<clsItemLogSync>();
            XmlSerializer ser = new XmlSerializer(typeof(List<clsItemLogSync>));
            try
            {
                FileStream fs = new FileStream($"{pathXml}XML\\clsSyncLog.xml", FileMode.OpenOrCreate);
                try
                {
                    list = ser.Deserialize(fs) as List<clsItemLogSync>;
                }
                catch (InvalidOperationException ex)
                {
                    ser.Serialize(fs, list);
                }
                finally
                {
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
            }

            var aux = list.Where(r => r.Syncronizado == true).ToList();
            return aux;
        }
*/

        public void SalvarLog(clsItemLogSync setup)
        {

            if (!System.IO.Directory.Exists($"{pathXml}XML\\"))
            {
                System.IO.Directory.CreateDirectory($"{pathXml}XML\\");
            }

            if (setup != null)
            {
                this.listLog = CarregarLog();
                this.listLog.Add(setup);
                XmlSerializer ser = new XmlSerializer(typeof(List<clsItemLogSync>));

                using (FileStream fs = new FileStream($"{pathXml}XML\\clsSyncLog.xml", FileMode.OpenOrCreate))
                {
                    ser.Serialize(fs, this.listLog);
                    fs.Close();
                }
            }
            else
            {
                using (FileStream fs = new FileStream($"{pathXml}XML\\clsSyncLog.xml", FileMode.OpenOrCreate))
                {
                    fs.SetLength(0);
                    fs.Close();
                }
            }
        }

        public JsonResultAPI IsLogged(clsAutenticacaoItem clsAutenticacaoItem)
        {

            JsonResultAPI jsonResult = new JsonResultAPI();

            string url = $"{clsAutenticacaoItem.API_URL_EXAME}/IsLogged";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "application/json";
            request.Headers.Add("Authorization", clsAutenticacaoItem.API_CHAVE);
            request.ContentType = "application/json";

            try
            {
                var resultJson = "";
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                resultJson = responseReader.ReadToEnd();
                responseReader.Close();

                jsonResult = JsonConvert.DeserializeObject<JsonResultAPI>(resultJson);
            }
            catch (Exception ex)
            {
                jsonResult = new JsonResultAPI() { Message = ex.Message, Code = 0 };
            }

            return jsonResult;

        }
        public JsonResultAPI SyncExame(clsAutenticacaoItem clsAutenticacaoItem, clsExameItem exame)
        {

            JsonResultAPI jsonResult = new JsonResultAPI();

            CarregarClinicaId(exame.Clinica, out int _clinicaId, out string _telefone, out string _email);
            exame.ClinicaId = _clinicaId;
            exame.ClinicaTelefone = _telefone;
            exame.ClinicaEmail = _email;
            if (_clinicaId == 0 || String.IsNullOrEmpty(_email))
            {
                jsonResult = new JsonResultAPI() { Message = "Clinica não cadastrada e/ou e-mail nao encontrado", Code = 0 };
            }
            else
            {
                var _racaId = CarregarRacaId(exame.Raca);
                exame.RacaId = _racaId;

                string url = $"{clsAutenticacaoItem.API_URL_EXAME}/SalvarExame";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Accept = "application/json";
                request.Headers.Add("Authorization", clsAutenticacaoItem.API_CHAVE);
                request.ContentType = "application/json";


                try
                {
                    //request.Headers.Add("Content-Disposition", "form-data");
                    var jsonExame = Newtonsoft.Json.JsonConvert.SerializeObject(exame);
                    // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                    //request.ContentLength = jsonExame.Length;
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(jsonExame);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    var resultJson = "";
                    HttpWebResponse httpResponse;
                    try
                    {
                        httpResponse = (HttpWebResponse)request.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            resultJson = streamReader.ReadToEnd();
                        }
                    }
                    catch (WebException we)
                    {
                        httpResponse = (HttpWebResponse)we.Response;
                        if (httpResponse != null)
                        {
                            Stream webStream = httpResponse.GetResponseStream();
                            StreamReader responseReader = new StreamReader(webStream);
                            string response = responseReader.ReadToEnd();
                        }
                    }



                    jsonResult = JsonConvert.DeserializeObject<JsonResultAPI>(resultJson);
                }
                catch (Exception ex)
                {
                    jsonResult = new JsonResultAPI() { Message = ex.Message, Code = 0 };
                }
            }
            return jsonResult;
        }

        private void CarregarClinicaId(string clinica, out int clinicaId, out string telefone, out string email)
        {
            clinicaId = 0;
            telefone = "";
            email = "";
            var setup = new SetupService().Carregar();
            string dbConnectionString = $@"Data Source={setup.String_de_conexao};Cache=Shared;";
            try
            {
                SqliteConnection sqlite_con = new SqliteConnection(dbConnectionString);
                sqlite_con.Open();
                string query = "select ID, Telefone, Email from tb_clinica where Nome = @NOME;";
                SqliteCommand sqlite_cmd = new SqliteCommand(query, sqlite_con);
                sqlite_cmd.Parameters.Add(new SqliteParameter("@NOME", clinica));
                SqliteDataReader dr = sqlite_cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["ID"] != DBNull.Value)
                    {
                        clinicaId = Int32.Parse(dr["ID"].ToString());
                    }

                    if (dr["Telefone"] != DBNull.Value)
                    {
                        telefone = dr["Telefone"].ToString();
                    }

                    if (dr["Email"] != DBNull.Value)
                    {
                        email = dr["Email"].ToString();
                    }
                }

                sqlite_con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private int CarregarRacaId(string raca)
        {
            if (String.IsNullOrEmpty(raca))
            {
                return 0;
            }
            var clinicaId = 0;
            var setup = new SetupService().Carregar();
            string dbConnectionString = $@"Data Source={setup.String_de_conexao};Cache=Shared;";
            try
            {
                SqliteConnection sqlite_con = new SqliteConnection(dbConnectionString);
                sqlite_con.Open();
                string query = @"SELECT  ID
                                FROM
                                (
                                SELECT ID FROM tb_RacaCanino where Nome = @NOME
                                UNION
                                SELECT ID FROM tb_RacaFelino where Nome = @NOME
                                  ) as TB LIMIT 0,1; ";

                SqliteCommand sqlite_cmd = new SqliteCommand(query, sqlite_con);
                sqlite_cmd.Parameters.Add(new SqliteParameter("@NOME", raca));
                SqliteDataReader dr = sqlite_cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["ID"] != DBNull.Value)
                    {
                        clinicaId = Int32.Parse(dr["ID"].ToString());
                    }
                }

                sqlite_con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return clinicaId;
        }




        public JsonResultAPI SyncExameArquivo(clsAutenticacaoItem clsAutenticacaoItem, int codigoFromWeb, string pathDocument, string fileDocument)
        {

            var response = new JsonResultAPI();
            JsonResultAPI jsonResult = new JsonResultAPI();
            string url = $"{clsAutenticacaoItem.API_URL_EXAME}/SalvarImagemExame";


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "application/json";
            //request.ContentType = "application/json";
            // request.ContentType = "application/json; charset=UTF-8";
            request.Headers.Add("Authorization", clsAutenticacaoItem.API_CHAVE);
            request.ContentType = "application/json";

            try
            {
                //request.Headers.Add("Content-Disposition", "form-data");
                var documentName = Path.GetFileName(fileDocument);
                string jsonExame =
                "{\"ExameId\": \"" + codigoFromWeb + "\"," +
                 "\"FileNome\": \"" + documentName + "\"," +
                 "\"FileBase64\": \"" + encodeFileToBase64Binary(pathDocument, documentName) + "\"}";

                // request.ContentLength = jsonExame.Length;
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonExame);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var resultJson = "";
                HttpWebResponse httpResponse;
                try
                {
                    httpResponse = (HttpWebResponse)request.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        resultJson = streamReader.ReadToEnd();
                    }
                }
                catch (WebException we)
                {
                    httpResponse = (HttpWebResponse)we.Response;
                    if (httpResponse != null)
                    {
                        Stream webStream = httpResponse.GetResponseStream();
                        StreamReader responseReader = new StreamReader(webStream);
                        //string response = responseReader.ReadToEnd();
                    }
                }

                jsonResult = JsonConvert.DeserializeObject<JsonResultAPI>(resultJson);
            }
            catch (Exception ex)
            {
                jsonResult = new JsonResultAPI() { Message = ex.Message, Code = 0 };
            }

            return jsonResult;
        }



        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private String encodeFileToBase64Binary(string pathFile, string fileName)
        {
            String encodedfile = null;
            try
            {
                var path = pathFile.EndsWith("\\") ? pathFile : string.Concat(pathFile, "\\");
                byte[] fBytes = System.IO.File.ReadAllBytes(string.Format("{0}{1}", path, fileName));
                encodedfile = Convert.ToBase64String(fBytes);
            }
            catch (FileNotFoundException e)
            {
                // TODO Auto-generated catch block  
                Console.Write(e.Message);
            }
            catch (IOException e)
            {
                // TODO Auto-generated catch block  
                Console.Write(e.Message);
            }
            return encodedfile;
        }


        //public void Salvar(clsSetup setup)
        //{
        //    XmlSerializer ser = new XmlSerializer(typeof(clsSetup));

        //    if (!System.IO.Directory.Exists($"{pathXml}XML\\"))
        //    {
        //        System.IO.Directory.CreateDirectory($"{pathXml}XML\\");
        //    }
        //    FileStream fs = new FileStream($"{pathXml}XML\\clsSync.xml", FileMode.OpenOrCreate);
        //    ser.Serialize(fs, setup);
        //    fs.Close();
        //}
    }
}
