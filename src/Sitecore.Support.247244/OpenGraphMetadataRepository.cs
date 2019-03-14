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
using Sitecore.XA.Feature.SiteMetadata.Services;

namespace Sitecore.Support.XA.Feature.SiteMetadata.Repositories.OpenGraphMetadata
{
  public class OpenGraphMetadataRepository: Sitecore.XA.Feature.SiteMetadata.Repositories.OpenGraphMetadata.OpenGraphMetadataRepository
  {

    protected IPageContext PageContext;

    public OpenGraphMetadataRepository()
    {
      PageContext = ServiceLocator.ServiceProvider.GetService<IPageContext>();
    }

    protected override IEnumerable<MetaTagModel> GetMetaTags()
    {
      IExternalLinkGenerator service = ServiceLocator.ServiceProvider.GetService<IExternalLinkGenerator>();
      List<MetaTagModel> list = base.BuildModelMapping().ToList();
      list.Add(new MetaTagModel
      {
        Property = "og:title",
        Content = this.GetTitle(PageContext.Current)
      });
      list.Add(new MetaTagModel
      {
        Property = "og:url",
        Content = service.GetExternalUrl(PageContext.Current)
      });
      return from metaTagModel in list
        where metaTagModel != null
        select metaTagModel;
    }
  }
}