namespace Sitecore.Support.XA.Feature.SiteMetadata.Repositories.TwitterMetadata
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.DependencyInjection;
    using Sitecore.XA.Feature.SiteMetadata.Extensions;
    using Sitecore.XA.Feature.SiteMetadata.Models;
    using Sitecore.XA.Feature.SiteMetadata.Repositories.TwitterMetadata;
    using Sitecore.XA.Foundation.SitecoreExtensions.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class TwitterMetadataRepository : Sitecore.XA.Feature.SiteMetadata.Repositories.TwitterMetadata.TwitterMetadataRepository
    {
        protected IPageContext PageContext;

        public TwitterMetadataRepository()
        {
            PageContext = ServiceLocator.ServiceProvider.GetService<IPageContext>();
        }

        protected override IEnumerable<MetaTagModel> GetMetaTags()
        {
            List<MetaTagModel> list = BuildModelMapping().ToList();
            list.Add(new MetaTagModel
            {
                Property = "twitter:title",
                Content = GetTitle(PageContext.Current)// main fix for 247244
            });
            list.Add(new MetaTagModel
            {
                Property = "twitter:card",
                Content = GetCardType(PageContext.Current)// main fix for 247244
            });
            return from metaTagModel in list
                where metaTagModel != null
                select metaTagModel;
        }

        protected override MetaTagModel GetMetatag(string property, ID fieldId)
        {
            PageContext = ServiceLocator.ServiceProvider.GetService<IPageContext>();
            Field field = PageContext.Current.Fields[fieldId];// main fix for 247244
            if (!string.IsNullOrEmpty(field?.Value))
            {
                if (FieldTypeManager.GetField(field) is ImageField)
                {
                    return new MetaTagModel
                    {
                        Id = fieldId,
                        Property = property,
                        Content = field.GetImageUrl()
                    };
                }

                return new MetaTagModel
                {
                    Id = fieldId,
                    Property = property,
                    Content = field.Value.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ")
                };
            }

            return null;
        }
    }
}