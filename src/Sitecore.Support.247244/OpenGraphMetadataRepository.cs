﻿namespace Sitecore.Support.XA.Feature.SiteMetadata.Repositories.OpenGraphMetadata
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.XA.Feature.SiteMetadata.Models;
    using Sitecore.XA.Feature.SiteMetadata.Extensions;
    using Sitecore.XA.Feature.SiteMetadata.Repositories.OpenGraphMetadata;
    using Sitecore.XA.Foundation.SitecoreExtensions.Interfaces;
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using System.Collections.Generic;
    using System.Linq;

    public class OpenGraphMetadataRepository : Sitecore.XA.Feature.SiteMetadata.Repositories.OpenGraphMetadata.OpenGraphMetadataRepository
    {
        protected IPageContext PageContext;

        public OpenGraphMetadataRepository()
        {
            PageContext = ServiceLocator.ServiceProvider.GetService<IPageContext>();
        }

        protected override IEnumerable<MetaTagModel> GetMetaTags()
        {
            ExternalLinkGenerator service = new ExternalLinkGenerator();// Used in place of IExternalLinkGenerator
            List<MetaTagModel> list = BuildModelMapping().ToList();
            list.Add(new MetaTagModel
            {
                Property = "og:title",
                Content = GetTitle(PageContext.Current)// main fix for 247244
            });
            list.Add(new MetaTagModel
            {
                Property = "og:url",
                Content = service.GetExternalUrl(PageContext.Current)// main fix for 247244
            });
            return from metaTagModel in list
                where metaTagModel != null
                select metaTagModel;
        }
        protected override MetaTagModel GetMetatag(string property, ID fieldId)
        {
            PageContext = ServiceLocator.ServiceProvider.GetService<IPageContext>();
            Field field = PageContext.Current.Fields[fieldId];
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