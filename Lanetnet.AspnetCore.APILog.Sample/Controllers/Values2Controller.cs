using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Lanetnet.AspnetCore.APILog.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v2")]
    [ApiController]
    public class Values2Controller : ControllerBase
    {
        /// <summary>
        /// 样例2GET获取列表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //long responseTime = 2;
            //var _db = HttpContext.RequestServices.GetRequiredService<SqlSugarClient>();
            //string _accessUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
            //string _token = "";
            //try
            //{
            //    _token = HttpContext.GetTokenAsync("access_token").Result;
            //}
            //catch (Exception ex)
            //{

            //}
            //_db.Ado.ExecuteCommand($"insert into ApiLog (ClientIP,ResponseTime,AccessToken,AccessTime,AccessApiUrl) values('{HttpContext.Connection.RemoteIpAddress}',{responseTime},'{_token}','{DateTime.Now.AddMilliseconds(-responseTime)}','{_accessUrl}')");
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 样例2GET根据ID值获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 样例2增加数据
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        
        /// <summary>
        /// 样例2修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// 样例2删除数据Del
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
