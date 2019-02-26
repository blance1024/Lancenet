using System;
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
        public void DataSave(HttpContext context, long responseTime)
        {
            var _cfg = context.RequestServices.GetRequiredService<IConfiguration>();
            var _db = context.RequestServices.GetRequiredService<SqlSugarClient>();
            string _accessUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";
            string _token = "";
            try
            {
                _token = context.GetTokenAsync("access_token").Result;
            }
            catch (Exception ex)
            {

            }
            _db.Ado.ExecuteCommand($"insert into ApiLog (ClientIP,ResponseTime,AccessToken,AccessTime,AccessApiUrl) values('{context.Connection.RemoteIpAddress}',{responseTime},'{_token}','{DateTime.Now.AddMilliseconds(-responseTime)}','{_accessUrl}')");
        }
    }
}
