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

        private ApplicationBarMenuItem CompleteMenuItem;
        private ApplicationBarMenuItem ActivateMenuItem;
        private ApplicationBarMenuItem PutOnHoldMenuItem;

        public DetailsPage()
        {
            InitializeComponent();

            CompleteMenuItem = ApplicationBar.MenuItems[0] as ApplicationBarMenuItem;
            ActivateMenuItem = ApplicationBar.MenuItems[1] as ApplicationBarMenuItem;
            PutOnHoldMenuItem = ApplicationBar.MenuItems[2] as ApplicationBarMenuItem;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var id = Int32.Parse(NavigationContext.QueryString["id"]);
            Item = Item.FindById(id);
            Item.Reload();

            if (Item.Status == Status.Active)
                ApplicationBar.MenuItems.Remove(ActivateMenuItem);
            else if (Item.Status == Status.Complete)
                ApplicationBar.MenuItems.Remove(CompleteMenuItem);
            else
                ApplicationBar.MenuItems.Remove(PutOnHoldMenuItem);
        }

        private void Edit(object sender, EventArgs e)
        {
            NavigationService.OpenEditItem(Item.Id);
        }

        private void Delete(object sender, EventArgs e)
        {
            Utils.Confirm("Delete item?", "This item will be deleted from your phone.", () =>
            {
                Item.DestroyNow();
                NavigationService.TryGoBack();
            }, "delete", "cancel");
        }

        private void Complete(object sender, EventArgs e)
        {
            Item.Status = Status.Complete;
            NavigationService.TryGoBack();
        }

        private void Activate(object sender, EventArgs e)
        {
            Item.Status = Status.Active;
            NavigationService.TryGoBack();
        }

        private void OnHold(object sender, EventArgs e)
        {
            Item.Status = Status.OnHold;
            NavigationService.TryGoBack();
        }
    }
}