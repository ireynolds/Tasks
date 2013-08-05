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

namespace Tasks.Views.Items
{
    public partial class CreateEditPage : PhoneApplicationPage
    {
        private Group _parentGroup;
        public Group ParentGroup
        {
            get { return _parentGroup; }
            set { _parentGroup = value; }
        }

        private Item _item;
        private Item Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                this.DataContext = value;
            }
        }

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

        public CreateEditPage()
        {
            InitializeComponent();

            var okCancel = new Common.OkCancelApplicationBar(Ok, Cancel);
            ApplicationBar = okCancel.ApplicationBar;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationBar.Enable();
            base.OnNavigatedTo(e);

            Mode = (Mode)Int32.Parse(NavigationContext.QueryString["mode"]);
            if (Mode == Mode.Create)
            {
                var parentId = Int32.Parse(NavigationContext.QueryString["groupId"]);
                ParentGroup = Group.FindWithId(parentId);

                Item = Item.New(ParentGroup);
            }
            else
            {
                var id = Int32.Parse(NavigationContext.QueryString["id"]);
                Item = Item.FindWithId(id);
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            Item.Reload();
        }

        private void Cancel(object sender, EventArgs e)
        {
            ApplicationBar.Disable();
            Item.Reload();
            NavigationService.TryGoBack();
        }

        private void Ok(object sender, EventArgs e)
        {
            ApplicationBar.Disable();
            Item.Save();
            
            if (Mode == Mode.Create)
            {
                ParentGroup.MergeIntoThis(Item);
            }

            NavigationService.TryGoBack();
        }
    }
}