using System;
using BohFoundation.WebApi.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;


namespace BohFoundation.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var oAuthServerConfig = new OAuthAuthorizationServerOptions
            {
                Provider = new CustomOAuthProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(6),
                TokenEndpointPath = new PathString("/token")
            };

            var oAuthBearerConfig = new OAuthBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = "Bearer"
            };

            app.UseOAuthAuthorizationServer(oAuthServerConfig);

            app.UseOAuthBearerAuthentication(oAuthBearerConfig);
        }
    }
}
