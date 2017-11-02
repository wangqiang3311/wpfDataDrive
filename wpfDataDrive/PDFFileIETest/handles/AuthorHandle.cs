using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace pdfResearch
{
    public class AuthorHandle
    {
        public string Title { set; get; }
        public string Author { set; get; }

        /// <summary>
        /// 获取作者，如果标题为空，顺便补充标题
        /// </summary>
        /// <param name="markedText">标识后的文本</param>
        /// <param name="oriText">原始文本</param>
        /// <param name="title"></param>
        public void GetAuthor(string markedText, string oriText, string title)
        {
            var sentences = markedText.Split('\n');

            Dictionary<int, string> nameDics = new Dictionary<int, string>();

            int i = 0;

            int titleLineNo = 0;

            List<string> handledSentences = new List<string>();

            string pattern = "^[a-z,\\s]*\r$";

            string patternNumSpace = "^[\\d,\\s]*\r$";

            foreach (var item in sentences)
            {
                if (item.Trim() == "" || item.Trim().Length < 2) continue;

                if (Regex.IsMatch(item, pattern, RegexOptions.Singleline)) continue;
                if (Regex.IsMatch(item, patternNumSpace, RegexOptions.Singleline)) continue;

                handledSentences.Add(item);
            }

            sentences = handledSentences.ToArray();

            foreach (var item in sentences)
            {
                i++;
                if (item.Contains("<person>") || item.Contains("</person>"))
                {
                    nameDics.Add(i, item.Replace("<person>", "").Replace("</person>", ""));
                }

                if (title != "" && title.Contains(item.TrimEnd('\r')))
                {
                    if (titleLineNo == 0)
                    {
                        titleLineNo = i;
                    }
                }

            }

            string author = "";

            titleLineNo = ReGetTitle(title, sentences, nameDics, titleLineNo);

            int minD = 100;
            int minKey = 0;

            foreach (var item in nameDics)
            {
                if (Regex.IsMatch(item.Value, @"\d{2,}")) continue;
                if (item.Key - titleLineNo > 0)
                {

                    if (item.Key - titleLineNo < minD)
                    {
                        minD = item.Key - titleLineNo;
                        minKey = item.Key;
                    }
                }
            }


            //获取离标题最近的人名

            if (nameDics.Keys.Contains(minKey) && minD < 20)
            {
                i = 0;
                int abstractLine = 0;
                //如果有Abstract关键词，则在Abstract之前的行
                foreach (var item in sentences)
                {
                    i++;
                    if (item.ToLower().StartsWith("abstract"))
                    {
                        abstractLine = i;
                        break;
                    }
                }

                if (minKey < abstractLine || abstractLine == 0)
                {
                    author = nameDics[minKey].TrimEnd('\r');
                }

                minKey = minKey + 1;

                while (nameDics.Keys.Contains(minKey))
                {
                    if (minKey == abstractLine) break;

                    if (!author.Contains(" and "))
                    {
                        var nextAuthor = nameDics[minKey].ToLower();
                        if (!nextAuthor.Contains("institute") && !nextAuthor.Contains("department") && !nextAuthor.Contains("college") && !nextAuthor.Contains("university"))
                            author += " " + nameDics[minKey];
                    }

                    minKey++;
                }
            }

            if (author.EndsWith(","))
            {
                if (sentences.Length > minKey)
                {
                    var input = sentences[minKey - 1].TrimEnd('\r').TrimEnd();
                    if (input.Length < 3 && Regex.IsMatch(input, @"^[A-Z]\."))
                    {
                        author += input + sentences[minKey];
                    }
                    else
                    {
                        author += sentences[minKey - 1];
                    }
                }
            }

            author = Regex.Replace(author, @"\(\d+\)", "");

            sentences = oriText.Split('\n');

            if (author == "")
            {
                //如果作者为空，从原始文本中获取作者下一行信息
                int j = 0;

                //过滤下
                handledSentences.Clear();
                foreach (var item in sentences)
                {
                    if (item.Trim() == "") continue;
                    if (Regex.IsMatch(item, pattern, RegexOptions.Singleline)) continue;
                    if (Regex.IsMatch(item, patternNumSpace, RegexOptions.Singleline)) continue;
                    handledSentences.Add(item);
                }

                sentences = handledSentences.ToArray();

                foreach (var item in sentences)
                {
                    if (title.Contains(item.TrimEnd('\r')))
                    {
                        break;
                    }
                    j++;
                }
                if (sentences.Count() > j + 1)
                    author = sentences[j + 1];


                //如果获取的作者是日期格式，那么就为空

                foreach (var m in TextHandleTool.months)
                {
                    if (author.Contains(m))
                    {
                        string pattren = m + @"\s*\d+,\s*\d{4}";

                        var maches = Regex.Matches(author, pattren, RegexOptions.IgnoreCase);

                        for (int k = 0; k < maches.Count; k++)
                        {
                            if (maches[k].Success)
                            {
                                author = "";
                            }
                        }
                    }
                }
            }
            else
            {
                //从原始的文本中获取作者行

                int c = 0;
                int authorLine = 0;
                foreach (var item in sentences)
                {
                    c++;
                    if (Regex.IsMatch(item, pattern, RegexOptions.Singleline)) continue;
                    if (Regex.IsMatch(item, @"\d{2,}")) continue;


                    if (item.Contains(author.TrimEnd('\r')))
                    {
                        author = item;
                        authorLine = c;
                        break;
                    }
                }

                string nextAuthorLine = "";

                author = author.TrimEnd('\r');
                if (author.ToLower().EndsWith("and"))
                {
                    if (sentences.Count() > authorLine)
                    {
                        nextAuthorLine = sentences[authorLine];

                        if (nextAuthorLine.Length < 3)
                        {
                            nextAuthorLine = sentences[authorLine + 1];
                        }

                    }
                }
                author += " " + nextAuthorLine;
            }
            //替换特殊字符
            author = author.Replace("Æ", ";");
            this.Author = author;
        }
        /// <summary>
        /// 重新获取标题
        /// </summary>
        /// <param name="title"></param>
        /// <param name="sentences"></param>
        /// <param name="nameDics"></param>
        /// <param name="titleLineNo"></param>
        /// <returns></returns>
        private int ReGetTitle(string title, string[] sentences, Dictionary<int, string> nameDics, int titleLineNo)
        {
            //说明标题获取不正确
            if (titleLineNo > 10 || (titleLineNo == 0 && title == "") || (titleLineNo == 0 && title.Length > 200))
            {
                //纠正标题
                //取第一个人名
                if (nameDics.Count > 0)
                {
                    var pk = nameDics.First().Key;

                    Dictionary<int, string> possibleTitles = new Dictionary<int, string>();

                    int k = 0;

                    foreach (var item in sentences)
                    {
                        k++;

                        if (item.StartsWith("www.") || item.Length < 10) continue;


                        if (k < pk)
                        {
                            possibleTitles.Add(k, item);
                        }
                        else
                        {
                            break;
                        }
                    }

                    //筛选出标题行,离作者最近

                    List<Stack<int>> stacks = new List<Stack<int>>();

                    var s = 0;
                    foreach (var item in possibleTitles)
                    {
                        if (s == 0 || (stacks[stacks.Count - 1].Count > 0 && item.Key - stacks[stacks.Count - 1].Peek() != 1))
                        {
                            s++;
                            stacks.Add(new Stack<int>());
                        }

                        stacks[stacks.Count - 1].Push(item.Key);
                    }

                    int max = 0;
                    int min = 100;
                    Stack<int> maxStack = null;
                    Stack<int> minDistanceOfAuthor = null;


                    foreach (var item in stacks)
                    {
                        if (item.Count > max)
                        {
                            max = item.Count;
                            maxStack = item;
                        }

                        if (pk - item.Peek() < min)
                        {
                            min = pk - item.Peek();
                            minDistanceOfAuthor = item;
                        }
                    }

                    var maxCount = stacks.Where(t => t.Count == max);

                    if (maxCount != null)
                    {
                        if (maxCount.Count() > 1)
                        {
                            //说明按最大字数不行，那么就按照最近作者的原则获取标题

                            if (minDistanceOfAuthor != null)
                            {
                                var b = minDistanceOfAuthor.OrderBy(m => m);

                                string t = "";
                                foreach (var item in b)
                                {
                                    t += sentences[item - 1].TrimEnd('\r') + " ";
                                }

                                titleLineNo = b.Max();

                                Title = t;

                            }
                        }
                        else
                        {
                            if (maxStack != null)
                            {
                                var b = maxStack.OrderBy(m => m);

                                string t = "";
                                foreach (var item in b)
                                {
                                    t += sentences[item - 1].TrimEnd('\r') + " ";
                                }

                                titleLineNo = b.Max();

                                this.Title = t;
                            }
                        }
                    }
                }
            }
            return titleLineNo;
        }
    }
}
