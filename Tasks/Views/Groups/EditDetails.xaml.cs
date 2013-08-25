using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Tasks.Common;

namespace Tasks.Views.Groups
{
    public partial class DetailsEditPage : PhoneApplicationPage
    {
        private Mode _mode;
        public Mode Mode
        {
            get { return _mode; }
            set 
            { 
                _mode = value;
                PageTitle.Text = _mode.ToString().ToLower();
            }
        }

        private Group _group;
        public Group Group
        {
            get
            {
                return _group;
            }
            set
            {
                _group = value;
                this.DataContext = value;
            }
        }

        public DetailsEditPage()
        {
            InitializeComponent();
            this.ApplicationBar = new OkCancelApplicationBar(Ok, Cancel).ApplicationBar;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Mode = (Mode)Int32.Parse(NavigationContext.QueryString["mode"]);
            if (Mode == Mode.Create)
            {
                Group = Group.Build();
            }
            else
            {
                var id = Int32.Parse(NavigationContext.QueryString["id"]);
                Group = Group.FindById(id);
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            Group.Reload();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (Mode == Mode.Create)
            {
                NavigationService.RemoveBackEntry();
            }
        }

        private void Cancel(object sender, EventArgs e)
        {
            ApplicationBar.Disable();
            Group.Reload();
            NavigationService.TryGoBack();
        }

        private void Ok(object sender, EventArgs e)
        {
            ApplicationBar.Disable();

            Group.InsertNow();
            
            if (Mode == Mode.Create)
            {
                NavigationService.OpenGroup(Group.Id);
            }
            else
            {
                NavigationService.TryGoBack();
            }
        }
    }
}