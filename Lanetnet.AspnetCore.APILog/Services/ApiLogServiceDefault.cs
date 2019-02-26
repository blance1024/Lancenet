using Lanetnet.AspnetCore.APILog.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Lanetnet.AspnetCore.APILog.Services
{
    public class ApiLogServiceDefault : IApiLogService
    {
        public void DataSave(HttpContext context, long responseTime)
        {
        }
    }
}
