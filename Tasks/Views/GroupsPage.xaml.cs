using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Tasks.ViewModels;

namespace Tasks.Views
{
    public partial class GroupsPage : PhoneApplicationPage
    {
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

        public GroupsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Space = new Space();
        }

        private void Group_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }
    }
}