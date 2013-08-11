using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tasks.Common;

namespace Tasks
{
    public partial class Filter : BindableBase
    {        
        public bool IsShowingActive
        {
            get { return _isShowingActive; }
            set { SetProperty(ref _isShowingActive, value); }
        }
        
        public bool IsShowingDone
        {
            get { return _isShowingDone; }
            set { SetProperty(ref _isShowingDone, value); }
        }

        public bool IsShowingOnHold
        {
            get { return _isShowingOnHold; }
            set { SetProperty(ref _isShowingOnHold, value); }
        }

        public bool AreFiltersEnabled 
        { 
            get; 
            set; 
        }

        public Visibility FiltersBlockVisibility
        {
            get
            {
                return (AreFiltersEnabled) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public List<Status> ShownStatuses
        {
            get 
            {
                var lst = new List<Status>();
                if (IsShowingDone) lst.Add(Status.Complete);
                if (IsShowingActive) lst.Add(Status.Active);
                if (IsShowingOnHold) lst.Add(Status.OnHold);
                return lst;
            }
        }

        public string ShownStatusesAsString
        {
            get
            {
                if (ShownStatuses.Count == 0)
                {
                    return "NONE";
                }
                else if (ShownStatuses.Count == 1) 
                {
                    return ShownStatuses[0].ToString().ToUpper();
                }
                else
                {
                    return String.Format("({0})", string.Join(", ", ShownStatuses).ToUpper());
                }
            }
        }
    }
}
