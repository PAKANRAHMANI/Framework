using System.Collections.Generic;

namespace Framework.Restful
{
    public class Resource : IResource
    {
        public Resource(object state)
        {
            this.State = state;
        }
        public object State { get; set; }
        public List<Link> Links { get; set; }
        public List<EmbeddedResource> EmbeddedResources { get; set; }

        public void AddLinks(List<Link> links)
        {
            this.Links ??= new List<Link>();
            this.Links.AddRange(links);
        }
        public void AddEmbeddedResource(List<EmbeddedResource> embedded)
        {
            this.EmbeddedResources ??= new List<EmbeddedResource>();
            this.EmbeddedResources.AddRange(embedded);
        }
    }
}
