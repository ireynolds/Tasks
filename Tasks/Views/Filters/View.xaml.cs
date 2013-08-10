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

namespace Tasks.Views.Filters
{
    public partial class View : PhoneApplicationPage
    {
        private Filter _filter;
        public Filter Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                this.DataContext = value;
            }
        }

        public View()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Filter = new Filter();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Filter.Save();
        }
    }
}