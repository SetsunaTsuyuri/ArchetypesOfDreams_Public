using System.Text;
using System.Security.Cryptography;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 暗号化・復号のユーティリティ
    /// </summary>
    public static class CryptographyUtility
    {
        static readonly int s_blockSize = 128;
        static readonly int s_keySize = 128;
        static readonly string s_iv = "1K9hf715zU8sm59H";
        static readonly string s_key = "R18FaEzPyxdv0WwW";

        /// <summary>
        /// 暗号化する
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] bytes)
        {
            using AesManaged managed = CreateAesManaged();
            ICryptoTransform encryptor = managed.CreateEncryptor();
            byte[] encrypted = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return encrypted;
        }

        /// <summary>
        /// 復号する
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] bytes)
        {
            using AesManaged managed = CreateAesManaged();
            ICryptoTransform decryptor = managed.CreateDecryptor();
            byte[] decrypted = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return decrypted;
        }

        /// <summary>
        /// AesManagedを作る
        /// </summary>
        /// <returns></returns>
        private static AesManaged CreateAesManaged()
        {
            AesManaged managed = new()
            {
                BlockSize = s_blockSize,
                KeySize = s_keySize,
                IV = Encoding.UTF8.GetBytes(s_iv),
                Key = Encoding.UTF8.GetBytes(s_key),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            return managed;
        }
    }
}
