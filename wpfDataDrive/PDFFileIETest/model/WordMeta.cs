using javax.swing.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pdfResearch
{
    public class WordMeta : IEquatable<WordMeta>
    {
        public int Index { set; get; }

        public StringBuilder Text { set; get; }

        public List<WordInfo> WordChars { set; get; }

        public float SpaceHeight { set; get; }

        public float Width { set; get; }

        public float MaxY { set; get; }
        public float MaxX { set; get; }

        public float OffSetHeight { set; get; }
        public float OffSetWidth { set; get; }

        public bool IsFirst { set; get; }

        public bool IsEndBlock { set; get; }

        public bool IsSubTitleStart { set; get; }

        public WordMeta LastWord { set; get; }

        public bool IsSmallWord { set; get; }




        public bool Equals(WordMeta compare)
        {
            if (compare == null || compare.WordChars.Count == 0 || this.WordChars.Count == 0) return false;
            var cur = WordChars.First();
            var other = compare.WordChars.First();


            var curLast = WordChars.Last();
            var otherLast = compare.WordChars.Last();

            if (curLast.YSize > cur.YSize && curLast.YSize / (double)cur.YSize < 5) cur = curLast;
            if (otherLast.YSize > other.YSize && otherLast.YSize / (double)other.YSize < 5) other = otherLast;

            if (cur.Word.Trim() == "" || cur.Word == "《" && WordChars.Count > 1) cur = WordChars[1];
            if (other.Word.Trim() == "" || other.Word == "《" && compare.WordChars.Count > 1) other = compare.WordChars[1];

            var aveHeight = (cur.Height + other.Height) / 2;

            var isSmallWord = IsUpOrDown(cur, other, compare, otherLast);

            if (isSmallWord)
            {
                this.IsSmallWord = isSmallWord;
                return true;
            }

            var curAveHeight = WordChars.Average(w => w.Height);

            if (compare.IsSmallWord && compare.LastWord != null)
            {
                if (compare.LastWord.WordChars.Average(w => w.Height) - curAveHeight > 5)
                {
                    return false;
                }
            }

            var first = WordChars.Average(w => w.Y);
            var second = compare.WordChars.Average(w => w.Y);

            //破折号，副标题

            int subFlag = 0;

            for (int i = 0; i < WordChars.Count; i++)
            {
                var w = WordChars[i].Word.Trim();

                if (w == "")
                {
                    continue;
                }
                if (w == "—")
                    subFlag++;
                else
                    break;
            }

            if (subFlag > 0)
            {
                IsSubTitleStart = true;
                return true;
            }

            if (otherLast.Word == "：" && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) <= aveHeight)
            {
                IsSubTitleStart = true;
                return true;
            }

            if (compare.SpaceHeight > 5 && (int)((Math.Abs(first - second) - curAveHeight) - (int)compare.SpaceHeight) >= 4)
            {
                IsEndBlock = true;
                return false;
            }
            if (compare.SpaceHeight > 3 && compare.SpaceHeight < 5 && (int)((Math.Abs(cur.Y - other.Y) - cur.Height) - (int)compare.SpaceHeight) > 8)
            {
                IsEndBlock = true;
                return false;
            }
            if (compare.SpaceHeight > 1 && compare.SpaceHeight < 1.2 && (int)((Math.Abs(cur.Y - other.Y) - cur.Height) - (int)compare.SpaceHeight) > 14)
            {
                IsEndBlock = true;
                return false;
            }

            if (compare.WordChars.Count == 1 && other.Word == "—")
            {
                IsEndBlock = true;
                return true;
            }

            if (compare.IsFirst && other.Y < cur.Y && other.X - cur.X > 100 && this.Width > 0 && compare.Width > 0 && Math.Round(compare.Width / this.Width, 2) < 0.4 && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) > 0 && Math.Abs(cur.Height - other.Height) > 1 && Math.Abs(cur.Space - other.Space) > 1) return false;
            if (compare.IsFirst && other.Y < cur.Y && cur.Y - other.Y > 15 && this.Width > 200 && Math.Round(compare.Width / this.Width, 2) < 0.4 && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) > 3 && Math.Abs(cur.Height - other.Height) > 1 && Math.Abs(cur.Space - other.Space) > 1) return false;

            var stricMode = IsPassStrictModeGroup(cur, other, compare, aveHeight);

            if (stricMode) return true;

            var isCloseMode = IsCloseMode(cur, other, compare, aveHeight);

            if (isCloseMode) return true;

            if (Math.Abs(cur.Height - other.Height) > 1.5
               && Math.Abs((int)cur.Space - (int)other.Space) >= 1
               && Math.Abs(cur.YSize - other.YSize) > 3
               && Math.Abs(cur.XSize - other.XSize) > 3)
                return false;

            if (Math.Abs(cur.Height - other.Height) > 2
            && Math.Abs((int)cur.Space - (int)other.Space) >= 2
            && Math.Abs(cur.YSize - other.YSize) > 3
            && Math.Abs(cur.XSize - other.XSize) >= 3)
                return false;

            if (Math.Abs(cur.Height - other.Height) > 1
             && Math.Abs(cur.FontSize - other.FontSize) > 1
             && Math.Abs((int)cur.Space - (int)other.Space) >= 1
             && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) > 5
             && Math.Abs(cur.YSize - other.YSize) > 30
             && Math.Abs(cur.XSize - other.XSize) > 30)
                return false;

            if (Math.Abs(cur.Space - other.Space) > 0.8 && Math.Abs(cur.YSize - other.YSize) > 3 && Math.Abs(cur.XSize - other.XSize) > 3
                && Math.Abs(cur.Y - other.Y) > 20 && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) > aveHeight
                )
                return false;

            if (IsSameFont(compare.WordChars, WordChars)
               && cur.FontSize == other.FontSize
               && cur.YSize == other.YSize
               && Math.Abs(cur.XSize - other.XSize) < 5
               && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) <= aveHeight + 2)
            {
                return true;
            }

            if (IsSameFont(compare.WordChars, WordChars)
             && cur.FontSize == other.FontSize
             && cur.YSize == other.YSize
             && cur.XSize == other.XSize
             && cur.Height == other.Height
             && cur.Space == other.Space
             && cur.IsBold == other.IsBold
             && cur.IsItalic == other.IsItalic
             && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) <= aveHeight + 12)
            {
                return true;
            }

            if (IsSameFont(compare.WordChars, WordChars)
               && Math.Abs(cur.FontSize - other.FontSize) < 2
               && Math.Abs(cur.YSize - other.YSize) <= (int)cur.YSize / 4
               && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) < 2)
            {
                return true;
            }

            return IsSameFont(compare.WordChars, WordChars)
                && Math.Abs(cur.FontSize - other.FontSize) <= 3
                && Math.Abs(cur.YSize - other.YSize) <= (int)cur.YSize / 4 + 3
                && Math.Abs(cur.XSize - other.XSize) <= (int)cur.XSize / 4 + 3
                && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) <= aveHeight
                && Math.Abs(cur.Space - other.Space) < 5
          ;
        }


        public bool IsPassStrictModeGroup(WordInfo cur, WordInfo other, WordMeta compare, double aveHeight)
        {
            bool strictMode = (int)cur.Height == (int)other.Height && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) <= aveHeight && cur.YSize == other.YSize;
            if (strictMode) return true;

            strictMode = (int)(Math.Abs(cur.Height - other.Height)) <= 1 && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) <= aveHeight && cur.YSize == other.YSize && cur.Space == other.Space;
            if (strictMode) return true;


            strictMode = IsSameFont(compare.WordChars, WordChars)
            && cur.FontSize == other.FontSize
            && cur.YSize == other.YSize
            && cur.XSize == other.XSize
            && cur.Space == other.Space
            && cur.IsBold == other.IsBold
            && cur.IsItalic == other.IsItalic
            && cur.Height == other.Height
            && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) <= Math.Round(aveHeight + 5, 0);
            if (strictMode) return true;

            return false;
        }


        public bool IsUpOrDown(WordInfo cur, WordInfo other, WordMeta compare, WordInfo otherLast)
        {
            if (cur.Y < other.Y && (int)(other.Y - cur.Y) <= 20 && WordChars.Count == 1 && WordChars[0].Word.Length == 1) return true;
            if (other.Y < cur.Y && (int)(cur.Y - other.Y) <= 20 && compare.WordChars.Count == 1 && compare.WordChars[0].Word.Length == 1) return true;
            if (other.Y < cur.Y && (int)(cur.Y - other.Y) <= 20 && WordChars.Count == 1 && WordChars[0].Word.Length == 1 && Math.Abs(cur.X - otherLast.X) < 8) return true;
            if (other.Y - cur.Y < 9 && cur.Y < other.Y && other.Y - cur.Y < other.Height) return true;
            if (cur.Y - other.Y < 9 && cur.Y > other.Y && cur.Y - other.Y < cur.Height) return true;

            if (other.Y - cur.Y < 9 && cur.Y < other.Y && Math.Abs((int)cur.Height - (int)other.Height) < 8 && Math.Abs(cur.X - otherLast.X) < 8) return true;
            if (cur.Y - other.Y < 9 && cur.Y > other.Y && Math.Abs((int)cur.Height - (int)other.Height) < 8 && Math.Abs(cur.X - otherLast.X) < 8) return true;


            if (Math.Abs(cur.Y - other.Y) < 5) return true;

            return false;
        }

        public bool IsCloseMode(WordInfo cur, WordInfo other, WordMeta compare, double aveHeight)
        {
            bool closeMode = IsSameFont(compare.WordChars, WordChars) && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) < 5
             && (Math.Abs(cur.Height - other.Height)) < 3
             && (Math.Abs((int)cur.Space - (int)other.Space)) < 5
             && Math.Abs(cur.YSize - other.YSize) < 7
             ;

            if (closeMode == false)
            {
                if (IsSameFont(compare.WordChars, WordChars) && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) < 0
                && (Math.Abs(cur.Height - other.Height)) < 3
                && (Math.Abs((int)cur.Space - (int)other.Space)) < 5
                && cur.YSize > 1000 && other.YSize > 1000
                && Math.Abs(cur.YSize - other.YSize) < 300)
                {
                    return true;
                }
            };


            if (closeMode)
            {
                if (cur.X < other.X && cur.Y > other.Y && cur.Height > other.Height && cur.Space > other.Space && cur.YSize > other.YSize) return false;
                return true;
            }

            closeMode = IsSameFont(compare.WordChars, WordChars) && (int)(Math.Abs(cur.Y - other.Y) - cur.Height - aveHeight) < 0
            && (Math.Abs(cur.Height - other.Height)) < 2
            && (int)cur.Space == (int)other.Space;

            if (closeMode) return true;
            return false;
        }

        public bool IsValid(WordMeta compare)
        {
            var cur = WordChars.First();
            if (cur.Word.Trim() == "" && WordChars.Count > 1) cur = WordChars[1];

            var other = compare.WordChars.First();

            if (other.Word.Trim() == "" && compare.WordChars.Count > 1) other = compare.WordChars[1];

            if (cur.Height < other.Height / 4) return false;

            if (cur.Height < other.Height / 3.5 && other.Y - cur.Y > 80 && other.Space - cur.Space > 18) return false;

            if (Math.Abs(cur.Height - other.Height) < 1.5 && Math.Abs(cur.Space - other.Space) < 4
                && other.Y - cur.Y >= 14
                && WordChars.Count < 7
                && cur.X - compare.WordChars.Last().X > 8) return false;


            var special = 0;
            var w = WordChars.Where(r => r.Word == "!" || r.Word == "/" || r.Word == "$").ToList();
            if (w != null && w.Count > 0)
            {
                special += w.Count();
            }
            if (special > 3) return false;

            w = WordChars.Where(r => r.Word == "," || r.Word == "*" || r.Word == "#" || r.Word == "." || r.Word == "+").ToList();

            if (w != null && w.Count > 0)
            {
                if (w.Count >= 3 && this.Text.ToString().Contains(" ") && this.Text.ToString().Length < 60)
                {
                    return false;
                }
                if (w.Count >= 4)
                {
                    return false;
                }

            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.Text.GetHashCode();
        }

        private bool IsSameFont(List<WordInfo> compareInfo, List<WordInfo> info)
        {
            bool isResult = false;

            if (info.Count == 0 || compareInfo.Count == 0) return isResult;

            int sameCount = 0;
            int difCount = 0;


            foreach (var item in info)
            {
                foreach (var citem in compareInfo)
                {
                    if (citem.Basefont == item.Basefont)
                    {
                        sameCount++;
                    }
                    else
                    {
                        difCount++;
                    }
                }
            }
            if (difCount == 0) return true;

            var sameRate = 1.0 * sameCount / difCount;
            if (sameRate > 0.5) isResult = true;

            if (isResult == false)
            {
                if (info.Last().Basefont == compareInfo.Last().Basefont && info.First().Basefont == compareInfo.First().Basefont)
                {
                    isResult = true;
                }

                if (isResult == false && sameRate > 0.2)
                {
                    if (info.Last().Basefont == compareInfo.Last().Basefont)
                    {
                        isResult = true;
                    }
                }

            }
            return isResult;
        }

    }
}
