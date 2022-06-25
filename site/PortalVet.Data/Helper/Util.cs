using PortalVet.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PortalVet.Data.Helper
{
    public static class Util
    {
        private static byte[] chave = { };
        private static readonly byte[] iv = { 12, 34, 56, 78, 90, 102, 114, 126 };
        private static readonly string _key = "PVET2021";
        //private static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        public static string GetTelefoneFormatoWhats(string Telefone)
        {
            string tmp1 = string.Empty;

            if (Telefone != null && Telefone.Length > 0)
            {
                for (int i = 0; i < Telefone.Length; i++)
                {
                    if (int.TryParse(Telefone.Substring(i, 1), out int tmp2))
                    {
                        tmp1 += tmp2;
                    }
                }
            }

            if (!string.IsNullOrEmpty(tmp1))
            {
                tmp1 = $"+{tmp1}";
                if (tmp1.Length == 10 || tmp1.Length == 11)
                {
                    tmp1 = $"55{tmp1}";
                }
            }

            return tmp1.Replace("+", string.Empty);
        }




        public static bool SendEmail(string subject, string to, string body)
        {
            bool result = false;
            try
            {
                MailMessage msg = new MailMessage
                {
                    Subject = subject,
                    From = new MailAddress("no_replay@lifeaudiologia.com.br", "Web Imagem"),
                    Body = body,
                    IsBodyHtml = true
                };

                string[] emailsTo = to.Split(';');
                foreach (string destinatario in emailsTo)
                {
                    msg.To.Add(new MailAddress(destinatario));
                }


                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.lifeaudiologia.com.br",
                    Port = 587,
                    UseDefaultCredentials = false,
                    EnableSsl = false
                };
                NetworkCredential nc = new NetworkCredential("contato@lifeaudiologia.com.br", "Amandita10");
                smtp.Credentials = nc;
                
                if(System.Configuration.ConfigurationManager.AppSettings["SendEmail"] != null && System.Configuration.ConfigurationManager.AppSettings["SendEmail"].ToString() == "1")
                {
                    smtp.Send(msg);
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public static bool SendEmail(string subject, string to, string body, Attachment file = null)
        {
            bool result = false;
            try
            {
                MailMessage msg = new MailMessage
                {
                    Subject = subject,
                    From = new MailAddress("no_replay@lifeaudiologia.com.br", "Web Imagem"),
                    Body = body,
                    IsBodyHtml = true
                };

                string[] emailsTo = to.Split(';');
                foreach (string destinatario in emailsTo)
                {
                    msg.To.Add(new MailAddress(destinatario));
                }

                if (file != null)
                    msg.Attachments.Add(file);

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.lifeaudiologia.com.br",
                    Port = 587,
                    UseDefaultCredentials = false,
                    EnableSsl = false
                };
                NetworkCredential nc = new NetworkCredential("contato@lifeaudiologia.com.br", "Amandita10");
                smtp.Credentials = nc;

                smtp.Send(msg);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public static string GetMapUpload()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ServerMapPathUpload"].ToString();
        }

        public static string GetUrlUpload()
        {
            return System.Configuration.ConfigurationManager.AppSettings["UrlRootUpload"].ToString();
        }

        public static string GerarHashMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Converter a String para array de bytes, que é como a biblioteca trabalha.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Cria-se um StringBuilder para recompôr a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop para formatar cada byte como uma String em hexadecimal
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
        public static bool IsEmail(string emailAddress)
        {
            //if (emailAddress == null)
            //{
            //    return false;
            //}

            //bool isValid = ValidEmailRegexSimple.IsMatch(emailAddress);

            //return isValid;

            if (emailAddress == null)
            {
                return false;
            }
            emailAddress = emailAddress.Trim();

            // VALIDA SE TEM ARROBA
            bool check1 = emailAddress.Contains("@");

            // VERIFICA SE CONTEM "." após "@" e nao termina com "." ponto.
            bool check2 = false;

            // VALIDA SE CONTEM VIRGULA
            bool check3 = !emailAddress.Contains(",");

            // VALIDA ESPAÇO
            bool check4 = !emailAddress.Contains(" ");
            if (check1)
            {
                // VERIFICA SE CONTEM "." após "@" e nao termina com "." ponto.
                string emailAux = emailAddress.Substring(emailAddress.IndexOf("@"));
                check2 = emailAux.Contains(".") && !emailAux.EndsWith(".");
            }
            return check1 == true && check2 == true && check3 == true && check4 == true;
        }

        public static string Encriptar(string value)
        {
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs;
            byte[] input;

            try
            {
                des = new DESCryptoServiceProvider();
                ms = new MemoryStream();
                input = Encoding.UTF8.GetBytes(value);
                chave = Encoding.UTF8.GetBytes(_key.Substring(0, 8));

                cs = new CryptoStream(ms, des.CreateEncryptor(chave, iv), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                //new SOH.Data.Helper.Util().EventLog(ex); 
                throw ex;
            }
        }


        public static string Descriptar(string value)
        {
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs;
            byte[] input;

            try
            {
                des = new DESCryptoServiceProvider();
                ms = new MemoryStream();

                input = new byte[value.Length];
                input = Convert.FromBase64String(value.Replace(" ", "+"));
                chave = Encoding.UTF8.GetBytes(_key.Substring(0, 8));

                cs = new CryptoStream(ms, des.CreateDecryptor(chave, iv), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }


        public static string GetResourcesNovaSenha()
        {
            return Properties.Resources.NovaSenha;
        }

        public static string GetResourcesMensagemModelo()
        {
            return Properties.Resources.MensagemModelo;
        }

        public static string GetResourcesMensagemModeloBotao()
        {
            return Properties.Resources.MensagemModeloBotao;
        }

        public static string GetResourcesNotificacaoExame()
        {
            return Properties.Resources.NotificacaoExame;
        }

        public static string GetResourcesBemVindoCredenciais()
        {
            return Properties.Resources.BemVindoCredenciais;
        }
        public static string CarregaConteudoMaster(Entity.EmpresaItem empresa, string conteudo)
        {
            string logoOuNome = string.Empty;

            StringBuilder sbHtml = new StringBuilder();

            if (empresa != null)
            {
                if (!string.IsNullOrEmpty(empresa.Imagem))
                {
                    logoOuNome = $"<img src=\"{string.Concat(GetUrlUpload(), "empresas/", empresa.Imagem)}\" style=\"width:150px;\" width=\"150\" />";
                }
                else
                {
                    logoOuNome = $"<span style=\"color: #ffffff;\">{empresa.Nome}</span>";
                }

                if (!string.IsNullOrEmpty(empresa.Nome))
                {
                    sbHtml.AppendLine($" • {empresa.Nome} • ");
                }
            }
            else
            {
                logoOuNome = $"<span style=\"color: #ffffff;\">Portal</span>";
            }
            string bodyMaster = Properties.Resources.MasterNovo;
            bodyMaster = bodyMaster.Replace("#CORCABECALHO#", "#ffffff");
            bodyMaster = bodyMaster.Replace("#LOGOOUNOME#", logoOuNome);
            bodyMaster = bodyMaster.Replace("#CONTEUDO#", conteudo);

            bodyMaster = bodyMaster.Replace("#DADOS_EMPRESA#", sbHtml.ToString());


            return bodyMaster;
        }

        public static void SaveHistoricoEmail(int exameid, EnumExameSituacao situacao, int usuarioid, string usuarionome, string usuarioemail, string descricao)
        {

            var hist = new ExameHistoricoItem();
            hist.ExameId = exameid;
            hist.UsuarioId = usuarioid;
            hist.UsuarioNome = usuarionome;
            hist.UsuarioEmail = usuarioemail;
            hist.Descricao = descricao;
            hist.SituacaoId = situacao.GetHashCode();
            hist.Conteudo = string.Empty;
            new PortalVet.Data.Service.ExameService().SaveHistorico(hist);
        }
    }
}
