using PortalVet.Data.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalVet.Data.Entity
{
    public class AdminUserItem
    {
        public int Id { get; set; }
        public EnumAdminProfile PerfilId { get; set; }
        public int CompanyId { get; set; }
        public string Nome { get; set; }
        public string EmpresaNome { get; set; }
        public string Sobrenome { get; set; }
        public string CPFCNPJ { get; set; }

        public string CPFCNPJFmt
        {
            get
            {

                if (String.IsNullOrEmpty(CPFCNPJ))
                {
                    return string.Empty;
                }
                else
                {
                    if (CPFCNPJ.Length == 11)
                    {
                        return Convert.ToUInt64(CPFCNPJ).ToString(@"000\.000\.000\-00");
                    }
                    else if (CPFCNPJ.Length == 14)
                    {
                        return Convert.ToUInt64(CPFCNPJ).ToString(@"00\.000\.000\/0000\-00");
                    }
                    else
                    {
                        return CPFCNPJ;
                    }
                }
            }
        }
        public string Imagem { get; set; }
        public string Password { get; set; }
        public string Telefone { get; set; }
        public string TelefoneFmt
        {
            get
            {
                if (Telefone == null) { Telefone = ""; }

                var tel = Telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

                if (Int64.TryParse(tel, out Int64 telfmt))
                {
                    if (tel.Length == 10)
                        return String.Format("{0:(##) ####-####}", telfmt);
                    else if (tel.Length == 11)
                        return String.Format("{0:(##) ####-#####}", telfmt);
                    else
                        return String.Format("{0:(##) ####-#####}", telfmt);
                }
                return Telefone;
            }
        }

        public string Telefone2 { get; set; }
        public string Telefone2Fmt
        {
            get
            {
                if (Telefone2 == null) { Telefone2 = ""; }

                var tel = Telefone2.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

                if (Int64.TryParse(tel, out Int64 telfmt))
                {
                    if (tel.Length == 10)
                        return String.Format("{0:(##) ####-####}", telfmt);
                    else if (tel.Length == 11)
                        return String.Format("{0:(##) ####-#####}", telfmt);
                    else
                        return String.Format("{0:(##) ####-#####}", telfmt);
                }
                return Telefone2;
            }
        }
        public string Email { get; set; }
        public bool Active { get; set; }

        public List<AdminCompanyItem> Empresas { get; set; }
        public List<AdminProfileItem> Perfis { get; set; }
    }


    [Serializable]
    public class AdminCompanyItem
    {
        public int Id { get; set; }
        //public int EmpresaId { get; set; }
        public string Nome { get; set; }
        public string Texto { get; set; }
        public string Imagem { get; set; }

        public string Whatsapp { get; set; }

        public string Url { get; set; }
        public string Email { get; set; }
        public string Chave { get; set; }
        public bool Ativo { get; set; }
    }


    [Serializable]
    public class AdminProfileItem
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    [Serializable]
    public class AdminAcesso
    {
        public int? MenuId { get; set; }
        public string ParentNome { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int ParentId { get; set; }
        public string Nome { get; set; }
        public string Path { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; }
        public bool AtivoTeste { get; set; }
        public int Tipo { get; set; }
        public string IconeCss { get; set; }
        public string Chave { get; set; }
        public string Modulo { get; set; }
        public string Pagina { get; set; }

        public override string ToString()
        {
            return $"{MenuId} - {Controller}";
        }
    }

}
