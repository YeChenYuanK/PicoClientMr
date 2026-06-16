using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class MD5  {
    static public string create ( string _v ) {
#if UNITY_WP8
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(_v);
        byte[] hash = UnityEngine.Windows.Crypto.ComputeMD5Hash(bytes);
        string r = "";
        for (int i = 0; i < hash.Length; i++ ) {
            r += string.Format("{0:x2}", hash[i]);
        }
        return r;
#else
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes2 = ue.GetBytes(_v);
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes2);
        string hashString = "";
        for (int i = 0; i < hashBytes.Length; i++ ) {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
        return hashString.PadLeft(32, '0');
#endif
    }
}
