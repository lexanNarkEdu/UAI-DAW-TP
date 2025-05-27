using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Linq;


namespace Services
{
    public class EncriptadorService
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////    ATRIBUTOS     ///////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        private static EncriptadorService singleton;
        private byte[] key;
        private byte[] IV;

        /////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////    CONSTRUCTOR     //////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        public static EncriptadorService GetEncriptadorService()
        {
            if (singleton == null)
            {
                singleton = new EncriptadorService();
            }
            return singleton;
        }

        private EncriptadorService()
        {
            AesManaged aes = new AesManaged();
            this.key = System.Convert.FromBase64String("LVz1Hiu7b1K7aQ2a4WeieE+b3tvrEUcI/nvOunFLzNk=");
            this.IV = System.Convert.FromBase64String("Kr1qHrXodi5Vf0eDwYCt9Q==");
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////    METODOS PUBLICOS CRIPTOGRAFIA     //////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        public string EncriptarAES(string raw)
        {
            byte[] encrypted = Encrypt(raw, this.key, this.IV);
            return System.Convert.ToBase64String(encrypted);
        }

        public string DesencriptarAES(string encryptedb64)
        {
            byte[] encrypted = System.Convert.FromBase64String(encryptedb64);
            string decrypted = Decrypt(encrypted, this.key, this.IV);
            return decrypted;
        }

        public string EncriptarMD5(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////    METODOS PRIVADOS DE CRIPTOGRAFIA    //////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        private byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream
                    // to encrypt
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data
            return encrypted;
        }

        private string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }

    }



}
