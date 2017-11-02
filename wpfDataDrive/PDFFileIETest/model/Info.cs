using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pdfResearch
{
    public class Info
    {
        ///<summary>
        ///成果类型(1:期刊；2：会议；3：学位；4：图书；6：报纸；7：专利；8：标准；9：成果；13：年鉴；14：法律法规；0：通用类型
        ///</summary>
        public int BibType { set; get; }

        ///<summary>
        ///标题
        ///</summary>
        public string Title { set; get; }
        ///<summary>
        ///作者
        ///</summary>
        public string Author { set; get; }
        ///<summary>
        ///年度
        ///</summary>
        public int Year { set; get; }
        ///<summary>
        ///期刊
        ///</summary>
        public string Media { set; get; }
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
        ///<summary>
        ///出版社
        ///</summary>
        public string Press { set; get; }
        ///<summary>
        ///DOI
        ///</summary>
        public string DOI { set; get; }

        public string ISSN { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        /// <remarks>专利公开日期</remarks>
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// 出版日期
        /// </summary>
        /// <remarks>获奖日期, 专利申请日期，会议开始日期</remarks>
        public DateTime? PrintDate { get; set; }

        /// <summary>
        /// 官方链接地址
        /// </summary>
        public string Url { set; get; }

        /// <summary>
        /// 出版地
        /// </summary>
        public string Place { set; get; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { set; get; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { set; get; }
        /// <summary>
        /// 会议地点
        /// </summary>
        public string ConferencePlace { set; get; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { set; get; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { set; get; }
    }
}
