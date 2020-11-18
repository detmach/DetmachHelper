using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Helper.Detmach
{
    /// <summary>
    /// https://www.daltinkurt.com/Google-Bunu-mu-demek-istediniz-sorusunu-neye-gore-soruyor
    /// </summary>
    public static class SoundexExtension
    {
        public static string Soundex(this string s)
        {
            return Soundex(s, 4);
        }

        public static string Soundex(this string s, int length)
        {
            return FullSoundex(s)
            .PadRight(length, '0')
            .Substring(0, length);
        }

        public static string FullSoundex(this string s)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string codes = "0123012D02245501262301D202";

            Regex hwBeginString = new Regex("^D+");
            Regex simplify = new Regex(@"(\d)\1*D?\1+");
            Regex cleanup = new Regex("[D0]");

            s = s.ToUpper();

            StringBuilder coded = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {
                int index = chars.IndexOf(s[i]);
                if (index >= 0)
                    coded.Append(codes[index]);
            }

            string result = coded.ToString();

            result = simplify.Replace(result, "$1").Substring(1);

            result = hwBeginString.Replace(result, string.Empty);

            result = cleanup.Replace(result, string.Empty);

            return string.Format("{0}{1}", s[0], result);
        }
    }
}
