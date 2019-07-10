namespace Sitecore.Support.XA.Feature.SiteMetadata.Repositories.SeoMetadata
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.DependencyInjection;
    using Sitecore.XA.Feature.SiteMetadata.Models;
    using Sitecore.XA.Feature.SiteMetadata.Repositories.SeoMetadata;
    using Sitecore.XA.Foundation.SitecoreExtensions.Interfaces;

    public class SeoMetadataRepository : Sitecore.XA.Feature.SiteMetadata.Repositories.SeoMetadata.SeoMetadataRepository
    {
        protected IPageContext PageContext;

        public SeoMetadataRepository()
        {
            PageContext = ServiceLocator.ServiceProvider.GetService<IPageContext>();
        }

        protected override MetaTagModel GetMetatag(string name, ID fieldId)
        {
            Field field = PageContext.Current.Fields[fieldId];// main fix for 247244
            return new MetaTagModel
            {
                Id = fieldId,
                Name = name,
                Content = field.Value.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ")
            };
        }
    }
}