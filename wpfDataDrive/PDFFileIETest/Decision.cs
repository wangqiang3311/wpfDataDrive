using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pdfResearch
{
    public class DecisionTreeID3<T> where T : IEquatable<T>
    {
        T[,] Data;
        string[] Names;
        int Category;
        T[] CategoryLabels;
        public DecisionTreeNode<T> Root;
        public DecisionTreeID3(T[,] data, string[] names, T[] categoryLabels)
        {
            Data = data;
            Names = names;
            Category = data.GetLength(1) - 1;//类别变量需要放在最后一列
            CategoryLabels = categoryLabels;
        }
        public void Learn()
        {
            int nRows = Data.GetLength(0);
            int nCols = Data.GetLength(1);
            int[] rows = new int[nRows];
            int[] cols = new int[nCols];
            for (int i = 0; i < nRows; i++) rows[i] = i;
            for (int i = 0; i < nCols; i++) cols[i] = i;
            Root = new DecisionTreeNode<T>(-1, default(T));
            Learn(rows, cols, Root);

            DisplayNode(Root);
        }

        public bool Search(string[] test, DecisionTreeNode<T> Node = null)
        {
            bool isResult = false;

            if (Node == null) Node = Root;

            foreach (var item in Node.Children)
            {
                var label = item.Label;
                if (label < test.Length - 1 && test[label] != item.Value.ToString()) continue;
                else
                {
                    if (label == test.Length - 1 && item.Value.ToString() == "True")
                    {
                        isResult = true;
                        return isResult;
                    }
                    else
                    {
                        isResult = Search(test, item);
                    }
                }
            }
            return isResult;
        }

        public StringBuilder sb = new StringBuilder();

        public void DisplayNode(DecisionTreeNode<T> Node, int depth = 0)
        {
            if (Node.Label != -1)
            {
                string nodeStr = string.Format("{0} {1}: {2}", new string('-', depth * 3), Names[Node.Label], Node.Value);
                sb.AppendLine(nodeStr);
            }
            foreach (var item in Node.Children)
                DisplayNode(item, depth + 1);
        }
        private void Learn(int[] pnRows, int[] pnCols, DecisionTreeNode<T> Root, int depth = 0)
        {
            var categoryValues = GetAttribute(Data, Category, pnRows);
            var categoryCount = categoryValues.Distinct().Count();
            if (categoryCount == 1)
            {
                var node = new DecisionTreeNode<T>(Category, categoryValues.First());
                Root.Children.Add(node);
            }
            else
            {
                if (depth > 10) return;

                if (pnRows.Length == 0) return;
                else if (pnCols.Length == 1)
                {
                    //投票～
                    //多数票表决制
                    var Vote = categoryValues.GroupBy(i => i).OrderBy(i => i.Count()).First();
                    var node = new DecisionTreeNode<T>(Category, Vote.First());
                    Root.Children.Add(node);
                }
                else
                {
                    //var maxCol = MaxEntropy(pnRows, pnCols);

                    //按c4.5算法
                    var maxCol = MaxEntropyRate(pnRows, pnCols);

                    var attributes = GetAttribute(Data, maxCol, pnRows).Distinct();
                    string currentPrefix = Names[maxCol];
                    foreach (var attr in attributes)
                    {
                        int[] rows = pnRows.Where(irow => Data[irow, maxCol].Equals(attr)).ToArray();
                        int[] cols = pnCols.Where(i => i != maxCol).ToArray();
                        var node = new DecisionTreeNode<T>(maxCol, attr);
                        Root.Children.Add(node);
                        Learn(rows, cols, node, depth + 1);//递归生成决策树
                    }
                }
            }
        }
        public double AttributeInfo(int attrCol, int[] pnRows)
        {
            var tuples = AttributeCount(attrCol, pnRows);
            var sum = (double)pnRows.Length;
            double Entropy = 0.0;
            foreach (var tuple in tuples)
            {
                int[] count = new int[CategoryLabels.Length];
                foreach (var irow in pnRows)
                    if (Data[irow, attrCol].Equals(tuple.Item1))
                    {
                        int index = Array.IndexOf(CategoryLabels, Data[irow, Category]);
                        count[index]++;//目前仅支持类别变量在最后一列
                    }
                double k = 0.0;
                for (int i = 0; i < count.Length; i++)
                {
                    double frequency = count[i] / (double)tuple.Item2;
                    double t = -frequency * Log2(frequency);
                    k += t;
                }
                double freq = tuple.Item2 / sum;
                Entropy += freq * k;
            }
            return Entropy;
        }

        public double AttributeInfoRate(int attrCol, int[] pnRows)
        {
            var tuples = AttributeCount(attrCol, pnRows);
            var sum = (double)pnRows.Length;
            double SplitE = 0.0;

            foreach (var tuple in tuples)
            {
                double frequency = tuple.Item2 / (double)sum;
                double t = -frequency * Log2(frequency);
                SplitE += t;
            }
            return SplitE;
        }

        public double CategoryInfo(int[] pnRows)
        {
            var tuples = AttributeCount(Category, pnRows);
            var sum = (double)pnRows.Length;
            double Entropy = 0.0;
            foreach (var tuple in tuples)
            {
                double frequency = tuple.Item2 / sum;
                double t = -frequency * Log2(frequency);
                Entropy += t;
            }
            return Entropy;
        }
        private static IEnumerable<T> GetAttribute(T[,] data, int col, int[] pnRows)
        {
            foreach (var irow in pnRows)
                yield return data[irow, col];
        }
        private static double Log2(double x)
        {
            return x == 0.0 ? 0.0 : Math.Log(x, 2.0);
        }
        /// <summary>
        /// 计算增益率
        /// </summary>
        /// <param name="pnRows"></param>
        /// <param name="pnCols"></param>
        /// <returns></returns>
        public int MaxEntropy(int[] pnRows, int[] pnCols)
        {
            double cateEntropy = CategoryInfo(pnRows);
            int maxAttr = 0;
            double max = double.MinValue;
            foreach (var icol in pnCols)
                if (icol != Category)
                {
                    double Gain = cateEntropy - AttributeInfo(icol, pnRows);
                    if (max < Gain)
                    {
                        max = Gain;
                        maxAttr = icol;
                    }
                }
            return maxAttr;
        }
        /// <summary>
        /// 计算增益率最大的属性
        /// </summary>
        /// <param name="pnRows"></param>
        /// <param name="pnCols"></param>
        /// <returns></returns>
        public int MaxEntropyRate(int[] pnRows, int[] pnCols)
        {
            double cateEntropy = CategoryInfo(pnRows);
            int maxAttr = 0;
            double max = double.MinValue;
            foreach (var icol in pnCols)
                if (icol != Category)
                {
                    double Gain = cateEntropy - AttributeInfo(icol, pnRows);

                    double SplitE = AttributeInfoRate(icol, pnRows);

                    double GrainRation = Gain / SplitE;

                    if (max < GrainRation)
                    {
                        max = GrainRation;
                        maxAttr = icol;
                    }
                }
            return maxAttr;
        }

        public IEnumerable<Tuple<T, int>> AttributeCount(int col, int[] pnRows)
        {
            var tuples = from n in GetAttribute(Data, col, pnRows)
                         group n by n into i
                         select Tuple.Create(i.First(), i.Count());
            return tuples;
        }
    }

    public sealed class DecisionTreeNode<T>
    {
        public int Label { get; set; }
        public T Value { get; set; }
        public List<DecisionTreeNode<T>> Children { get; set; }
        public DecisionTreeNode(int label, T value)
        {
            Label = label;
            Value = value;
            Children = new List<DecisionTreeNode<T>>();
        }
    }
}
