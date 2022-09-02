namespace Framework.Restful.Builders
{
    public interface ILinkItemBuilder
    {
        ILinkBuilder WithLinkItem(string href);
        ILinkBuilder WithLinkItem(string href,bool isTemplated);
        ILinkBuilder WithLinkItem(LinkItem linkItem);
      
    }
}
