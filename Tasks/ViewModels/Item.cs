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

        private Group _containingGroup;
        public Group ContainingGroup
        {
            get 
            {
                if (_containingGroup == null)
                {
                    _containingGroup = (from entry in App.Database.GroupItemJoins
                                        where entry._itemId == this._id
                                        select Group.FindById(entry._groupId)).First();
                }

                return _containingGroup;
            }
        }

        public Visibility IsGroupNameVisible
        {
            get
            {
                return (ContainingGroup.Id == Source.Id) ? Visibility.Collapsed : Visibility.Visible;
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
