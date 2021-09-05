using System.IO;
using System.Security.Cryptography;

namespace AIDMusicApp
{
    public static class Crypto
    {
        private static byte[] _key = { 56, 45, 88, 152, 254, 154, 59, 137 };
        private static byte[] _iv = { 85, 91, 141, 163, 251, 61, 22, 93 };

        public static byte[] Crypt(string text)
        {
            var des = DES.Create();
            using (var stream = new MemoryStream())
            {
                using (var crypto = new CryptoStream(stream, des.CreateEncryptor(_key, _iv), CryptoStreamMode.Write))
                using (var writer = new StreamWriter(crypto))
                    writer.Write(text);

                return stream.ToArray();
            }
        }

        public static string Encrypt(byte[] data)
        {
            var des = DES.Create();
            using (var stream = new MemoryStream(data))
            using (var crypto = new CryptoStream(stream, des.CreateDecryptor(_key, _iv), CryptoStreamMode.Read))
            using (var reader = new StreamReader(crypto))
                return reader.ReadToEnd();
        }
    }
}
