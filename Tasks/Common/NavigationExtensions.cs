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

        private const string ItemsPagePath = _prefix + "ItemsPage.xaml";
        private const string ItemCreateEditPagePath = _prefix + "Items/CreateEditPage.xaml";
        private const string ItemDetailsPagePath = _prefix + "Items/DetailsPage.xaml";
        private const string GroupsPagePath = _prefix + "GroupsPage.xaml";
        private const string GroupDetailsEditPagePath = _prefix + "Groups/DetailsEditPage.xaml";
        private const string GroupItemsEditPagePath = _prefix + "Groups/ItemsEditPage.xaml";

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
            Nav.Navigate(ItemsPagePath);
        }

        public static void OpenItems(this NavigationService Nav)
        {
            Nav.Navigate(ItemsPagePath);
        }

        public static void OpenCreateItem(this NavigationService Nav)
        {
            Nav.Navigate(String.Format("{0}?mode={1}", ItemCreateEditPagePath, (int)Mode.Create));
        }

        public static void OpenItemDetails(this NavigationService Nav, int Id)
        {
            Nav.Navigate(String.Format("{0}?id={1}", ItemDetailsPagePath, Id));
        }

        public static void OpenEditItem(this NavigationService Nav, int Id)
        {
            Nav.Navigate(String.Format("{0}?id={1}&mode={2}", ItemCreateEditPagePath, Id, (int)Mode.Edit));
        }

        public static void OpenGroups(this NavigationService Nav)
        {
            Nav.Navigate(GroupsPagePath);
        }

        public static void OpenEditGroupDetails(this NavigationService Nav, int Id)
        {
            Nav.Navigate(String.Format("{0}?id={1}&mode={2}", GroupDetailsEditPagePath, Id, (int)Mode.Edit));
        }

        public static void OpenCreateGroup(this NavigationService Nav)
        {
            Nav.Navigate(String.Format("{0}?mode={1}", ItemCreateEditPagePath, (int)Mode.Create));
        }

        public static void OpenEditGroupItems(this NavigationService Nav, int Id)
        {
            Nav.Navigate(String.Format("{0}?id={1}", GroupItemsEditPagePath, Id));
        }
    }
}
