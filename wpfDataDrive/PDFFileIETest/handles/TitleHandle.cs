using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace pdfResearch
{
    public class TitleHandle
    {
        public static List<BlockData> TrainDatas = new List<BlockData>();

        private static List<string> ExceptBaseFonts = new List<string>();

        private static string[] XingShi = {"李","王","张", "刘", "杨","陈","黄","赵","冯","周","吴","徐","郑","马","朱","胡","郭","何",
                                              "高","林","罗","孙","梁","谢","宋" };

        private static string[] filterWords = new string[] { "［摘 要］", "[摘要]", "【摘要】", "摘要", "关键词", "一、引言", "ABSTRACT", "INTRODUCTION", "Keywords:" };

        private static string[] containsWords = new string[] { "www.", "http://", "DOI:", "DOI：" };


        static TitleHandle()
        {
            ExceptBaseFonts.Add("Frutiger-UltraBlack");
            ExceptBaseFonts.Add("CKMBJG+GillSans-Bold");
            ExceptBaseFonts.Add("GIDJOG+AdvP0DEA");
            ExceptBaseFonts.Add("DGGNPP+Goudy-Bold");
            ExceptBaseFonts.Add("NEMJNG+Frutiger-UltraBlack");
            ExceptBaseFonts.Add("ADBAJE+Frutiger-UltraBlack");
            ExceptBaseFonts.Add("AJDNHA+AdvTT3b30f6db.B");
            ExceptBaseFonts.Add("BCPAKE+AdvTT3b30f6db.B");
        }



        public static HandleResult GetTitle(Block block, bool isTrainModel = true)
        {
            HandleResult result = new HandleResult() { Title = "" };

            List<WordMeta> list = new List<WordMeta>();

            List<int> blockIds = new List<int>();

            double maxYSize = 0;
            double maxXSize = 0;

            double maxSpace = 0;

            //最大块Id
            int maxYSizeBlockId = 0;

            int maxXSizeBlockId = 0;

            List<int> maxSpaceBIds = new List<int>();

            Dictionary<int, BlockInfo> blockInfos = new Dictionary<int, BlockInfo>();


            double aveX = 0;

            if (block.BlockDic.Count > 0)
            {
                //由于库先读取的行不一定是第一行，所以按Y坐标排序
                var dicTop = block.BlockDic.OrderBy(b => b.Value.First().WordChars.First().Y).Take(15);


                var minX = dicTop.Min(s => s.Value.First().WordChars.First().X);
                var maxX = dicTop.Max(s => s.Value.Last().WordChars.Last().X);

                if (maxX > 1000) maxX = 600;
                if (minX < 0) minX = 5;

                aveX = (minX + maxX) / 2;

                foreach (var item in dicTop)
                {
                    #region 过滤

                    //过滤掉空块
                    if (item.Value.Sum(s => s.Text.ToString().Trim().Length) < 3) continue;

                    //过滤敏感词
                    bool isValid = IsContainUrl(item.Value);
                    if (!isValid) continue;

                    isValid = IsContainsSomeWords(item.Value);

                    if (!isValid) continue;

                    //过滤字数比较少的
                    bool isChinese = false;

                    isValid = IsValidCount(item.Value, out isChinese);

                    if (!isValid) continue;

                    //过滤掉特殊字体的

                    var firstChar = item.Value.First().WordChars.First();

                    if (ExceptBaseFonts.Contains(firstChar.Basefont)) continue;

                    //过滤掉Y值特别小的
                    if (firstChar.Y < 30) continue;

                    var text = string.Join(" ", item.Value.Select(s => s.Text));

                    //去掉字数特别多的块

                    var wordCount = text.Split(' ').Length;

                    if (wordCount > 50) continue;

                    if (text.Contains(",") && text.Split(',').Length >= 3 && wordCount == 1) continue;

                    if (text.Contains("&") && wordCount == 1 && text.Length < 40) continue;

                    string[] items = text.Split('，');

                    bool isAuthor = false;
                    //过滤掉带有百家姓的

                    if (isChinese)
                    {
                        if (items.Length > 1)
                        {
                            foreach (var a in items)
                            {
                                if (a.Length == 0) continue;
                                if (XingShi.Contains(a.First().ToString()))
                                {
                                    isAuthor = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            items = text.Split(' ');

                            if (items.Length > 1)
                            {
                                foreach (var a in items)
                                {
                                    if (a.Length == 0) continue;

                                    if (XingShi.Contains(a.First().ToString()))
                                    {
                                        isAuthor = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (isAuthor) continue;

                    #endregion

                    #region 创建块信息

                    BlockInfo bi = new BlockInfo();
                    bi.WordMetas = item.Value;
                    bi.FirstChar = firstChar;
                    bi.EndChar = item.Value.Last().WordChars.Last();
                    bi.Text = text;

                    List<WordInfo> wordInfos = new List<WordInfo>(512);

                    foreach (var word in bi.WordMetas)
                    {
                        foreach (var sitem in word.WordChars)
                        {
                            wordInfos.Add(sitem);
                        }
                    }

                    if (wordInfos.Count > 100 && isChinese) continue;
                    if (wordInfos.Count > 280 && !isChinese) continue;


                    bi.WordChars = wordInfos;
                    bi.MinX = wordInfos.Min(v => v.X);
                    bi.MaxX = wordInfos.Max(v => v.X);

                    bi.Width = bi.MaxX - bi.MinX;
                    bi.Height = wordInfos.Max(v => v.Y) - wordInfos.Min(v => v.Y);
                    bi.IsChinese = isChinese;
                    bi.WordMetaCount = item.Value.Count;
                    bi.CharAveHeight = wordInfos.Average(v => v.Height);
                    bi.CharAveWidth = wordInfos.Average(v => v.Width);
                    bi.CharAveSpace = wordInfos.Average(v => v.Space);
                    bi.CharAveXSize = wordInfos.Average(v => v.XSize);
                    bi.CharAveYSize = wordInfos.Average(v => v.YSize);

                    bi.CharCount = wordInfos.Count;
                    bi.LineCount = item.Value.GroupBy(v => v.Index).Count();

                    if (bi.MinX < aveX + 40 && bi.MaxX > aveX) bi.Position = BlockPostion.Center;
                    else if (bi.MinX < aveX + 40 && bi.MaxX < aveX) bi.Position = BlockPostion.Left;
                    else bi.Position = BlockPostion.Right;

                    //过滤掉位置偏右，且字块非常小的
                    if (bi.IsChinese && bi.Position == BlockPostion.Right && bi.CharCount < 7 && (maxX - bi.MaxX) / maxX < 0.20) continue;
                    //过滤掉位置偏左，且字块非常小的
                    if (bi.IsChinese && bi.Position == BlockPostion.Left && bi.CharCount < 7 && bi.Width / maxX < 0.3) continue;

                    if (!bi.IsChinese && bi.Position == BlockPostion.Right && bi.CharCount < 25 && (maxX - bi.MaxX) / maxX < 0.11) continue;

                    //找到代表字符
                    var wordInfo = bi.FirstChar;

                    //从所有的去掉Height最大值
                    var top5Chars = wordInfos.Take(5);

                    var hgroup = top5Chars.GroupBy(t => (int)t.Height);

                    var filterTop = hgroup.OrderByDescending(h => h.Count()).Take(3);

                    var gCount = filterTop.Count();

                    if (filterTop != null && gCount > 0)
                    {
                        var wInfos = filterTop.First().ToList();

                        if (gCount >= 3)
                        {
                            wInfos = filterTop.OrderByDescending(h => h.Key).ToArray()[1].ToList();
                        }
                        if (wInfos.Count > 0)
                            wordInfo = wInfos.First();
                    }

                    bi.RepresentativeChar = wordInfo;

                    blockInfos.Add(item.Key, bi);

                    #endregion

                    #region 计算最大值

                    //计算最大值
                    var chars = wordInfos;

                    if (chars.Count > 0)
                    {
                        var aveCharYsize = chars.Average(c => c.YSize);
                        //获取最大字体
                        if (aveCharYsize > maxYSize)
                        {
                            maxYSize = aveCharYsize;
                            maxYSizeBlockId = item.Key;
                        }

                        var aveCharXsize = chars.Average(c => c.XSize);
                        if (aveCharXsize > maxXSize)
                        {
                            maxXSize = aveCharXsize;
                            maxXSizeBlockId = item.Key;
                        }

                        var aveCharSpace = chars.Average(c => c.Space);

                        //获取到最大space
                        if (aveCharSpace > maxSpace)
                        {
                            maxSpace = aveCharSpace;
                        }
                    }

                    #endregion
                }
            }

            if (blockInfos.Count == 0) return result;

            var infos = blockInfos[maxYSizeBlockId].WordMetas;

            var candidateList = blockInfos.OrderByDescending(b => b.Value.WordMetas.First().WordChars.First().YSize)
                 .OrderBy(b => b.Value.WordMetas.First().WordChars.First().Y);


            var cList = candidateList.Take(10).ToList();

            if (cList.Count == 1)
            {
                infos = cList.First().Value.WordMetas;
            }
            if (cList.Count > 1)
            {
                var maxHeight = cList.Max(c => c.Value.CharAveHeight);
                var maxWidth = cList.Max(c => c.Value.CharAveWidth);

                Dictionary<int, double> marks = new Dictionary<int, double>();

                double maxMark = 0;
                double maxPM = 0;

                int maxMarkKey = 0;

                int maxMarkParmKey = 0;

                Dictionary<int, double> paramDic = new Dictionary<int, double>();

                //计算每个块的评分
                foreach (var item in cList)
                {
                    var wordInfo = item.Value.FirstChar;
                    var wordLastInfo = item.Value.EndChar;

                    var tempWordMeta = item.Value.WordMetas;

                    if (item.Value.WordMetas.Where(s => s.Text.Length > 5).Count() == item.Value.WordMetaCount)
                    {
                        tempWordMeta = item.Value.WordMetas.OrderBy(v => v.WordChars.First().Y).ToList();
                    }

                    //去掉前后字符差异大的块
                    if (!tempWordMeta.Exists(v => v.IsSubTitleStart) && tempWordMeta.Count > 1
                        && tempWordMeta.Last().WordChars.Count > 3 && wordInfo.YSize - wordLastInfo.YSize >= 2
                        && wordInfo.Space - wordLastInfo.Space > 2 && wordInfo.Basefont != wordLastInfo.Basefont)
                    {
                        continue;
                    }
                    if (item.Value.CharAveSpace == 0) item.Value.CharAveSpace = (float)maxSpace / 2;

                    var pm = (item.Value.CharAveHeight / maxHeight) * 0.5 + (item.Value.CharAveSpace / maxSpace) * 0.5;

                    if (pm > maxPM)
                    {
                        maxPM = pm;

                        maxMarkParmKey = item.Key;

                    }
                    item.Value.ParamMark = pm;

                    paramDic.Add(item.Key, pm);

                }

                var filterParamDic = paramDic.Where(p => maxPM - p.Value <= 0.3 || maxPM - p.Value <= 0.4 && blockInfos[p.Key].Width > blockInfos[maxMarkParmKey].Width).Take(3).ToList();

                foreach (var item in filterParamDic)
                {
                    var m = GetMark(blockInfos[item.Key], maxHeight, maxWidth, maxYSize, maxXSize, maxSpace);

                    if (m > maxMark)
                    {
                        maxMark = m;
                        maxMarkKey = item.Key;
                    }
                    marks.Add(item.Key, m);
                }

                //判断是否需要去掉最高分

                if (maxMarkKey == 0) maxMarkKey = cList.First().Key;

                var maxItem = blockInfos[maxMarkKey];

                var iText = maxItem.Text;


                var mr = maxItem.RepresentativeChar;


                if (marks.Count > 1)
                {
                    var second = marks.Where(m => m.Key != maxMarkKey).OrderByDescending(m => m.Value).First();


                    var secondMark = second.Value;

                    var secondItem = blockInfos[second.Key];

                    var t = secondItem.Text;
                    var width = secondItem.Width;

                    var sr = secondItem.RepresentativeChar;

                    //待定

                    if (maxItem.IsChinese && maxItem.CharCount < 5 && maxItem.FirstChar.Word.Contains("o"))
                    {
                        marks.Remove(maxMarkKey);
                        maxMark = secondMark;
                    }
                    else if (maxItem.FirstChar.Y < 40 && maxItem.Width < secondItem.Width && maxItem.WordMetaCount < secondItem.WordMetaCount && secondItem.WordMetaCount < 5 && maxItem.Height <= secondItem.Height)
                    {
                        marks.Remove(maxMarkKey);
                        maxMark = secondMark;
                    }
                }

                var almostMax = marks.Where(m => maxMark - m.Value <= 0.13);

                var keys = almostMax.OrderByDescending(a => a.Value).Take(2).Select(m => m.Key).ToList();

                var find = cList.Where(c => keys.Contains(c.Key)).OrderBy(c => c.Value.WordChars.First().Y).ToList();

                if (find != null)
                {
                    if (find.Count > 1)
                    {
                        double[] poses = new double[2];
                        double[] posesLast = new double[2];

                        double[] posesX = new double[2];
                        double[] posesLastX = new double[2];

                        double[] widths = new double[2];

                        double[] grades = new double[2];

                        double[] heights = new double[2];

                        double[] spaces = new double[2];


                        int[] wordsCount = new int[2];
                        int[] indexs = new int[2];


                        var firstKey = find.First().Key;
                        var lastKey = find.Last().Key;

                        var firstItem = blockInfos[firstKey];
                        var lastItem = blockInfos[lastKey];


                        result.TitleCandidateList = new List<string>();
                        result.TitleCandidateList.Add(string.Join(" ", firstItem.WordMetas.Select(s => s.Text)));
                        result.TitleCandidateList.Add(string.Join(" ", lastItem.WordMetas.Select(s => s.Text)));


                        var firstWordInfo = firstItem.RepresentativeChar;
                        var lastWordInfo = lastItem.RepresentativeChar;


                        poses[0] = firstWordInfo.Y;
                        poses[1] = lastWordInfo.Y;

                        posesX[0] = firstWordInfo.X;
                        posesX[1] = lastWordInfo.X;


                        posesLast[0] = firstItem.WordMetas.Last().WordChars.First().Y;
                        posesLast[1] = lastItem.WordMetas.Last().WordChars.First().Y;

                        indexs[0] = firstItem.WordMetas.First().Index;
                        indexs[1] = lastItem.WordMetas.First().Index;


                        wordsCount[0] = firstItem.Text.Split(' ').Length;
                        wordsCount[1] = lastItem.Text.Split(' ').Length;

                        widths[0] = firstItem.Width;
                        widths[1] = lastItem.Width;

                        posesLastX[0] = firstItem.WordChars.Max(v => v.X);
                        posesLastX[1] = lastItem.WordChars.Max(v => v.X);

                        grades[0] = marks[firstKey];
                        grades[1] = marks[lastKey];


                        heights[0] = firstItem.CharAveHeight;
                        heights[1] = lastItem.CharAveHeight;

                        spaces[0] = firstItem.CharAveSpace;
                        spaces[1] = lastItem.CharAveSpace;


                        var subDis = Math.Min(Math.Abs(poses[0] - posesLast[1]), Math.Abs(poses[1] - posesLast[0]));

                        //根据逗号分隔判断
                        var isFind = JudgeByComma(blockInfos, ref infos, firstKey, lastKey, firstItem, lastItem);

                        if (isFind == false)
                            //根据位置判断
                            isFind = JudgeByPosition(ref infos, firstItem, lastItem);

                        if (isFind == false)

                            if (subDis < 100)
                            {
                                //根据3w和Title判断
                                isFind = JudgeBy3w(blockInfos, block.BlockDic, ref infos, firstKey, lastKey);

                                //相似长度的判断
                                if (isFind == false) isFind = JudgeBySameLength(ref infos, firstItem, lastItem);

                                //根据位置及长度判断
                                if (isFind == false && firstItem.Position == BlockPostion.Center && lastItem.Position == BlockPostion.Center)
                                {
                                    if (firstItem.WordMetaCount == lastItem.WordMetaCount && firstItem.Height == lastItem.Height)
                                    {
                                        if (firstItem.ParamMark - lastItem.ParamMark < 0.03 && lastItem.Width / firstItem.Width > 0.6)
                                        {
                                            var bf = lastItem.RepresentativeChar.Basefont;

                                            if (bf.ToLower().Contains("-b") || bf.ToLower().Contains(".b"))
                                            {
                                                infos = lastItem.WordMetas;
                                            }
                                        }

                                        else if (lastItem.Width > firstItem.Width && lastItem.RepresentativeChar.IsBold && firstItem.RepresentativeChar.IsBold)
                                        {
                                            infos = lastItem.WordMetas;
                                        }
                                        else if (firstItem.Width / lastItem.Width > 0.6 && (firstItem.CharAveHeight > lastItem.CharAveHeight || firstItem.CharAveSpace > lastItem.CharAveSpace) && lastItem.CharAveWidth >= lastItem.CharAveWidth)
                                        {
                                            infos = firstItem.WordMetas;
                                        }
                                        else
                                        {
                                            infos = firstItem.Width > lastItem.Width ? firstItem.WordMetas : lastItem.WordMetas;
                                        }
                                        isFind = true;
                                    }
                                    else if (firstItem.WordMetaCount > lastItem.WordMetaCount && firstItem.Width / lastItem.Width > 0.5 && firstItem.Height > lastItem.Height && (firstItem.WordMetaCount < 6 || firstItem.Height < 55))
                                    {

                                        infos = firstItem.WordMetas;
                                        isFind = true;
                                    }
                                    else if (firstItem.WordMetaCount < lastItem.WordMetaCount && lastItem.Width / firstItem.Width > 0.5 && firstItem.Height < lastItem.Height && (lastItem.WordMetaCount < 6 || lastItem.Height < 55))
                                    {
                                        infos = lastItem.WordMetas;
                                        isFind = true;
                                    }
                                }
                                if (isFind == false && firstItem.Position == BlockPostion.Center && lastItem.Position == BlockPostion.Left)
                                {
                                    if (Math.Round(grades[0] - grades[1], 2) <= 0.09 && (int)spaces[0] - (int)spaces[1] < 3

                                       && heights[0] > heights[1] && firstKey > 5 && firstKey < 20 && lastKey == 1

                                       && (widths[0] - widths[1]) / widths[1] < 0.25)
                                    {
                                        infos = lastItem.WordMetas;
                                    }
                                    else
                                    {
                                        infos = firstItem.Width > lastItem.Width ? firstItem.WordMetas : lastItem.WordMetas;
                                    }
                                    isFind = true;
                                }
                                if (isFind == false && firstItem.Position == BlockPostion.Left && lastItem.Position == BlockPostion.Center)
                                {
                                    infos = lastItem.Width > firstItem.Width && (lastItem.CharAveHeight > firstItem.CharAveHeight || lastItem.CharAveSpace > firstItem.CharAveSpace) ? lastItem.WordMetas : firstItem.WordMetas;
                                    isFind = true;
                                }
                                if (isFind == false)
                                    if (spaces[1] > 0 && spaces[0] > 0 && Math.Abs((int)heights[0] - (int)heights[1]) < 2 && Math.Abs((int)spaces[1] - (int)spaces[0]) >= 5)
                                    {
                                        infos = (int)spaces[1] > (int)spaces[0] ? lastItem.WordMetas : firstItem.WordMetas;
                                    }

                                    else if (Math.Abs(grades[0] - grades[1]) < 0.03 && Math.Abs(heights[0] - heights[1]) < 0.4 && Math.Abs(spaces[0] - spaces[1]) < 0.4)
                                    {
                                        if (firstItem.Position == BlockPostion.Center && lastItem.Position == BlockPostion.Center && (lastItem.WordMetaCount < 6 || lastItem.Height < 26) && lastItem.WordMetaCount > firstItem.WordMetaCount
                                          && lastItem.Width > firstItem.Width) infos = lastItem.WordMetas;

                                        else if (firstItem.Position == BlockPostion.Center && lastItem.Position == BlockPostion.Center && lastItem.WordMetaCount >= firstItem.WordMetaCount
                                             && Math.Abs(lastItem.Width - firstItem.Width) < 40) infos = lastItem.WordMetas;

                                        else if (lastItem.Width < firstItem.Width && lastItem.WordMetaCount <= firstItem.WordMetaCount)
                                        {
                                            infos = firstItem.WordMetas;
                                        }
                                        else if (lastItem.Width > firstItem.Width && lastItem.WordMetaCount >= firstItem.WordMetaCount)
                                        {
                                            infos = lastItem.WordMetas;
                                        }
                                    }
                                    else if (posesX[0] < aveX - 110 && posesX[1] < aveX && posesLastX[0] < aveX && posesLastX[1] > aveX && widths[1] - widths[0] > 20)
                                    {
                                        //上面的标题特别短，没有超过中轴线，而下面的标题居中分布
                                        infos = lastItem.WordMetas;
                                    }
                                    else if (posesX[1] < aveX - 110 && posesX[0] < aveX && posesLastX[1] < aveX && posesLastX[0] > aveX && widths[0] - widths[1] > 20)
                                    {
                                        infos = firstItem.WordMetas;
                                    }

                                    else if (posesX[0] < aveX - 110 && posesX[1] < aveX && posesLastX[0] < aveX && posesLastX[1] > aveX && widths[1] - widths[0] < 20)
                                    {
                                        //上面的标题特别短，没有超过中轴线，而下面的标题居中分布
                                        infos = lastItem.WordMetas.GroupBy(w => w.Index).Count() == 1 ? firstItem.WordMetas : lastItem.WordMetas;
                                    }
                                    else if (posesX[1] < aveX - 110 && posesX[0] < aveX && posesLastX[1] < aveX && posesLastX[0] > aveX && widths[0] - widths[1] < 20)
                                    {
                                        infos = firstItem.WordMetas.GroupBy(w => w.Index).Count() == 1 ? lastItem.WordMetas : firstItem.WordMetas;
                                    }


                                    else if (Math.Abs(grades[0] - grades[1]) < 0.004 && spaces[1] == spaces[0] && heights[0] == heights[1])
                                    {
                                        infos = widths[1] > widths[0] ? lastItem.WordMetas : firstItem.WordMetas;

                                    }

                                    else if (Math.Abs(grades[1] - grades[0]) < 0.08)
                                    {
                                        if (lastItem.WordMetaCount >= firstItem.WordMetaCount && (lastItem.WordMetaCount < 6 || lastItem.Height < 26) && lastItem.Position == BlockPostion.Center
                                            && firstItem.Position == BlockPostion.Center && lastItem.Width / firstItem.Width > 0.5
                                            && lastItem.FirstChar.X - firstItem.FirstChar.X > 100)
                                        {
                                            infos = lastItem.WordMetas;
                                        }
                                        else if (lastItem.WordMetaCount >= firstItem.WordMetaCount && (lastItem.WordMetaCount < 6 || lastItem.Height < 26) && lastItem.Width / firstItem.Width > 0.7)
                                        {
                                            infos = lastItem.WordMetas;
                                        }
                                        else if (Math.Abs(lastItem.WordMetaCount - firstItem.WordMetaCount) < 2 && lastItem.WordMetaCount < 6 && lastItem.Width - firstItem.Width > 100)
                                        {
                                            infos = lastItem.WordMetas;
                                        }
                                        else
                                        {
                                            infos = firstItem.WordMetas;
                                        }
                                    }

                                    else if (grades[0] >= grades[1] && Math.Abs((int)spaces[1] - (int)spaces[0]) < 5 && widths[0] / widths[1] > 0.7)
                                    {
                                        infos = firstItem.WordMetas;
                                    }
                                    else if (Math.Abs(grades[1] - grades[0]) < 0.06)
                                    {
                                        if (Math.Abs((int)heights[1] - (int)heights[0]) < 0.5)
                                        {
                                            if (wordsCount[0] > 4 && widths[0] / widths[1] > 0.6 && widths[0] > aveX)
                                            {
                                                infos = firstItem.WordMetas;
                                            }
                                            else if (wordsCount[1] > 4 && Math.Abs((int)heights[1] - (int)heights[0]) < 0.5 && widths[1] / widths[0] > 0.6 && widths[1] > aveX)
                                            {
                                                infos = lastItem.WordMetas;
                                            }

                                            else if (Math.Abs((int)heights[1] - (int)heights[0]) < 0.5 && widths[0] / widths[1] > 0.6 && widths[0] > aveX)
                                            {
                                                infos = firstItem.WordMetas;
                                            }
                                            else if (Math.Abs((int)heights[1] - (int)heights[0]) < 0.5 && widths[1] / widths[0] > 0.6 && widths[1] > aveX)
                                            {
                                                infos = lastItem.WordMetas;
                                            }

                                            else if (Math.Abs((int)heights[1] - (int)heights[0]) < 0.5 && widths[1] > widths[0])
                                            {
                                                infos = lastItem.WordMetas;
                                            }
                                            else if (Math.Abs((int)heights[1] - (int)heights[0]) < 0.5 && widths[0] > widths[1])
                                            {
                                                infos = firstItem.WordMetas;
                                            }
                                        }
                                        if ((int)spaces[1] - (int)spaces[0] > 5 && widths[1] > widths[0])
                                        {
                                            infos = lastItem.WordMetas;
                                        }
                                        else if ((int)spaces[0] - (int)spaces[1] > 5 && widths[0] > widths[1])
                                        {
                                            infos = firstItem.WordMetas;
                                        }
                                    }
                                    else if (Math.Abs(grades[0] - grades[1]) < 0.02 && Math.Abs(indexs[0] - indexs[1]) > 20)
                                    {
                                        if (widths[0] > widths[1])
                                        {
                                            infos = firstItem.WordMetas;
                                        }
                                        else
                                        {
                                            infos = lastItem.WordMetas;
                                        }
                                    }
                                    else if (widths[0] / widths[1] > 1.5 && lastWordInfo.X - firstWordInfo.X > aveX - 80)
                                    {
                                        infos = firstItem.WordMetas;
                                    }
                                    else if (widths[1] / widths[0] > 1.5 && firstWordInfo.X - lastWordInfo.X > aveX - 80)
                                    {
                                        infos = lastItem.WordMetas;
                                    }

                                    else if ((int)spaces[0] - (int)spaces[1] >= 5 && spaces[0] > 0 && spaces[1] > 0)
                                    {
                                        infos = firstItem.WordMetas;
                                    }

                                    else if (grades[0] >= grades[1] && Math.Abs((int)spaces[1] - (int)spaces[0]) < 5)
                                    {
                                        infos = firstItem.WordMetas;
                                    }

                                    else if (spaces[0] - spaces[1] >= 0.8 && heights[0] - heights[1] > 0.5)
                                    {
                                        infos = firstItem.WordMetas;
                                    }
                                    else if (spaces[1] - spaces[0] >= 0.8 && heights[1] - heights[0] > 0.5)
                                    {
                                        infos = lastItem.WordMetas;
                                    }

                                    else if (widths[0] > widths[1] && widths[1] / widths[0] < 0.5)
                                    {
                                        infos = firstItem.WordMetas;
                                    }
                                    else if (widths[1] > widths[0] && widths[0] / widths[1] < 0.5)
                                    {
                                        infos = lastItem.WordMetas;
                                    }

                                    else if (grades[1] - grades[0] < 0.09 && widths[0] / widths[1] > 0.9)
                                    {
                                        infos = firstItem.WordMetas;
                                    }
                                    else if (grades[1] - grades[0] < 0.09 && widths[1] / widths[0] > 0.9)
                                    {
                                        infos = lastItem.WordMetas;
                                    }
                                    else if (grades[1] > grades[0] && widths[1] / widths[0] > 0.7)
                                    {
                                        infos = lastItem.WordMetas;
                                    }
                                    else if (grades[1] < grades[0] && widths[1] / widths[0] > 0.7)
                                    {
                                        infos = firstItem.WordMetas;
                                    }
                                    else
                                    {
                                        var orderFind = find.Where(d => d.Key != maxMarkKey).OrderBy(c => c.Value.WordMetas.First().WordChars.First().Y).ToList();
                                        infos = firstItem.WordMetas;
                                    }
                            }
                            else
                            {
                                //选择Y值比较小的那个

                                if (lastItem.WordMetaCount >= firstItem.WordMetaCount && lastItem.Width / firstItem.Width > 0.7)
                                {
                                    if (lastItem.Position == BlockPostion.Left && firstItem.Position == BlockPostion.Center)
                                    {
                                        if ((firstItem.CharAveHeight > lastItem.CharAveHeight || firstItem.CharAveSpace > lastItem.CharAveSpace) && firstItem.Width / lastItem.Width > 0.6)
                                        {
                                            infos = firstItem.WordMetas;
                                        }
                                    }
                                    else if (lastItem.Position == BlockPostion.Center && firstItem.Position == BlockPostion.Left)
                                    {
                                        if ((firstItem.CharAveHeight < lastItem.CharAveHeight || firstItem.CharAveSpace < lastItem.CharAveSpace) && lastItem.Width / firstItem.Width > 0.6)
                                        {

                                            infos = lastItem.RepresentativeChar.Y - firstItem.RepresentativeChar.Y < 200 ? lastItem.WordMetas : firstItem.WordMetas;
                                        }
                                    }
                                    else if (lastItem.RepresentativeChar.Y - firstItem.RepresentativeChar.Y > 450 && firstItem.Width / lastItem.Width > 0.5)
                                    {
                                        infos = firstItem.WordMetas;
                                    }
                                    else if (lastItem.Width > firstItem.Width && lastItem.CharCount <= firstItem.CharCount)
                                        infos = firstItem.WordMetas;
                                    else
                                        infos = firstItem.WordMetas;
                                }
                                else if (lastItem.WordMetaCount > firstItem.WordMetaCount && (lastItem.WordMetaCount < 6 || lastItem.Height < 26) && lastItem.Width / firstItem.Width > 1)
                                {
                                    infos = lastItem.WordMetas;
                                }
                                else if (lastItem.WordMetaCount == firstItem.WordMetaCount && (lastItem.WordMetaCount < 6 || lastItem.Height < 26) && firstItem.Width / lastItem.Width > 0.9)
                                {
                                    infos = firstItem.WordMetas;
                                }
                                else
                                {
                                    infos = firstItem.WordMetas;
                                }

                            }
                    }
                    else if (find.Count == 1)
                    {
                        infos = blockInfos[find.First().Key].WordMetas;
                    }
                }
            }

            infos = infos.Where(s => s.Text.ToString().Trim() != "").ToList();

            var f = infos.OrderBy(u => u.Index).ToList();

            if (f.Where(s => s.Text.Length > 5).Count() == f.Count)
            {
                f = infos.OrderBy(v => v.WordChars.First().Y).ToList();
            }

            result.Title = string.Join(" ", f.Select(s => s.Text)).Replace("Title: ", "");

            return result;
        }

        private static bool JudgeBySameLength(ref List<WordMeta> infos, BlockInfo firstItem, BlockInfo lastItem)
        {
            bool isFind = false;
            if (Math.Abs(lastItem.Width - firstItem.Width) < 7)
            {
                infos = lastItem.WordMetaCount > firstItem.WordMetaCount || lastItem.Height > firstItem.Height ? lastItem.WordMetas : firstItem.WordMetas;

                isFind = true;
            }
            else if (Math.Abs(lastItem.Width - firstItem.Width) < 30)
            {
                if (lastItem.WordMetaCount >= firstItem.WordMetaCount && (lastItem.WordMetaCount < 6 || lastItem.Height < 26) && lastItem.Height >= firstItem.Height)
                {
                    infos = lastItem.WordMetas;
                }
                else
                {
                    infos = firstItem.WordMetas;
                }
                isFind = true;
            }
            return isFind;
        }

        private static bool JudgeByComma(Dictionary<int, BlockInfo> blockInfos, ref List<WordMeta> infos, int firstKey, int lastKey, BlockInfo firstItem, BlockInfo lastItem)
        {
            bool isFind = false;


            if (isFind == false && blockInfos.ContainsKey(lastKey + 1) && blockInfos[lastKey + 1].Text.Contains(",")
    && blockInfos[lastKey + 1].RepresentativeChar.Y - blockInfos[lastKey].RepresentativeChar.Y < 100)
            {
                var dotSplit = blockInfos[lastKey + 1].Text.Split(',');

                List<int> dotLength = new List<int>();

                if (dotSplit.Length >= 2)
                {
                    foreach (var item in dotSplit)
                    {
                        dotLength.Add(item.Length);
                    }

                    int count = 0;

                    for (int i = 0; i < dotLength.Count; i++)
                    {
                        for (int j = i + 1; j < dotLength.Count; j++)
                        {
                            if (Math.Abs(dotLength[i] - dotLength[j]) < 20)
                                count++;

                        }
                    }
                    if (count > 0 && (lastItem.Width / firstItem.Width > 0.6 || lastItem.Height > firstItem.Height))
                    {
                        if (lastItem.RepresentativeChar.Y - firstItem.RepresentativeChar.Y < 100)
                            infos = lastItem.WordMetas;

                        else if (lastItem.Height < firstItem.Height)
                        {
                            infos = firstItem.WordMetas;
                        }
                    }
                    else if ((lastItem.WordMetaCount >= firstItem.WordMetaCount || lastItem.CharCount > firstItem.CharCount) && lastItem.Width > firstItem.Width || lastItem.Height > firstItem.Height)
                    {
                        infos = lastItem.WordMetas;
                    }
                    else
                    {
                        infos = firstItem.WordMetas;
                    }
                    isFind = true;
                }
            }

            if (isFind == false && blockInfos.ContainsKey(firstKey + 1) && blockInfos[firstKey + 1].Text.Contains(",")
                && blockInfos[firstKey + 1].RepresentativeChar.Y - blockInfos[firstKey].RepresentativeChar.Y < 100)
            {
                var dotSplit = blockInfos[firstKey + 1].Text.Split(',');

                List<int> dotLength = new List<int>();

                if (dotSplit.Length >= 2)
                {
                    foreach (var item in dotSplit)
                    {
                        dotLength.Add(item.Length);
                    }

                    int count = 0;

                    for (int i = 0; i < dotLength.Count; i++)
                    {
                        for (int j = i + 1; j < dotLength.Count; j++)
                        {
                            if (Math.Abs(dotLength[i] - dotLength[j]) < 9)
                                count++;

                        }
                    }
                    if (count > 0 && (firstItem.Width / lastItem.Width > 0.6 || firstItem.Height > lastItem.Height))
                    {
                        infos = firstItem.WordMetas;
                    }
                    else if ((firstItem.WordMetaCount >= lastItem.WordMetaCount || firstItem.CharCount > lastItem.CharCount) && firstItem.Width > lastItem.Width || firstItem.Height > lastItem.Height)
                    {
                        infos = firstItem.WordMetas;
                    }
                    else if (firstKey + 1 == lastKey)
                    {
                        if (lastItem.WordMetaCount > firstItem.WordMetaCount)
                            infos = lastItem.WordMetas;
                        else
                            infos = firstItem.WordMetas;
                    }
                    else
                    {
                        infos = lastItem.WordMetas;
                    }
                    isFind = true;
                }
            }
            return isFind;
        }

        private static bool JudgeBy3w(Dictionary<int, BlockInfo> blockInfos, Dictionary<int, List<WordMeta>> orginDic, ref List<WordMeta> infos, int firstKey, int lastKey)
        {
            bool isFind = false;
            for (int j = 1; j <= 2; j++)
            {
                if (orginDic.Keys.Contains(lastKey + j))
                {
                    var findWWW = orginDic[lastKey + j].Find(w => w.Text.ToString().Contains("w") && Regex.IsMatch(w.Text.ToString(), @"[w\s]{3,}[.]"));

                    if (findWWW != null)
                    {
                        infos = orginDic[firstKey];
                        isFind = true;
                        break;
                    }
                }

                if (orginDic.Keys.Contains(firstKey + j))
                {
                    var findWWW = orginDic[firstKey + j].Find(w => w.Text.ToString().Contains("w") && Regex.IsMatch(w.Text.ToString(), @"[w\s]{3,}[.]"));

                    if (findWWW != null)
                    {
                        infos = orginDic[lastKey];
                        isFind = true;
                        break;
                    }
                }
            }
            if (isFind == false)
            {
                if (blockInfos[firstKey].WordMetas.First().Text.ToString().ToLower().StartsWith("title:"))
                {
                    infos = blockInfos[firstKey].WordMetas;
                    isFind = true;
                }
                if (blockInfos[lastKey].WordMetas.First().Text.ToString().ToLower().StartsWith("title:"))
                {
                    infos = blockInfos[lastKey].WordMetas;
                    isFind = true;
                }
            }
            return isFind;
        }

        private static bool JudgeByPosition(ref List<WordMeta> infos, BlockInfo firstItem, BlockInfo lastItem)
        {
            bool isFind = false;

            if (firstItem.Position == BlockPostion.Right && lastItem.Position == BlockPostion.Center)
            {
                infos = lastItem.WordMetas;
                isFind = true;
            }
            else if (firstItem.Position == BlockPostion.Center && lastItem.Position == BlockPostion.Right)
            {
                infos = firstItem.WordMetas;
                isFind = true;
            }
            else if (firstItem.Position == BlockPostion.Left && lastItem.Position == BlockPostion.Right)
            {
                infos = firstItem.Width / lastItem.Width > 0.9 ? firstItem.WordMetas : lastItem.WordMetas;
                isFind = true;
            }
            else if (firstItem.Position == BlockPostion.Right && lastItem.Position == BlockPostion.Left)
            {
                infos = lastItem.Width / firstItem.Width > 0.9 ? lastItem.WordMetas : firstItem.WordMetas;
                isFind = true;
            }

            return isFind;
        }


        #region 私有方法

        private static bool IsValidCount(List<WordMeta> item, out bool isChinese)
        {
            int chineseCount = 3;
            int englishCount = 14;

            isChinese = false;

            bool isValid = true;

            foreach (var sItem in item)
            {
                if (StringTools.IsChineseLetter(sItem.Text.ToString()))
                {
                    isChinese = true;
                    break;
                }
            }
            if (isChinese)
            {
                if (item.Sum(v => v.Text.ToString().Trim().Length) < chineseCount || item.Count >= 16)
                {
                    isValid = false;
                }
            }
            else
            {
                if (item.Count == 1)
                {
                    var iText = item.First().Text.ToString().Trim();

                    var textArray = iText.Split(' ');

                    if (iText.Length <= 2 * englishCount && textArray.Length < 3) isValid = false;

                    if (!iText.Contains(",") && iText.Length < englishCount && textArray.Length < 3) isValid = false;

                    textArray = iText.Split('&');

                    if (!iText.Contains(",") && iText.Length < englishCount && textArray.Length < 3) isValid = false;
                }

                if (item.Sum(v => v.Text.ToString().Trim().Length) < englishCount || item.Count >= 16)
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        private static bool IsContainsSomeWords(List<WordMeta> item)
        {
            bool isValid = true;

            foreach (var cw in filterWords)
            {
                var findSpecial = item.Find(v => v.Text.ToString().ToLower().StartsWith(cw.ToLower()));

                if (findSpecial != null)
                {
                    isValid = false;
                    break;
                }

            }
            return isValid;
        }

        private static bool IsContainUrl(List<WordMeta> item)
        {
            bool isValid = true;


            foreach (var cw in containsWords)
            {
                var find3w = item.Find(v => v.Text.ToString().Contains(cw));

                if (find3w != null)
                {
                    isValid = false;
                    break;
                }

            }
            return isValid;
        }

        private static double GetMark(BlockInfo block, double maxHeight, double maxWidth, double maxYSize, double maxXSize, double maxSpace)
        {
            double result = 0;

            if (maxYSize > 0)
                result += 0.4 * ((double)block.CharAveYSize / maxYSize);

            if (maxXSize > 0)
                result += 0.3 * ((double)block.CharAveXSize / maxXSize);

            if (maxSpace > 0)

                result += (block.CharAveSpace / maxSpace) * 0.1;

            if (maxHeight > 0)

                result += (block.CharAveHeight / maxHeight) * 0.1;

            if (maxWidth > 0)
                result += (block.CharAveWidth / maxWidth) * 0.1;
            if (block.RepresentativeChar.IsBold) result += 0.1;
            return result;
        }

        #endregion
    }
}
