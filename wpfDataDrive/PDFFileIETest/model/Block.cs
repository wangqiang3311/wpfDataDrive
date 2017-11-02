using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pdfResearch
{
    /// <summary>
    /// 块（由行构成）
    /// </summary>
    public class Block
    {
        public Dictionary<int, List<WordMeta>> BlockDic = new Dictionary<int, List<WordMeta>>();

        public void MakeBlock(Line line)
        {
            var dic = line.LineDic;
            WordMeta last = null;

            int i = 1;

            //过滤掉空行
            var tempDic = new Dictionary<int, List<WordMeta>>();

            var filter = dic.Where(d => d.Value.Count == 1 && d.Value[0].Text.ToString().Trim() != "" || d.Value.Count > 1).OrderBy(d => d.Key);

            int j = 0;

            if (filter != null && filter.Count() > 0)
            {

                foreach (var item in filter)
                {
                    tempDic.Add(j, item.Value);

                    j++;
                }
            }

            foreach (var item in tempDic)
            {
                bool isNextEqual = false;
                bool isLastPreMatch = false;

                if (item.Value.Count > 0)
                {
                    var current = item.Value[0];

                    if (last != null)
                    {
                        if (!current.IsValid(last)) continue;

                        var gindex = BlockDic.Last().Value.GroupBy(v => v.Index).OrderBy(v => v.Key).Take(2);

                        float spaceHeight = 0;

                        if (gindex.Count() > 1)
                        {
                            var first = gindex.First().First().WordChars.Average(w => w.Y);

                            var second = gindex.Last().First().WordChars.Average(w => w.Y);

                            var secondAveHeight = gindex.Last().First().WordChars.Average(w => w.Height);

                            spaceHeight = Math.Abs(second - first) - secondAveHeight;

                            last.SpaceHeight = spaceHeight;
                        }

                        if (BlockDic.Last().Value.Count == 1 && BlockDic.Last().Value.First() == last)
                        {
                            last.IsFirst = true;
                        }

                        if (BlockDic.Last().Value.Count > 1 && BlockDic.Last().Value.Last() == last)
                        {
                            last.OffSetHeight = Math.Abs(BlockDic.Last().Value.Last().WordChars.First().Y - BlockDic.Last().Value.First().WordChars.First().Y);
                            last.OffSetWidth = Math.Abs(BlockDic.Last().Value.Last().WordChars.Max(v => v.X) - BlockDic.Last().Value.First().WordChars.Min(v => v.X));

                        }


                        //计算宽度
                        var minX = last.WordChars.Min(v => v.X);
                        var maxX = last.WordChars.Max(v => v.X);

                        last.Width = maxX - minX;
                        current.Width = current.WordChars.Max(v => v.X) - current.WordChars.Min(v => v.X);


                        last.MaxY = last.WordChars.Max(v => v.Y);
                        last.MaxX = last.WordChars.Max(v => v.X);
                        current.MaxX = current.WordChars.Max(v => v.X);


                        if (current.Equals(last))
                        {
                            if (BlockDic.Keys.Contains(i))
                            {
                                BlockDic[i].AddRange(item.Value);
                            }
                        }

                        else
                        {
                            //如果last是一个上缀，那么取last的上一个与当前的比较

                            if (last.WordChars.Count == 1 && BlockDic.Last().Value.Count > 1 && BlockDic.Last().Value.Last() == last)
                            {
                                isLastPreMatch = false;

                                var lastPre = BlockDic.Last().Value[BlockDic.Last().Value.Count - 2];

                                if (current.Equals(lastPre))
                                {
                                    if (BlockDic.Keys.Contains(i))
                                    {
                                        BlockDic[i].AddRange(item.Value);

                                    }

                                    isLastPreMatch = true;
                                }
                                else
                                {
                                    isLastPreMatch = false;
                                }

                            }
                            else
                            {

                                int count = 0;

                                isNextEqual = false;

                                while (count < 3 && !current.IsEndBlock)
                                {
                                    count++;

                                    if (tempDic.Keys.Contains(item.Key + count))
                                    {
                                        var next = tempDic[item.Key + count];

                                        var nextFirstChar = next[0].WordChars.First();

                                        var nextLastChar = next[0].WordChars.Last();

                                        if (Math.Abs(nextFirstChar.Y - last.WordChars.First().Y) < 30
                                             && Math.Abs(current.WordChars.First().Y - nextFirstChar.Y) < 30
                                             && last.WordChars.First().Word.Trim() != ""
                                             && next[0].WordChars.Count > 1
                                             && (Math.Abs(nextFirstChar.X - last.WordChars.First().X) < 60
                                             || Math.Abs(nextFirstChar.X - last.WordChars.Last().X) < 60))
                                        {
                                            if (next[0].Equals(last))
                                            {
                                                isNextEqual = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (isNextEqual)
                            {
                                if (current.WordChars.First().X - last.WordChars.First().X < 30 || current.WordChars.First().X - last.WordChars.Last().X < 30)
                                {
                                    if (BlockDic.Keys.Contains(i))
                                    {
                                        BlockDic[i].AddRange(item.Value);
                                    }
                                }
                                else
                                    continue;
                            }
                            else if (isLastPreMatch == false)
                            {
                                i++;
                                BlockDic.Add(i, item.Value);
                            }
                        }
                    }
                    else
                    {
                        BlockDic.Add(i, item.Value);
                    }
                    if (isNextEqual == false)
                    {
                        current.LastWord = last;
                        last = current;
                    }
                }
            }
        }
    }
}
