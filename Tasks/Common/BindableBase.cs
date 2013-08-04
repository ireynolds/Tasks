using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Common
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T Property, T value, string PropertyName = null) 
        {
            if (object.Equals(Property, value)) return;

            Property = value;
            OnPropertyChanged(PropertyName);
        }

        protected void OnPropertyChanged(string PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
    }
}
