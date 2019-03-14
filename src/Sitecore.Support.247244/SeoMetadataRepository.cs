﻿using System;
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

namespace Sitecore.Support.XA.Feature.SiteMetadata.Repositories.SeoMetadata
{
  public class SeoMetadataRepository: Sitecore.XA.Feature.SiteMetadata.Repositories.SeoMetadata.SeoMetadataRepository
  {
    protected IPageContext PageContext;

    public SeoMetadataRepository()
    {
      PageContext = ServiceLocator.ServiceProvider.GetService<IPageContext>();
    }

    protected override MetaTagModel GetMetatag(string name, ID fieldId)
    {
      Field field = PageContext.Current.Fields[fieldId];
      return new MetaTagModel
      {
        Id = fieldId,
        Name = name,
        Content = field.Value.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ")
      };
    }
  }
}