using Framework.DataAccess.CH.Columns;

namespace Framework.DataAccess.CH.Tables
{
    public class Table
    {
        public string Name { get; set; }

        public List<Column> Columns { get; set; }

        public string Schema { get; set; }

        public string Engine { get; set; }
    }
}
