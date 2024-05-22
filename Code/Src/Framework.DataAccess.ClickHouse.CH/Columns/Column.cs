namespace Framework.DataAccess.CH.Columns
{
    public class Column(string name, string type)
    {
        public string Name { get; set; } = name;

        public string Type { get; set; } = type;
    }
}
