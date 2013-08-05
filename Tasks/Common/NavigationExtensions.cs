using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Tasks.Common
{
    public static class NavigationExtensions
    {
        private const string _prefix = "/Views/";

        private const string EditItemPath = _prefix + "Items/EditDetails.xaml";
        private const string ViewItemPath = _prefix + "Items/View.xaml";

        private const string EditGroupPath = _prefix + "Groups/EditDetails.xaml";
        private const string ViewGroupPath = _prefix + "Groups/View.xaml";

        private const string ViewSpacePath = _prefix + "Space.xaml";
        
        public static void Navigate(this NavigationService Nav, string Uri)
        {
            Nav.Navigate(new Uri(Uri, UriKind.RelativeOrAbsolute));
        }

        public static void TryGoBack(this NavigationService Nav)
        {
            if (Nav.CanGoBack)
            {
                Nav.GoBack();
            }
            else
            {
                Nav.GoHome();
            }
        }

        public static void GoHome(this NavigationService Nav)
        {
            Nav.Navigate(ViewGroupPath);
        }

        public static void OpenGroup(this NavigationService Nav, int Id)
        {
            Nav.Navigate(String.Format("{0}?id={1}&mode={2}", ViewGroupPath, Id, (int)Mode.View));
        }

        public static void OpenCreateItem(this NavigationService Nav, int GroupId)
        {
            Nav.Navigate(String.Format("{0}?mode={1}&groupId={2}", EditItemPath, (int)Mode.Create, GroupId));
        }

        public static void OpenItem(this NavigationService Nav, int Id)
        {
            Nav.Navigate(String.Format("{0}?id={1}", ViewItemPath, Id));
        }

        public static void OpenEditItem(this NavigationService Nav, int Id)
        {
            Nav.Navigate(String.Format("{0}?id={1}&mode={2}", EditItemPath, Id, (int)Mode.Edit));
        }

        public static void OpenSpace(this NavigationService Nav)
        {
            Nav.Navigate(ViewSpacePath);
        }

        public static void OpenEditGroup(this NavigationService Nav, int Id)
        {
            Nav.Navigate(String.Format("{0}?id={1}&mode={2}", EditGroupPath, Id, (int)Mode.Edit));
        }

        public static void OpenCreateGroup(this NavigationService Nav)
        {
            Nav.Navigate(String.Format("{0}?mode={1}", EditGroupPath, (int)Mode.Create));
        }
    }
}
