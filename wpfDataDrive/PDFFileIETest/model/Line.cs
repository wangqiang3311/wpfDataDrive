using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pdfResearch
{
    /// <summary>
    /// 行（由单词包括标点组成）
    /// </summary>
    public class Line
    {
        public Dictionary<int, List<WordMeta>> LineDic = new Dictionary<int, List<WordMeta>>();

        public void MakeLine(Word w)
        {
            List<WordMeta> words=new List<WordMeta>();

            int index = 0;

            foreach (var item in w.Words)
            {
                if (index == 0)
                {
                    index = item.Index;
                    words.Add(item);
                    LineDic.Add(index, words);
                }
                else
                {
                    if (item.Index == index)
                    {
                        LineDic[index].Add(item);
                    }
                    else
                    {
                        words = new List<WordMeta>();
                        words.Add(item);
                        index = item.Index;
                        LineDic.Add(item.Index, words);
                    }
                }
            }
        }
    }
}
