using System.Web.Http;
using Owin;
using Swashbuckle.Application;

namespace WinFormsQtBridge.Plugin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            // Подключаем Swagger
            config.EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Bridge Plugin API");
                })
                .EnableSwaggerUi();

            app.UseWebApi(config);
        }
    }
}