using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Helper_Detmach
{
    public static class TextHelper
    {
        public static CultureInfo cultureENG = new CultureInfo("en-US", false);
        public static CultureInfo cultureTR = new CultureInfo("tr-TR", false);

        public static string ToPascalCase(this String original)
        {
            Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            Regex whiteSpace = new Regex(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new Regex("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            // replace white spaces with undescore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
                // split by underscores
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                // set first letter to uppercase
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

            return string.Concat(pascalCase);
        }

        public static string ToTitleCaseENG(this string str)
        {
            str = str.ToLower(cultureENG);
            str = cultureENG.TextInfo.ToTitleCase(str);
            return str;
        }
        public static string ToTitleCaseTR(this string str)
        {
            str = str.ToLower(cultureENG);
            str = cultureENG.TextInfo.ToTitleCase(str);
            return str;
        }
        public static string ToSEOQueryString(this string title, int maxlength = 80)
        {
            bool remapToAscii = true;
            if (title == null)
            {
                return string.Empty;
            }

            int length = title.Length;
            bool prevdash = false;
            StringBuilder stringBuilder = new StringBuilder(length);
            char c;

            for (int i = 0; i < length; ++i)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    stringBuilder.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lower-case
                    stringBuilder.Append((char)(c | 32));
                    prevdash = false;
                }
                else if ((c == ' ') || (c == ',') || (c == '.') || (c == '/') ||
                  (c == '\\') || (c == '-') || (c == '_') || (c == '='))
                {
                    if (!prevdash && (stringBuilder.Length > 0))
                    {
                        stringBuilder.Append('-');
                        prevdash = true;
                    }
                }
                else if (c >= 128)
                {
                    int previousLength = stringBuilder.Length;

                    if (remapToAscii)
                    {
                        stringBuilder.Append(RemapInternationalCharToAscii(c));
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }

                    if (previousLength != stringBuilder.Length)
                    {
                        prevdash = false;
                    }
                }

                if (i == maxlength)
                {
                    break;
                }
            }

            if (prevdash)
            {
                return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
            }
            else
            {
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Remaps the international character to their equivalent ASCII characters. See
        /// http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
        /// </summary>
        /// <param name="character">The character to remap to its ASCII equivalent.</param>
        /// <returns>The remapped character</returns>
        private static string RemapInternationalCharToAscii(char character)
        {
            string s = character.ToString().ToLowerInvariant();

            if ("àåáâäãåąā".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøő".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }


            else if (character == 'ř')
            {
                return "r";
            }
            else if (character == 'ł')
            {
                return "l";
            }
            else if ("đð".Contains(s))
            {
                return "d";
            }
            else if (character == 'ß')
            {
                return "ss";
            }
            else if (character == 'Þ')
            {
                return "th";
            }
            else if (character == 'ĥ')
            {
                return "h";
            }
            else if (character == 'ĵ')
            {
                return "j";
            }
            else
            {
                return string.Empty;
            }
        }
        public static void AdSoyadParcala(this string text, out string Ad, out string Soyad)
        {
            Ad = string.Empty;
            Soyad = string.Empty;
            if (!string.IsNullOrEmpty(text))
            {
                var dizi = text.Split(' ');
                for (int i = 0; i < dizi.Length; i++)
                {
                    if (i != dizi.Length - 1)
                    {
                        Ad += dizi[i] + " ";
                    }
                    else
                    {
                        Soyad += dizi[i];
                    }
                }
            }
        }

        public static string ToJson(this object value, Formatting formatting = Formatting.None)
        {
            if (value == null) return null;

            try
            {
                string json = JsonConvert.SerializeObject(value, formatting);
                return json;
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
    }
}
