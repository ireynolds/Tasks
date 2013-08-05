﻿using System;
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

namespace Tasks.Views
{
    public partial class ItemsPage : PhoneApplicationPage
    {
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

                DetailsBlock.Visibility = Group.Inbox.Equals(value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private IApplicationBar DefaultAppBar 
        { 
            get { return (IApplicationBar)this.Resources["DefaultAppBar"]; } 
        }
        
        private IApplicationBar SelectionAppBar 
        { 
            get { return (IApplicationBar)this.Resources["SelectionAppBar"]; } 
        }

        public ItemsPage()
        {
            InitializeComponent();
            ApplicationBar = DefaultAppBar;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string mode;
            if (NavigationContext.QueryString.TryGetValue("mode", out mode))
            {
                Mode = (Mode)Int32.Parse(mode);

                var id = Int32.Parse(NavigationContext.QueryString["id"]);
                Group = Group.FindWithId(id);
            }
            else
            {
                Mode = Mode.Inbox;
                Group = Group.Inbox;
            }
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
            NavigationService.OpenGroups();
        }

        private void Delete(object sender, EventArgs e)
        {
            foreach (Item item in ItemsList.SelectedItems.ToList<Item>())
            {
                Group.DeleteItem(item);
            }
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

        private void LongListMultiSelectorItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = (sender as LongListMultiSelectorItem).DataContext as Item;
            NavigationService.OpenItemDetails(item.Id);
        }

        private void MakeFixtures(object sender, EventArgs e)
        {
            App.Database.MakeFixtures();
            Group = Group.Inbox;
        }
    }
}