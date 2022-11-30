using System;
using System.Text;
using UnityEngine;
using System.Security.Cryptography;
using System.Linq;

public static class CypherScript
{
    public static byte[] GetKeyArray()
    {
#if UNITY_EDITOR
        return UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
#elif UNITY_ANDROID || UNITY_IOS || UNITY_WINDOWSPHONE
            return UTF8Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier).Take(32).ToArray();
#endif
    }

    public static string Encrypt(string toEncrypt)
    {
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = GetKeyArray();
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public static string Decrypt(string toDecrypt)
    {
        byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = GetKeyArray();
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
}