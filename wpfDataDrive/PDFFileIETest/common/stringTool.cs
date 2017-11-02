using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace pdfResearch
{
    public class TextHandleTool
    {
        public static string[] DOIPatterns = { "DOI:", "DOI ", "doi:", "doi " };
        public static string[] KeywordPatterns = { "Keywords ", "Keywords:", "keywords:", "keywords ", "key words:","Key words:" };
        public static string[] DatePatterns = { "published online:", "Published online:", "Published:", "published:" };

        public static string Introductionpattern = @"^\w*\.?\s*introduction";

      public static string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

      public static string ISSNPattern = @"ISSN\s*\d{4}-\d{4}";

        public static bool IsFindDoi(string text)
        {
            return IsFind(DOIPatterns, text);
        }

        public static string GetDoi(string text, int index, string[] sentences)
        {
            string result = text;
            foreach (var p in DOIPatterns)
            {
                if (text.IndexOf(p) < 0) continue;

                var doi = text.Substring(text.IndexOf(p));

                var handledDoi = doi.Replace(p, "").Split(' ');

                foreach (var item in handledDoi)
                {
                    if (item.Trim() == "") continue;

                    result = item;
                    break;
                }

                if (result.TrimEnd('\r') == "" && sentences.Length > index + 1)
                {
                    result = sentences[index + 1];
                }

                return result;
            }
            return "";
        }


        public static bool IsFindKeywords(string text)
        {
            return IsFind(KeywordPatterns, text);
        }

        public static string GetKeyWords(string text)
        {
            foreach (var p in KeywordPatterns)
            {
                if (text.IndexOf(p) < 0) continue;

                if (text.IndexOf(p) > -1)
                {
                    text = text.Substring(text.IndexOf(p));
                    return text.Replace(p, "");
                }
            }
            return "";
        }

        public static bool IsFind(string[] patterns, string text)
        {
            foreach (var item in patterns)
            {
                if (text.ToLower().Contains(item)) return true;
            }
            return false;
        }

        public static bool IsFindPublishDate(string text)
        {
            return IsFind(DatePatterns, text);
        }

        public static string GetPublishDate(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                foreach (var item in DatePatterns)
                {
                    if (text.IndexOf(item) > -1)
                    {
                        text = text.Substring(text.IndexOf(item));
                        break;
                    }
                }
                return text.Split(':')[1];
            }
            return "";
        }

        public static string GetISSN(string text)
        {
            var maches = Regex.Matches(text, ISSNPattern, RegexOptions.IgnoreCase);

            for (int k = 0; k < maches.Count; k++)
            {
                if (maches[k].Success)
                {
                    return maches[k].Value.Replace("ISSN","").TrimEnd();
                }
            }
            return "";
        }
    }
}
