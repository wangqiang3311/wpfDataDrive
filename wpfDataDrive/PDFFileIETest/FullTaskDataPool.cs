using PDFFileFetch;
using ResourceShare.UserClient.Common;
using ShareControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PDFFileIETest
{
    /// <summary>
    /// 全文任务数据池，专门用来存储数据（包括数据库的读写以及用户界面数据显示）
    /// </summary>
    public class FullTaskDataPool
    {
        #region  属性

        /// <summary>
        /// 在全文任务删除和任务状态更改同步时加锁
        /// </summary>
        public static readonly object SyncRoot = new object();

        /// <summary>
        /// 当全文任务列表页面数据源变动时，锁定
        /// </summary>
        public static readonly object UiSourceLock = new object();

        private static readonly object SingleFetcherLock = new object();


        private static List<PDFTestListViewModel> dbList = new List<PDFTestListViewModel>();


        private static ObservableCollection<PDFTestListViewModel> uiResource = new ObservableCollection<PDFTestListViewModel>();

        public static List<PDFTestListViewModel> Data
        {
            get
            {
                return dbList;
            }
        }

        public static ObservableCollection<PDFTestListViewModel> Resource
        {
            get
            {
                return uiResource;
            }
        }

        public static readonly Fetcher fetcherInstance = new Fetcher();

        /// <summary>
        /// 是否初始化完成
        /// </summary>
        public static bool IsInited = false;
        /// <summary>
        /// 是否同步过
        /// </summary>
        public static bool IsSyned = false;

        public static int ItemCount { set; get; }

        public static Int64 maxId = Int64.Parse(ConfigurationManager.AppSettings["MaxId"].ToString());

        public static Int64 minId = Int64.Parse(ConfigurationManager.AppSettings["MinId"].ToString());


        public static event DataPoolChangedHandle DataPoolChangedEvent;

        #endregion

        /// <summary>
        /// 数据池初始化
        /// </summary>
        public static void Init(bool isStartGetState = true)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                var list = DBHelper.Column<int>("select count(1) from TempPdf where Id> " + minId + " and Id<=" + maxId);

                if (list != null)
                {
                    ItemCount = list.First();
                    //根据count决定数据分页方式
                    PagerWay = ItemCount > Global.FullTextDBLargeData ? PagerMode.DB : PagerMode.Memory;
                }
                InitPool(ItemCount);
            });
        }

        public static PagerMode PagerWay = PagerMode.Memory;
        public static void ReadSource(int pageSize = 20, int index = 1)
        {
            try
            {
                if (PagerWay == PagerMode.DB)
                {
                    string condition = string.IsNullOrEmpty(searchkey) ? "" : " and  TestResult='" + searchkey + "'";

                    var list = DBHelper.Column<int>(string.Format("select count(1) from TempPdf where 1=1 and Id>{0} and Id<={1}  {2}", minId, maxId, condition));

                    if (list != null && list.Count > 0)
                    {
                        ItemCount = list.First();

                        string sql = string.Format(" select * from (select ROW_NUMBER()over(order by " + orderFiledName + desc + ") as rowNo,* from  TempPdf where 1=1  and Id>" + minId + " and Id<=" + maxId + " " + condition + ") as t   where rowNo between {0} and {1}", (pageSize * (index - 1) + 1), pageSize * index);

                        var pageRecords = DBHelper.Select<TempPdf>(sql);

                        var models = ReadFullTaskRecord(pageRecords);
                        DoInitUI(models);
                    }
                }
                else if (PagerWay == PagerMode.Memory)
                {
                    var list = dbList.OrderByDescending(d => d.Year).ToList();

                    if (!string.IsNullOrEmpty(searchkey))
                    {
                        if (orderFiledName.ToLower() == "year")
                        {

                            if (desc.Trim() == "desc")
                            {
                                list = dbList.Where(d => d.TestResult == (TestState)int.Parse(searchkey)).OrderByDescending(d => d.Year).ToList();

                            }
                            else
                            {
                                list = dbList.Where(d => d.TestResult == (TestState)int.Parse(searchkey)).OrderBy(d => d.Year).ToList();
                            }
                        }

                        if (orderFiledName.ToLower() == "testresult")
                        {
                            if (desc.Trim() == "desc")
                            {
                                list = dbList.Where(d => d.TestResult == (TestState)int.Parse(searchkey)).OrderByDescending(d => d.TestResult).ToList();
                            }
                            else
                            {
                                list = dbList.Where(d => d.TestResult == (TestState)int.Parse(searchkey)).OrderBy(d => d.TestResult).ToList();
                            }

                        }
                    }

                    ItemCount = list.Count;
                    DoInitUI(list);
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("全文读取出错：" + ex.Message);
            }
        }


        public static void InitPool(int count)
        {
            IsInited = true;

            if (PagerWay == PagerMode.DB)
            {
                //数据库分页方式，只需要依赖数据库
                NotifyOnlyUIUpdate("初始化数据库分页");
            }

            //内存分页依赖于数据池中的数据

            Logger.Log("任务列表初始化开始，共有" + count + "条数据,当前线程Id：" + Thread.CurrentThread.ManagedThreadId);

            var records = DBHelper.Select<TempPdf>(d => d.Id > minId && d.Id <= maxId).OrderByDescending(f => f.Year);

            count = records.Count();

            int maxNum = Global.FullTextMemoryLargeData;

            lock (SyncRoot)
            {
                FullTaskDataPool.dbList.Clear();
            }

            if (count <= maxNum)
            {
                var models = ReadFullTaskRecord(records);
                if (models != null && models.Count > 0)
                {
                    DoInit(models);

                    if (PagerWay == PagerMode.Memory)
                    {
                        ItemCount = dbList.Count;
                        NotifyOnlyUIUpdate("小数据量初始化，更新条数：" + models.Count);
                    }
                }
            }
            else
            {
                //如果数据超过maxNum条，那么分批次加载数据

                int times = count % maxNum == 0 ? count / maxNum : count / maxNum + 1;

                var task = Task.Factory.StartNew(() =>
                {
                    for (int i = 1; i <= times; i++)
                    {
                        var pagerDatas = records.Skip((i - 1) * maxNum).Take(maxNum);

                        var models = ReadFullTaskRecord(pagerDatas);

                        if (models != null && models.Count > 0)
                        {
                            DoInit(models);

                            if (PagerWay == PagerMode.Memory)
                            {
                                ItemCount = dbList.Count;
                                NotifyOnlyUIUpdate("大数据量分批次初始化,当前批次更新条数：" + models.Count);
                            }
                        }
                    }
                    Logger.Log("任务列表初始化完成，初始化条数：" + count);
                });

            }
        }

        public static void DoInit(IEnumerable<PDFTestListViewModel> models, bool isStartGetState = true)
        {
            lock (FullTaskDataPool.SyncRoot)
            {
                foreach (var item in models)
                {
                    if (!FullTaskDataPool.dbList.Exists(c => c.Id == item.Id)) FullTaskDataPool.dbList.Add(item);
                }
            }

            StartFetchState(false);
        }

        public static void DoInitUI(IEnumerable<PDFTestListViewModel> models)
        {
            lock (FullTaskDataPool.UiSourceLock)
            {
                FullTaskDataPool.uiResource.Clear();

                foreach (var item in models)
                {
                    FullTaskDataPool.uiResource.Add(item);
                }
            }
        }

        public static void ClearUIResource()
        {
            lock (FullTaskDataPool.UiSourceLock)
            {
                FullTaskDataPool.uiResource.Clear();
            }
        }

        /// <summary>
        /// 通知UI更新
        /// </summary>
        /// <param name="isStartGetState"></param>
        public static void NotifyOnlyUIUpdate(string message = "")
        {
            if (DataPoolChangedEvent != null)
            {
                DataPoolChangedEvent(null, new DataPoolChangedEventArgs(message));
            }
        }
        //通知获取状态
        private static void StartFetchState(bool isStartGetState = true)
        {
            if (isStartGetState)
            {
                StartGetState();
            }
        }
        /// <summary>
        /// 通知ui更新和开启状态获取
        /// </summary>
        private static void NotifyAll(string message = "")
        {
            NotifyOnlyUIUpdate(message);
            StartFetchState();
        }
        /// <summary>
        /// 向数据池中批量添加
        /// </summary>
        public static void AddRange(IEnumerable<TempPdf> records, string message = "")
        {
            var models = ReadFullTaskRecord(records);

            if (models != null && models.Count > 0)
            {
                lock (FullTaskDataPool.SyncRoot)
                {
                    foreach (var item in models)
                    {
                        if (!FullTaskDataPool.dbList.Exists(d => d.Id == item.Id)) FullTaskDataPool.dbList.Add(item);
                    }
                }
                NotifyAll(message);
            }
        }
        public static void AddOne(PDFTestListViewModel model, string message = "")
        {
            lock (FullTaskDataPool.SyncRoot)
            {
                if (!FullTaskDataPool.dbList.Exists(d => d.Id == model.Id)) FullTaskDataPool.dbList.Insert(0, model);
            }
            NotifyAll(message);
        }

        public static void RemoveOne(PDFTestListViewModel model)
        {
            lock (FullTaskDataPool.SyncRoot)
            {
                FullTaskDataPool.dbList.Remove(model);
            }
            NotifyOnlyUIUpdate("删除一条任务");
        }
        public static void RemoveRange(IEnumerable<PDFTestListViewModel> removes)
        {
            lock (FullTaskDataPool.SyncRoot)
            {
                foreach (var model in removes)
                {
                    FullTaskDataPool.dbList.Remove(model);
                }
            }
            NotifyOnlyUIUpdate("批量删除一组任务");
        }

        /// <summary>
        /// 启动任务状态线程
        /// </summary>
        public static void StartGetState()
        {
            lock (SingleFetcherLock)
            {
                if (fetcherInstance.WorkState == WorkState.Stopped)
                {
                    fetcherInstance.WorkState = WorkState.Running;
                    fetcherInstance.Start();
                }
            }
        }

        public static void Cancel()
        {
            fetcherInstance.Cancel();

            fetcherInstance.WorkState = WorkState.Stopped;
        }


        #region 全文任务列表操作API

        public static bool IsEndPass(PDFTestListViewModel model)
        {
            return IsEndPass(model.TestResult);
        }
        public static bool IsEndPass(TestState state)
        {
            return state == TestState.Pass || state == TestState.Unpass;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="taskClientId"></param>
        public static bool RemoveTask(PDFTestListViewModel model, bool isDeleteFile, Window w = null)
        {
            var flag = false;

            var result = RemoveTaskRecord(model);

            if (result == false)
            {
                Logger.Log("删除任务记录失败");
                return flag;
            }
            flag = result;

            RemoveOne(model);
            return flag;
        }

        /// <summary>
        /// 删除一组任务
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static bool RemoveTasks(List<PDFTestListViewModel> models, bool isDeleteFile)
        {
            var flag = false;
            List<PDFTestListViewModel> removes = new List<PDFTestListViewModel>();

            foreach (var model in models)
            {
                removes.Add(model);
            }

            var result = RemoveTaskRecords(removes);

            if (result == false)
            {
                new MessageTip().Show("全文任务", "批量删除任务记录失败");
                return flag;
            }
            flag = result;

            RemoveRange(removes);

            return flag;
        }
        #endregion

        #region  全文任务记录数据库表操作

        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public static List<PDFTestListViewModel> ReadFullTaskRecord(IEnumerable<TempPdf> models)
        {
            var viewModels = new List<PDFTestListViewModel>();

            foreach (var item in models)
            {
                var vm = new PDFTestListViewModel()
                {
                    AuthorDisplay = item.AuthorDisplay,
                    DOI = item.DOI,
                    FilePath = item.FilePath,
                    Id = item.Id,
                    IETitle = item.IETitle,
                    ISSN = item.ISSN,
                    Issue = item.Issue,
                    Media = item.Media,
                    PageScope = item.PageScope,
                    TestResult = (TestState)item.TestResult,
                    Title = item.Title,
                    URL = item.URL,
                    Volume = item.Volume,
                    Year = item.Year
                };
                viewModels.Add(vm);
            }
            return viewModels;
        }

        /// <summary>
        /// 更新全文任务记录
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="file">最终的全文文件信息</param>
        /// <returns></returns>
        public static bool UpdateTaskRecord(PDFTestListViewModel viewModel)
        {
            bool result = false;

            try
            {
                var d = DBHelper.Single<TempPdf>(f => f.Id == viewModel.Id);

                if (d != null)
                {
                    d.IETitle = viewModel.IETitle;
                    d.TestResult = (int)viewModel.TestResult;
                    d.Language = viewModel.Language;
                    result = DBHelper.Update<TempPdf>(d) > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("更新全文任务记录表异常：" + ex);
            }

            return result;
        }
        /// <summary>
        /// 删除任务记录
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static bool RemoveTaskRecord(PDFTestListViewModel viewModel)
        {
            return DBHelper.Delete<TempPdf>(d => d.Id == viewModel.Id) > 0;
        }

        /// <summary>
        /// 批量删除任务记录
        /// </summary>
        /// <returns></returns>
        public static bool RemoveTaskRecords(List<PDFTestListViewModel> viewModels)
        {
            bool result = false;

            if (viewModels.Count > 0)
            {
                var taskIds = viewModels.Select(d => d.Id).ToArray();

                result = DBHelper.Delete<TempPdf>(d => taskIds.Contains(d.Id)) > 0;
            }
            return result;
        }

        public static void UpdateFullTextTask(TempPdf item)
        {
            var model = Data.FirstOrDefault(d => d.Id == item.Id);
            if (model != null)
            {
                model.IETitle = item.IETitle;
            }
        }

        #endregion

        private static string orderFiledName = "year";

        private static string desc = " desc";

        private static string searchkey;


        public static void Order(OrderViewModel model)
        {
            orderFiledName = model.FiledName;
            desc = model.Desc ? " desc" : "";

            NotifyOnlyUIUpdate("排序");
        }
        public static void Search(string keyWord)
        {
            searchkey = keyWord;
            NotifyOnlyUIUpdate("搜索");
        }
    }


    public delegate void DataPoolChangedHandle(object sender, DataPoolChangedEventArgs e);

    public class DataPoolChangedEventArgs : EventArgs
    {
        public string Message { set; get; }
        public DataPoolChangedEventArgs(string message)
        {
            this.Message = message;
        }
    }
    public enum PagerMode
    {
        /// <summary>
        /// 内存分页
        /// </summary>
        Memory = 0,
        /// <summary>
        /// 数据库分页
        /// </summary>
        DB = 1
    }

}
