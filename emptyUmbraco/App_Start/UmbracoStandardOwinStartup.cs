using Microsoft.Owin;
using Owin;
using Umbraco.Core;
using Umbraco.Core.Security;
using Umbraco.Web;
using Umbraco.Web.Security.Identity;
using Umbraco.IdentityExtensions;
using emptyUmbraco;
using Umbraco.RestApi;
using Microsoft.Owin.Cors;
using System.Web.Cors;

//To use this startup class, change the appSetting value in the web.config called 
// "owin:appStartup" to be "UmbracoStandardOwinStartup"

[assembly: OwinStartup("UmbracoStandardOwinStartup", typeof(UmbracoStandardOwinStartup))]

namespace emptyUmbraco
{
    /// <summary>
    /// The standard way to configure OWIN for Umbraco
    /// </summary>
    /// <remarks>
    /// The startup type is specified in appSettings under owin:appStartup - change it to "StandardUmbracoStartup" to use this class
    /// </remarks>
    public class UmbracoStandardOwinStartup : UmbracoDefaultOwinStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            // Start using cors widespread, including REST API
            app.UseCors(CorsOptions.AllowAll);

            // Ensure the default options are configured
            base.Configuration(app);

            // Configuring the Umbraco REST API options
            app.ConfigureUmbracoRestApi(new UmbracoRestApiOptions()
            {
                // Modify the CorsPolicy as required
                CorsPolicy = new CorsPolicy()
                {
                    AllowAnyHeader = false,
                    AllowAnyMethod = false,
                    AllowAnyOrigin = false,
                }
            });

            // Enabling the usage of auth token retrieved by backoffice user / login
            // Uncomment below line when testing the PoC website
            app.UseUmbracoBackOfficeTokenAuth();
        }
    }
}
