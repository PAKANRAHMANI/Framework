namespace Framework.Restful.Builders
{
    public interface ILinkBuilder
    {
        ILinkItemBuilder AddSelfLink();
        ILinkItemBuilder AddLink(string rel);
        IEmbeddedResourceBuilder AddEmbedded(string name);
        IResource Build();
    }
}
