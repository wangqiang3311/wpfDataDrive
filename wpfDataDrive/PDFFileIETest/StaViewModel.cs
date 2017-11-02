using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFFileIETest
{
    public class StaViewModel : INotifyPropertyChanged
    {
        private int passCount;

        /// <summary>
        /// 通过数
        /// </summary>
        public int PassCount
        {
            get
            {
                return passCount;
            }
            set
            {
                if (passCount == value) return;

                passCount = value;
                Notify("PassCount");
            }
        }

        private int unpassEmptyCount;

        /// <summary>
        /// 空白未通过数
        /// </summary>
        public int UnpassEmptyCount
        {
            get
            {
                return unpassEmptyCount;
            }
            set
            {
                if (unpassEmptyCount == value) return;

                unpassEmptyCount = value;
                Notify("UnpassEmptyCount");
            }
        }

        private double exceptImagePassRate;

        /// <summary>
        /// 除去图片的通过率
        /// </summary>
        public double ExceptImagePassRate
        {
            get
            {
                return exceptImagePassRate;
            }
            set
            {
                if (exceptImagePassRate == value) return;

                exceptImagePassRate = value;
                Notify("ExceptImagePassRate");
            }
        }


        private int unpassCount;

        /// <summary>
        /// 未通过数
        /// </summary>
        public int UnPassCount
        {
            get
            {
                return unpassCount;
            }
            set
            {
                if (unpassCount == value) return;

                unpassCount = value;
                Notify("UnPassCount");
            }
        }
        private int unpassHalfCount;

        /// <summary>
        /// 标题获取不完整的未通过数
        /// </summary>
             public int UnpassHalfCount
        {
            get
            {
                return unpassHalfCount;
            }
            set
            {
                if (unpassHalfCount == value) return;

                unpassHalfCount = value;
                Notify("UnpassHalfCount");
            }
        }


             private int locationTitleRange;

             /// <summary>
             /// 定位到标题范围
             /// </summary>
             public int LocationTitleRange
             {
                 get
                 {
                     return locationTitleRange;
                 }
                 set
                 {
                     if (locationTitleRange == value) return;

                     locationTitleRange = value;
                     Notify("LocationTitleRange");
                 }
             }



             private int cannotFindCount;

             /// <summary>
             /// 已定位未能找到标题数
             /// </summary>
             public int CannotFindCount
             {
                 get
                 {
                     return cannotFindCount;
                 }
                 set
                 {
                     if (cannotFindCount == value) return;

                     cannotFindCount = value;
                     Notify("CannotFindCount");
                 }
             }





        private double passRate;

        public double PassRate
        {
            get
            {
                return passRate;
            }
            set
            {
                if (passRate == value) return;

                passRate = value;
                Notify("PassRate");
            }
        }

        private DateTime startTime;

        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                if (startTime == value) return;

                startTime = value;
                StartTimeFormat = startTime.ToString("yyyy-MM-dd HH:mm:ss");

                Notify("StartTime");
            }
        }


        private string startTimeFormat;

        public string  StartTimeFormat
        {
            get
            {
                return startTimeFormat;
            }
            set
            {
                if (startTimeFormat == value) return;

                startTimeFormat = value;
                Notify("StartTimeFormat");
            }
        }

        private string endTimeFormat;

        public string EndTimeFormat
        {   
            get
            {
                return endTimeFormat;
            }
            set
            {
                if (endTimeFormat == value) return;

                endTimeFormat = value;
                Notify("EndTimeFormat");
            }
        }


        private DateTime endTime;

        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                if (endTime == value) return;

                endTime = value;

                EndTimeFormat = endTime.ToString("yyyy-MM-dd HH:mm:ss");


                Notify("EndTime");
            }
        }
        private string duringTime;

        public string DuringTime
        {
            get
            {
                return duringTime;
            }
            set
            {
                if (duringTime == value) return;

                duringTime = value;
                Notify("DuringTime");
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
}
