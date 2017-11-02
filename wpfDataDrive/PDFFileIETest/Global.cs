using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows;

namespace PDFFileIETest
{
    /// <summary>
    /// 此类主要用于做一些全局的初始化
    /// </summary>
    public class Global
    {
        public static string InfoUrl, ModifyPwdUrl, ModifyOrgUrl, ContactUrl, GuideUrl;
        public static readonly string RegistUrl, FindPwdUrl;

        public static readonly int LoginTryTimes = 3;
        public static readonly int ConnectVPNTryTimes = 3;
        public static readonly string FullTextAddWaitTip = "正在获取页面中标题、作者等详细信息，请耐心等待...";
        public static readonly int FullTextDBLargeData = 2000;       //大数据按数据库分页
        public static readonly int FullTextMemoryLargeData = 1000;  //内存中大数据按分块加载
        public static readonly int FullTextStateGetLargeData = 500;  //内存中大数据按分块获取状态
        public static readonly double OpenModalOpacity = 0.6;

        /// <summary>
        /// 用于线程取消
        /// </summary>
        public static CancellationTokenSource Cancel { set; get; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public static int UserType { set; get; }
    }
}
