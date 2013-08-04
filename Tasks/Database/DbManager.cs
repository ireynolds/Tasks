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

            var items1 = new List<Item>() 
            {
                Item.Create("Task1f", "Description1"),
                Item.Create("Task2f", "Description2"),
                Item.Create("Task3f", "Description3"),
                Item.Create("Task4f", "Description4")
            };
            foreach (var item in items1) Group.Inbox.AddItem(item);
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
