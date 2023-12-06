using System.Security.Cryptography;
using System.Text;
using Tarjetas_ENC_Liber.Interfaces;

namespace Tarjetas_ENC_Liber.Services
{
    public class CardEncryptionService : ICardEncryptionService
    {
        private byte[] IV;
        private byte[] key;

        public CardEncryptionService()
        {
            IV = HexStringToByteArray("0123456789abcdef");
            key = Encoding.GetEncoding(28605).GetBytes("J9ws*7X912A75BC8ECNP9F53");
        }
        static byte[] HexStringToByteArray(string hex)
        {
            hex = hex.Length % 2 == 1 ? '0' + hex : hex;
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        static string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:X2}", b);
            }
            return hex.ToString();
        }
        public string Encrypt(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                return inputText;
            }

            byte[] bytes = Encoding.GetEncoding(28605).GetBytes(inputText);
            byte[] array = new byte[(bytes.Length + 7) / 8 * 8];
            Array.Copy(bytes, 0, array, 0, bytes.Length);
            string text = string.Empty;

            using TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None
            };
            var data = tripleDESCryptoServiceProvider.CreateEncryptor(key, IV).TransformFinalBlock(array, 0, array.Length);

            return ByteArrayToHexString(data);
        }
        public string Decrypt(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                return inputText;
            }

            byte[] value = HexStringToByteArray(inputText);
            byte[] bytes;
            using (TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None
            })
            {
                bytes = tripleDESCryptoServiceProvider.CreateDecryptor(key, IV).TransformFinalBlock(value, 0, value.Length);
            }

            string text = Encoding.GetEncoding(28605).GetString(bytes);
            int num = text.IndexOf('\0');
            if (num > 0)
            {
                text = text.Substring(0, num);
            }

            return text;
        }
       }
}