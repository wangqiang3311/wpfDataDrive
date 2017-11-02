using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace pdfResearch
{
    public abstract class VolumeHandleBase
    {
        /// <summary>
        /// 模式
        /// </summary>
        public string Pattern { set; get; }

        /// <summary>
        /// 是否匹配
        /// </summary>
        public bool IsMatch { set; get; }

        /// <summary>
        /// 匹配结果
        /// </summary>
        public List<string> Results { set; get; }


        public VolumeHandleBase(string pattern)
        {
            this.Pattern = pattern;
        }


        public VolumeInfo GetVolume(string text)
        {
            VolumeInfo v = new VolumeInfo();
            Results = new List<string>();

            List<string> values = new List<string>();

            var maches = Regex.Matches(text, this.Pattern, RegexOptions.IgnoreCase);

            for (int k = 0; k < maches.Count; k++)
            {
                if (maches[k].Success)
                {
                    Results.Add(maches[k].Value);
                }
            }

            Results = Results.Distinct().ToList();

            if (Results.Count > 0)
            {
                IsMatch = true;
                HandleInfo(v);
            }
            return v;

        }

        public abstract void HandleInfo(VolumeInfo v);
    }
    /// <summary>
    /// 23 (2009) 881–897
    /// </summary>
    public class VolumeYearPage : VolumeHandleBase
    {

        public VolumeYearPage(string pattern)
            : base(pattern)
        {

        }

        public override void HandleInfo(VolumeInfo v)
        {
            var y = this.Results.First();

            var lstr = y.Split('(');
            v.Volume = lstr[0].Trim();

            var rstr = lstr[1].Split(')');
            v.Year = rstr[0].Trim();

            v.PageScope = rstr[1].Trim();
        }
    }

    /// <summary>
    /// (2009) 23[:,]881–897
    /// (2005), 80, pp. 387–401
    /// </summary>
    public class YearVolumePage : VolumeHandleBase
    {
        public YearVolumePage(string pattern)
            : base(pattern)
        {

        }
        public override void HandleInfo(VolumeInfo v)
        {
            var y = this.Results.First();

            if (y.Contains("pp"))
            {
                var lstr = y.Split(',');

                v.Volume = lstr[1].Trim();

                v.Year = lstr[0].TrimStart('(').TrimEnd(')');

                v.PageScope = lstr[2].Replace("pp.", "").Trim();
            }
            else
            {
                var lstr = y.Split(':');

                if (lstr.Count() == 1)
                {
                    lstr = y.Split(',');
                }

                v.PageScope = lstr[1].Trim();

                var rstr = lstr[0].Split(')');
                v.Volume = rstr[1].Trim();

                v.Year = rstr[0].Trim('(');
            }
        }
    }

    /// <summary>
    /// VOL. 38, NO. 6 AMERICAN WATER RESOURCES ASSOCIATION DECEMBER 2002
    /// VOL. 34, NO.1
    /// VOL. 36, NO. 3
    /// VOL. 64, NO. 2, FEBRUARY 2017
    /// </summary>
    public class VolumeNoYear : VolumeHandleBase
    {
        public VolumeNoYear(string pattern)
            : base(pattern)
        {

        }
        public override void HandleInfo(VolumeInfo v)
        {
            var y = this.Results.First();

            var lstr = y.Split(' ');

            v.Volume = lstr[1].Trim(',');

            if (lstr.Length == 3)
            {
                v.Issue = lstr[2].Split('.')[1];
            }
            if (lstr.Length == 4)
            {
                v.Issue = lstr.Last();
            }
            if (lstr.Length > 4)
            {
                v.Year = lstr.Last().Trim();
                v.Issue = lstr[3].Trim(',');
            }
        }
    }
    /// <summary>
    /// 41(1):145-155
    /// </summary>
    public class VolumeNoPage : VolumeHandleBase
    {
        public VolumeNoPage(string pattern)
            : base(pattern)
        {

        }
        public override void HandleInfo(VolumeInfo v)
        {
            var y = this.Results.First();

            var lstr = y.Split(':');
            v.PageScope = lstr[1].Trim();

            var rstr = lstr[0].Split('(');
            v.Volume = rstr[0].Trim();

            v.Issue = rstr[1].Trim(')');
        }
    }
    /// <summary>
    /// 30, 1751–1761
    /// 52: 77–105, 1998
    /// 14, 271–277, 2010
    /// </summary>
    public class VolumePage : VolumeHandleBase
    {
        public VolumePage(string pattern)
            : base(pattern)
        {

        }
        public override void HandleInfo(VolumeInfo v)
        {
            var y = this.Results.First();

            if (y.Contains(":"))
            {
                var lstr = y.Split(':');

                v.Volume = lstr[0].Trim();
                var rstr = lstr[1].Split(',');

                v.PageScope = rstr[0].Trim();
                v.Year = rstr[1].Trim();
            }
            else
            {
                var lstr = y.Split(',');

                v.PageScope = lstr[1].Trim();
                v.Volume = lstr[0].Trim();

                if (lstr.Length == 3)
                {
                    v.Year = lstr[2].Trim();
                }
            }
        }
    }
    /// <summary>
    /// November 18-21, 2003
    /// </summary>
    public class MonthPageYear : VolumeHandleBase
    {
        public MonthPageYear(string pattern)
            : base(pattern)
        {

        }
        public override void HandleInfo(VolumeInfo v)
        {
            var y = this.Results.First();

            var rstr = y.Split(',');
            v.Year = rstr[1].Trim();
            var lstr = rstr[0].Split(' ');
            v.PageScope = lstr[1];
        }
    }

    /// <summary>
    ///67:3, 252-262
    /// </summary>
    public class VolumeNoPage1 : VolumeHandleBase
    {
        public VolumeNoPage1(string pattern)
            : base(pattern)
        {

        }
        public override void HandleInfo(VolumeInfo v)
        {
            var y = this.Results.First();

            var rstr = y.Split(',');
            v.PageScope = rstr[1].Trim();
            var lstr = rstr[0].Split(':');
            v.Issue = lstr[1];
            v.Volume = lstr[0];
        }
    }
    /// <summary>
    ///2017, Vol. 11, Iss. 1, pp. 72–79
    /// </summary>
    public class YearVolumeNoPage : VolumeHandleBase
    {
        public YearVolumeNoPage(string pattern)
            : base(pattern)
        {

        }
        public override void HandleInfo(VolumeInfo v)
        {
            var y = this.Results.First();

            var rstr = y.Split(',');
            v.Year = rstr[0].Trim();
            var lstr = rstr[1].Split('.');
            v.Volume = lstr[1].Trim();

            lstr = rstr[2].Split('.');
            v.Issue = lstr[1].Trim();

            lstr = rstr[3].Split('.');
            v.PageScope = lstr[1].Trim();
        }
    }
    /// <summary>
    ///VOLUME 12
    /// </summary>
    public class Volume : VolumeHandleBase
    {
        public Volume(string pattern)
            : base(pattern)
        {

        }
        public override void HandleInfo(VolumeInfo v)
        {
            var y = this.Results.First();

            var rstr = y.Split(' ');
            v.Volume = rstr[1].Trim();
        }
    }

    public enum PatternMode
    {
        /// <summary>
        /// 卷年页
        /// </summary>
        VolumeYearPage,
        /// <summary>
        /// 年卷页
        /// </summary>
        YearVolumePage,
        /// <summary>
        /// 卷期年
        /// </summary>
        VOLNOYear,

        /// <summary>
        /// 卷期页
        /// </summary>
        VolumeNoPage,

        /// <summary>
        /// 卷页
        /// </summary>
        VolumePage,
        /// <summary>
        /// 月页年
        /// </summary>
        MonthPageYear,

        /// <summary>
        /// 卷期页1
        /// </summary>
        VolumeNoPage1,
        /// <summary>
        /// 年卷期页
        /// </summary>
        YearVolumeNoPage,
        Volume
    }


    public class VolumeInfo
    {
        ///<summary>
        ///年度
        ///</summary>
        public string Year { set; get; }
        ///<summary>
        ///卷
        ///</summary>
        public string Volume { set; get; }
        ///<summary>
        ///期
        ///</summary>
        public string Issue { set; get; }
        ///<summary>
        ///页码
        ///</summary>
        public string PageScope { set; get; }
    }
}
