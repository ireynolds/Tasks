using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    [Table]
    public partial class Item
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int _id;

        [Column]
        public string _title;
        
        [Column]
        public string _description;        

        public Item() { }
    }
}
