using Sitecore.Support.XA.Feature.SiteMetadata.Repositories.OpenGraphMetadata;
using Sitecore.Support.XA.Feature.SiteMetadata.Repositories.SeoMetadata;
using Sitecore.Support.XA.Feature.SiteMetadata.Repositories.TwitterMetadata;
using Sitecore.XA.Feature.SiteMetadata.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;

namespace Sitecore.Support
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.XA.Foundation.SitecoreExtensions.Services;

    public class RegisterSiteMetadataServices : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAbstractRepository<OpenGraphRenderingModel>, OpenGraphMetadataRepository>();
            serviceCollection.AddSingleton<IAbstractRepository<SeoMetadataRenderingModel>, SeoMetadataRepository>();
            serviceCollection.AddSingleton<IAbstractRepository<TwitterMetadataRenderingModel>, TwitterMetadataRepository>();
        }
    }
}