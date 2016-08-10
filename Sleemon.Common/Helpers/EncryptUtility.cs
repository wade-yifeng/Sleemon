namespace Sleemon.Common
{
    using System;
    using System.IO;
    using System.Text;
    using System.Security.Cryptography;

    public static class EncryptUtility
    {
        private static readonly MD5 MD5Provider = new MD5CryptoServiceProvider();
        private static readonly Rijndael AES = Rijndael.Create();
        private static readonly byte[] AES_KEY = new byte[] { 79, 83, 90, 120, 103, 115, 54, 81, 118, 82, 72, 102, 105, 101, 51, 71 };
        private static readonly byte[] AES_IV = new byte[] { 75, 108, 109, 104, 90, 66, 67, 121, 74, 100, 52, 74, 82, 84, 68, 57 };

        public static string MD5(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext)) return string.Empty;

            var buffer = Encoding.Unicode.GetBytes(plaintext);
            buffer = MD5Provider.ComputeHash(buffer);
            return BitConverter.ToString(buffer).Replace("-", "");
        }

        public static byte[] AESEncrypt(string plaintext)
        {
            var buffer = Encoding.Unicode.GetBytes(plaintext);
            using (var stream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(stream, AES.CreateEncryptor(AES_KEY, AES_IV), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(buffer, 0, buffer.Length);
                    cryptoStream.FlushFinalBlock();
                    cryptoStream.Close();
                }
                buffer = stream.ToArray();
                stream.Close();
            }
            return buffer;
        }
    }
}
