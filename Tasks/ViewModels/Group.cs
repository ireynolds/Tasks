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
    public partial class Group : DbItem<Group>
    {
        #region Fundamental Properties

        /* ------------------------------------------------------------- */
        // Fundamental properties

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

        #endregion

        #region Computed Properties

        /* ------------------------------------------------------------- */
        // Computed Properties

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



        #region Collections

        /* ------------------------------------------------------------- */
        // Collections

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
                    var elements = (from item in Items
                                    where item._sourceId != Id
                                    select Group.FindById(item._sourceId)).Distinct();
                    ReloadCollection(ref _groups, elements);
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
                    var elements = from entry in App.Database.GroupItemJoins
                                   where entry._groupId == Id
                                   select Item.FindById(entry._itemId);
                    ReloadCollection(ref _items, elements);
                }

                return _items;
            }
        }

        private ObservableCollection<Item> _filteredItems;
        public ObservableCollection<Item> FilteredItems
        {
            get
            {
                if (_filteredItems == null)
                {
                    var all = Items;

                    var statuses = new Filter().ShownStatusesAsInt;
                    var elements = from entry in Items
                                   where statuses.Contains(entry._status)
                                   select entry;
                    ReloadCollection(ref _filteredItems, elements);
                }

                return _filteredItems;
            }
        }

        private ObservableCollection<GroupItemJoinTable> _groupItems;
        public ObservableCollection<GroupItemJoinTable> GroupItems
        {
            get
            {
                if (_groupItems == null)
                {
                    var elements = from tuple in App.Database.GroupItemJoins
                                   where tuple._groupId == Id
                                   select tuple;
                    ReloadCollection(ref _groupItems, elements);
                }

                return _groupItems;
            }
        }

        #endregion
        #endregion

        #region CRUD for Items

        /* ------------------------------------------------------------- */
        // CRUD for Items

        /// <summary>
        /// Creates a new Item as a member of this and saves it to the database.
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Description"></param>
        public void CreateItem(string Title = "", string Description = "")
        {
            if (!this.Exists()) throw new InvalidOperationException("Cannot add an Item to a Group without a valid database id.");

            var item = Item.Create(this, Title, Description);
            this.MergeIntoThisNow(item);
        }

        /// <summary>
        /// Deletes Item from this and from the database.
        /// </summary>
        /// <param name="Item"></param>
        public void DeleteItem(Item Item)
        {
            if (!this.Exists()) throw new InvalidOperationException("Cannot add an Item to a Group without a valid database id.");

            Items.Remove(Item);
            Item.Destroy();

            var groupItem = (from entry in GroupItems
                             where entry._itemId == Item.Id
                             select entry).First();

            GroupItems.Remove(groupItem);
            App.Database.GroupItemJoins.DeleteOnSubmit(groupItem);
        }

        /// <summary>
        /// Deletes Item from this and saves to the database.
        /// </summary>
        /// <param name="Item"></param>
        public void DeleteItemNow(Item Item)
        {
            this.DeleteItem(Item);
            SubmitChanges();
        }

        /// <summary>
        /// Adds Item to this. No copies are made.
        /// </summary>
        /// <param name="Item"></param>
        private void MergeIntoThis(Item Item)
        {
            if (!this.Exists()) throw new InvalidOperationException("Cannot add an Item to a Group without a valid database id.");

            Items.Add(Item);

            var entry = new GroupItemJoinTable() { _groupId = Id, _itemId = Item.Id };

            GroupItems.Add(entry);
            App.Database.GroupItemJoins.InsertOnSubmit(entry);
        }

        /// <summary>
        /// Adds Item to this and saves to the database. No copies are made.
        /// </summary>
        /// <param name="Item"></param>
        public void MergeIntoThisNow(Item Item)
        {
            this.MergeIntoThis(Item);
            SubmitChanges();
        }

        /// <summary>
        /// Adds to this a copy of each item in Group.
        /// </summary>
        /// <param name="Group"></param>
        public void MergeIntoThis(Group Group)
        {
            Groups.Add(Group);
            
            var clones = new List<Item>();
            foreach (var item in Group.Items)
            {
                var clone = item.BuildClone();

                // This ensures that if Item1 was added to Group2 as part of Group1, 
                // and then Group2 was added to Inbox, that Item1 in inbox is shown
                // as coming from Group2 (and not Group1).
                clone.Source = Group;

                clones.Add(clone);
                Item.All.InsertOnSubmit(clone);
            }

            // This ensures that all the clones have valid IDs
            SubmitChanges();

            foreach (var clone in clones)
            {
                this.MergeIntoThis(clone);
            }

            SubmitChanges();
        }

        /// <summary>
        /// Adds to this a copy of each item in Group, and saves all changes to the database.
        /// </summary>
        /// <param name="Group"></param>
        public void MergeIntoThisNow(Group Group)
        {
            this.MergeIntoThis(Group);
            SubmitChanges();
        }

        #endregion
    }
}
