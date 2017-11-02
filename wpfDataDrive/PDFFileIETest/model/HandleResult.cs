using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdfResearch
{
    public class HandleResult
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// 候选标题
        /// </summary>
        public List<string> TitleCandidateList { set; get; }

        /// <summary>
        /// 是否乱码
        /// </summary>
        public bool IsGarbled { set; get; }
    }
}
