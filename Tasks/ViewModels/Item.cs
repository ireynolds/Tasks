using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public Group Container
        {
            get { return Group.FindById(_containerId); }
            set { SetProperty(ref _containerId, value.Id); }
        }

        public Group Source
        {
            get { return Group.FindById(_sourceId); }
            set { SetProperty(ref _sourceId, value.Id); }
        }

        public Visibility IsGroupNameVisible
        {
            get
            {
                return (Container.Id == Source.Id) ? Visibility.Collapsed : Visibility.Visible;
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
                return Source.Title;
            }
        }

        public string BracketedGroupName
        {
            get
            {
                return String.Format("[{0}]", Source.Title);
            }
        }
    }
}
