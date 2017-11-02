using ResourceShare.UserClient.Common;
using ResourceSharing.CommonModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShareControl
{
    public partial class FullTextDBUserControl : UserControl
    {

        ObservableCollection<OrderViewModel> orders = new ObservableCollection<OrderViewModel>();

        public ContextViewModel ContextModel { set; get; }

        public FullTextDBUserControl()
        {
            InitializeComponent();

            this.DataContext = ContextModel;

            orders.Add(new OrderViewModel()
            {
                Id = 1,
                Desc = true,
                FiledName = "TestResult"
            });
            orders.Add(new OrderViewModel()
            {
                Id = 2,
                Desc = true,
                FiledName = "YEAR"
            });

            ContextModel = new ContextViewModel()
            {
                FiledName=orders.Last().FiledName,
            };

            this.cbxOrder.ItemsSource = orders;

        }
        /// <summary>
        /// 分页管理
        /// </summary>
        public PageDataManager<PDFTestListViewModel> Data;

        /// <summary>
        /// 打开等待窗口
        /// </summary>
        public Action OpenWaitingWindow { set; get; }
        /// <summary>
        /// 关闭等待窗口
        /// </summary>
        public Action CloseWaitingWindow { set; get; }


        public Action<OrderViewModel> Order { set; get; }

        public Action<string> Search { set; get; }

        public void SetSource(ObservableCollection<PDFTestListViewModel> models, int itemCount, bool isMemoryPager, int pageIndex = 1)
        {
            this.FullTextList = models;
            this.Data = new PageDataManager<PDFTestListViewModel>(FullTextList, itemCount, isMemoryPager, this.PagerFullTask, 20, pageIndex);

            this.Data.OpenWaitingWindow = OpenWaitingWindow;
            this.Data.CloseWaitingWindow = CloseWaitingWindow;
            this.Data.Owner = this;

            this.DataContext = Data;
            this.fullTaskDataGrid.DataContext = Data.PagerSource;

            fulltextPager.Visibility = itemCount == 0 ? Visibility.Collapsed : Visibility.Visible;

            prePage.Content = "<<";
            btnGoLastPage.Visibility = Data.Total == 1 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;

        }

        private int pageIndex = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            set
            {
                pageIndex = value;
            }
            get
            {
                return pageIndex;
            }
        }

        public ObservableCollection<PDFTestListViewModel> FullTextList { set; get; }


        #region  全文列表按钮操作

        /// <summary>
        /// 取消全文任务
        /// </summary>
        public Func<PDFTestListViewModel, bool> CancelFullTask { set; get; }

        /// <summary>
        /// 取消任务（向服务器端提出取消请求）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var item = this.fullTaskDataGrid.SelectedItem as PDFTestListViewModel;

            if (item != null)
            {
                if (CancelFullTask != null)
                {
                    CancelFullTask(item);
                }
            }
        }
        /// <summary>
        /// 更新标题
        /// </summary>
        public Action<PDFTestListViewModel> ReSummitFullTask { set; get; }

        private void btnReSummit_Click(object sender, RoutedEventArgs e)
        {
            var item = this.fullTaskDataGrid.SelectedItem as PDFTestListViewModel;

            if (item != null)
            {
                if (ReSummitFullTask != null)
                {
                    ReSummitFullTask(item);
                }
            }
        }
        /// <summary>
        /// 打开全文所在路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFullTextPath_Click(object sender, RoutedEventArgs e)
        {
            var item = this.fullTaskDataGrid.SelectedItem as PDFTestListViewModel;
            if (item != null)
            {
                var fileBaseDir = Tools.ReadConfig("file", "@baseDir");
                OpenFolderAndSelectFile(fileBaseDir+item.FilePath.Replace("-","")+".pdf");
            }
        }

        private void OpenFolderAndSelectFile(String fileFullName)
        {
            if (!string.IsNullOrEmpty(fileFullName))
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                psi.Arguments = "/e,/select," + fileFullName;
                System.Diagnostics.Process.Start(psi);
            }
        }
        private void OpenFile(String fileFullName)
        {
            if (!string.IsNullOrEmpty(fileFullName))
            {
                if (File.Exists(fileFullName))
                {
                    System.Diagnostics.Process.Start(fileFullName);
                }
                else
                {
                    new MessageTip().Show("全文文件", "全文文件不存在");
                }
            }
        }

        /// <summary>
        /// 删除全文任务
        /// </summary>
        public Func<PDFTestListViewModel, bool, Window, bool> DeleteFullTask { set; get; }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //删除任务
            var item = this.fullTaskDataGrid.SelectedItem as PDFTestListViewModel;

            if (item != null)
            {
                if (DeleteFullTask != null)
                {
                    StackPanel container;
                    var tip = CreateTipInstance(out container);
                    tip.Show("全文任务", "确定要删除吗？", null, () =>
                    {
                        bool isDeleteFile = false;
                        foreach (var c in container.Children)
                        {
                            var u = c as CheckBox;

                            if (u != null)
                            {
                                isDeleteFile = u.IsChecked.Value;
                                break;
                            }
                        }
                        DeleteFullTask(item, isDeleteFile, null);
                    }, null, true);
                }
            }
        }
        #endregion


        /// <summary>
        /// 全选\取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSelected_OnClick(object sender, RoutedEventArgs e)
        {
            CheckBox chkSelected = e.OriginalSource as CheckBox;
            if (chkSelected == null)
            {
                return;
            }

            bool isChecked = chkSelected.IsChecked.HasValue ? chkSelected.IsChecked.Value : true;

            FrameworkElement templateParent = chkSelected.TemplatedParent is FrameworkElement
                                                  ? (chkSelected.TemplatedParent as FrameworkElement).TemplatedParent as FrameworkElement
                                                  : null;

            if (templateParent is DataGridColumnHeader)
            {
                foreach (var item in this.Data.DataSource)
                {
                    item.IsSelected = isChecked;
                }
            }
        }

        /// <summary>
        /// 批量删除全文任务
        /// </summary>
        public Func<List<PDFTestListViewModel>, bool, bool> DeleteFullTasks { set; get; }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBachDelete_Click(object sender, RoutedEventArgs e)
        {
            var models = this.fullTaskDataGrid.DataContext as ObservableCollection<PDFTestListViewModel>;

            if (models != null && models.Count > 0)
            {
                var filters = models.Where(d => d.IsSelected == true).ToList();
                if (filters.Count > 0)
                {
                    if (DeleteFullTasks != null)
                    {
                        StackPanel container;
                        var tip = CreateTipInstance(out container);

                        tip.Show("全文任务", "确定要删除吗？", null, () =>
                        {
                            bool isDeleteFile = false;
                            foreach (var c in container.Children)
                            {
                                var u = c as CheckBox;

                                if (u != null)
                                {
                                    isDeleteFile = u.IsChecked.Value;
                                    break;
                                }
                            }
                            DeleteFullTasks(filters, isDeleteFile);
                        }, null, true);
                    }
                }
            }
        }

        private static MessageTip CreateTipInstance(out StackPanel container)
        {
            var tip = new MessageTip();
            container = tip.FindName("ContentContainer") as StackPanel;

            if (container != null)
            {
                CheckBox box = new CheckBox();
                box.Content = "同时删除对应文件";
                box.Margin = new Thickness(0, 8, 0, 0);
                box.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                box.IsChecked = true;
                container.Children.Add(box);
            }
            return tip;
        }
        /// <summary>
        /// 添加全文任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            if (OpenAddFullTask != null)
            {
                OpenAddFullTask();
            }
        }

        public Action OpenAddFullTask;

        public Action<int, int> PagerFullTask;

        private void fullTaskDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point aP = e.GetPosition(this.fullTaskDataGrid);
            IInputElement obj = this.fullTaskDataGrid.InputHitTest(aP);
            DependencyObject target = obj as DependencyObject;

            try
            {
                while (target != null)
                {
                    if (target is DataGridCell)
                    {
                        var dg = target as DataGridCell;

                        if (dg != null)
                        {
                            if (dg.Column.DisplayIndex == 1)
                            {
                                var item = this.fullTaskDataGrid.SelectedItem as PDFTestListViewModel;
                                if (item != null)
                                {
                                    var fileBaseDir = Tools.ReadConfig("file", "@baseDir");
                                    OpenFile(fileBaseDir + item.FilePath.Replace("-", "")+".pdf");
                                }
                            }
                        }
                        break;
                    }
                    target = VisualTreeHelper.GetParent(target);
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("打开全文文件出错：" + ex.Message);
            }
        }

        private void btn_GotoPage(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                var pages = button.DataContext as Pages;
                if (pages != null)
                {
                    this.PageIndex = pages.PageIndex;
                    Data.Pager(PageIndex);
                }
            }
        }

        private void goPage_Click(object sender, RoutedEventArgs e)
        {
            int pageIndex = 1;
            int.TryParse(this.wantToGo.Text, out pageIndex);

            this.PageIndex = pageIndex;
            Data.Pager(pageIndex);
        }

        private void prePage_Click(object sender, RoutedEventArgs e)
        {
            if (this.PageIndex > 1)
            {
                this.PageIndex--;
                Data.Pager(this.PageIndex);
            }
        }

        private void nextPage_Click(object sender, RoutedEventArgs e)
        {
            if (this.PageIndex < this.Data.Total)
            {
                this.PageIndex++;
                Data.Pager(this.PageIndex);
            }
        }

        private void bntGoFirstPage_Click(object sender, RoutedEventArgs e)
        {
            this.PageIndex = 1;
            Data.Pager(1);
        }

        private void btnGoLastPage_Click(object sender, RoutedEventArgs e)
        {
            int pageIndex = int.Parse((sender as Button).Content.ToString());
            this.PageIndex = pageIndex;
            Data.Pager(pageIndex);
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (Search != null)
            {
                this.ContextModel.KeyWord = this.searchContent.Text;
                Search(this.ContextModel.KeyWord);
            }
        }

        private void cbxOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentOrder = this.cbxOrder.SelectedItem as OrderViewModel;

            if (currentOrder == null) return;

            this.ContextModel.FiledName = currentOrder.FiledName;

            if (Order != null)
            {
                Order(currentOrder);
            }
        }
    }
}
