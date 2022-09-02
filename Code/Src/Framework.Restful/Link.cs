using System.Collections.Generic;

namespace Framework.Restful
{
    public sealed class Link 
    {
        public string Rel { get; private set; }
        public List<LinkItem> Items { get; set; }

        public Link(string rel)
        {
            this.Rel = rel;
            this.Items = new List<LinkItem>();
        }

        public void AddItem(LinkItem item)
        {
            this.Items.Add(item);
        }
    }
}