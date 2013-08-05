using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Common;

namespace Tasks.Database
{
    public class DbManager : DataContext
    {
        private const string _connStringPrefix = "isostore:/";
        private const string _maindbFileName = "main_db.sdf";

        public Table<Item> Items;
        public Table<Group> Groups;
        public Table<GroupItemJoinTable> GroupItemJoins;

        public DbManager()
            : base(_connStringPrefix + _maindbFileName) { }

        public static DbManager EnsureCreated()
        {
            var db = new DbManager();
            if (!db.DatabaseExists())
            {
                db.CreateDatabase();
            }
            return db;
        }

        public void MakeFixtures()
        {
            App.Database.Clear();

            var inboxItems = new List<Item>() 
            {
                Item.Create("Inbox.1", "Description1"),
                Item.Create("Inbox.2", "Description2"),
                Item.Create("Inbox.3", "Description3"),
                Item.Create("Inbox.4", "Description4")
            };
            foreach (var item in inboxItems) Group.Inbox.AddItem(item);

            var group1Items = new List<Item>()
            {
                Item.Create("Group1.1", "Description1"),
                Item.Create("Group1.2", "Description2"),
                Item.Create("Group1.3", "Description3"),
                Item.Create("Group1.4", "Description4"),
                Item.Create("Group1.5", "Description4")
            };
            var group1 = Group.Create("Group1", "This is a brief description of the first group", group1Items);

            var group2Items = new List<Item>()
            {
                Item.Create("Group2.1", "Description1"),
                Item.Create("Group2.2", "Description2"),
                Item.Create("Group2.3", "Description3")
            };
            var group2 = Group.Create("Group2", "This is a brief description of the second group", group2Items);
        }

        public void Clear()
        {
            IsolatedStorageSettings.ApplicationSettings.Clear();
            IsolatedStorageSettings.ApplicationSettings.Save();

            Items.DeleteAllOnSubmit(Items);
            Groups.DeleteAllOnSubmit(Groups);            
            GroupItemJoins.DeleteAllOnSubmit(GroupItemJoins);
            SubmitChanges();
        }
    }
}
