using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShareControl
{
    /// <summary>
    /// MessageTip.xaml 的交互逻辑
    /// </summary>
    public partial class MessageTip : Window
    {
        public MessageTip()
        {
            InitializeComponent();
            this.AllowsTransparency = Environment.OSVersion.Version.Major >= 6;
            this.KeyDown += MessageTip_KeyDown;
        }

        private Action sureAction;
        private Action cancelAction;
        private bool ischooseDialog;

        private void MakeSureClose()
        {
            this.Close();
            if (sureAction != null)
            {
                sureAction();
            }
        }
        private void CancelClose()
        {
            this.Close();
            if (cancelAction != null)
            {
                cancelAction();
            }
        }

        void MessageTip_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.btnmakeSure.Content.ToString() == "确定" && e.Key == Key.Enter)
            {
                MakeSureClose();
            }
            if (this.btnCancel.Content.ToString() == "取消" && e.Key == Key.Escape && ischooseDialog)
            {
                CancelClose();
            }
        }
        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            if (this.sureAction != null)
            {
                this.sureAction();
            }
            this.Close();
        }
        public void Show(string title, string content, Window owner = null, Action sureAction = null, Action cancelAction = null, bool ischooseDialog = false, string makeSureText = "确定", string cancelText = "取消")
        {
            this.cancelAction = cancelAction;
            this.sureAction = sureAction;
            this.ischooseDialog = ischooseDialog;

            if (owner != null)
            {
                this.Owner = owner;
                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            }
            else
            {
                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            }

            this.title.Text = title;
            this.Title = title;
            this.content.Text = content;
            this.btnmakeSure.Content = makeSureText;
            this.btnCancel.Content = cancelText;

            this.btnmakeSure.Command = new BtnCommand(() =>
            {
                MakeSureClose();
            });

            if (ischooseDialog == false)
            {
                //消息框只保留确定按钮

                this.btnCancel.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.btnCancel.Command = new BtnCommand(() =>
                {
                    CancelClose();
                });
            }

            this.ShowDialog();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
