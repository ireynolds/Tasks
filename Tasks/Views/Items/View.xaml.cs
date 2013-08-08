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
    public partial class DetailsPage : PhoneApplicationPage
    {
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

        public DetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var id = Int32.Parse(NavigationContext.QueryString["id"]);
            Item = Item.FindWithId(id);
        }

        private void Edit(object sender, EventArgs e)
        {
            NavigationService.OpenEditItem(Item.Id);
        }

        private void Delete(object sender, EventArgs e)
        {
            Utils.Confirm("Delete item?", "This item will be deleted from your phone.", () =>
            {
                Item.DeleteAndSave();
                NavigationService.TryGoBack();
            }, "delete", "cancel");
        }
    }
}