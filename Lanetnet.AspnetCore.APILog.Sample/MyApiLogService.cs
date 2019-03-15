using System;
using System.IO;
using System.Threading.Tasks;
using Lanetnet.AspnetCore.APILog.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Lanetnet.AspnetCore.APILog.Sample
{
    public class MyApiLogService : IApiLogService
    {
        private IConfiguration _cfg;
        private SqlSugarClient _db;

        public MyApiLogService(IConfiguration cfg, SqlSugarClient sqlSugarClient)
        {
            _cfg = cfg;
            _db = sqlSugarClient;
        }

        public void DataSave(HttpContext context, long responseTime)
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
                try
                {
                    _db.Ado.ExecuteCommand($"insert into ApiLog (ClientIP,ResponseTime,AccessToken,AccessTime,AccessApiUrl,AccessAction,AccessParameterGet,AccessParameterPost,HttpStatus) values('{context.Connection.RemoteIpAddress}',{responseTime},'{_token}','{DateTime.Now.AddMilliseconds(-responseTime)}','{_accessUrl}','{_httpMethod}',N'{_parameterGet}',N'{_parameterPost}','{_httpStatus}')");
                }
                catch(Exception ex)
                {
                    var _a = ex.Message;
                }
            });
        }
    }
}
