using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Umbraco.IdentityExtensions;
using Umbraco.Web.Security.Identity;
using emptyUmbraco;

namespace emptyUmbraco
{

    /// <summary>
    /// Extension methods to configure Umbraco for issuing and processing tokens for authentication
    /// </summary>    
    public static class UmbracoAuthTokenServerExtensions
    {

        /// <summary>
        /// Configures Umbraco to issue and process authentication tokens
        /// </summary>
        /// <param name="app"></param>
        /// <param name="backofficeAuthServerProviderOptions"></param>
        /// <remarks>
        /// This is a very simple implementation of token authentication, the expiry below is for a single day and with
        /// this implementation there is no way to force expire tokens on the server however given the code below and the additional
        /// callbacks that can be registered for the BackOfficeAuthServerProvider these types of things could be implemented. Additionally the
        /// BackOfficeAuthServerProvider could be overridden to include this functionality instead of coding the logic into the callbacks.
        /// </remarks>
        /// <example>
        /// 
        /// An example of using this implementation is to use the UmbracoStandardOwinSetup and execute this extension method as follows:
        /// 
        /// <![CDATA[
        /// 
        ///   public override void Configuration(IAppBuilder app)
        ///   {
        ///       //ensure the default options are configured
        ///       base.Configuration(app);
        ///   
        ///       //configure token auth
        ///       app.UseUmbracoBackOfficeTokenAuth();
        ///   }
        /// 
        /// ]]>
        /// 
        /// Then be sure to read the details in UmbracoStandardOwinSetup on how to configure Owin to startup using it.
        /// </example>
        public static void UseUmbracoBackOfficeTokenAuth(this IAppBuilder app, BackOfficeAuthServerProviderOptions backofficeAuthServerProviderOptions = null)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/umbraco/oauth/token"),

                AuthenticationType = Umbraco.Core.Constants.Security.BackOfficeTokenAuthenticationType,
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new BackOfficeAuthServerProvider(backofficeAuthServerProviderOptions)
                {
                    OnValidateClientAuthentication = context =>
                    {
                        context.Response.Headers.Remove("Access-Control-Allow-Origin");
                        context.Response.Headers.Remove("Access-Control-Allow-Credentials");
                        context.Validated();
                        return Task.FromResult(0);
                    }
                }
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}
