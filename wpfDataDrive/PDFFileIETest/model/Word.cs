using javax.swing.text;
using ResourceShare.UserClient.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pdfResearch
{
    public class WordInfo
    {
        /// <summary>
        /// x坐标
        /// </summary>
        public float X { set; get; }
        /// <summary>
        /// y坐标
        /// </summary>
        public float Y { set; get; }

        public int XSize { set; get; }

        public int YSize { get; set; }

        public float XDirAdj { set; get; }

        public float YDirAdj { set; get; }

        /// <summary>
        /// 字号
        /// </summary>
        public float FontSize { set; get; }

        public float Xscale { set; get; }

        public float Yscale { set; get; }
        /// <summary>
        /// 高度
        /// </summary>
        public float Height { set; get; }

        /// <summary>
        /// 空格大小
        /// </summary>
        public float Space { set; get; }
        /// <summary>
        /// 宽度
        /// </summary>
        public float Width { set; get; }
        /// <summary>
        /// 子字体
        /// </summary>
        public string Subfont { set; get; }
        /// <summary>
        /// 基本字体
        /// </summary>
        public string Basefont { set; get; }
        /// <summary>
        /// 是否加粗
        /// </summary>
        public bool IsBold { set; get; }
        /// <summary>
        /// 是否倾斜
        /// </summary>
        public bool IsItalic { set; get; }
        /// <summary>
        /// 单词
        /// </summary>
        public string Word { set; get; }

        public override string ToString()
        {
            return "String[" + this.XDirAdj + ","
                 + this.YDirAdj
                 + " fs=" + this.FontSize
                 + " xscale=" + this.Xscale
                 + " isBold=" + this.IsBold
                 + " space=" + this.Space
                 + " isItalic=" + this.IsItalic
                 + "xSize" + this.XSize
                 + "ySize" + this.YSize
                 + " width=" + this.Width + "]"
                 + this.Word;
        }
        /// <summary>
        /// 计算当前字符和lastChunk的距离
        /// </summary>
        /// <param name="lastChunk"></param>
        /// <returns></returns>
        public float DistanceFromEndOf(WordInfo lastChunk)
        {
            return this.X - lastChunk.X - lastChunk.Width;
        }


    }

    public enum WordStyle
    {
        HEADING,
        PARAGRAPH

    }

    public class Word
    {
        /// <summary>
        /// 单词集合
        /// </summary>
        public List<WordMeta> Words = new List<WordMeta>();

        public void MakeWord(List<WordInfo> wordInfos)
        {
            int i = 1;
            //上一个字符块
            WordInfo lastChunk = null;

            //上一个单词
            WordMeta lastTextInfo = new WordMeta()
            {
                Index = i,
                Text = new StringBuilder(),
                WordChars = new List<WordInfo>()
            };
            foreach (var chunk in wordInfos)
            {
                if (lastChunk == null)
                {
                    lastTextInfo.Text.Append(chunk.Word);
                    lastTextInfo.WordChars.Add(chunk);
                    i++;
                    Words.Add(lastTextInfo);
                }
                else
                {
                    //属于同一行
                    if (IsSameLine(chunk, lastChunk))
                    {
                        float dist = chunk.DistanceFromEndOf(lastChunk);

                        if (dist < -chunk.Space)
                        {
                            lastTextInfo.Text.Append(" ");
                        }
                        //append a space if the trailing char of the prev string wasn't a space && the 1st char of the current string isn't a space
                        else if (dist > chunk.Space / 2.0f && chunk.Word.Length > 0 && chunk.Word[0] != ' ' && lastChunk.Word.Length > 0 && lastChunk.Word[lastChunk.Word.Length - 1] != ' ')
                        {
                            lastTextInfo.Text.Append(" ");
                        }

                        lastTextInfo.Text.Append(chunk.Word);
                        lastTextInfo.WordChars.Add(chunk);
                    }
                    else
                    {
                        lastTextInfo = new WordMeta()
                        {
                            Index = i,
                            Text = new StringBuilder(),
                            WordChars = new List<WordInfo>()
                        };
                        lastTextInfo.Text.Append(chunk.Word);
                        lastTextInfo.WordChars.Add(chunk);
                        i++;
                        Words.Add(lastTextInfo);
                    }
                }
                lastChunk = chunk;
            }
        }


        protected WordStyle GetStyle(WordInfo pos)
        {
            if ((pos.FontSize * pos.Yscale) > 14)
            {
                return WordStyle.HEADING;
            }
            else
            {
                return WordStyle.PARAGRAPH;
            }
        }

        private static bool IsSameLine(WordInfo cur, WordInfo prev)
        {
            if (cur.Y == prev.Y)
            {
                return true;
            }
            else
            {
                var prevCenter = prev.Y + prev.Height / 2.0f;
                var prevHeight = prev.Height;
                var curCenter = cur.Y + cur.Height / 2.0f;

                var result = Math.Abs(curCenter - prevCenter) < (prevHeight * 0.25f);

                return result;
            }
        }


    }
}
