using SwaggerBootstrapUI.AspnetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerBootstrapUIOptionsExtension
    {
        /// <summary>
        /// Adds Swagger JSON endpoints. Can be fully-qualified or relative to the UI page
        /// </summary>
        /// <param name="options"></param>
        /// <param name="url">Can be fully qualified or relative to the current host</param>
        /// <param name="name">The description that appears in the document selector drop-down</param>
        public static void SwaggerEndpoint(this SwaggerBootstrapUIOptions options, string url, string name)
        {
            var urls = new List<UrlDescriptor>(options.ConfigObject.Urls ?? Enumerable.Empty<UrlDescriptor>())
            {
                new UrlDescriptor { url = url, name = name }
            };
            options.ConfigObject.Urls = urls;
        }
    }
}
