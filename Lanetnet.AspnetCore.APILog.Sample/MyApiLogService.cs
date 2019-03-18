using System;
using System.IO;
using System.Threading.Tasks;
using Lanetnet.AspnetCore.APILog.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Lanetnet.AspnetCore.APILog.Sample
{
    /// <summary>
    /// 写在入响应日志到数据库
    /// </summary>
    public class MyApiLogService : IApiLogService
    {
        private IConfiguration _cfg;
        private SqlSugarClient _db;
        private readonly ILogger<MyApiLogService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="sqlSugarClient"></param>
        /// <param name="logger"></param>
        public MyApiLogService(IConfiguration cfg, SqlSugarClient sqlSugarClient, ILogger<MyApiLogService> logger)
        {
            _cfg = cfg;
            _db = sqlSugarClient;
            _logger = logger;
        }

        /// <summary>
        /// 保存数据到数据库
        /// </summary>
        /// <param name="context"></param>
        /// <param name="responseTime"></param>
        public void DataSave(HttpContext context, long responseTime)
        {
            if (_db != null)
            {
                var _apiLogIsEnable = _cfg.GetValue<bool>("ApiLog:IsEnable");
                string _accessUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";
                var _httpMethod = context.Request.Method;
                var _httpStatus = context.Response.StatusCode;
                var _parameterGet = context.Request.QueryString.ToString();
                var _parameterPost = "";
                if (_httpMethod == "POST")
                {
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    var _reader = new StreamReader(context.Request.Body);
                    _parameterPost = _reader.ReadToEnd();
                }
                string _token = "";
                try
                {
                    _token = context.GetTokenAsync("access_token").Result;
                }
                catch (Exception ex)
                {

                }
                Task.Run(() =>
                {
                    _db.Ado.ExecuteCommand($"insert into ApiLog (ClientIP,ResponseTime,AccessToken,AccessTime,AccessApiUrl,AccessAction,AccessParameterGet,AccessParameterPost,HttpStatus) values('{context.Connection.RemoteIpAddress}',{responseTime},'{_token}','{DateTime.Now.AddMilliseconds(-responseTime)}','{_accessUrl}','{_httpMethod}',N'{_parameterGet}',N'{_parameterPost}','{_httpStatus}')");
                });
            }
            else
            {
                _logger.LogError("_db变量为空");
            }

        }
    }
}
