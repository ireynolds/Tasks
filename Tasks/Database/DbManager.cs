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

        public string[] bigTitles = { "vel scelerisque", "aliquam quam", "facilisis", "Cras nec venenatis", "et", "vel", "felis sed", "libero at", "arcu", "eu vehicula lectus", "dui", "aliquet Curabitur", "facilisis", "non enim porttitor", "at pellentesque ultricies", "ante sed", "commodo", "nibh mollis", "dolor sit", "Sed dignissim nisl", "ut velit", "nisl fermentum gravida", "vehicula congue sem", "dignissim Praesent bibendum", "metus et ante", "Donec", "ipsum dolor", "porttitor", "ornare lacus quis", "ut pulvinar tempus", "at mattis", "et malesuada", "at metus varius", "pharetra eleifend nisi", "id faucibus", "mollis mi", "feugiat dapibus", "interdum ligula", "vel sapien id", "libero", "est nec metus", "in", "posuere risus", "Maecenas ullamcorper", "lacinia nisl porttitor", "Lorem", "Vivamus quis malesuada", "dui nunc", "bibendum In lobortis", "placerat leo", "molestie Cras neque", "eu", "orci non", "odio", "eros euismod a", "nisi lacinia", "tristique", "luctus", "vitae justo id", "laoreet mauris", "fringilla augue Proin", "et", "ac", "nec vestibulum Praesent", "amet", "sit", "lobortis mi", "odio a", "amet consectetur", "sodales", "quis mollis", "felis Suspendisse", "accumsan Quisque diam", "et malesuada fames", "vestibulum neque", "Nullam quam", "pharetra", "Integer sed ante", "Sed", "ut velit", "odio vehicula", "at", "tincidunt", "Proin vulputate", "suscipit tellus", "elementum", "amet", "dapibus", "porttitor nulla ac", "tempus", "dolor Sed", "Sed id sapien", "a cursus nibh", "metus ut", "et molestie", "id faucibus", "nec", "posuere enim Proin", "tempor augue nec", "tempor neque posuere", "pharetra", "feugiat Mauris", "ipsum porttitor", "dapibus", "enim", "elit ullamcorper at", "id aliquet", "auctor mattis Curabitur", "vitae", "nulla diam mattis", "Fusce", "orci diam", "consectetur adipiscing", "ut nisi malesuada", "nec egestas", "eu", "Nulla at", "at bibendum", "malesuada augue Donec", "sagittis justo nulla", "nisl", "in Morbi", "sit amet", "Mauris luctus magna", "ac", "porta Integer", "aliquam Fusce vel", "mollis sapien", "lobortis justo", "laoreet", "dui quam rhoncus", "consectetur orci nec", "arcu", "augue Maecenas ultricies", "magna", "Vivamus", "at", "ipsum sit", "accumsan odio", "id hendrerit odio", "augue bibendum consequat", "ante tempor", "ligula Morbi", "velit felis a", "ultrices", "vestibulum", "molestie Donec", "fames", "ante Cras", "nunc", "fermentum lorem", "eget", "feugiat magna", "sed", "et", "volutpat", "Fusce venenatis lacus", "nunc", "non", "in", "urna Etiam", "porta justo Cras", "suscipit", "mollis non orci", "eget", "lorem Duis orci", "auctor Integer", "adipiscing", "quam In", "lobortis", "Lorem ipsum", "ante aliquet", "eu nec", "justo egestas bibendum", "a", "elit Proin aliquet", "nisl faucibus", "et pulvinar ac", "in", "dui Nunc et", "sagittis", "Pellentesque dapibus", "Nullam porta odio", "ullamcorper dolor Vestibulum", "dui", "a", "nisi turpis semper", "dui", "at condimentum et", "amet", "quis leo non", "adipiscing", "Maecenas bibendum", "ipsum", "et tristique magna", "ut ornare", "morbi tristique", "hendrerit", "sed", "aliquam Nulla facilisi", "velit vitae neque", "hendrerit viverra", "ullamcorper facilisis", "ultricies", "eget tempus", "In", "mauris gravida", "posuere est", "ante", "vitae tempus", "Proin aliquet", "sed massa", "Donec", "lacus In", "tincidunt", "pretium interdum", "lacus adipiscing tortor", "erat", "amet tortor sed", "molestie urna nec", "ligula congue", "amet", "Proin", "blandit", "tristique nisl a", "aliquam", "Phasellus sed", "Vivamus ultricies", "ornare", "eu eleifend mollis", "elit vel", "vehicula velit", "Morbi", "senectus et netus", "lacinia lobortis", "augue", "quam tincidunt", "lorem", "tempus libero consectetur", "magna Curabitur porttitor", "Pellentesque quis sem", "Praesent gravida", "imperdiet", "adipiscing", "orci eget consectetur", "sagittis", "Mauris ipsum justo", "diam quis fringilla", "congue Mauris neque", "purus", "metus", "est", "ante", "turpis egestas", "turpis tellus Donec", "risus", "mi", "ipsum", "turpis egestas Donec", "consequat nibh", "a faucibus sed", "egestas", "congue", "Aenean a magna", "Aliquam consequat lorem", "a", "bibendum nisl suscipit", "Morbi id condimentum", "nec augue", "tempus rutrum", "sodales Maecenas", "quis porttitor", "fringilla in ut", "diam dignissim", "hac habitasse platea", "lectus", "Mauris cursus tincidunt", "pharetra Pellentesque", "risus commodo", "a porttitor Quisque", "blandit id", "Pellentesque habitant", "dui vulputate in", "mattis", "justo", "a magna Aenean", "Pellentesque habitant", "sagittis Cras enim", "in", "libero eget posuere", "nunc", "sodales laoreet", "adipiscing elit", "ligula pellentesque vestibulum", "nec risus", "sit", "pulvinar dui", "aliquam volutpat dolor", "sapien", "elit fringilla id", "ac", "semper quis", "feugiat", "dictumst Curabitur", "ultricies Aliquam", "sit amet", "nulla", "ac", "Aliquam tincidunt", "suscipit", "amet nec", "ipsum", "urna Donec", "malesuada nec semper", "vehicula Vestibulum", "mattis", "at accumsan enim", "sed hendrerit quam", "Quisque pretium", "Nulla", "Quisque imperdiet", "Pellentesque", "dignissim Fusce egestas", "feugiat eros", "In", "sit", "quam quam vehicula", "quis purus tincidunt", "eget massa", "Integer hendrerit accumsan", "ultricies", "in metus auctor", "tincidunt", "quis tristique", "a dui", "ut massa at", "suscipit Nulla scelerisque", "Praesent", "vehicula mollis nibh", "ultricies sem dignissim", "quis magna sodales", "porttitor", "scelerisque Vestibulum", "sem dolor", "rhoncus", "orci egestas", "in ultricies", "Mauris pretium eros", "adipiscing", "et Ut", "nulla pretium", "justo Vestibulum purus", "commodo malesuada", "ut", "libero", "Integer", "Proin quis", "mattis", "senectus et netus", "pharetra eros Cras", "tincidunt", "scelerisque ligula ac", "urna Nam", "orci auctor", "at dolor", "suscipit mollis", "in viverra justo", "tempor Pellentesque dignissim", "id Donec", "turpis Nunc sed", "fringilla lacus lectus", "quam et leo", "justo", "at Praesent", "posuere eget nunc", "bibendum", "imperdiet et", "sit", "dignissim", "ac", "rhoncus vulputate", "a", "in enim pulvinar", "cursus", "vel", "Suspendisse", "mattis scelerisque Sed", "in", "fringilla diam", "adipiscing nisi Donec", "lacus eu mollis", "quam volutpat et", "Sed sed", "id Suspendisse consequat", "morbi", "dapibus nulla", "tincidunt ipsum", "id fermentum at", "vitae", "vel", "id vestibulum urna", "id", "Nulla facilisi", "sed id", "erat", "posuere", "vehicula mauris nisi", "tristique", "magna dapibus", "tincidunt Mauris", "egestas", "a", "orci Curabitur", "vitae", "pulvinar", "adipiscing vel", "feugiat lorem lobortis", "ac pellentesque", "rutrum", "condimentum", "mattis", "id mauris Quisque", "Aliquam porttitor urna", "ut", "sem id", "tellus", "et elementum", "scelerisque faucibus", "in suscipit erat", "diam", "augue", "Pellentesque ac felis", "sit amet", "hendrerit", "Sed", "aliquam Nam eleifend", "dui", "non", "facilisi Quisque", "lobortis", "tempor", "sit", "eros cursus dolor", "eget", "a", "Donec accumsan", "iaculis", "sit amet libero", "sed", "sed", "ligula quam", "arcu consequat", "turpis", "scelerisque tellus sollicitudin", "nibh Nulla", "metus dolor", "Phasellus", "Pellentesque", "justo Ut", "velit ac", "sit amet", "vestibulum elit", "interdum", "lacus", "ultrices eleifend", "id" };
        public string[] bigDescriptions = { "id tempor", "dapibus molestie Donec vehicula mauris nisi eu vehicula lectus semper quis Sed", "lacinia nisl porttitor in Morbi tempus libero consectetur nibh mollis adipiscing", "mattis a faucibus sed erat Morbi id condimentum dui Quisque pretium", "elit vel feugiat Mauris ac metus dolor Vivamus quis malesuada nisl", "egestas congue Mauris neque eros euismod a", "Lorem ipsum dolor sit amet consectetur adipiscing elit Proin aliquet iaculis", "a urna", "tortor suscipit mollis Suspendisse fringilla", "Cras vitae justo", "dignissim magna dapibus at Praesent a ultricies odio a pulvinar dui", "Sed sed porta justo Cras sed massa mi Pellentesque imperdiet volutpat molestie", "enim pulvinar molestie urna", "felis sed sagittis Integer", "ultricies auctor mattis Curabitur et elementum", "purus elit fringilla id libero eget posuere dapibus nulla Proin vulputate hendrerit", "metus ut porttitor ante tempor et Ut mollis sapien eu", "purus Nulla facilisi Proin quis ligula pellentesque vestibulum velit", "Cras neque orci egestas ac pellentesque in", "ac aliquet Curabitur bibendum nisl suscipit quam tincidunt tempor Pellentesque dignissim tempor augue", "accumsan ipsum non dui vulputate in tempor neque posuere", "orci eget consectetur suscipit quam quam vehicula lacus id vestibulum urna quam et", "consequat lorem ut pulvinar tempus augue arcu consequat arcu in ultricies velit felis", "Morbi nisi turpis", "enim nulla pretium accumsan odio eget tincidunt posuere", "libero", "leo Praesent at accumsan enim vel ultricies orci Curabitur id hendrerit odio Cras nec venenatis lorem", "dolor sit", "lacus ac ipsum porttitor eget tempus risus commodo Vivamus rhoncus aliquam quam ut scelerisque tellus sollicitudin", "habitasse platea dictumst Curabitur vehicula congue sem quis", "dolor ullamcorper facilisis Mauris ipsum justo malesuada nec", "scelerisque Sed a fringilla diam et malesuada augue Donec tempus rutrum scelerisque", "quam facilisis id Donec sit amet libero ut", "Quisque blandit id sem id suscipit Nulla scelerisque diam quis fringilla sagittis justo nulla", "semper et pulvinar ac odio Mauris", "nec posuere est Nullam porta odio quis purus tincidunt ultricies Aliquam vehicula", "porttitor urna at mattis condimentum Nullam quam diam dignissim at condimentum et lobortis ornare", "quam sed hendrerit", "Maecenas ullamcorper dui id aliquet porta Integer in", "suscipit diam nec feugiat eros velit vitae neque", "Proin magna ante aliquet non enim porttitor ultrices eleifend dui Aliquam", "porttitor Aliquam tincidunt sit amet tortor sed", "in metus auctor quis tristique ipsum tincidunt Mauris", "Suspendisse", "magna", "rutrum", "ac consequat nibh Mauris luctus", "laoreet nisl faucibus Pellentesque in suscipit erat sit", "Quisque nulla diam mattis sit amet mattis vitae tempus a lacus In interdum ligula nec", "augue Fusce venenatis", "lacus lectus Quisque imperdiet hendrerit viverra Phasellus sit amet fringilla augue Proin quis magna sodales egestas", "aliquam Nam eleifend lectus feugiat lorem lobortis vel scelerisque odio vehicula Vivamus ultricies suscipit tellus", "Sed lorem elit ullamcorper at porttitor in feugiat", "Sed", "ante at metus varius dignissim Praesent bibendum eget massa in elementum Proin feugiat", "nisl a quam volutpat et mollis mi dapibus Donec et tristique magna vitae feugiat", "vehicula velit ante", "mollis nibh eu sodales Fusce quis mollis felis", "enim", "id faucibus aliquam Fusce vel nulla pharetra eleifend nisi sed", "Etiam ornare lacus quis nunc dapibus fringilla in ut nunc Donec", "Vestibulum quis leo non orci auctor congue eu nec quam In ut placerat leo", "amet consectetur adipiscing elit", "cursus tincidunt justo vel ultricies sem dignissim id Suspendisse consequat porttitor nulla", "scelerisque ligula", "nec bibendum In lobortis tincidunt ipsum nec vestibulum Praesent sodales laoreet sapien id faucibus Integer hendrerit", "et netus et malesuada", "id nisl fermentum gravida sed id justo Ut posuere eget nunc", "egestas et molestie nec risus Integer sed", "adipiscing", "fames ac turpis egestas Phasellus sed metus et ante bibendum dignissim Fusce egestas lobortis justo vitae", "fermentum at magna Curabitur porttitor commodo", "tristique senectus et netus et malesuada fames ac turpis egestas Donec tincidunt interdum augue a porttitor", "semper vitae metus sed aliquam volutpat dolor Donec fermentum lorem a dui luctus non laoreet mauris", "ac facilisis libero ante nec", "posuere ipsum eget blandit lobortis mi", "orci non ullamcorper dolor Vestibulum adipiscing mattis", "et risus turpis Nunc sed adipiscing nisi Donec vestibulum neque hendrerit consectetur orci nec pharetra eros", "tincidunt ante at", "accumsan accumsan Quisque diam", "mattis Pellentesque ac felis sit amet mauris gravida adipiscing vel mattis nibh", "a lacus", "nisi malesuada imperdiet et", "velit commodo malesuada Pellentesque dapibus", "Duis orci ligula congue vel rhoncus vulputate tempus a magna Aenean pretium interdum sagittis Cras", "amet vestibulum elit Sed id sapien sit amet tellus lacinia lobortis et ac dui Lorem ipsum", "Nulla", "cursus dui Nunc", "consequat augue Maecenas", "urna Nam dui nunc aliquam at bibendum id", "Mauris pretium eros ut ornare auctor Integer adipiscing ante sed nisi lacinia vehicula Vestibulum tristique", "nunc tincidunt sed turpis at pellentesque ultricies justo Vestibulum", "eleifend mollis Praesent gravida in arcu at pulvinar Nulla facilisi", "at justo In a turpis tellus", "vestibulum libero at aliquam Nulla facilisi Sed dignissim nisl", "Nulla at posuere risus in viverra justo Pellentesque", "ligula", "diam id mauris Quisque et est ligula Morbi id tristique augue bibendum", "pharetra Pellentesque habitant morbi tristique senectus", "a sodales Maecenas a", "magna Maecenas bibendum lacus eu mollis adipiscing erat eros cursus dolor sit amet ultrices orci", "Cras ut massa at justo egestas bibendum in pharetra", "ut velit", "Donec eget sem dolor Pellentesque habitant morbi", "lobortis vel sapien id pharetra Pellentesque dui quam rhoncus sed", "mollis non orci In hac", "cursus nibh Aliquam", "urna Donec eu est nec metus scelerisque faucibus sit amet nec enim Aenean a magna dolor", "Proin aliquet sagittis ipsum sit amet", "quis sem ut" };
        public int[] bigGroupSizes = { 23, 77, 66, 93, 18, 45, 72, 60, 77, 22, 61, 36, 87, 23, 58, 55, 15, 85, 76, 11, 22, 33, 35, 12, 29, 97, 79, 89, 43, 86, 77, 29, 57, 79, 43, 42, 7, 79, 15, 77, 27, 10, 92, 5, 50, 76, 38, 97, 70, 90, 33, 50, 39, 82, 16, 61, 53, 7, 25, 80, 73, 8, 87, 89, 33, 4, 63, 11, 17, 66, 59, 13, 57, 16, 93, 9, 11, 0, 12, 36, 0, 48, 4, 20, 69, 79, 12, 42, 58, 31, 29, 20, 68, 27, 42, 50, 50, 30, 17, 31, };

        public void MakeBigFixtures()
        {
            App.Database.ClearAllStores();

            Filter.EnsureExisting();

            // Fill the inbox
            for (var i = 0; i < 29; i++)
            {
                Group.Inbox.CreateItem(bigTitles[i], bigDescriptions[i]);
            }

            // Create a bunch of groups
            var groups = new List<Group>();
            for (var i = 0; i < bigGroupSizes.Length; i++)
            {
                var group = Group.Create(bigTitles[i]);
                groups.Add(group);

                // Add a bunch of items to each group
                for (int j = 0; j < bigGroupSizes[i]; j++)
                {
                    group.BuildItem(bigTitles[j], bigDescriptions[j]);
                }

                SubmitChanges();
            }

            // Add several groups to the 
            for (int i = 0; i < 0; i++)
            {
                Group.Inbox.MergeIntoThisNow(groups[i], groups[i].UnfilteredItems);
            }
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
