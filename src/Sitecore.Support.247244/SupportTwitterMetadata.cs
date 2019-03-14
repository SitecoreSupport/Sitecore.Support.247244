using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.XA.Feature.SiteMetadata.Extensions;
using Sitecore.XA.Feature.SiteMetadata.Models;
using Sitecore.XA.Foundation.SitecoreExtensions.Interfaces;

namespace Sitecore.Support.XA.Feature.SiteMetadata.Repositories.TwitterMetadata
{
  public class TwitterMetadataRepository : Sitecore.XA.Feature.SiteMetadata.Repositories.TwitterMetadata.TwitterMetadataRepository
  {
    protected IPageContext PageContext;
    public TwitterMetadataRepository()
    {
      //fix 247244
      PageContext = ServiceLocator.ServiceProvider.GetService<IPageContext>();
    }

    protected override IEnumerable<MetaTagModel> GetMetaTags()
    {
      List<MetaTagModel> list = base.BuildModelMapping().ToList();
      list.Add(new MetaTagModel
      {
        Property = "twitter:title",
        Content = this.GetTitle(PageContext.Current)
      });
      list.Add(new MetaTagModel
      {
        Property = "twitter:card",
        Content = this.GetCardType(PageContext.Current)
      });
      return from metaTagModel in list
        where metaTagModel != null
        select metaTagModel;
    }

    protected override MetaTagModel GetMetatag(string property, ID fieldId)
    {
      //fix 247244
      PageContext = ServiceLocator.ServiceProvider.GetService<IPageContext>();
      Field field = PageContext.Current.Fields[fieldId];
      //end of fix 247244
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