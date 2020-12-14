using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AssetWebManager.Areas.Identity.IdentityHostingStartup))]
namespace AssetWebManager.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}