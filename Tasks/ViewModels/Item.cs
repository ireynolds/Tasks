using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Common;

namespace Tasks
{
    public partial class Item : BindableBase
    {
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public void Reload()
        {
            if (this.Exists())
            {
                App.Database.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, this);
            }
        }

        public bool Exists()
        {
            return this.Equals(Item.FindWithIdOrDefault(Id));
        }

        public void Save()
        {
            if (!this.Exists())
            {
                this.Insert();
            }
            else
            {
                App.Database.SubmitChanges();
            }
        }

        public void Delete()
        {
            if (this.Exists())
            {
                App.Database.Items.DeleteOnSubmit(this);
                App.Database.SubmitChanges();
            }
        }

        private void Insert()
        {
            App.Database.Items.InsertOnSubmit(this);
            App.Database.SubmitChanges();
        }
    }
}
