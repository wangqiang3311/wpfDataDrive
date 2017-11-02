using pdfResearch;
using ResourceShare.UserClient.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFFileIETest
{
    public class IETitle
    {
        public static List<WordInfo> WordsInfo = new List<WordInfo>();

        private static string pdfcontent;
        public static HandleResult GetTitle(string path, string realtitle)
        {
            WordsInfo.Clear();

            string content = string.Empty;
            try
            {
                content = ITextSharpLib.ExtractTextFromPdf(path);
            }
            catch
            {
                try
                {
                    content = PDFBoxLib.Pdf2txt(path);
                }
                catch (Exception ex)
                {

                }
            }

            pdfcontent = content;

            PDFBoxLib.HandleContent(path);

            //处理字符

            Word w = new Word();
            w.MakeWord(WordsInfo);

            Line line = new Line();
            line.MakeLine(w);

            //处理行
            Block block = new Block();
            block.MakeBlock(line);

            //获取全部的文本
            string text = string.Empty;

            try
            {
                text = ITextSharpLib.ExtractTextFromPdf(path, 0);
            }
            catch (Exception ex)
            {
                text = content;
            }

            HandleResult title = new HandleResult() { Title = "" };

            try
            {
                var sentences = text.Split('\n');

                InfoExtract ie = new InfoExtract(sentences, text);

                title = ie.ExtractTitle(block, realtitle);

            }
            catch (Exception ex)
            {
                Logger.Debug(ex.Message);
            }

            return title;
        }
    }
}
