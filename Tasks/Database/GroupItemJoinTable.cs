using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Database
{
    [Table]
    public class GroupItemJoinTable
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int _entryId;

        [Column]
        public int _groupId;

        [Column]
        public int _itemId;
    }
}
