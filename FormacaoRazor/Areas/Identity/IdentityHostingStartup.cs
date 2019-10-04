using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(FormacaoRazor.Areas.Identity.IdentityHostingStartup))]
namespace FormacaoRazor.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {

        }
    }
}
