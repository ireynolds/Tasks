﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Tasks.ViewModels;
using Tasks.Common;
using Tasks.Views.Groups;

namespace Tasks.Views
{
    public partial class GroupsPage : PhoneApplicationPage
    {
        public ApplicationBar DefaultAppBar
        {
            get
            {
                return (ApplicationBar)this.Resources["DefaultAppBar"];
            }
        }

        public ApplicationBar SelectedAppBar
        {
            get
            {
                return (ApplicationBar)this.Resources["SelectedAppBar"];
            }
        }

        public bool IsSelectionEnabled
        {
            get
            {
                return GroupsList.IsSelectionEnabled;
            }
            set
            {
                GroupsList.IsSelectionEnabled = value;
                if (value)
                {
                    this.ApplicationBar = SelectedAppBar;
                }
                else
                {
                    this.ApplicationBar = DefaultAppBar;
                    GroupsList.SelectedItems.Clear();
                }
            }
        }

        private Space _space;
        public Space Space
        {
            get
            {
                return _space;
            }
            set
            {
                _space = value;
                this.DataContext = value;
            }
        }

        private Group _group;
        public Group Group
        {
            get { return _group; }
            set { _group = value; }
        }

        public GroupsPage()
        {
            InitializeComponent();
            this.ApplicationBar = DefaultAppBar;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            Space = new Space();

            var targetId = Int32.Parse(NavigationContext.QueryString["targetId"]);
            Group = Group.FindById(targetId);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (GroupsList.IsSelectionEnabled)
            {
                this.IsSelectionEnabled = false;
                e.Cancel = true;
            }
        }

        private void GroupsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.IsSelectionEnabled = GroupsList.SelectedItems.Count > 0;
        }

        private void Group_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.OpenGroup(((sender as GroupPartial).DataContext as Group).Id);
        }

        private void Select(object sender, EventArgs e)
        {
            this.IsSelectionEnabled = true;
        }

        private void Ok(object sender, EventArgs e)
        {
            var title = String.Format("Merge into {0}?", Group.Title);
            var message = String.Format("All the selected groups will be merged into {0}.", Group.Title);
            
            Utils.Confirm(title, message, () =>
            {
                foreach (Group group in GroupsList.SelectedItems)
                {
                    Group.MergeIntoThis(group, group.UnfilteredItems);
                }
                NavigationService.TryGoBack();
            }, "merge", "cancel");
        }

        private void Cancel(object sender, EventArgs e)
        {
            this.IsSelectionEnabled = false;
        }

        private void New(object sender, EventArgs e)
        {
            NavigationService.OpenCreateGroup();
        }
    }
}