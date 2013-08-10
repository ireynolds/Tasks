using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Common;
using Tasks.ViewModels;

namespace Tasks
{
    public partial class Item : DbItem<Item>
    {
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private Group _source;
        public Group Source
        {
            get 
            {
                if (_source == null)
                {
                    _source = Group.FindById(_sourceId);
                }
                return _source;
            }
            set  
            {
                _sourceId = value.Id;
                _source = value;
            }
        }

        public Status Status
        {
            get { return (Status)_status; }
            set { SetProperty(ref _status, (int)value); }
        }

        public string GroupName
        {
            get
            {
                return String.Format("[{0}]", Source.Title);
            }
        }
    }
}
