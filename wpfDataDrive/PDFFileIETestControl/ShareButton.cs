using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShareControl
{
    public class ShareButton : Button
    {
        /// <summary>
        /// 鼠标移走
        /// </summary>
        public static readonly DependencyProperty MoveBrushProperty;
        public Brush MoveBrush
        {
            set
            {
                base.SetValue(ShareButton.MoveBrushProperty, value);
            }
            get
            {
                return base.GetValue(ShareButton.MoveBrushProperty) as Brush;
            }
        }
        public static readonly DependencyProperty EnterBrushProperty;
        /// <summary>
        /// 鼠标进入
        /// </summary>
        public Brush EnterBrush
        {
            set
            {
                base.SetValue(ShareButton.EnterBrushProperty, value);
            }
            get
            {
                return base.GetValue(ShareButton.EnterBrushProperty) as Brush;
            }
        }
        static ShareButton()
        {
            //注册属性
            ShareButton.MoveBrushProperty = DependencyProperty.Register("MoveBrush", typeof(Brush), typeof(ShareButton), new PropertyMetadata(null));
            ShareButton.EnterBrushProperty = DependencyProperty.Register("EnterBrush", typeof(Brush), typeof(ShareButton), new PropertyMetadata(null));
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ShareButton), new FrameworkPropertyMetadata(typeof(ShareButton)));
        }
        public ShareButton()
        {
            base.Content = "";
        }
    }

}
