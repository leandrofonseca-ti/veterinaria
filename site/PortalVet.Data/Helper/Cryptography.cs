using PortalVet.Data.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace PortalVet.Data.Helper
{
    public class Cryptography : ICryptographyRepository
    {
        private static string chave = "82USJUDNY3MDY214";
        private static string vetorInicializacao = "ABCDEFGHKMNOPRSU";

        private static Rijndael CriarInstanciaRijndael()
        {
            if (!(chave != null && (chave.Length == 16 || chave.Length == 24 || chave.Length == 32)))
            { throw new Exception("A chave de criptografia deve possuir " + "16, 24 ou 32 caracteres."); }
            if (vetorInicializacao == null || vetorInicializacao.Length != 16)
            { throw new Exception("O vetor de inicialização deve possuir " + "16 caracteres."); }

            Rijndael algoritmo = Rijndael.Create(); algoritmo.Key = Encoding.ASCII.GetBytes(chave);
            algoritmo.IV = Encoding.ASCII.GetBytes(vetorInicializacao); return algoritmo;
        }

        public string Encrypt(string text)
        {
            if (String.IsNullOrWhiteSpace(text)) { throw new Exception("O conteúdo a ser encriptado não pode " + "ser uma string vazia."); }
            using (Rijndael algoritmo = CriarInstanciaRijndael())
            {
                ICryptoTransform encryptor = algoritmo.CreateEncryptor(algoritmo.Key, algoritmo.IV);
                using (MemoryStream streamResultado = new MemoryStream())
                {
                    using (CryptoStream csStream = new CryptoStream(streamResultado, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(csStream))
                        {
                            writer.Write(text);
                        }
                    }
                    return ArrayBytesToHexString(streamResultado.ToArray());
                }
            }
        }

        private static string ArrayBytesToHexString(byte[] conteudo)
        {
            string[] arrayHex = Array.ConvertAll(conteudo, b => b.ToString("X2"));
            return string.Concat(arrayHex);
        }

        public string Decrypt(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                throw new Exception("O conteúdo a ser decriptado não pode " + "ser uma string vazia.");
            }
            if (text.Length % 2 != 0)
            {
                throw new Exception("O conteúdo a ser decriptado é inválido.");
            }
            using (Rijndael algoritmo = CriarInstanciaRijndael())
            {
                ICryptoTransform decryptor = algoritmo.CreateDecryptor(algoritmo.Key, algoritmo.IV);
                string textoDecriptografado = null;
                using (MemoryStream streamTextoEncriptado = new MemoryStream(HexStringToArrayBytes(text)))
                {
                    using (CryptoStream csStream = new CryptoStream(streamTextoEncriptado, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(csStream))
                        {
                            textoDecriptografado = reader.ReadToEnd();
                        }
                    }
                }
                return textoDecriptografado;
            }
        }
        private static byte[] HexStringToArrayBytes(string conteudo)
        {
            int qtdeBytesEncriptados = conteudo.Length / 2;
            byte[] arrayConteudoEncriptado = new byte[qtdeBytesEncriptados];
            for (int i = 0;
                i < qtdeBytesEncriptados;
                i++) { arrayConteudoEncriptado[i] = Convert.ToByte(conteudo.Substring(i * 2, 2), 16); }
            return arrayConteudoEncriptado;
        }
    }
}
