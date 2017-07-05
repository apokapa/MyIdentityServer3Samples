using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Newtonsoft.Json;
using System.Web.Http;
using Microsoft.Owin.Security.Jwt;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.AccessTokenValidation;

namespace SecureApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            //Routes
            config.MapHttpAttributeRoutes();
            // clear the supported mediatypes of the xml formatter
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //json
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.SerializerSettings.NullValueHandling = NullValueHandling.Include;

            //Injection
            //var container = new Container();
            //container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            //InitializeContainer(container);
            //container.Register<IProductsRepository, ProductsRepository>(Lifestyle.Scoped);
            //container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            //container.Verify();
            //config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            //Swagger
            //SwaggerConfig.Register(config);



            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions {

                Authority= "http://localhost:59248"

            });


        
            app.UseWebApi(config);
        }
    }
}
