﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HeyUrl.Utils
{
	public class HelperUtil
	{
        public static string GetShortUrl(string url)
        {
            StringBuilder sb = new StringBuilder();

            string urlHash = MD5Hash(url, true);

            int i = 0;

            foreach (char s in urlHash)
            {
                if (char.IsLetterOrDigit(s) && char.IsUpper(s))
                {
                    sb.Append(s);

                    if (i++ == 4)
                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Encodes a byte array into hexadecimal string.
        /// </summary>
        /// <param name="array">A byte-array to encode.</param>
        /// <param name="uppercase">Indicates whether the returned hexadecimal string should be in uppercase.</param>
        /// <returns>Hex-encoded string.</returns>
        public static string HexEncode(byte[] array, bool uppercase = true)
        {
            if (array == null || array.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat(uppercase ? "{0:X2}" : "{0:x2}", b);
            }

            return hex.ToString();
        }

        /// <summary>
        /// Calculates MD5 hash on a string.
        /// </summary>
        /// <param name="input">String to calculate hash on.</param>
        /// <param name="uppercase">Indicates whether the returned hexadecimal string should be in uppercase.</param>
        /// <returns>MD5 hash as hex-encoded string.</returns>
        public static string MD5Hash(string input, bool uppercase = true)
        {
            if (input == null)
            {
                return string.Empty;
            }

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = md5.ComputeHash(inputBytes);

                return HexEncode(hash, uppercase);
            }
        }

        public static Tuple<bool, string> IsValidUrl(string url)
        {
            bool res1 = true;
            string res2 = "OK";
            string pattern = @"^((http://)|(https://))*[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$";
    
            if (string.IsNullOrEmpty(url))
            {
                res1 = false;
                res2 = "Enter your original Url";
            }
            else
			{
                Regex rgx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                res1 = rgx.IsMatch(url);

                if(!res1)
				{
                    res2 = "Incorrect Url format";
				}
            }

            return new Tuple<bool, string>(res1, res2);
        }

        public static bool IsValidShortUrl(string url)
        {               
            string pattern = @"^[A-F]{5}$";
            Regex rgx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rgx.IsMatch(url); 
        }
    }
}
