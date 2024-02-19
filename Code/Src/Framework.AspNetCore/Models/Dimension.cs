namespace Framework.AspNetCore.Models
{
    public class Dimension
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Dimension(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public Dimension()
        {
        }
    }
}
