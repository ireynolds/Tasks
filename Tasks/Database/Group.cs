using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks // Database
{
    public partial class Group
    {
        public static IQueryable<Group> All
        {
            get { return App.Database.Groups; }
        }

        public static IQueryable<Group> AllExceptInbox
        {
            get 
            {
                return from grp in All
                       where grp._id != Inbox.Id
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
                    _inbox = Group.FindWithId((int)IsolatedStorageSettings.ApplicationSettings["InboxId"]);
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

        public static Group Create(string Title = "", string Description = "", ICollection<Item> Items = null)
        {
            var group = Group.New(Title, Description, Items);
            group.Save();
            return group;
        }

        public static Group New(string Title = "", string Description = "", ICollection<Item> Items = null)
        {
            var group = new Group() { Title = Title, Description = Description };

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    group.AddItem(item);
                }
            }

            return group;
        }

        public static void MergeIntoInbox(Group Group)
        {
            foreach (var item in Group.Items)
            {
                Inbox.AddItem(item.Clone());
            }
        }

        public static Group FindWithId(int Id)
        {
            var group = FindWithIdOrDefault(Id);
            if (group == null)
            {
                throw new ArgumentException();
            }

            return group;
        }

        public static Group FindWithIdOrDefault(int Id)
        {
            return (from grp in All
                    where grp._id == Id
                    select grp).FirstOrDefault();
        }
    }
}
