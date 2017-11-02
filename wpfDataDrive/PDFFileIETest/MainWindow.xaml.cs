using PDFFileFetch;
using pdfResearch;
using ResourceShare.UserClient.Common;
using ShareControl;
using Sharey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PDFFileIETest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static StaViewModel StaModel { set; get; }

        public MainWindow()
        {
            InitializeComponent();

            StaModel = new StaViewModel();

            this.DataContext = StaModel;

            FullTaskDataPool.DataPoolChangedEvent += FullTaskDataPool_DataPoolChangedEvent;
        }

        void FullTaskDataPool_DataPoolChangedEvent(object sender, DataPoolChangedEventArgs e)
        {
            //手动刷新界面
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                 (ThreadStart)delegate()
                                 {
                                     FullTaskDataPool.ReadSource();

                                     bool isMemoryPager = FullTaskDataPool.PagerWay == PagerMode.Memory ? true : false;

                                     fulltextDBControl.SetSource(FullTaskDataPool.Resource, FullTaskDataPool.ItemCount, isMemoryPager);

                                 });

        }

        /// <summary>
        /// 更新标题
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void ReAddTask(PDFTestListViewModel model)
        {
            try
            {
                var d = DBHelper.Single<TempPdf>(f => f.Id == model.Id);

                if (d != null)
                {
                    d.Title = model.IETitle;
                    model.TestResult = TestState.Pass;
                    d.TestResult = (int)model.TestResult;

                    if (DBHelper.Update<TempPdf>(d) > 0)
                    {
                        new MessageTip().Show("更新标题", "成功");
                    }
                }
            }
            catch (Exception ex)
            {
                new MessageTip().Show("更新标题", "失败");
                Logger.Debug("更新标题异常：" + ex);
            }
        }
        public void OpenAddTask()
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (FullTaskDataPool.IsInited == false)
            {
                fulltextDBControl.DeleteFullTask = FullTaskDataPool.RemoveTask;
                fulltextDBControl.DeleteFullTasks = FullTaskDataPool.RemoveTasks;

                fulltextDBControl.ReSummitFullTask = ReAddTask;
                fulltextDBControl.OpenAddFullTask = OpenAddTask;

                fulltextDBControl.PagerFullTask = FullTaskDataPool.ReadSource;

                fulltextDBControl.Search = FullTaskDataPool.Search;

                fulltextDBControl.Order = FullTaskDataPool.Order;


                fulltextDBControl.OpenWaitingWindow = OpenWaitingWindow;
                fulltextDBControl.CloseWaitingWindow = CloseWaitingWindow;

            }


            FullTaskDataPool.Init();
        }

        TaskAdding adding;
        private void OpenWaitingWindow()
        {
            adding = new TaskAdding();
            adding.lbTip.Content = "正在加载数据...";
            adding.Owner = this;
            adding.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            adding.ShowDialog();
        }

        private void CloseWaitingWindow()
        {
            if (adding != null)
            {
                adding.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            FullTaskDataPool.DataPoolChangedEvent -= FullTaskDataPool_DataPoolChangedEvent;
            FullTaskDataPool.Cancel();
        }

        private void ReadFile_Click(object sender, RoutedEventArgs e)
        {
            string path = "";
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

            ofd.InitialDirectory = @"D:\FullText";
            ofd.DefaultExt = ".pdf";
            ofd.Filter = "pdf file|*.pdf";
            if (ofd.ShowDialog() == true)
            {
                path = ofd.FileName;
            }

            var result = IETitle.GetTitle(path, "");

            this.title.Text = result.Title;
        }

        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
            StaModel.StartTime = DateTime.Now;

            FullTaskDataPool.StartGetState();
        }

        private void InitTable_Click(object sender, RoutedEventArgs e)
        {
            DatabaseInit.CreateTables(true);
        }

        private void Learn_Click(object sender, RoutedEventArgs e)
        {
            MachineStudyTest t = new MachineStudyTest();
            t.test();
        }

        private void StartTestPart_Click(object sender, RoutedEventArgs e)
        {
            FullTaskDataPool.fetcherInstance.SourceFrom = FullTaskDataPool.Resource.ToList();
            StaModel.StartTime = DateTime.Now;
            FullTaskDataPool.StartGetState();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            FullTaskDataPool.fetcherInstance.Cancel();
        }

        private void ReStart_Click(object sender, RoutedEventArgs e)
        {
            var result = DBHelper.ExecuteSql("update TempPdf set TestResult=-1,IETitle='' where Id>" + FullTaskDataPool.maxId + " and TestResult!=3");
            StaModel.StartTime = DateTime.Now;
            FullTaskDataPool.fetcherInstance.SourceFrom = FullTaskDataPool.Data.ToList();
            FullTaskDataPool.StartGetState();
        }
    }
}
