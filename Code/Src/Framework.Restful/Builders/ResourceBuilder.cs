using System.Collections.Generic;

namespace Framework.Restful.Builders
{
    public class ResourceBuilder :
        IResourceBuilder,
        ILinkBuilder,
        ILinkItemBuilder,
        IEmbeddedResourceBuilder
    {
        private object _state;
        private Link _currentLink;
        private EmbeddedResource _currentEmbeddedResource;
        private readonly List<Link> _links;
        private List<EmbeddedResource> _embeddedResources;


        private ResourceBuilder()
        {
            this._links = new List<Link>();
        }
        public static IResourceBuilder Setup() => new ResourceBuilder();

        public ILinkBuilder WithState(object state)
        {
            this._state = state;
            return this;
        }
        public ILinkItemBuilder AddSelfLink()
        {
            this._currentLink = new Link("self");
            return this;
        }
        public ILinkItemBuilder AddLink(string rel)
        {
            if (this._currentLink != null)
                this._links.Add(this._currentLink);
            this._currentLink = new Link(rel);
            return this;
        }

        public IEmbeddedResourceBuilder AddEmbedded(string name)
        {

            this._embeddedResources ??= new List<EmbeddedResource>();
            this._currentEmbeddedResource = new EmbeddedResource(this._state, name);
            this._embeddedResources.Add(this._currentEmbeddedResource);
            return this;
        }
        public ResourceBuilder AddResource(IResource resource)
        {
            this._currentEmbeddedResource.AddResource(resource);
            return this;
        }
        public ILinkBuilder WithLinkItem(string href)
        {
            this._currentLink.AddItem(new LinkItemBuilder().WithHref(href).Build());
            return this;
        }

        public ILinkBuilder WithLinkItem(string href, bool isTemplated)
        {
            this._currentLink.AddItem(new LinkItemBuilder().WithHref(href).WithTemplate(isTemplated).Build());
            return this;
        }

        public ILinkBuilder WithLinkItem(LinkItem linkItem)
        {
            this._currentLink.AddItem(linkItem);
            return this;
        }

        public IResource Build()
        {
            var resource = new Resource(this._state);
            resource.AddLinks(this._links);
            if (this._currentEmbeddedResource != null)
                resource.AddEmbeddedResource(this._embeddedResources);
            return resource;
        }

    }
}
