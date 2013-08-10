using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Common;
using Tasks.Database;
using Tasks.ViewModels;

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

        public string UppercaseTitle
        {
            get { return _title.ToUpper(); }
        }

        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
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
                return String.Format("[{0} items]", Items.Count);
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

        private ObservableCollection<Group> _groups;
        public ObservableCollection<Group> Groups
        {
            get 
            {
                if (_groups == null)
                {
                    _groups = new ObservableCollection<Group>();
                    ReloadGroups();
                }
                return _groups; 
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

        private ObservableCollection<Item> _filteredItems;
        public ObservableCollection<Item> FilteredItems
        {
            get
            {
                if (_items == null)
                {
                    _items = new ObservableCollection<Item>();
                    ReloadFilteredItems();
                }

                return _items;
            }
        }

        public void Reload()
        {
            App.Database.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, this);
            ReloadItems();
            ReloadGroups();
        }

        private IQueryable<Item> ItemsForGroup()
        {
            var lst = new List<GroupItemJoinTable>(from entry in App.Database.GroupItemJoins
                                                   select entry);

            var query = from entry in App.Database.GroupItemJoins
                        where entry._groupId == Id
                        select Item.FindWithId(entry._itemId);
            return query;
        }

        private void ReloadItems()
        {
            var query = ItemsForGroup();

            if (_items == null)
                _items = new ObservableCollection<Item>();

            _items.Clear();
            foreach (var item in query) _items.Add(item);
        }

        private void ReloadGroups()
        {
            var query = (from item in Items
                         where item._sourceId != Id
                         select Group.FindWithId(item._sourceId)).Distinct();

            if (_groups == null)
                _groups = new ObservableCollection<Group>();

            _groups.Clear();
            foreach (var group in query) _groups.Add(group);
        }

        private void ReloadFilteredItems()
        {
            var query = ItemsForGroup();

            if (_filteredItems == null)
                _filteredItems = new ObservableCollection<Item>();

            _filteredItems.Clear();
            foreach (var item in query) _filteredItems.Add(item);
        }

        public bool Exists()
        {
            return this.Equals(Group.FindWithIdOrDefault(Id));
        }

        public void Save()
        {
            if (!this.Exists())
            {
                App.Database.Groups.InsertOnSubmit(this);
            }
            App.Database.SubmitChanges();
            OnPropertyChanged();
        }

        public void Delete()
        {
            if (this.Exists())
            {
                foreach (var item in new List<Item>(Items))
                {
                    this.DeleteItem(item);
                }
                this.IsDeleted = true;
            }
        }

        public void DeleteAndSubmit()
        {
            this.Delete();
            this.Save();
        }

        public void CreateItem(string Title = "", string Description = "")
        {
            if (!this.Exists()) throw new InvalidOperationException("Cannot add an Item to a Group without a valid database id.");

            var item = Item.New(this, Title, Description);
            this.MergeIntoThisAndSubmit(item);
        }

        public void MergeIntoThis(Item Item)
        {
            if (!this.Exists()) throw new InvalidOperationException("Cannot add an Item to a Group without a valid database id.");

            Item = Item.CreateClone();
            var entry = new GroupItemJoinTable() { _groupId = Id, _itemId = Item.Id };
            Items.Add(Item);

            App.Database.GroupItemJoins.InsertOnSubmit(entry);
        }

        public void MergeIntoThisAndSubmit(Item Item)
        {
            this.MergeIntoThis(Item);
            this.Save();
        }

        public void MergeIntoThis(Group Group)
        {
            _groups.Add(Group);
            foreach (var item in Group.Items)
            {
                var clone = item.NewClone();
                clone.Source = Group;
                this.MergeIntoThis(clone);
            }
            OnPropertyChanged();
        }

        public void MergeIntoThisAndSubmit(Group Group)
        {
            this.MergeIntoThis(Group);
            this.Save();
        }

        public void DeleteItem(Item Item)
        {
            if (!this.Exists()) throw new InvalidOperationException("Cannot add an Item to a Group without a valid database id.");

            var entries = from tuple in App.Database.GroupItemJoins
                          where tuple._groupId == Id && tuple._itemId == Item.Id
                          select tuple;

            foreach (var entry in entries)
            {
                this.Items.Remove(Item.FindWithId(entry._itemId));
                App.Database.GroupItemJoins.DeleteOnSubmit(entry);
            }

            Item.Delete();
        }

        public void DeleteItemAndSubmit(Item Item)
        {
            this.DeleteItem(Item);
            this.Save();
        }
    }
}
