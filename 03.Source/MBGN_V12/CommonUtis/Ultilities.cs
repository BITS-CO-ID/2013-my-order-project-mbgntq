using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System.Reflection;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Globalization;
using System.Configuration;
using System.IO;

namespace CommonUtils
{
	public class ZMyException
	{
		#region // Constructors and Destructors:
		private const string c_strKeyID = "ZMyException";
		private const string c_strKeyErrorCode = "ErrorCode";
		private const string c_strKeyData = "Data";
		#endregion

		#region // public members
		public static Exception Raise(
			string strErrorCode
			, Exception excInner
			, params object[] arrobjParamsCouple
			)
		{
			string strExc = string.Format("ZMyException [{0}]", strErrorCode);
			Exception exc = new Exception(strErrorCode, excInner);
			exc.Data.Add(c_strKeyID, c_strKeyID);
			exc.Data.Add(c_strKeyErrorCode, strErrorCode);
			exc.Data.Add(c_strKeyData, arrobjParamsCouple);

			return exc;
		}
		#endregion
	}
    public static class Ultilities
    {	

		public static string ProcessException(object[] arrobjParamsCouple, Exception ex, bool sendmail)
		{
			StringBuilder strMessage = new StringBuilder();
			for (int i = 0; i < arrobjParamsCouple.Length; i += 2)
			{
				strMessage.Append("------------------------------------------------------------\n");
				strMessage.AppendFormat("{0} : {1}", arrobjParamsCouple[i], arrobjParamsCouple[i + 1]);
				strMessage.Append("\n");
			}
			strMessage.Append("------------------------------------------------------------\n");
			strMessage.AppendFormat("{0}\t\n{1}\n", ex.Message, ex.StackTrace);
			return strMessage.ToString();
		}	
		
    }

	public static class Utilities
	{
		//public static string EncryptPassword(string pass)
		//{
		//    MD5 md5 = new MD5CryptoServiceProvider();

		//    //compute hash from the bytes of text
		//    md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pass));

		//    //get hash result after compute it
		//    byte[] result = md5.Hash;

		//    StringBuilder strBuilder = new StringBuilder();
		//    for (int i = 0; i < result.Length; i++)
		//    {
		//        //change it into 2 hexadecimal digits
		//        //for each byte
		//        strBuilder.Append(result[i].ToString("x2"));
		//    }

		//    return strBuilder.ToString();
		//}

		/// <summary>
		/// Encrypt a string using dual encryption method. Return a encrypted cipher Text
		/// </summary>
		/// <param name="toEncrypt">string to be encrypted</param>
		/// <param name="useHashing">use hashing? send to for extra secirity</param>
		/// <returns></returns>
		public static string Encrypt(string toEncrypt)
		{
			byte[] keyArray;
			byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

			//System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
			// Get the key from config file
			string key = "md5SecurityKey";//(string)settingsReader.GetValue("SecurityKey", typeof(String));
			//System.Windows.Forms.MessageBox.Show(key);
			bool useHashing = true;
			if (useHashing)
			{
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
				hashmd5.Clear();
			}
			else
				keyArray = UTF8Encoding.UTF8.GetBytes(key);

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			tdes.Key = keyArray;
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			ICryptoTransform cTransform = tdes.CreateEncryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			tdes.Clear();
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);

		}
		/// <summary>
		/// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
		/// </summary>
		/// <param name="cipherString">encrypted string</param>
		/// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
		/// <returns></returns>
		public static string Decrypt(string cipherString)
		{
			byte[] keyArray;
			byte[] toEncryptArray = Convert.FromBase64String(cipherString);

			//System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
			//Get your key from config file to open the lock!
			string key = "md5SecurityKey";// (string)settingsReader.GetValue("SecurityKey", typeof(String));
			bool useHashing = true;
			if (useHashing)
			{
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
				hashmd5.Clear();
			}
			else
				keyArray = UTF8Encoding.UTF8.GetBytes(key);

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			tdes.Key = keyArray;
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			ICryptoTransform cTransform = tdes.CreateDecryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

			tdes.Clear();
			return UTF8Encoding.UTF8.GetString(resultArray);
		}

		public static void deleteFile(string deleteFile)
		{
			File.Delete(deleteFile);
		}
        public static bool existFile(string fileName)
        {
            return File.Exists(fileName);
        }
	}

	public static class DatetimeUtils
	{
		public static DateTime GetDateTextbox(string dDate)
		{
			CultureInfo viVN = new CultureInfo("vi-VN");
			return DateTime.ParseExact(dDate, "dd/MM/yyyy", viVN);
		}
	}
}
