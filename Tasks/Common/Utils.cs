using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Tasks.Views;

namespace Tasks.Common
{
    public static class Utils
    {
        public static IList<T> ToList<T>(this ICollection Collection)
        {
            var list = new List<T>();
            foreach (T item in Collection)
            {
                list.Add(item);
            }
            return list;
        }

        public static void Disable(this IApplicationBar AppBar)
        {
            foreach (ApplicationBarIconButton button in AppBar.Buttons)
            {
                button.IsEnabled = false;
            }
        }

        public static void Enable(this IApplicationBar AppBar)
        {
            foreach (ApplicationBarIconButton button in AppBar.Buttons)
            {
                button.IsEnabled = true;
            }
        }

        public static T[] a<T>(params T[] args)
        {
            return args;
        }

        public static void Confirm(string Title, string Message, Action Action, string PostiveButtonContent = "continue", string NegativeButtonContent = "cancel")
        {
            var messageBox = new CustomMessageBox()
            {
                Caption = Title,
                Message = Message,
                LeftButtonContent = PostiveButtonContent,
                RightButtonContent = NegativeButtonContent
            };

            messageBox.Dismissed += (s, e) =>
            {
                switch (e.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        Action();
                        break;
                    case CustomMessageBoxResult.RightButton:
                        break;
                    case CustomMessageBoxResult.None:
                        break;
                }
            };

            messageBox.Show();
        }
    }
}
