using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Sharey.Utility
{
    /// <summary>
    /// 元素隐藏转换器
    /// </summary>
    public class ValidVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value > int.Parse(parameter.ToString()) ? Visibility.Visible : Visibility.Collapsed;

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class cvtImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string loveImagePath = "";

            var isLove = (bool)value;

            if (isLove)
            {
                loveImagePath = "../Skin/Images/love_hover.png";
            }
            else
            {
                loveImagePath = "../Skin/Images/love.png";

            }
            return loveImagePath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
