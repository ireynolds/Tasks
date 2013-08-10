using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    public partial class Item
    {
        public static IQueryable<Item> All
        {
            get { return App.Database.Items; }
        }

        public static Item Create(Group Source, string Title = "", string Description = "")
        {
            var item = Item.New(Source, Title, Description);
            item.Save();
            return item;
        }

        public static Item New(Group Source, string Title = "", string Description = "")
        {
            return new Item() { Source = Source, Title = Title, Description = Description };
        }

        public static Item FindWithId(int Id)
        {
            var items = new List<Item>(from nitem in All select nitem);

            var item = FindWithIdOrDefault(Id);
            if (item == null)
                throw new ArgumentException();
            return item;
        }

        private static Item FindWithIdOrDefault(int Id)
        {
            return (from item in All
                    where item._id == Id
                    select item).FirstOrDefault();
        }
    }
}
