using pdfResearch;
using ResourceShare.UserClient.Common;
using ShareControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PDFFileIETest
{
    /// <summary>
    /// 抓取器，用来获取任务状态
    /// </summary>
    public class Fetcher
    {

        #region 属性和构造函数
        /// <summary>
        /// 数据源
        /// </summary>
        public List<PDFTestListViewModel> Source { set; get; }

        public List<PDFTestListViewModel> SourceFrom = FullTaskDataPool.Data;


        /// <summary>
        /// 数据执行中的状态过滤
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private bool ExcuteStateFilter(PDFTestListViewModel d)
        {
            return d.TestResult == TestState.UnRun;
        }
        public int SourceCount
        {
            get
            {
                if (Source == null)
                {
                    return 0;
                }
                else
                {
                    var filter = SourceFrom.Where(d => ExcuteStateFilter(d));

                    this.Source = filter.ToList();
                    return this.Source.Count;
                }
            }
        }
        private WorkState workState = WorkState.Stopped;
        /// <summary>
        /// 工作状态,初始状态为停止状态
        /// </summary>
        public WorkState WorkState
        {
            set
            {
                workState = value;
            }
            get
            {
                return workState;
            }
        }

        static CancellationTokenSource cancel = null;


        #endregion

        public void Start()
        {
            cancel = new CancellationTokenSource();

            var task = Task.Factory.StartNew((token) =>
            {
                try
                {
                    Logger.Log("抓取器开启了工作线程,共有" + this.SourceCount + "个任务等待测试");
                    Excute(token);
                }
                catch (Exception ex)
                {
                    Logger.Debug("执行全文任务状态更新时异常：" + ex.Message + "," + ex.StackTrace);
                }
            }, cancel.Token);
        }

        public void Cancel()
        {
            if (cancel != null)
            {
                cancel.Cancel();
            }
            Logger.Log("取消状态线程的执行");
        }


        private string fileBaseDir = ResourceShare.UserClient.Common.Tools.ReadConfig("file", "@baseDir");

        private void ExcutePiece(IEnumerable<PDFTestListViewModel> source, CancellationToken t)
        {
            if (t.IsCancellationRequested) return;

            foreach (var item in source)
            {
                if (StringTools.IsChineseLetter(item.Title))
                {
                    item.Language = PDFFileFetch.Language.Chinese;
                }
                else
                {
                    item.Language = PDFFileFetch.Language.English;
                }

                var path = fileBaseDir + item.FilePath.Replace("-", "") + ".pdf";

                if (File.Exists(path))
                {
                    var result = GetTitle(path, item.Title);

                    if (item.Language == PDFFileFetch.Language.Chinese)
                    {
                        //去掉汉语句子中的空白字符
                        result.Title = result.Title.Replace(" ", "");
                        item.Title = item.Title.Replace(" ", "");
                    }

                    item.IETitle = result.Title;
                    item.TestResult = StringTools.GetSimilarity(item.IETitle.ToLower(), item.Title.ToLower()) > 80 ? TestState.Pass : TestState.Unpass;


                    if (result.TitleCandidateList != null && result.TitleCandidateList.Count > 0)
                    {
                        if (item.TestResult == TestState.Pass)
                        {
                            MainWindow.StaModel.LocationTitleRange++;
                        }
                    }


                    if (item.TestResult == TestState.Pass)
                    {
                        MainWindow.StaModel.PassCount++;
                    }
                    if (item.TestResult == TestState.Unpass)
                    {
                        MainWindow.StaModel.UnPassCount++;
                    }

                    if (item.IETitle == "")
                    {
                        MainWindow.StaModel.UnpassEmptyCount++;
                        item.TestResult = TestState.Image;
                    }


                    if (item.TestResult == TestState.Unpass)
                    {
                        if (item.Title.Contains(item.IETitle))
                        {
                            item.TestResult = TestState.OnlyContainPart;

                            MainWindow.StaModel.UnpassHalfCount++;
                        }

                        if (result.TitleCandidateList != null && result.TitleCandidateList.Count > 0)
                        {
                            var another = result.TitleCandidateList.Find(n => n != result.Title);
                            if (another != null)
                            {
                                if (StringTools.GetSimilarity(another, item.Title) > 80)
                                {
                                    MainWindow.StaModel.CannotFindCount++;
                                    MainWindow.StaModel.LocationTitleRange++;

                                    item.TestResult = TestState.LoactionRangeButNotFound;
                                }
                            }
                        }

                    }

                    var total = MainWindow.StaModel.UnPassCount - MainWindow.StaModel.UnpassEmptyCount + MainWindow.StaModel.PassCount;

                    if (total == 0) total = 1;

                    var pureRate = (double)(MainWindow.StaModel.PassCount) / total;

                    var rate = (double)(MainWindow.StaModel.PassCount) / (MainWindow.StaModel.UnPassCount + MainWindow.StaModel.PassCount);

                    MainWindow.StaModel.PassRate = Math.Round(rate, 2, MidpointRounding.AwayFromZero);
                    MainWindow.StaModel.ExceptImagePassRate = Math.Round(pureRate, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    item.TestResult = TestState.FileNotFound;
                }

                UpdateModel(item, t);

                MainWindow.StaModel.EndTime = DateTime.Now;

                var totalSeconds = MainWindow.StaModel.EndTime.Subtract(MainWindow.StaModel.StartTime).TotalSeconds;

                var totalMinutes = totalSeconds / 60;

                var second = (int)totalSeconds % 60;

                var hours = (int)totalMinutes / 60;

                var minutes = (int)totalMinutes % 60;


                MainWindow.StaModel.DuringTime = string.Format("{0} 时：{1}分：{2}秒", hours, minutes, second);
            }
        }

        private HandleResult GetTitle(string path, string realtitle)
        {
            return IETitle.GetTitle(path, realtitle);
        }


        /// <summary>
        /// 每次接口请求，执行多个全文任务
        /// </summary>
        /// <param name="item"></param>
        private void Excute(object cancelToken)
        {
            CancellationToken t = (CancellationToken)cancelToken;
            FetchSource();

            if (this.SourceCount > 0)
            {
                WorkState = WorkState.Running;
            }
            else
            {
                Logger.Log("当前抓取器可执行数为0，准备退出");
                WorkState = WorkState.Stopped;
                return;
            }

            int maxNum = Global.FullTextStateGetLargeData;//5万数据预估，向服务器请求一次得5分钟

            while (this.SourceCount > 0 && !t.IsCancellationRequested)
            {
                int count = this.SourceCount;

                //分解大数据为小数据，然后执行
                if (count > maxNum)
                {
                    int threadCount = count % maxNum == 0 ? count / maxNum : count / maxNum + 1;

                    for (int i = 1; i <= threadCount; i++)
                    {
                        var pagerDatas = this.Source.Skip((i - 1) * maxNum).Take(maxNum);

                        ExcutePiece(pagerDatas, t);
                    }
                }
                else
                {
                    ExcutePiece(this.Source, t);
                }

                //每次不管当前的数据有没有消费掉，都会去拿新的数据
                FetchSource();

                if (SourceCount == 0)
                {
                    WorkState = WorkState.Stopped;
                    Logger.Log(string.Format("当前抓取全文状态的线程Id：{0},已完成任务，准备退出", Thread.CurrentThread.ManagedThreadId));
                    break;
                }
            }
            WorkState = WorkState.Stopped;
            Logger.Log(string.Format("当前抓取全文状态的线程Id：{0},已完成任务，准备退出", Thread.CurrentThread.ManagedThreadId));
        }

        private void UpdateModel(PDFTestListViewModel model, CancellationToken t)
        {
            if (t.IsCancellationRequested) return;

            ////更新数据库
            FullTaskDataPool.UpdateTaskRecord(model);
        }

        /// <summary>
        /// 每次都全盘扫描数据池中的数据
        /// </summary>
        private void FetchSource()
        {
            int count = 0;

            lock (FullTaskDataPool.SyncRoot)
            {
                if (FullTaskDataPool.Data.Count > 0)
                {
                    this.Source = FullTaskDataPool.Data.Where(d => ExcuteStateFilter(d)).ToList();
                    count = this.Source.Count;
                }
            }

            if (count == 0)
            {
                if (this.Source != null)
                {
                    this.Source.Clear();
                }
            }
        }
    }

    /// <summary>
    /// 自定义抓取器工作状态
    /// </summary>
    public enum WorkState
    {
        /// <summary>
        /// 运行
        /// </summary>
        Running,
        /// <summary>
        /// 停止
        /// </summary>
        Stopped
    }
}