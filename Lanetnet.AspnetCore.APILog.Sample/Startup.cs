using Lanetnet.AspnetCore.APILog.Interfaces;
using Lanetnet.AspnetCore.APILog.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace Lanetnet.AspnetCore.APILog.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _cfg = configuration;
        }

        public IConfiguration _cfg { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddApiLogService<IApiLogService, MyApiLogService>();
            services.AddSingleton(new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = _cfg.GetConnectionString("DefaultConnection"),//必填, 数据库连接字符串
                DbType = DbType.SqlServer,         //必填, 数据库类型
                IsAutoCloseConnection = true,       //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                InitKeyType = InitKeyType.SystemTable    //默认SystemTable, 字段信息读取, 如：该属性是不是主键，是不是标识列等等信息
            }));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "SwaggerBootstrapUI测试",
                    Version = "v1",
                    Description = "SwaggerBootstrapUI测试 RESTful APIs",
                    TermsOfService = "http://hui.dgjy.net",
                    Contact = new Contact
                    {
                        Name = "谢海棠",
                        Email = "xieht@alltosea.com",
                        Url = "http://www.alltosea.com"
                    }
                });
                c.SwaggerDoc("v2", new Info
                {
                    Title = "SwaggerBootstrapUI测试V2",
                    Version = "v2",
                    Description = "SwaggerBootstrapUI测试v2 RESTful APIs",
                    TermsOfService = "http://hui.dgjy.net",
                    Contact = new Contact
                    {
                        Name = "谢海棠",
                        Email = "xieht@alltosea.com",
                        Url = "http://www.alltosea.com"
                    }
                });
                var filePath = Path.Combine(AppContext.BaseDirectory, "MyApi.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseTime();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Host = httpReq.Host.Value);
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.BasePath = "/");
            });
            app.UseSwaggerBootstrapUI(c=> {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V2");
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V2");
                c.DisplayOperationId();
            });
            app.UseMvc();
        }
    }
}
