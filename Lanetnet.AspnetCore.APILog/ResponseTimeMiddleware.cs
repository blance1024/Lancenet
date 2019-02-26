using Lanetnet.AspnetCore.APILog.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lanetnet.AspnetCore.APILog
{
    /// <summary>
    /// 输出响应时间
    /// </summary>
    public class ResponseTimeMiddleware
    {
        // Name of the Response Header, Custom Headers starts with "X-"  
        private const string RESPONSE_HEADER_RESPONSE_TIME = "X-Response-Time-ms";
        // Handle to the next Middleware in the pipeline  
        private readonly RequestDelegate _next;

        private IConfiguration _cfg;

        private IApiLogService _apiLogService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public ResponseTimeMiddleware(RequestDelegate next, IConfiguration cfg, IApiLogService apiLogService)
        {
            _next = next;
            _cfg = cfg;
            _apiLogService = apiLogService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task InvokeAsync(HttpContext context)
        {
            // Start the Timer using Stopwatch  
            var watch = new Stopwatch();
            watch.Start();
            context.Response.OnStarting(() =>
            {
                // Stop the timer information and calculate the time   
                watch.Stop();

                //是否启用访问日志功能，默认为不启用
                if (!_cfg.GetValue<bool>("ApiLog:IsEnable"))
                {
                    // Add the Response time information in the Response headers.   
                    context.Response.Headers[RESPONSE_HEADER_RESPONSE_TIME] = watch.ElapsedMilliseconds.ToString();
                }
                else
                {
                    Task.Run(() =>
                    {
                        _apiLogService.DataSave(context, watch.ElapsedMilliseconds);
                    });
                }
                return Task.CompletedTask;
            });
            // Call the next delegate/middleware in the pipeline   
            return this._next(context);
        }
    }
}
