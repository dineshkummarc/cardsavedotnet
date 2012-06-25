using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace CardsaveDotNet.Utils
{
    public static class HashUtil
    {
        public static string ComputeHashDigest(string hashString, string preSharedKey, HashMethod hashMethod)
        {
            byte[] key = StringToByteArray(preSharedKey);
            byte[] data = StringToByteArray(hashString);

            byte[] hashDigest;

            switch (hashMethod)
            {
                case HashMethod.HMACMD5:
                    hashDigest = new HMACMD5(key).ComputeHash(data);
                    break;
                case HashMethod.HMACSHA1:
                    hashDigest = new HMACSHA1(key).ComputeHash(data);
                    break;
                case HashMethod.MD5:
                    hashDigest = new MD5CryptoServiceProvider().ComputeHash(data);
                    break;
                case HashMethod.SHA1:
                    hashDigest = new SHA1CryptoServiceProvider().ComputeHash(data);
                    break;
                default:
                    throw new InvalidOperationException("Invalid hash method");
            }

            return (ByteArrayToHexString(hashDigest));
        }

        public static byte[] StringToByteArray(string source, bool useASCII = true) {
            Encoding e;
            if (useASCII)
                e = new ASCIIEncoding();
            else
                e = new UTF8Encoding();
            return e.GetBytes(source);
        }

        public static string ByteArrayToHexString(byte[] source) {
            var sb = new StringBuilder();
            for (int i = 0; i < source.Length; i++) {
                sb.Append(source[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }

    public enum HashMethod {
        MD5,
        SHA1,
        HMACMD5,
        HMACSHA1
    }
}