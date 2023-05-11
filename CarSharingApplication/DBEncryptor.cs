using System;
using System.Security.Cryptography;
using System.Text;

namespace CarSharingApplication
{
    public static class PasswordEncryptor
    {

        private static byte[] key = { 176, 83, 18, 89, 11, 22, 200, 166 };
        private static byte[] iv = { 174, 65, 47, 75, 72, 28, 81, 128 };

        public static string EncryptString(string inputString)
        {
            byte[] encryptedData;

            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv;

                ICryptoTransform encryptor = des.CreateEncryptor();

                byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
                encryptedData = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
            }

            return Convert.ToBase64String(encryptedData);
        }

        public static string DecryptString(string inputString)
        {
            byte[] decryptedData;

            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv;

                ICryptoTransform decryptor = des.CreateDecryptor();

                byte[] inputBytes = Convert.FromBase64String(inputString);
                decryptedData = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
            }

            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}
