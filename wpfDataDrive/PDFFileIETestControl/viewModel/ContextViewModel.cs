using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareControl
{
    /// <summary>
    /// 上下文模型
    /// </summary>
    public class ContextViewModel : INotifyPropertyChanged
    {

        private string filedName;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string FiledName
        {
            get
            {
                return filedName;
            }
            set
            {
                if (filedName == value) return;

                filedName = value;
                Notify("FiledName");
            }
        }
        private string keyWord;

        /// <summary>
        /// 搜索
        /// </summary>
        public string KeyWord
        {
            get
            {
                return keyWord;
            }
            set
            {
                if (keyWord == value) return;

                keyWord = value;
                Notify("KeyWord");
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
