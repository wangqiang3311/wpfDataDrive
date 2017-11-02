using ResourceShare.UserClient.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ShareControl
{
    public class PageDataManager<T> : INotifyPropertyChanged
    {
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                pageSize = value;
                NotifyPropertyChanged("PageSize");
            }
        }

        private int pageIndex;
        public int PageIndex
        {
            get { return pageIndex; }
            set
            {
                pageIndex = value;
                NotifyPropertyChanged("PageIndex");
            }
        }

        private int total;
        public int Total
        {
            get { return total; }
            set
            {
                total = value;
                NotifyPropertyChanged("Total");
            }
        }

        private Visibility preVisible = Visibility.Collapsed;

        public Visibility PreVisible
        {
            get { return preVisible; }
            set
            {
                preVisible = value;
                NotifyPropertyChanged("PreVisible");
            }
        }


        private Visibility nextVisible = Visibility.Collapsed;

        public Visibility NextVisible
        {
            get { return nextVisible; }
            set
            {
                nextVisible = value;
                NotifyPropertyChanged("NextVisible");
            }
        }


        private ObservableCollection<Pages> pages;
        public ObservableCollection<Pages> Pages
        {
            get { return pages; }
            set
            {
                pages = value;
                NotifyPropertyChanged("Pages");
            }
        }
        /// <summary>
        /// 总数
        /// </summary>
        private int itemCount;
        public int ItemCount
        {
            get { return itemCount; }
            set
            {
                itemCount = value;
                NotifyPropertyChanged("ItemCount");
            }
        }



        private ObservableCollection<T> dataSource;

        /// <summary>
        /// 总的数据源
        /// </summary>
        public ObservableCollection<T> DataSource
        {
            get { return dataSource; }
            set
            {
                dataSource = value;
                NotifyPropertyChanged("DataSource");
            }
        }

        private ObservableCollection<T> pagerSource = new ObservableCollection<T>();

        /// <summary>
        /// 每页的数据源
        /// </summary>
        public ObservableCollection<T> PagerSource
        {
            get { return pagerSource; }
            set
            {
                pagerSource = value;
                NotifyPropertyChanged("PagerSource");
            }
        }

        public Action<int, int> PagerOp;

        public bool IsMemoryPager { set; get; }


        //负责监视属性的变化
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string Propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(Propertyname));
            }
        }

        /// <summary>
        /// 打开等待窗口
        /// </summary>
        public Action OpenWaitingWindow { set; get; }
        /// <summary>
        /// 关闭等待窗口
        /// </summary>
        public Action CloseWaitingWindow { set; get; }

        public UIElement Owner { get; set; }

        public PageDataManager(ObservableCollection<T> source, int count, bool isMemoryPager = true, Action<int, int> PagerOp = null, int pageSize = 10, int pageIndex = 1)
        {
            this.PageSize = pageSize;
            this.DataSource = source;
            this.ItemCount = count;
            this.Total = this.ItemCount % PageSize == 0 ? ItemCount / PageSize : ItemCount / PageSize + 1;

            this.PagerOp = PagerOp;

            this.PageIndex = pageIndex;

            this.IsMemoryPager = isMemoryPager;

            Pager(this.PageIndex, false);
        }

        private void MakePagerNum()
        {
            //初始化页数数组
            if (this.Pages == null)
            {
                this.Pages = new ObservableCollection<Pages>();
            }
            else
            {
                this.Pages.Clear();
            }

            this.PreVisible = Visibility.Collapsed;
            this.NextVisible = Visibility.Collapsed;

            if (this.Total > 7)
            {
                //以当前页为分界点，向左借2个，向右借2个

                int leftLength = this.PageIndex - 1;
                int rightLength = this.Total - this.PageIndex;

                if (leftLength > 3 && rightLength > 3)
                {
                    this.PreVisible = Visibility.Visible;

                    for (int i = PageIndex - 2; i <= PageIndex + 2; i++)
                    {
                        this.Pages.Add(new Pages() { Name = i.ToString(), PageIndex = i });
                    }
                    this.NextVisible = Visibility.Visible;
                }

                if (rightLength <= 3)
                {
                    //右边的不够，向左边借
                    this.PreVisible = Visibility.Visible;

                    for (int i = this.PageIndex - (5 - rightLength); i <= this.Total - 1; i++)
                    {
                        this.Pages.Add(new Pages() { Name = i.ToString(), PageIndex = i });
                    }
                }
                if (leftLength <= 3)
                {
                    //左边的不够，向右边借
                    for (int i = 2; i <= this.PageIndex + (5 - leftLength); i++)
                    {
                        this.Pages.Add(new Pages() { Name = i.ToString(), PageIndex = i });
                    }
                    this.NextVisible = Visibility.Visible;
                }
            }
            else
            {
                for (int i = 2; i <= Total - 1; i++)
                {
                    this.Pages.Add(new Pages() { Name = i.ToString(), PageIndex = i });
                }
            }
        }

        private void PagerOpCompleted(IAsyncResult result)
        {
            try
            {
                var handler = (Action<int, int>)((AsyncResult)result).AsyncDelegate;
                handler.EndInvoke(result);

                if (this.Owner != null)
                {
                    this.Owner.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                               (ThreadStart)delegate()
                               {
                                   FillPagerSource();
                               });
                }
            }
            catch (Exception ex)
            {
                Logger.Log("异步分页出错：" + ex.Message);
            }
            finally
            {
                //关闭等待图标
                if (this.Owner != null)
                {
                    this.Owner.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                 (ThreadStart)delegate()
                                 {
                                     if (CloseWaitingWindow != null)
                                     {
                                         CloseWaitingWindow();
                                     }
                                 });
                }
            }
        }
        public void Pager(int pageIndex, bool canPager = true)
        {
            if (pageIndex < 1 || pageIndex > this.Total)
            {
                return;
            }
            this.PageIndex = pageIndex;

            MakePagerNum();

            if (PagerOp != null && canPager)
            {
                //委托异步执行

                IAsyncResult result = PagerOp.BeginInvoke(this.PageSize, pageIndex, new AsyncCallback(PagerOpCompleted), null);

                //打开等待图标

                if (OpenWaitingWindow != null)
                {
                    OpenWaitingWindow();
                }
            }
            else
            {
                FillPagerSource();
            }
        }

        private void FillPagerSource()
        {
            IEnumerable<T> pagerDatas = DataSource;

            if (this.IsMemoryPager)
            {
                List<T> tempSource = new List<T>();
                tempSource.AddRange(this.DataSource);
                pagerDatas = tempSource.Skip((this.PageIndex - 1) * PageSize).Take(this.PageSize);
            }

            this.PagerSource.Clear();

            foreach (var item in pagerDatas)
            {
                this.PagerSource.Add(item);
            }
        }
    }
}

public class Pages
{
    public string Name { get; set; }
    public int PageIndex { get; set; }
}
