﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Common;

namespace Tasks
{
    public partial class Item : BindableBase
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
                    _source = Group.FindWithId(_sourceId);
                }
                return _source;
            }
            set  
            {
                _sourceId = value.Id;
                _source = value;
            }
        }

        public string GroupName
        {
            get
            {
                return String.Format("[{0}]", Source.Title);
            }
        }

        public void Reload()
        {
            if (this.Exists())
            {
                App.Database.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, this);
            }
        }

        public bool Exists()
        {
            return this.Equals(Item.FindWithIdOrDefault(Id));
        }

        public void Save()
        {
            if (!this.Exists())
            {
                App.Database.Items.InsertOnSubmit(this);
            }
            App.Database.SubmitChanges();
        }

        public void DeleteAndSave()
        {
            this.Delete();
            this.Save();
        }

        public void Delete()
        {
            if (this.Exists())
            {
                App.Database.Items.DeleteOnSubmit(this);
            }
        }

        public Item CreateClone()
        {
            return Item.Create(Source, Title, Description);
        }

        public Item NewClone()
        {
            return Item.New(Source, Title, Description);
        }
    }
}
