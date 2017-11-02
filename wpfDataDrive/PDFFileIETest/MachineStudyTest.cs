using PDFFileIETest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pdfResearch
{
    public class MachineStudyTest
    {
        public string test()
        {
            var tree = BuildTree();
            return tree.sb.ToString();

            //var test = new string[] { "True", "False", "True", "False", "False", "" };
            ////var test = new string[] { "True", "False", "True", "False", "False",""};

            //bool isTitle = tree.Search(test);
        }

        private static DecisionTreeID3<string> BuildTree()
        {
            //var blockList = Tools.SelectList("/config/Blocks/Block");

            var blockList = DBHelper.Select<BlockData>();

            string[,] da = new string[blockList.Count, 6];

            for (int i = 0; i < blockList.Count; i++)
            {
                var index = blockList[i].Index;

                if (index >= 1 && index <= 5)
                {
                    da[i, 0] = "high";
                }
                else if (index >= 6 && index <= 12)
                {
                    da[i, 0] = "middle";
                }
                else
                {
                    da[i, 0] = "low";
                }
                var space = blockList[i].Space.ToString() == "非数字" ? 0 : (int)blockList[i].Space;

                if (space >= 3 && space <= 10 || space >= 17 && space <= 20)
                {
                    da[i, 1] = "high";
                }
                else if (space >= 11 && space <= 16)
                {
                    da[i, 1] = "middle";
                }
                else
                {
                    da[i, 1] = "low";
                }

                var xSize = blockList[i].XSize;

                if (xSize >= 11 && xSize <= 19 || xSize >= 400 && xSize <= 440 || xSize >= 250 && xSize <= 260)
                {
                    da[i, 2] = "high";
                }
                else
                {
                    da[i, 2] = "low";
                }

                var ySize = blockList[i].YSize;

                if (ySize >= 11 && ySize <= 19 || ySize >= 250 && ySize <= 290 || ySize >= 400 && ySize <= 440)
                {
                    da[i, 3] = "high";
                }
                else
                {
                    da[i, 3] = "low";
                }

                var height = (int)blockList[i].Height;

                if (height >= 6 && height <= 13 || height >= 22 && height <= 24)
                {
                    da[i, 4] = "high";
                }
                else
                {
                    da[i, 4] = "low";
                }
                da[i, 5] = blockList[i].IsTitle.ToString();
            }

            var names = new string[] { "Index", "Space", "XSize", "YSize", "Height", "IsTitle" };
            var tree = new DecisionTreeID3<string>(da, names, new string[] { "True", "False" });
            tree.Learn();
            return tree;
        }
    }
}
