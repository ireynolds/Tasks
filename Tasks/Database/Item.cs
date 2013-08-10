using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    public partial class Item
    {
        public new static Table<Item> All
        {
            get { return App.Database.Items; }
        }

        public Item CreateClone()
        {
            return Item.Create(Source, Title, Description);
        }

        public static Item Create(Group Source, string Title = "", string Description = "")
        {
            var item = Item.Build(Source, Title, Description);
            item.InsertNow();
            return item;
        }

        public Item BuildClone()
        {
            return Item.Build(Source, Title, Description);
        }

        public static Item Build(Group Source, string Title = "", string Description = "")
        {
            return new Item() { Source = Source, Title = Title, Description = Description };
        }

        public static Item FindById(int Id)
        {
            var items = new List<Item>(from nitem in All select nitem);

            var item = FindByIdOrDefault(Id);
            if (item == null)
                throw new ArgumentException();

            return item;
        }

        private static Item FindByIdOrDefault(int Id)
        {
            return (from item in All
                    where item._id == Id
                    select item).FirstOrDefault();
        }

        public override void Reload()
        {
            if (this.Exists())
            {
                App.Database.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, this);
            }
        }

        public override void Insert()
        {
            App.Database.Items.InsertOnSubmit(this);
        }

        public override bool Exists()
        {
            return this.Equals(Item.FindByIdOrDefault(Id));
        }

        public override void Destroy()
        {
            if (this.Exists())
            {
                App.Database.Items.DeleteOnSubmit(this);
            }
        }
    }
}
