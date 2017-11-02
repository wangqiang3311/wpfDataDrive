using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pdfResearch
{
    public class BlockInfo
    {
        /// <summary>
        /// 行数
        /// </summary>
        public int LineCount { set; get; }
        /// <summary>
        /// 块高度
        /// </summary>
        public double Height { set; get; }
        /// <summary>
        /// 块宽度
        /// </summary>
        public double Width { set; get; }

        /// <summary>
        /// 参数评分（height和space）
        /// </summary>
        public double ParamMark { set; get; }

        /// <summary>
        /// 最终评分
        /// </summary>
        public double Mark { set; get; }

        /// <summary>
        /// 是否中文
        /// </summary>
        public bool IsChinese { set; get; }

        /// <summary>
        /// 代表字符
        /// </summary>
        public WordInfo RepresentativeChar { set; get; }

        /// <summary>
        /// 开始字符
        /// </summary>
        public WordInfo FirstChar { set; get; }

        /// <summary>
        /// 结束字符
        /// </summary>
        public WordInfo EndChar { set; get; }

        /// <summary>
        /// 字符数
        /// </summary>
        public int CharCount { set; get; }

        /// <summary>
        /// 单词数
        /// </summary>
        public int WordMetaCount { set; get; }

        /// <summary>
        /// 最大x
        /// </summary>
        public double MaxX { set; get; }

        /// <summary>
        /// 最小x
        /// </summary>
        public double MinX { set; get; }

        /// <summary>
        /// 1:偏左，2：居中,3：偏右
        /// </summary>
        public BlockPostion Position { set; get; }

        /// <summary>
        /// 字符平均高度
        /// </summary>
        public double CharAveHeight { set; get; }

        /// <summary>
        /// 字符平均宽度
        /// </summary>
        public double CharAveWidth { set; get; }


        /// <summary>
        /// 字符平均Space
        /// </summary>
        public double CharAveSpace { set; get; }

        /// <summary>
        /// xsize
        /// </summary>
        public double CharAveXSize { set; get; }

        /// <summary>
        /// ysize
        /// </summary>
        public double CharAveYSize { set; get; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { set; get; }

        /// <summary>
        /// 单词块集合
        /// </summary>
        public List<WordMeta> WordMetas { set; get; }

        /// <summary>
        /// 字符集合
        /// </summary>
        public List<WordInfo> WordChars { set; get; }

    }

    public enum BlockPostion
    {
        Left = 1,
        Center = 2,
        Right = 3
    }
}
