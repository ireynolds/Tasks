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

            Group.Inbox.CreateItem("Inbox.1", "Description1");
            Group.Inbox.CreateItem("Inbox.2", "Description2");
            Group.Inbox.CreateItem("Inbox.3", "Description3");
            Group.Inbox.CreateItem("Inbox.4", "Description4");

            var group1 = Group.Create("Group1", "This is a brief description of the first group");
            group1.CreateItem("Group1.1", "Description1");
            group1.CreateItem("Group1.2", "Description2");
            group1.CreateItem("Group1.3", "Description3");
            group1.CreateItem("Group1.4", "Description4");
            group1.CreateItem("Group1.5", "Description4");

            var group2 = Group.Create("Group2", "This is a brief description of the second group");
            group2.CreateItem("Group2.1", "Description1");
            group2.CreateItem("Group2.2", "Description2");
            group2.CreateItem("Group2.3", "Description3");            
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
