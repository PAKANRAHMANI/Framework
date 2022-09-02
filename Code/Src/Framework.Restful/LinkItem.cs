namespace Framework.Restful
{
    public sealed class LinkItem 
    {
        public string Href { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Deprecation { get; private set; }
        public string Title { get; private set; }
        public string Profile { get; private set; }
        public bool? Templated { get; private set; }
        public string HrefLang { get; private set; }
        public string HttpVerb { get; private set; }

        public LinkItem(string href)
        {
            this.Href = href;
        }

        public LinkItem(string href, string name, string type, string deprecation, string title, string profile, bool? templated, string hrefLang, string httpVerb)
        {
            Href = href;
            Name = name;
            Type = type;
            Deprecation = deprecation;
            Title = title;
            Profile = profile;
            Templated = templated;
            HrefLang = hrefLang;
            HttpVerb = httpVerb;
        }
    }
}
