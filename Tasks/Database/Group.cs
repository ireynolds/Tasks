using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Database;

namespace Tasks // Database
{
    public partial class Group
    {
        #region Finders

        /* ------------------------------------------------------------- */
        // Finders

        public new static Table<Group> All
        {
            get { return App.Database.Groups; }
        }

        public static Group FindById(int Id)
        {
            var group = FindByIdOrDefault(Id);
            if (group == null)
            {
                throw new ArgumentException();
            }

            return group;
        }

        public static Group FindByIdOrDefault(int Id)
        {
            // Make sure the search includes deleted groups.
            return (from grp in App.Database.Groups
                    where grp._id == Id
                    select grp).FirstOrDefault();
        }

        public static IQueryable<Group> AllExceptInbox
        {
            get 
            {
                return from grp in All
                       where grp._id != Inbox.Id && !grp._isDeleted
                       select grp; 
            }
        }

        private static Group _inbox;
        public static Group Inbox
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("InboxId"))
                {
                    Inbox = Group.Create(Title: "inbox");
                }
                else if (_inbox == null)
                {
                    var all = new List<Group>(Group.All);
                    _inbox = Group.FindById((int)IsolatedStorageSettings.ApplicationSettings["InboxId"]);
                }

                return _inbox;
            }
            private set
            {
                _inbox = value;
                IsolatedStorageSettings.ApplicationSettings["InboxId"] = value.Id;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        #endregion

        #region CRUD

        /* ------------------------------------------------------------- */
        // CRUD

        public static Group Build(string Title = "")
        {
            return new Group() { Title = Title };
        }

        public static Group Create(string Title = "")
        {
            var group = Group.Build(Title);
            group.InsertNow();
            return group;
        }

        public override void Insert()
        {
            App.Database.Groups.InsertOnSubmit(this);
        }

        public override bool Exists()
        {
            return this.Equals(Group.FindByIdOrDefault(Id));
        }

        public override void Destroy()
        {
            if (this.Exists())
            {
                foreach (var item in new List<Item>(UnfilteredItems))
                {
                    this.DeleteItem(item);
                }
                this.IsDeleted = true;
            }
        }

        public void DestroyOnSubmit()
        {
            All.DeleteOnSubmit(this);
        }

        public void InsertOnSubmit()
        {
            All.InsertOnSubmit(this);
        }

        public override void Reload()
        {
            App.Database.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, this);

            // NOTE: For efficiency, this does not currently reload _all_ collections

            ReloadCollection(ref _unfilteredItems, GetUnfilteredItems());
            ReloadCollection(ref _filteredItems, GetFilteredItems());
            ReloadCollection(ref _groups, GetGroups());

            OnPropertyChanged();
        }

        #endregion

    }
}
