using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Common;

namespace Tasks.ViewModels
{
    public class Filter : BindableBase
    {
        private bool _isShowingActive;
        public bool IsShowingActive
        {
            get { return _isShowingActive; }
            set { SetProperty(ref _isShowingActive, value); }
        }

        private bool _isShowingDone;
        public bool IsShowingDone
        {
            get { return _isShowingDone; }
            set { SetProperty(ref _isShowingDone, value); }
        }

        private bool _isShowingOnHold;
        public bool IsShowingOnHold
        {
            get { return _isShowingOnHold; }
            set { SetProperty(ref _isShowingOnHold, value); }
        }

        public ISet<Status> ShownStatuses
        {
            get 
            {
                var set = new HashSet<Status>();
                if (IsShowingActive) set.Add(Status.Active);
                if (IsShowingDone)   set.Add(Status.Done);
                if (IsShowingOnHold) set.Add(Status.OnHold);
                return set;
            }
        }

        public static void EnsureExisting()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("IsShowingActive"))
            {
                IsolatedStorageSettings.ApplicationSettings["IsShowingActive"] = true;
                IsolatedStorageSettings.ApplicationSettings["IsShowingDone"] = false;
                IsolatedStorageSettings.ApplicationSettings["IsShowingOnHold"] = false;
            }
        }

        public Filter()
        {
            IsShowingActive = (bool)IsolatedStorageSettings.ApplicationSettings["IsShowingActive"];
            IsShowingDone = (bool)IsolatedStorageSettings.ApplicationSettings["IsShowingDone"];
            IsShowingOnHold = (bool)IsolatedStorageSettings.ApplicationSettings["IsShowingOnHold"];
        }

        public void Save()
        {
            IsolatedStorageSettings.ApplicationSettings["IsShowingActive"] = IsShowingActive;
            IsolatedStorageSettings.ApplicationSettings["IsShowingDone"] = IsShowingDone;
            IsolatedStorageSettings.ApplicationSettings["IsShowingOnHold"] = IsShowingOnHold;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }
    }
}
