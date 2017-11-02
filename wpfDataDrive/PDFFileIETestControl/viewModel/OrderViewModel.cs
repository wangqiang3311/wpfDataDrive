using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareControl
{
    /// <summary>
    /// 排序模型
    /// </summary>
    public class OrderViewModel : INotifyPropertyChanged
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

        private int id;

        /// <summary>
        /// 排序Id
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (id == value) return;

                id = value;
                Notify("Id");
            }
        }

        private bool desc;

        /// <summary>
        /// 是否降序
        /// </summary>
        public bool Desc
        {
            get
            {
                return desc;
            }
            set
            {
                if (desc == value) return;

                desc = value;
                Notify("Desc");
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
