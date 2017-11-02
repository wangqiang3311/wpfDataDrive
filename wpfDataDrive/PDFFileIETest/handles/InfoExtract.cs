using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace pdfResearch
{
    /// <summary>
    /// 信息提取
    /// </summary>
    public class InfoExtract
    {
        public HandleResult TitleResult { set; get; }

        public string Author { set; get; }

        public string DOI { set; get; }

        public string Keywords { set; get; }

        public string Url { set; get; }

        public string ISSN { set; get; }

        public string[] Sentences { set; get; }

        public string Text { set; get; }

        public VolumeInfo VolumeInfo { set; get; }

        public StringBuilder Summary { set; get; }

        public string PublishDate { set; get; }


        public InfoExtract(string[] sentences, string text)
        {
            this.Sentences = sentences;
            this.Text = text;
        }

        private void ExtractDOI()
        {
            //获取DOI

            int i = 0;
            foreach (var item in this.Sentences)
            {
                if (TextHandleTool.IsFindDoi(item))
                {
                    DOI = TextHandleTool.GetDoi(item, i, this.Sentences);
                    break;
                }
                i++;
            }
        }

        private void ExtractKeywords()
        {
            //获取关键字，如有Introduction或者1. Introduction，获取之间的语句，否则获取keywords所在行

            int start = 0;
            int end = 0;
            int i = 0;
            foreach (var item in this.Sentences)
            {
                i++;

                var lowerItem = item.ToLower();

                if (start == 0)
                {

                    if (lowerItem.StartsWith("keywords") || lowerItem.StartsWith("key words:"))
                    {
                        start = i;
                    }
                }

                if (end == 0)
                {
                    if (Regex.IsMatch(item, TextHandleTool.Introductionpattern, RegexOptions.IgnoreCase))
                    {
                        end = i;
                    }
                }
                if (end == 0 && item.ToUpper().Trim('\r').Trim() == "CONTENTS")
                {
                    end = i;
                }

                if (start > 0 && end > 0 && end > start) break;
            }

            StringBuilder keywords = new StringBuilder();

            if (start > 0)
            {
                for (int j = start - 1; j < end - 1; j++)
                {
                    if (Regex.IsMatch(Sentences[j], @"\d+")) break;

                    keywords.Append(Sentences[j]);

                }
            }
            this.Keywords = keywords.ToString();

            if (this.Keywords == "")
            {
                foreach (var item in Sentences)
                {
                    if (TextHandleTool.IsFindKeywords(item))
                    {
                        Keywords = TextHandleTool.GetKeyWords(item);
                        break;
                    }
                }
            }
        }

        private void ExtractUrl()
        {
            //获取url
            foreach (var item in Sentences)
            {
                if (item.ToLower().StartsWith("url:"))
                {
                    this.Url = item;
                    break;
                }
                if (item.ToLower().StartsWith("www."))
                {
                    this.Url = item;
                    break;
                }
            }
        }



        public void Extract()
        {
            ExtractDOI();
            ExtractKeywords();
            ExtractUrl();
            ExtractVolume();
            ExtractAbstract();
            ExtractPublishDate();
            ExtractISSN();

        }

        public void ExtractISSN()
        {
            ISSN = TextHandleTool.GetISSN(this.Text);
        }

        /// <summary>
        /// 获取日期
        /// </summary>
        public string ExtractDate()
        {
            foreach (var item in this.Sentences)
            {
                foreach (var m in TextHandleTool.months)
                {
                    if (item.Contains(m))
                    {
                        string pattren = m + @"\s*(\d+,)?\s*\d{4}";

                        var maches = Regex.Matches(item, pattren, RegexOptions.IgnoreCase);

                        for (int k = 0; k < maches.Count; k++)
                        {
                            if (maches[k].Success)
                            {
                                return maches[k].Value;
                            }
                        }
                    }
                }
            }
            return "";
        }
        /// <summary>
        /// 获取出版日期
        /// </summary>
        /// <returns></returns>
        public void ExtractPublishDate()
        {
            foreach (var item in this.Sentences)
            {
                if (TextHandleTool.IsFindPublishDate(item))
                {
                    PublishDate = TextHandleTool.GetPublishDate(item);
                    break;
                }
            }
        }

        public HandleResult ExtractTitle(Block block, string realtitle)
        {
            this.TitleResult = TitleHandle.GetTitle(block, true);
            return this.TitleResult;
        }

        public string ExtractAuthor(string markedText, string oriText, string title)
        {
            AuthorHandle handle = new AuthorHandle();
            handle.GetAuthor(markedText, oriText, title);
            return handle.Author;
        }


        private void ExtractAbstract()
        {
            //获取摘要,在Abstract和keywords 或者 1. Introduction之间的语句

            int start = 0;
            int end = 0;
            int i = 0;

            foreach (var item in this.Sentences)
            {
                i++;

                if (start == 0)
                {

                    if (item.StartsWith("Abstract") || item.StartsWith("ABSTRACT:") || item.StartsWith("ABSTRACT") ||
                        item.StartsWith("SUMMARY") || item.StartsWith("A B S T R A C T"))
                    {
                        start = i;
                    }
                }

                if (end == 0)
                {
                    if (item.ToLower().StartsWith("keywords") || item.ToLower().StartsWith("key words:") || Regex.IsMatch(item, TextHandleTool.Introductionpattern, RegexOptions.IgnoreCase))
                    {
                        end = i;
                    }
                }

                if (start > 0 && end > 0 && end > start) break;
            }

            StringBuilder abstractWords = new StringBuilder();

            if (start > 0)
            {
                for (int j = start - 1; j < end - 1; j++)
                {
                    abstractWords.Append(Sentences[j]);
                }
            }
            Summary = abstractWords;
        }

        private void ExtractVolume()
        {
            //用正则验证期、卷、年

            Dictionary<PatternMode, string> patterns = new Dictionary<PatternMode, string>();

            patterns.Add(PatternMode.VolumeYearPage, @"\d+\s*\(\d{4}\)\s*\d+–\d+");
            patterns.Add(PatternMode.YearVolumePage, @"\(\d{4}\),?\s*\d+,?\s*[:,]\s*(pp\.\s*)?\d+\s*–\s*\d+");
            patterns.Add(PatternMode.VOLNOYear, @"VOL\.\s*\d+,\s*NO\.\s*\d+([\w\s,]+\d{4})?");
            patterns.Add(PatternMode.VolumePage, @"\d+\s*[,:]\s*\d+\s*–\s*\d+(,\s*\d{4})?");
            patterns.Add(PatternMode.VolumeNoPage1, @"\d+\s*[:]\s*\d+\s*,\s*\d+\s*[–-]\s*\d+");
            patterns.Add(PatternMode.VolumeNoPage, @"\d+\s*\(\d+\)\s*:\d+\s*-\s*\d+");
            patterns.Add(PatternMode.YearVolumeNoPage, @"\d{4}\s*[,]\s*Vol\.\s*\d+\s*,\s*Iss\.\s*\d+\s*,\s*pp\.\s*\d+\s*[–-]\s*\d+");
            patterns.Add(PatternMode.Volume, @"VOLUME\s*\d+");


            string monthPattern = "";

            foreach (var item in this.Sentences)
            {
                foreach (var m in TextHandleTool.months)
                {
                    if (item.Contains(m))
                    {
                        monthPattern = m;
                        break;
                    }
                }
            }

            string pagePattern = @"\s*\d+\s*[–-]\s*\d+\s*[,]\s*\d{4}";
            patterns.Add(PatternMode.MonthPageYear, monthPattern + pagePattern);


            VolumeHandleBase handle = null;

            foreach (var item in patterns)
            {
                if (item.Key == PatternMode.Volume)
                {
                    handle = new Volume(item.Value);

                    VolumeInfo = handle.GetVolume(this.Text);

                    if (handle.IsMatch) break;
                }

                if (item.Key == PatternMode.VolumeNoPage1)
                {
                    handle = new VolumeNoPage1(item.Value);

                    VolumeInfo = handle.GetVolume(this.Text);

                    if (handle.IsMatch) break;
                }

                if (item.Key == PatternMode.VolumeYearPage)
                {
                    handle = new VolumeYearPage(item.Value);

                    VolumeInfo = handle.GetVolume(this.Text);

                    if (handle.IsMatch) break;
                }

                if (item.Key == PatternMode.YearVolumePage)
                {
                    handle = new YearVolumePage(item.Value);

                    VolumeInfo = handle.GetVolume(this.Text);

                    if (handle.IsMatch) break;
                }

                if (item.Key == PatternMode.VOLNOYear)
                {
                    handle = new VolumeNoYear(item.Value);

                    VolumeInfo = handle.GetVolume(this.Text);

                    if (handle.IsMatch) break;
                }
                if (item.Key == PatternMode.VolumeNoPage)
                {
                    handle = new VolumeNoPage(item.Value);

                    VolumeInfo = handle.GetVolume(this.Text);

                    if (handle.IsMatch) break;
                }
                if (item.Key == PatternMode.VolumePage)
                {
                    handle = new VolumePage(item.Value);

                    VolumeInfo = handle.GetVolume(this.Text);

                    if (handle.IsMatch) break;
                }

                if (item.Key == PatternMode.MonthPageYear)
                {
                    handle = new MonthPageYear(item.Value);

                    VolumeInfo = handle.GetVolume(this.Text);

                    if (handle.IsMatch) break;

                }
                if (item.Key == PatternMode.YearVolumeNoPage)
                {
                    handle = new YearVolumeNoPage(item.Value);

                    VolumeInfo = handle.GetVolume(this.Text);

                    if (handle.IsMatch) break;

                }

            }
        }
    }
}
