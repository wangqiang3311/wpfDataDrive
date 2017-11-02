using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.apache.pdfbox.util;
using PDFFileIETest;

namespace pdfResearch
{
    public class PrintTextLocatins2 : PDFTextStripper
    {
        private static int BOLD_F_NUM = 2;
        private static String[] BOLD_FLAGS = { "Bold", "CAJ FNT04" };
        private static int ITALIC_F_NUM = 2;
        private static String[] ITALIC_FLAGS = { "Italic", "CAJ FNT03" };

        private static bool IsBold(String font)
        {
            int i;
            for (i = 0; i < BOLD_F_NUM; i++)
                if (font.Contains(BOLD_FLAGS[i]))
                    return true;
            return false;
        }

        private static bool IsItalic(String font)
        {
            int i;
            for (i = 0; i < ITALIC_F_NUM; i++)
                if (font.Contains(ITALIC_FLAGS[i]))
                    return true;
            return false;
        }

        public PrintTextLocatins2()
        {
            base.setSortByPosition(false);
        }
        protected override void processTextPosition(TextPosition text)
        {

            WordInfo info = new WordInfo()
            {
                X = text.getX(),
                Y = text.getY(),
                XDirAdj = text.getXDirAdj(),
                YDirAdj = text.getYDirAdj(),
                FontSize = text.getFontSize(),
                Xscale = text.getXScale(),
                Yscale = text.getYScale(),
                Height = text.getHeight(),
                Space = text.getWidthOfSpace(),
                Width = text.getWidth(),

                Subfont = text.getFont().getSubType(),
                Basefont = text.getFont().getBaseFont(),
                IsBold = IsBold(text.getFont().getBaseFont()),
                IsItalic = IsItalic(text.getFont().getBaseFont()),

                XSize = (int)(text.getFontSize() * text.getXScale()),

                YSize = (int)(text.getFontSize() * text.getYScale()),

                Word = text.getCharacter()
            };


            if (info.Space.ToString() == "非数字")
            {
                info.Space = 0;
            }

            IETitle.WordsInfo.Add(info);
        }
    }
}


