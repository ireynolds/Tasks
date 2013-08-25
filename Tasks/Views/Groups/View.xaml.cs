using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Tasks.Views.Items;
using Tasks.Common;
using Tasks.Views.Groups;
using Tasks.ViewModels;
using System.Diagnostics;

namespace Tasks.Views
{
    public partial class ItemsPage : PhoneApplicationPage
    {
        private Filter _filter;
        public Filter Filter
        {
            get { return _filter; }
            set 
            { 
                _filter = value;
                FiltersBlock.DataContext = value;
            }
        }
        

        private Mode _mode;
        public Mode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        private Group _group;
        private Group Group
        {
            get
            {
                return _group;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();

                _group = value;
                this.DataContext = value;
            }
        }

        private IApplicationBar DefaultAppBar 
        { 
            get 
            {
                if (Mode == Mode.Inbox)
                {
                    return (IApplicationBar)this.Resources["InboxAppBar"];
                }
                else
                {
                    return (IApplicationBar)this.Resources["NonInboxAppBar"];
                }
            } 
        }
        
        private IApplicationBar SelectionAppBar 
        {
            get
            {
                if (Mode == Mode.Inbox)
                {
                    return (IApplicationBar)this.Resources["InboxSelectionAppBar"];
                }
                else
                {
                    return (IApplicationBar)this.Resources["NonInboxSelectionAppBar"];
                }
            }
        }

        public ItemsPage()
        {
            InitializeComponent();

            Filter = new Filter();

            if (!Debugger.IsAttached)
            {
                // Remove the "make fixtures" menuitems
                (this.Resources["InboxAppBar"] as IApplicationBar).MenuItems.RemoveAt(1);
                (this.Resources["InboxAppBar"] as IApplicationBar).MenuItems.RemoveAt(0);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Parse parameters
            if (Group == null)
            {
                string mode;
                if (NavigationContext.QueryString.TryGetValue("mode", out mode))
                {
                    Mode = (Mode)Int32.Parse(mode);

                    var id = Int32.Parse(NavigationContext.QueryString["id"]);
                    Group = Group.FindById(id);
                }
                else
                {
                    Mode = Mode.Inbox;
                    Group = Group.Inbox;
                }
            }

            // Set up the application bar
            ApplicationBar = DefaultAppBar;

            // Adjust filters and pivot for inbox/non-inbox
            Group.FiltersAreEnabled = Filter.AreFiltersEnabled = (Mode == Mode.Inbox || Group.Container.Id == Group.Inbox.Id);
            if (Mode == Mode.Inbox)
            {
                if (Group.Groups.Count == 0)
                    MainPivot.Items.Remove(GroupsPivotItem);
                else if (!MainPivot.Items.Contains(GroupsPivotItem))
                    MainPivot.Items.Add(GroupsPivotItem);
            } 
            else 
            {
                MainPivot.Items.Remove(GroupsPivotItem);
            }

            Filter.Reload();
            Group.Reload();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (ItemsList.SelectedItems.Count > 0)
            {
                ItemsList.SelectedItems.Clear();
                e.Cancel = true;
            }
        }

        private void New(object sender, EventArgs e)
        {
            NavigationService.OpenCreateItem(Group.Id);
        }

        private void Groups(object sender, EventArgs e)
        {
            NavigationService.OpenSpace(Group.Id);
        }

        private void DeleteItems(object sender, EventArgs e)
        {
            Utils.Confirm("Delete items?", "All the selected items will be deleted from your phone.", () =>
            {
                foreach (Item item in ItemsList.SelectedItems.ToList<Item>())
                {
                    Group.DeleteItem(item);
                }
                
                App.Database.SubmitChanges();
                Group.Reload();

                if (GroupsList.ItemsSource == null) return;
                foreach (Group group in GroupsList.ItemsSource)
                {
                    group.Reload();
                }

                ItemsList.SelectedItems.Clear();

            }, "delete", "cancel");
        }

        private void ItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemsList.SelectedItems.Count == 0)
            {
                ApplicationBar = DefaultAppBar;
            }
            else
            {
                ApplicationBar = SelectionAppBar;
            }
        }

        private void Item_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ItemsList.SelectedItems.Count > 0) return;

            var item = (sender as ItemPartial).DataContext as Item;
            NavigationService.OpenItem(item.Id);
        }

        private void Group_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.OpenGroup(((sender as GroupPartial).DataContext as Group).Id);
        }

        private void MakeFixtures(object sender, EventArgs e)
        {
            App.Database.MakeFixtures();

            Group = Group.Inbox;
            Group.Reload();
        }

        private void MakeBigFixtures(object sender, EventArgs e)
        {
            App.Database.MakeBigFixtures();

            Group = Group.Inbox;
            Group.Reload();
        }

        private void DeleteGroup(object sender, EventArgs e)
        {
            Utils.Confirm("Delete group?", "All the items in this group will be deleted from your phone.", () =>
            {
                Group.DestroyNow();
                NavigationService.GoBack();
            }, "delete", "cancel");
        }

        private void GroupsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Filters(object sender, EventArgs e)
        {
            NavigationService.OpenFilters();
        }

        private void FiltersBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.OpenFilters();
        }

        private void SetAllTo(IEnumerable<Item> Items, Status Status) 
        {
            foreach (Item item in ItemsList.SelectedItems)
            {
                item.Status = Status;
            }

            ItemsList.SelectedItems.Clear();

            App.Database.SubmitChanges();
            Group.Reload();
        }

        private void Complete(object sender, EventArgs e)
        {
            SetAllTo(ItemsList.SelectedItems.ToList<Item>(), Status.Complete);
        }

        private void Activate(object sender, EventArgs e)
        {
            SetAllTo(ItemsList.SelectedItems.ToList<Item>(), Status.Active);
        }

        private void OnHold(object sender, EventArgs e)
        {
            SetAllTo(ItemsList.SelectedItems.ToList<Item>(), Status.OnHold);
        }

        private void AboutAndTips(object sender, EventArgs e)
        {
            NavigationService.OpenAboutAndTips();
        }
    }
}