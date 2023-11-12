using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace CipherPad
{
    internal class EncryptString
    {
        private string key;
        public EncryptString()
        {
            key = "yuxiang118712023";
        }
        public void Key(string key)
        {
            this.key = key;
        }
        public string Encrypt(string text)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.Mode = CipherMode.ECB;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.Mode = CipherMode.ECB;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        try
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                        catch (CryptographicException ex)
                        {
                            _ = MessageBox.Show($"密钥错误，\n请检查其使用的密钥是否与您的密钥相匹配。\n\nCryptographicException: {ex.Message}", "YuXiang CipherPad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return null;
                        }
                    }
                }
            }
        }
    }
}