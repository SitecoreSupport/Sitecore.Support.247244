namespace Sitecore.Support
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Data.Items;
    using Sitecore.DependencyInjection;
    using Sitecore.Links;
    using Sitecore.Web;
    using Sitecore.XA.Foundation.Multisite;
    using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
    using System;
    using System.Globalization;

    public class ExternalLinkGenerator : LinkProvider.LinkBuilder // This class was used as a replacement because IExternalLinkGenerator doesn't exist in 1.6 yet.
    {
        public ExternalLinkGenerator()
            : base(new UrlOptions
            {
                AlwaysIncludeServerUrl = true
            })
        {
        }

        public string GetExternalUrl(Item item)
        {
            SiteInfo siteInfo = ServiceLocator.ServiceProvider.GetService<ISiteInfoResolver>().GetSiteInfo(item);
            string itemUrl = item.GetItemUrl();
            if (!string.IsNullOrWhiteSpace(siteInfo.TargetHostName))
            {
                Uri uri = new Uri(itemUrl);
                string itemPathElement = GetItemPathElement(item, siteInfo);
                return string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", uri.Scheme, siteInfo.TargetHostName,
                    itemPathElement);
            }

            return itemUrl;
        }
    }
}