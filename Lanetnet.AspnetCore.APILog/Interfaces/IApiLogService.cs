using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Lanetnet.AspnetCore.APILog.Interfaces
{
    public interface IApiLogService
    {
        /// <summary>
        /// 回调数据保存方法
        /// </summary>
        /// <param name="context">请求的上下文</param>
        /// <param name="responseTime">响应的时间(ms)</param>
         void DataSave(HttpContext context,long responseTime);
    }
}
