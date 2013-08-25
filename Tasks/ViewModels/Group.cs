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

        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }

        public Group Container
        {
            get { return (_containerId == -1) ? null : Group.FindById(_containerId); }
            set { SetProperty(ref _containerId, value.Id); }
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

                IEnumerator<Item> iter = FilteredItems.GetEnumerator();
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

        public string UppercaseTitle
        {
            get { return _title.ToUpper(); }
        }

        public string CountString
        {
            get
            {
                return String.Format("[{0} items]", FilteredItems.Count);
            }
        }

        #region Collections

        /* ------------------------------------------------------------- */
        // Collections

        public bool FiltersAreEnabled { get; set; }

        public ObservableCollection<Item> Items
        {
            get
            {
                return (FiltersAreEnabled) ? FilteredItems : UnfilteredItems;
            }
        }

        //public IEnumerable<IGrouping<Group, Item>> _constituents;
        //public IEnumerable<IGrouping<Group, Item>> Constituents
        //{
        //    get
        //    {
        //        if (_constituents == null)
        //        {
        //            _constituents = UnfilteredItems.GroupBy<Item, Group>((item) => item.Source);
        //        }

        //        return _constituents;
        //    }
        //}

        //private ObservableCollection<Group> _groups;
        public ObservableCollection<Group> Groups
        {
            get 
            {
                //if (_groups == null)
                //{
                //    ReloadCollection(ref _groups, GetGroups());
                //}
                //return _groups; 
                return new ObservableCollection<Group>(GetGroups());
            }
        }

        public IEnumerable<Group> GetGroups()
        {
            return (from item in FilteredItems
                    where item._sourceId != Id
                    select Group.FindById(item._sourceId)).Distinct();
        }

        //private ObservableCollection<Item> _unfilteredItems;
        public ObservableCollection<Item> UnfilteredItems
        {
            get
            {
                //if (_unfilteredItems == null)
                //{
                //    ReloadCollection(ref _unfilteredItems, GetUnfilteredItems());
                //}

                //return _unfilteredItems;
                return new ObservableCollection<Item>(GetUnfilteredItems());
            }
        }

        private IQueryable<Item> GetUnfilteredItems()
        {
            if (Id == Inbox.Id)
            {
                return from item in App.Database.Items
                       where item._containerId == Id
                       select item;
            }
            else if (_containerId == Inbox.Id)
            {
                return from item in App.Database.Items
                       where item._containerId == Inbox.Id
                          && item._sourceId == Id
                       select item;
            }
            else // _containerId == -1
            {
                return from item in App.Database.Items
                       where item._containerId == Id
                       select item;
            }
        }

        //private ObservableCollection<Item> _filteredItems;
        public ObservableCollection<Item> FilteredItems
        {
            get
            {
                //if (_filteredItems == null)
                //{
                //    ReloadCollection(ref _filteredItems, GetFilteredItems());
                //}

                //return _filteredItems;
                return new ObservableCollection<Item>(GetFilteredItems());
            }
        }

        private IEnumerable<Item> GetFilteredItems()
        {
            var statuses = new Filter().ShownStatuses;
            return from entry in UnfilteredItems
                   where statuses.Contains((Status)entry._status)
                   select entry;
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

        public void BuildItem(string Title = "", string Description = "")
        {
            if (!this.Exists()) throw new InvalidOperationException("Cannot add an Item to a Group without a valid database id.");

            var item = Item.Build(this, Title, Description);
            item.Insert();

            this.MergeIntoThis(item);
        }

        /// <summary>
        /// Deletes Item from this and from the database.
        /// </summary>
        /// <param name="Item"></param>
        public void DeleteItem(Item Item)
        {
            if (!this.Exists()) throw new InvalidOperationException("Cannot add an Item to a Group without a valid database id.");

            Item.Destroy();
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
        public void MergeIntoThis(Item Item)
        {
            if (!this.Exists()) throw new InvalidOperationException("Cannot add an Item to a Group without a valid database id.");

            Item.Container = this;

            UnfilteredItems.Add(Item);
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
        public void MergeIntoThis(Group Source, IEnumerable<Item> Items)
        {
            var groupClone = Group.Create(Source.Title);
            groupClone.Container = this;

            Groups.Add(groupClone);
           
            var clones = new List<Item>();
            foreach (var item in Items)
            {
                var clone = item.BuildClone();

                // This ensures that if Item1 was added to Group2 as part of Group1, 
                // and then Group2 was added to Inbox, that Item1 in inbox is shown
                // as coming from Group2 (and not Group1).
                clone.Source = groupClone;

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
        public void MergeIntoThisNow(Group Source, IEnumerable<Item> Items)
        {
            this.MergeIntoThis(Source, Items);
        }

        #endregion
    }
}
