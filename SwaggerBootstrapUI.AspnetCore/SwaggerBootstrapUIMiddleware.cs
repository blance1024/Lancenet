using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SwaggerBootstrapUI.AspnetCore
{
    public class SwaggerBootstrapUIMiddleware
    {
        private const string EmbeddedFileNamespace = "SwaggerBootstrapUI.AspnetCore.node_modules.swagger_bootstrap_ui";
        private readonly SwaggerBootstrapUIOptions _options;
        private readonly StaticFileMiddleware _staticFileMiddleware;

        public SwaggerBootstrapUIMiddleware(
           RequestDelegate next,
           IHostingEnvironment hostingEnv,
           ILoggerFactory loggerFactory,
           IOptions<SwaggerBootstrapUIOptions> optionsAccessor)
           : this(next, hostingEnv, loggerFactory, optionsAccessor.Value)
        {

        }

        public SwaggerBootstrapUIMiddleware(RequestDelegate next,
           IHostingEnvironment hostingEnv,
           ILoggerFactory loggerFactory,
            SwaggerBootstrapUIOptions options)
        {
            _options = options ?? new SwaggerBootstrapUIOptions();
            _staticFileMiddleware = CreateStaticFileMiddleware(next, hostingEnv, loggerFactory, options);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            // If the RoutePrefix is requested (with or without trailing slash), redirect to index URL
            //if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{_options.RoutePrefix}/?$"))
            //{
            //    // Use relative redirect to support proxy environments
            //    var relativeRedirectPath = path.EndsWith("/")
            //        ? "doc.html"
            //        : $"{path.Split('/').Last()}/doc.html";

            //    RespondWithRedirect(httpContext.Response, relativeRedirectPath);
            //    return;
            //}

            if (httpMethod == "GET" && Regex.IsMatch(path, $"/doc.html"))
            {
                await RespondWithIndexHtml(httpContext.Response);
                return;
            }
            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{_options.StaticFilePrefix}/"))
            {
                var _reg = new Regex("[.][0-9]");
                var _srcPath = path.Substring(0, path.LastIndexOf("/")) ?? "";
                if (!string.IsNullOrEmpty(_srcPath))
                {
                    var _destPath = _reg.Replace(_srcPath, r => r.Value.Replace(".", "._"));
                    httpContext.Request.Path = path.Replace(_srcPath, _destPath.Replace("-", "_"));
                }
            }
            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/swagger-resources$"))
            {
                var _data = new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object> {
                            { "name","默认分组"},
                            { "swaggerVersion","2.0"},
                            { "location","/swagger/v1/swagger.json"},
                            { "url","/swagger/v1/swagger.json"}
                        }
                    };
                if (_options.ConfigObject.Urls.Count() > 0)
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(_options.ConfigObject.Urls));
                else
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(_data));
                return;
            }
            await _staticFileMiddleware.Invoke(httpContext);
        }

        //private void RespondWithRedirect(HttpResponse response, string location)
        //{
        //    response.StatusCode = 301;
        //    response.Headers["Location"] = location;
        //}

        private async Task RespondWithIndexHtml(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html;charset=utf-8";

            using (var stream = _options.IndexStream())
            {
                // Inject arguments before writing to response
                var htmlBuilder = new StringBuilder(new StreamReader(stream).ReadToEnd());
                foreach (var entry in GetIndexArguments())
                {
                    htmlBuilder.Replace(entry.Key, entry.Value);
                }

                await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
            }
        }

        private IDictionary<string, string> GetIndexArguments()
        {
            return new Dictionary<string, string>()
            {
                { "%(DocumentTitle)", _options.DocumentTitle },
                { "%(HeadContent)", _options.HeadContent },
                //{ "%(ConfigObject)", SerializeToJson(_options.ConfigObject) },
                //{ "%(OAuthConfigObject)", SerializeToJson(_options.OAuthConfigObject) }
            };
        }

        //private string SerializeToJson(object obj)
        //{
        //    return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        //    {
        //        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        //        Converters = new[] { new StringEnumConverter(true) },
        //        NullValueHandling = NullValueHandling.Ignore,
        //        Formatting = Formatting.None,
        //        StringEscapeHandling = StringEscapeHandling.EscapeHtml
        //    });
        //}

        private StaticFileMiddleware CreateStaticFileMiddleware(
            RequestDelegate next,
            IHostingEnvironment hostingEnv,
            ILoggerFactory loggerFactory,
            SwaggerBootstrapUIOptions options)
        {
            var staticFileOptions = new StaticFileOptions
            {
                //RequestPath = string.IsNullOrEmpty(options.StaticFilePrefix) ? string.Empty : $"/{options.StaticFilePrefix}",
                FileProvider = new EmbeddedFileProvider(typeof(SwaggerBootstrapUIMiddleware).GetTypeInfo().Assembly, EmbeddedFileNamespace),
            };

            return new StaticFileMiddleware(next, hostingEnv, Options.Create(staticFileOptions), loggerFactory);
        }
    }
}
