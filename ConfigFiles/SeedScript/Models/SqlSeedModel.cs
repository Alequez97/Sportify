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

        public int TotalRowsCount => TotalRowsInSeedModel(this);

        private int TotalRowsInSeedModel(SqlSeedModel sqlSeedModel)
        {
            int sum = 0;
            sqlSeedModel.ListOfRows.ForEach(row => sum += (row.InnerInsertModel != null && sqlSeedModel.InsertInnerData == true) ? TotalRowsInSeedModel(row.InnerInsertModel) : 1);

            return sum;
        }
    }
}
