using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.ViewModels
{
    public class Space
    {
        private ObservableCollection<Group> _groups;
        public ObservableCollection<Group> Groups
        {
            get
            {
                if (_groups == null)
                {
                    _groups = new ObservableCollection<Group>(App.Database.Groups);
                }

                return _groups;
            }
        }
    }
}
