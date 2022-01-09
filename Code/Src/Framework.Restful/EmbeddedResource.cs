using System.Collections.Generic;

namespace Framework.Restful
{
    public sealed class EmbeddedResource : Resource
    {
        public string Name { get;private set; }
        public List<IResource> Resources { get;private set; }
        public EmbeddedResource(object state,string name):base(state)
        {
            this.Name = name;
            this.Resources = new List<IResource>();
        }

        public void AddResource(IResource resource)
        {
            this.Resources.Add(resource);
        }
    }
}