using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Peak.Common.Enums;
using Peak.Configuration;

namespace Peak.Common {
  /// <summary>
  /// 
  /// </summary>
  public class SecurityTool {
    #region Data Member(s)     
    private readonly string Peak_INTERNAL_KEY = PxConfigurationManager.AppSettings["PeakEncryptionKey"];
    private const char KEY_PADDING_CHAR = '#';
    private const int RIJNDAEL_KEY_SIZE = 32;
    private const int RIJNDAEL_IV_SIZE = 16; //128 bit block size için 128 bit IV
    private const int RIJNDAEL_KEY_SIZE_BITS = RIJNDAEL_KEY_SIZE * 8; //256 bit key
    private const int RIJNDAEL_IV_SIZE_BITS = RIJNDAEL_IV_SIZE * 8;
    private const int RIJNDAEL_BLOCK_SIZE_BITS = 128; //AES-256 uyumlu olabilmesi için! (AES tüm anahtar uzunlukları için (218,192,256) 128bit blok kullanır)
    #endregion

    #region Public Method(s)
    /// <summary>
    /// Bir grup nesnenin sırasına göre combine hash'ini alır.
    /// </summary>
    /// <param name="alg"></param>
    /// <param name="inputs"></param>
    /// <returns></returns>
    public string GetHash(HashAlgorithms alg, HashFormat pFormat, params object[] inputs) {
      List<byte> buffer = new List<byte>();

      foreach (var obj in inputs) {
        if (obj == null) {
          continue;
        }
        buffer.AddRange(getBytes(obj));
      }

      byte[] hash = this.GetHash(buffer.ToArray(), alg);

      if (pFormat == HashFormat.Base64) {
        return Convert.ToBase64String(hash);
      }
      else {
        return BitConverter.ToString(hash).Replace("-", string.Empty);
      }

    }

    /// <summary>
    /// String olarak verilen input verisinin ilgili encoding'e göre byte array karşılığının
    /// istenen algoritma için hash'ini üretir.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="encoding"></param>
    /// <param name="alg"></param>
    /// <returns></returns>
    public string GetHash(string input, Encoding encoding, HashAlgorithms alg, HashFormat pFormat) {
      byte[] buffer = encoding.GetBytes(input);

      byte[] hash = GetHash(buffer, HashAlgorithms.MD5);

      if (pFormat == HashFormat.Base64) {
        return Convert.ToBase64String(GetHash(hash, alg));
      }
      else {
        return BitConverter.ToString(hash).Replace("-", string.Empty);
      }


    }

    /// <summary>
    /// String olarak verilen input verisine string seed'i de ekleyerek ilgili encoding'e göre byte array karşılığının
    /// istenen algoritma için hash'ini üretir.
    /// </summary>
    /// <param name="alg"></param>
    /// <param name="encoding"></param>
    /// <param name="alg"></param>
    /// <returns></returns>
    public string GetHash(string input, string salt, Encoding encoding, HashAlgorithms alg, HashFormat pFormat) {
      return GetHash(input + '#' + salt, encoding, alg, pFormat);
    }

    /// <summary>
    /// Verilen input verisinin istenen algoritmaya göre hash'ini üretir.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="alg"></param>
    /// <returns></returns>
    public byte[] GetHash(byte[] input, HashAlgorithms alg) {
      HashAlgorithm hash = HashAlgorithm.Create(alg.ToString());
      return hash.ComputeHash(input);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="keyStr"></param>
    /// <returns></returns>
    public string Encrypt(string data, string keyStr, HashFormat pFormat) {
      // Encrypt the string to an array of bytes.
      byte[] plainText = Encoding.Unicode.GetBytes(data);
      ICryptoTransform transform = getEncryptionObject(keyStr).CreateEncryptor();
      byte[] cipherText = transform.TransformFinalBlock(plainText, 0, plainText.Length);
      if (pFormat == HashFormat.Base64) {
        return Convert.ToBase64String(cipherText);
      }
      else {
        return BitConverter.ToString(cipherText).Replace("-", string.Empty);
      }

    }

    public string Encrypt(string pPlainText, HashFormat pFormat = HashFormat.Base64) {
      string cipherText = this.Encrypt(pPlainText, Peak_INTERNAL_KEY, pFormat);
      return cipherText;
    }

    public string Decrypt(string pCipherText, HashFormat pFormat = HashFormat.Base64) {
      string plainText = this.Decrypt(pCipherText, Peak_INTERNAL_KEY, pFormat);
      return plainText;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="keyStr"></param>
    /// <returns></returns>
    public string Decrypt(string data, string keyStr, HashFormat pFormat) {
      // Decrypt the bytes to a string.
      ICryptoTransform transform = getEncryptionObject(keyStr).CreateDecryptor();
      byte[] dataBytes = null;
      if (pFormat == HashFormat.Base64) {
        dataBytes = Convert.FromBase64String(data);
      }
      else {
        dataBytes = Toolkit.Instance.HexStringToByteArray(data);
      }
      byte[] decDataBytes = transform.TransformFinalBlock(dataBytes, 0, dataBytes.Length);

      return Encoding.Unicode.GetString(decDataBytes);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pInput"></param>
    /// <param name="pSalt"></param>
    /// <param name="pEncoding"></param>
    /// <returns></returns>
    public string GetSecureHash(string pInput, string pSalt, Encoding pEncoding, HashFormat pFormat) {


      int DERIVED_KEY_LENGTH = 32;
      int ITERATION_COUNT = 24000; //TODO: Konfigurasyondan mı alınmalı

      byte[] hashValue;
      byte[] salt = Convert.FromBase64String(pSalt);
      string stringToHash = string.IsNullOrEmpty(pInput) ? string.Empty : pInput;
      byte[] bytesToHash = pEncoding.GetBytes(stringToHash);

      using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(bytesToHash, salt, ITERATION_COUNT)) {
        hashValue = pbkdf2.GetBytes(DERIVED_KEY_LENGTH);
      }

      string hashString;
      if (pFormat == HashFormat.Base64) {
        hashString = Convert.ToBase64String(hashValue);
      }
      else {
        hashString = BitConverter.ToString(hashValue).Replace("-", string.Empty);
      }

      return hashString;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetSecureSalt() {
      //192 bit uzunluğunda salt. Salt değerinin 128-192 bit arasında olması tavsiye ediliyor
      int SALT_BYTE_LENGTH = 24;  //TODO: konfigurasyondan mı alınmalı

      RNGCryptoServiceProvider cryptProvider = new RNGCryptoServiceProvider();
      byte[] salt = new byte[SALT_BYTE_LENGTH];
      cryptProvider.GetBytes(salt);

      string saltString = Convert.ToBase64String(salt);
      return saltString;
    }

    public string HMAC(string pInput, string pKey, HMACAlgorithms pAlgorithm, HashFormat pFormat) {
      UTF8Encoding utf8Encoding = new UTF8Encoding();
      byte[] keyBytes = utf8Encoding.GetBytes(pKey);
      byte[] inputBytes = utf8Encoding.GetBytes(pInput);
      HMAC hmac = null;

      switch (pAlgorithm) {
        case HMACAlgorithms.HMACMD5:
          hmac = new HMACMD5(keyBytes);
          break;
        case HMACAlgorithms.HMACRIPEMD160:
          hmac = new HMACRIPEMD160(keyBytes);
          break;
        case HMACAlgorithms.HMACSHA1:
          hmac = new HMACSHA1(keyBytes);
          break;
        case HMACAlgorithms.HMACSHA256:
          hmac = new HMACSHA256(keyBytes);
          break;
        case HMACAlgorithms.HMACSHA384:
          hmac = new HMACSHA384(keyBytes);
          break;
        case HMACAlgorithms.HMACSHA512:
          hmac = new HMACSHA512(keyBytes);
          break;
      }


      byte[] hmacBytes = hmac.ComputeHash(inputBytes);

      string result = string.Empty;
      if (pFormat == HashFormat.Hex) {
        result = BitConverter.ToString(hmacBytes).Replace("-", string.Empty);
      }
      else if (pFormat == HashFormat.Base64) {
        result = Convert.ToBase64String(hmacBytes);
      }

      return result;
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// Parametrede verilen nesnenin tipine göre byte array'e çevirir.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private byte[] getBytes(object obj) {
      switch (Type.GetTypeCode(obj.GetType())) {
        case TypeCode.Boolean:
          return BitConverter.GetBytes((bool)obj);
        case TypeCode.Byte:
          return new byte[] { (byte)obj };
        case TypeCode.Char:
          return BitConverter.GetBytes((char)obj);
        case TypeCode.DateTime:
          return BitConverter.GetBytes(((DateTime)obj).ToBinary());
        case TypeCode.Double:
          return BitConverter.GetBytes((double)obj);
        case TypeCode.Int16:
          return BitConverter.GetBytes((short)obj);
        case TypeCode.Int32:
          return BitConverter.GetBytes((int)obj);
        case TypeCode.Int64:
          return BitConverter.GetBytes((long)obj);
        case TypeCode.Single:
          return BitConverter.GetBytes((float)obj);
        case TypeCode.UInt16:
          return BitConverter.GetBytes((ushort)obj);
        case TypeCode.UInt32:
          return BitConverter.GetBytes((uint)obj);
        case TypeCode.UInt64:
          return BitConverter.GetBytes((ulong)obj);
        case TypeCode.String:
        case TypeCode.Decimal:
        case TypeCode.Object:
        case TypeCode.SByte:
        default:
          using (MemoryStream stream = new MemoryStream()) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream.ToArray();
          }
      }
    }

    private SymmetricAlgorithm getEncryptionObject(string keyStr) {
      keyStr = keyStr.PadRight(RIJNDAEL_KEY_SIZE + RIJNDAEL_IV_SIZE, KEY_PADDING_CHAR);
      byte[] keyBytes = Encoding.Unicode.GetBytes(keyStr);

      byte[] key = new byte[RIJNDAEL_KEY_SIZE];
      byte[] iv = new byte[RIJNDAEL_IV_SIZE];

      Array.Copy(keyBytes, key, RIJNDAEL_KEY_SIZE);
      Array.Copy(keyBytes, RIJNDAEL_KEY_SIZE, iv, 0, RIJNDAEL_IV_SIZE);

      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.BlockSize = RIJNDAEL_BLOCK_SIZE_BITS; //AES uyumu için blok size 128bit olarak araylandı
      rijndaelManaged.KeySize = RIJNDAEL_KEY_SIZE_BITS;
      rijndaelManaged.IV = iv;
      rijndaelManaged.Padding = PaddingMode.PKCS7;
      rijndaelManaged.Mode = CipherMode.CBC;
      rijndaelManaged.Key = key;
      return rijndaelManaged;
    }

    #endregion

  }
}
