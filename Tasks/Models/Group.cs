using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks // Models
{
    [Table]
    public partial class Group
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int _id;

        [Column]
        public string _title;

        [Column]
        public bool _isDeleted;

        [Column]
        public int _containerId;

        public Group() 
        {
            _isDeleted = false;
            _containerId = -1;
        }
    }
}
