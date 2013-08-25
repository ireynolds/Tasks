using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Common;
using Tasks.ViewModels;

namespace Tasks.Database
{
    public class DbManager : DataContext
    {
        private const string _connStringPrefix = "isostore:/";
        private const string _maindbFileName = "main_db.sdf";

        public Table<Item> Items;
        public Table<Group> Groups;

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
            App.Database.ClearAllStores();

            Filter.EnsureExisting();

            Group.Inbox.CreateItem("Buy birthday present", "Pillow pet?");
            Group.Inbox.CreateItem("Technical spec for scheduler", "");
            Group.Inbox.CreateItem("Cake from ice cream store", "Peanut butter chocolate");
            Group.Inbox.CreateItem("Scavenger hunt", "");

            var group1 = Group.Create("Burgers");
            group1.CreateItem("Burger (3 lb)", "Mom: 85/15 %");
            group1.CreateItem("Buns", "");
            group1.CreateItem("mustard", "");
            group1.CreateItem("ketchup", "");
            group1.CreateItem("onion", "");
            group1.CreateItem("pickle", "");
            group1.CreateItem("lettuce", "shredded");

            var group2 = Group.Create("Burritos");
            group2.CreateItem("Tortillas", "10 ct, medium");
            group2.CreateItem("Refried beans", "jalapeno");
            group2.CreateItem("Enchilada sauce", "");
            group2.CreateItem("burger", "2 pounds");
            group2.CreateItem("cheddar cheese", "do we have enough?");
        }

        public void ClearAllStores()
        {
            IsolatedStorageSettings.ApplicationSettings.Clear();
            IsolatedStorageSettings.ApplicationSettings.Save();

            Items.DeleteAllOnSubmit(Items);
            Groups.DeleteAllOnSubmit(Groups);            
            SubmitChanges();
        }
    }
}
