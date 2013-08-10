using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using Tasks.Common;
using System.Collections.ObjectModel;

namespace Tasks.ViewModels
{
    public abstract class DbItem<T> : BindableBase where T : class
    {
        public static Table<T> All;

        protected void SubmitChanges() 
        {
            App.Database.SubmitChanges();
            OnPropertyChanged();
        }

        public abstract void Destroy();

        public void DestroyNow()
        {
            this.Destroy();
            SubmitChanges();
        }

        public abstract void Insert();

        public void InsertNow()
        {
            this.Insert();
            SubmitChanges();
        }

        public abstract bool Exists();

        public abstract void Reload();

        protected void ReloadCollection<E>(ref ObservableCollection<E> Collection, IEnumerable<E> Elements)
        {
            if (Collection == null)
                Collection = new ObservableCollection<E>();

            foreach (var element in Elements)
                Collection.Add(element);
        }
    }
}
