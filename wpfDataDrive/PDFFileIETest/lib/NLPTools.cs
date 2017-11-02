using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace pdfResearch
{
    public class NLPTools
    {
        static string mModelPath;
        static NLPTools()
        {
            mModelPath = ConfigurationManager.AppSettings["MaximumEntropyModelDirectory"];
        }
        private static OpenNLP.Tools.SentenceDetect.MaximumEntropySentenceDetector mSentenceDetector;
        public static string[] SplitSentences(string paragraph)
        {
            if (mSentenceDetector == null)
            {
                mSentenceDetector = new OpenNLP.Tools.SentenceDetect.EnglishMaximumEntropySentenceDetector(mModelPath + "EnglishSD.nbin");
            }

            return mSentenceDetector.SentenceDetect(paragraph);
        }
        private static OpenNLP.Tools.NameFind.EnglishNameFinder mNameFinder;

        public static string FindNames(string sentence)
        {
            if (mNameFinder == null)
            {
                mNameFinder = new OpenNLP.Tools.NameFind.EnglishNameFinder(mModelPath + "namefind\\");
            }

            string[] models = new string[] { "person" };
            return mNameFinder.GetNames(models, sentence);
        }
    }
}
