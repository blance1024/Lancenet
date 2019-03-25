﻿using System;
using System.IO;
using System.Reflection;

namespace SwaggerBootstrapUI.AspnetCore
{
    public class SwaggerBootstrapUIOptions
    {
        /// <summary>
        /// Gets or sets a route prefix for accessing the swagger-ui
        /// </summary>
        public string StaticFilePrefix { get; set; } = "webjars";

        /// <summary>
        /// Gets or sets a Stream function for retrieving the swagger-ui page
        /// </summary>
        public Func<Stream> IndexStream { get; set; } = () => typeof(SwaggerBootstrapUIOptions).GetTypeInfo().Assembly
            .GetManifestResourceStream("SwaggerBootstrapUI.AspnetCore.node_modules.swagger_bootstrap_ui.doc.html");

        /// <summary>
        /// Gets or sets a title for the swagger-ui page
        /// </summary>
        public string DocumentTitle { get; set; } = "Swagger Bootstrap UI";

        /// <summary>
        /// Gets or sets additional content to place in the head of the swagger-ui page
        /// </summary>
        public string HeadContent { get; set; } = "";

        /// <summary>
        /// Gets the JavaScript config object, represented as JSON, that will be passed to the SwaggerUI
        /// </summary>
        public ConfigObject ConfigObject { get; set; } = new ConfigObject();

        ///// <summary>
        ///// Gets the JavaScript config object, represented as JSON, that will be passed to the initOAuth method
        ///// </summary>
        //public OAuthConfigObject OAuthConfigObject { get; set; } = new OAuthConfigObject();
    }
}
