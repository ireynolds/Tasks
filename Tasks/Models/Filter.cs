using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    public partial class Filter
    {
        private bool _isShowingActive;
        private bool _isShowingDone;
        private bool _isShowingOnHold;

        public Filter()
        {
            this.Reload();
        }

        public static void EnsureExisting()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("IsShowingActive"))
            {
                IsolatedStorageSettings.ApplicationSettings["IsShowingActive"] = true;
                IsolatedStorageSettings.ApplicationSettings["IsShowingDone"] = false;
                IsolatedStorageSettings.ApplicationSettings["IsShowingOnHold"] = false;
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public void Save()
        {
            IsolatedStorageSettings.ApplicationSettings["IsShowingActive"] = IsShowingActive;
            IsolatedStorageSettings.ApplicationSettings["IsShowingDone"] = IsShowingDone;
            IsolatedStorageSettings.ApplicationSettings["IsShowingOnHold"] = IsShowingOnHold;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public void Reload()
        {
            IsShowingActive = (bool)IsolatedStorageSettings.ApplicationSettings["IsShowingActive"];
            IsShowingDone = (bool)IsolatedStorageSettings.ApplicationSettings["IsShowingDone"];
            IsShowingOnHold = (bool)IsolatedStorageSettings.ApplicationSettings["IsShowingOnHold"];

            OnPropertyChanged();
        }
    }
}
