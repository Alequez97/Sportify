using System.Collections.Generic;

namespace SeedScript.Models
{
    public class SqlSeedModel
    {
        public string TableName { get; set; }

        public string ColumnNames { get; set; }

        public bool InsertOnlyIfEmpty { get; set; }

        public bool InsertInnerData { get; set; }

        public List<RowModel> ListOfRows { get; set; }
    }
}
