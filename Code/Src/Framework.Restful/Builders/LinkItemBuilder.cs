namespace Framework.Restful.Builders
{
    public class LinkItemBuilder
    {
        private string _href;
        private string _rel;
        private string _name;
        private string _type;
        private string _deprecation;
        private string _title;
        private string _profile;
        private bool? _templated;
        private string _hrefLang;
        private string _httpVerb;
        public LinkItemBuilder WithHref(string href)
        {
            this._href = href;
            return this;
        }
        public LinkItemBuilder WithRel(string rel)
        {
            this._rel = rel;
            return this;
        }
        public LinkItemBuilder WithName(string name)
        {
            this._name = name;
            return this;
        }
        public LinkItemBuilder WithType(string type)
        {
            this._type = type;
            return this;
        }
        public LinkItemBuilder WithDeprecation(string description)
        {
            this._deprecation = description;
            return this;
        }
        public LinkItemBuilder WithTitle(string title)
        {
            this._title = title;
            return this;
        }
        public LinkItemBuilder WithProfile(string profile)
        {
            this._profile = profile;
            return this;
        }
        public LinkItemBuilder WithHrefLang(string hrefLang)
        {
            this._hrefLang = hrefLang;
            return this;
        }
        public LinkItemBuilder WithTemplate(bool template)
        {
            this._templated = template;
            return this;
        }
        public LinkItemBuilder WithHttpVerb(string httpVerb)
        {
            this._httpVerb = httpVerb;
            return this;
        }

        public LinkItem Build()
        {
            return new LinkItem(this._href,this._name,this._type,this._deprecation,this._title,this._profile,this._templated,this._hrefLang,this._httpVerb);
        }
    }
}