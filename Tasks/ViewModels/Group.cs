using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Common;
using Tasks.Database;

namespace Tasks // ViewModels
{
    public partial class Group : BindableBase
    {
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public string ItemsString
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                IEnumerator<Item> iter = Items.GetEnumerator();
                if (iter.MoveNext())
                {
                    sb.Append(iter.Current.Title);
                }

                int i = 0;
                while (i < 5 && iter.MoveNext())
                {
                    sb.Append(", ");
                    sb.Append(iter.Current.Title);
                    i++;
                }

                if (iter.MoveNext())
                {
                    sb.Append(" ...");
                }

                return sb.ToString();
            }
        }

        public string CountString
        {
            get
            {
                return String.Format("({0} items)", Items.Count);
            }
        }

        public IEnumerable<IGrouping<Group, Item>> _constituents;
        public IEnumerable<IGrouping<Group, Item>> Constituents
        {
            get
            {
                if (_constituents == null)
                {
                    _constituents = Items.GroupBy<Item, Group>((item) => item.Source);
                }

                return _constituents;
            }
        }

        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ObservableCollection<Item>();
                    ReloadItems();
                }

                return _items;
            }
        }

        public void Reload()
        {
            App.Database.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, this);
            ReloadItems();
        }

        private void ReloadItems()
        {
            var query = from entry in App.Database.GroupItemJoins
                        where entry._groupId == Id
                        select Item.FindWithId(entry._itemId);

            _items.Clear();
            foreach (var item in query) _items.Add(item);
        }

        public bool Exists()
        {
            return this.Equals(Group.FindWithIdOrDefault(Id));
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
                foreach (var item in Items)
                {
                    item.Delete();
                }

                App.Database.Groups.DeleteOnSubmit(this);
                App.Database.SubmitChanges();
            }
        }

        private void Insert()
        {
            App.Database.Groups.InsertOnSubmit(this);
            App.Database.SubmitChanges();
        }

        public void CreateItem(string Title = "", string Description = "")
        {
            if (!this.Exists()) throw new InvalidOperationException("Cannot add an Item to a Group without a valid database id.");

            var item = Item.New(this, Title, Description);
            this.MergeIntoThis(item);
        }

        public void MergeIntoThis(Item Item)
        {
            if (!this.Exists()) throw new InvalidOperationException("Cannot add an Item to a Group without a valid database id.");

            Item = Item.Clone();
            var entry = new GroupItemJoinTable() { _groupId = Id, _itemId = Item.Id };
            Items.Add(Item);

            App.Database.GroupItemJoins.InsertOnSubmit(entry);
            App.Database.SubmitChanges();
        }

        public void DeleteItem(Item Item)
        {
            if (!this.Exists()) throw new InvalidOperationException("Cannot add an Item to a Group without a valid database id.");

            var entry = (from tuple in App.Database.GroupItemJoins
                        where tuple._groupId == Id && tuple._itemId == Item.Id
                        select tuple).First();
            Items.Remove(Item.FindWithId(entry._itemId));
            
            App.Database.GroupItemJoins.DeleteOnSubmit(entry);
            App.Database.SubmitChanges();

            Item.Delete();
        }
    }
}
