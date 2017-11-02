using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using java.io;
using java.util.logging;
using java.net;
using java.util;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;


namespace pdfResearch
{
    public class PDFBoxLib
    {
        public static void pdf2txt(string pdfName, string txtfileName)
        {
            PDDocument doc = PDDocument.load(pdfName);

            PDFTextStripper pdfStripper = new PDFTextStripper();

            string text = pdfStripper.getText(doc);

            StreamWriter writer = new StreamWriter(txtfileName, false, Encoding.GetEncoding("gb2312"));

            writer.Write(text);
            writer.Close();
            doc.close();
        }

        public static string Pdf2txt(string pdfName)
        {
            PDDocument doc = PDDocument.load(pdfName);

            PDFTextStripper pdfStripper = new PDFTextStripper();

            // 设置按顺序输出
            pdfStripper.setSortByPosition(true);

            return pdfStripper.getText(doc);
        }


        public static Info ReadDocInfo(string fileName)
        {
            Info result = new Info();

            try
            {
                PDDocument pDoc = PDDocument.load(fileName);

                PDDocumentInformation docInfo = pDoc.getDocumentInformation();

                if (docInfo != null)
                {
                    var author = docInfo.getAuthor();
                    var title = docInfo.getTitle();
                    var summary = docInfo.getSubject();
                    var keywords = docInfo.getKeywords();

                    result.Author = author;
                    result.Title = title;
                    result.Summary = summary;
                    result.Keywords = keywords;

                }
            }
            catch (Exception ex)
            {


            }
            return result;
        }

        /**
         * 提取部分页面文本
         * @param file pdf文档路径
         * @param startPage 开始页数
         * @param endPage 结束页数
         */
        public static string ExtractTXT(String file, int startPage, int endPage)
        {
            String content = string.Empty;
            try
            {
                PDDocument document = PDDocument.load(file);
                //获取一个PDFTextStripper文本剥离对象           
                PDFTextStripper stripper = new PDFTextStripper();

                // 设置按顺序输出
                stripper.setSortByPosition(true);

                // 设置起始页
                stripper.setStartPage(startPage);
                // 设置结束页
                stripper.setEndPage(endPage);
                //获取文本内容
                content = stripper.getText(document);
                document.close();
            }
            catch (java.io.FileNotFoundException ex)
            {

            }
            catch (java.io.IOException ex)
            {

            }
            return content;
        }

        public static string HandleContent(string fileName, int pageIndex = 1)
        {
            try
            {
                PDDocument document = null;
                try
                {
                    document = PDDocument.load(fileName);
                    List allPages = document.getDocumentCatalog().getAllPages();

                    int size = pageIndex == 0 ? allPages.size() : 1;

                    for (int i = 0; i < size; i++)
                    {
                        var page = (PDPage)allPages.get(i);

                        var contents = page.getContents();

                        PrintTextLocatins2 printer = new PrintTextLocatins2();

                        if (contents != null)
                        {
                            printer.processStream(page, page.findResources(), page.getContents().getStream());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (document != null)
                    {
                        document.close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return "";
        }

    }
}
