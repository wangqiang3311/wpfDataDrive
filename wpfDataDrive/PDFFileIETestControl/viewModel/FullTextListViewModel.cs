using PDFFileFetch;
using ResourceSharing.CommonModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareControl
{
    /// <summary>
    /// pdf测试列表
    /// </summary>
    public class PDFTestListViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { set; get; }


        public Language Language { set; get; }


        private string ieTitle;

        /// <summary>
        /// 抽取标题
        /// </summary>
        public string IETitle
        {
            get
            {
                return ieTitle;
            }
            set
            {
                if (ieTitle == value) return;

                ieTitle = value;
                Notify("IETitle");
            }
        }

        /// <summary>
        /// 测试结果
        /// </summary>
        private TestState testResult;
        public TestState TestResult
        {
            get
            {
                return testResult;
            }
            set
            {
                if (testResult == value) return;

                testResult = value;
                Notify("TestResult");
            }
        }

        /// <summary>
        /// 作者
        /// </summary>
        public string AuthorDisplay { set; get; }

        public int Year { get; set; }


        /// <summary>
        /// 媒体
        /// </summary>
        /// <remarks>期刊名称，图书的丛书名称</remarks>
        public string Media { get; set; }

        public string Volume { get; set; }

        public string Issue { get; set; }

        public string PageScope { get; set; }

        public string ISSN { get; set; }

        public string URL { get; set; }

        public string DOI { get; set; }


        private bool isSelected;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }

            set
            {
                isSelected = value;
                Notify("IsSelected");
            }
        }


        private bool canOpenFullTextPath;
        /// <summary>
        /// 是否可以打开全文路径
        /// </summary>
        public bool CanOpenFullTextPath
        {
            get { return canOpenFullTextPath; }

            set
            {
                canOpenFullTextPath = value;
                Notify("CanOpenFullTextPath");
            }
        }

        private bool cancancel = true;
        /// <summary>
        /// 是否可以取消
        /// </summary>
        public bool Cancancel
        {
            get { return cancancel; }

            set
            {
                cancancel = value;
                Notify("Cancancel");
            }
        }


        private bool canDelete = true;
        /// <summary>
        /// 是否可以删除
        /// </summary>
        public bool CanDelete
        {
            get { return canDelete; }

            set
            {
                canDelete = value;
                Notify("CanDelete");
            }
        }


        private bool canReSummit = true;
        /// <summary>
        /// 是否可用重提
        /// </summary>
        public bool CanReSummit
        {
            get { return canReSummit; }

            set
            {
                canReSummit = value;
                Notify("CanReSummit");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propName)
        {

            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }


    public enum TestState
    {
        /// <summary>
        /// 未通过
        /// </summary>
        Unpass = 0,
        /// <summary>
        /// 通过
        /// </summary>
        Pass = 1,
        /// <summary>
        /// 未执行
        /// </summary>
        UnRun = -1,

        /// <summary>
        /// 文件未找到
        /// </summary>
        FileNotFound = 2,

        /// <summary>
        /// pdf内容是图片
        /// </summary>
        Image = 3,

        /// <summary>
        /// 仅仅包含部分
        /// </summary>
        OnlyContainPart = 4,

        /// <summary>
        /// 定位到但是未找到标题
        /// </summary>
        LoactionRangeButNotFound = 5,

        /// <summary>
        /// 空
        /// </summary>
        Empty=6,

        /// <summary>
        /// 其它
        /// </summary>
        Other = 7,
    }

    public class PDFTestListViewModelComparer : IEqualityComparer<PDFTestListViewModel>
    {
        public bool Equals(PDFTestListViewModel x, PDFTestListViewModel y)
        {
            return x.Id == y.Id;
        }
        public int GetHashCode(PDFTestListViewModel obj)
        {
            return base.GetHashCode();
        }
    }

}
