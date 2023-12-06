namespace Tarjetas_ENC_Liber.Interfaces
{
    public interface ICardEncryptionService
    {
        public string Encrypt(string inputText);
        public string Decrypt(string encryptedData);
    }
}
