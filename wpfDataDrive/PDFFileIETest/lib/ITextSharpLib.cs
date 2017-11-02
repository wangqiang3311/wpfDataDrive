using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.io;

namespace pdfResearch
{
    public class ITextSharpLib
    {

        public static string ReadPDFOnePage(string path, int pageIndex = 1)
        {
            PdfReader reader = new PdfReader(path);

            PdfReaderContentParser parser = new PdfReaderContentParser(reader);

            ITextExtractionStrategy strategy;
            strategy = parser.ProcessContent<SimpleTextExtractionStrategy>(pageIndex, new SimpleTextExtractionStrategy());

            return strategy.GetResultantText();
        }

        public static string ReadPDF(string path)
        {
            //从每一页读出的字符串  
            string content = String.Empty;
            try
            {
                PdfReader p = new PdfReader(path);

                //"[......]"内部字符串  
                string subStr = String.Empty;
                //函数返回的字符串  
                string rtStr = String.Empty;
                //从每一页读出的8位字节数组  
                byte[] b = new byte[0];
                //"[","]","(",")"在字符串中的位置  
                //取得文档总页数  
                int pg = p.NumberOfPages;

                StringBuilder sb = new StringBuilder();

                for (int i = 1; i <= pg; i++)
                {
                    Array.Resize(ref b, 0);
                    //取得第i页的内容  
                    b = p.GetPageContent(i);

                    //取得每一页的字节数组,将每一个字节转换为字符,并将数组转换为字符串  
                    for (int j = 0; j < b.Length; j++) sb.Append(Convert.ToChar(b[j]));
                    content += sb.ToString();
                }
            }
            catch (Exception ex)
            {
               
            }
            return content;
        }

        /// <summary>
        /// 创建pdf
        /// </summary>
        public static void CreatePDF(string path, string content)
        {
            try
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document();

                PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
                document.Open();

                iTextSharp.text.Font font = CreateChineseFont();


                document.Add(new Paragraph(content, font));
                document.Close();
               
            }
            catch (Exception ex)
            {
            }
        }

        private static iTextSharp.text.Font CreateChineseFont()
        {
            //如果不使用CID字体，下面三行不需要
            //BaseFont.AddToResourceSearch("iTextAsian.dll");
            //BaseFont.AddToResourceSearch("iTextAsianCmaps.dll");
            //新版的AddToResourceSearch移到了StreamUtil
            StreamUtil.AddToResourceSearch("iTextAsian.dll");
            StreamUtil.AddToResourceSearch("iTextAsianCmaps.dll");

            //"UniGB-UCS2-H" "UniGB-UCS2-V"是简体中文，分别表示横向字 和 // 纵向字 //" STSong-Light"是字体名称 
            BaseFont baseFT = BaseFont.CreateFont(@"c:\windows\fonts\SIMHEI.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            //BaseFont baseFT = BaseFont.CreateFont("STSong-Light", "UniGB-UCS2-H", BaseFont.EMBEDDED);

            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT);
            return font;
        }

        public static string ExtractTextFromPdf(string path, int pageIndex = 1)
        {
            //ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();

            ITextExtractionStrategy its = new LocationTextExtractionStrategyEx();

            using (PdfReader reader = new PdfReader(path))
            {
                StringBuilder text = new StringBuilder();

                int pn = reader.NumberOfPages > 10 ? 10 : reader.NumberOfPages;

                for (int i = 1; i <= pn; i++)
                {
                    if (i != pageIndex && pageIndex != 0) continue;

                    string thePage = PdfTextExtractor.GetTextFromPage(reader, i, its);
                    string[] theLines = thePage.Split('\n');
                    foreach (var theLine in theLines)
                    {
                        text.AppendLine(theLine);
                    }
                }
                return text.ToString();
            }
        }
    }
}
