Lancenet asp.net core
===========
1、增加headers中输出响应时间ms【X-Response-Time-ms】
   （nuget包搜索 Lanetnet.AspnetCore.APILog）


## Lanetnet.AspnetCore.APILog的使用方法

如何配置asp.net core 在 headers中输出响应时间ms【X-Response-Time-ms】，步骤如下：

*  Install-Package Lanetnet.AspnetCore.APILog


* 引入命名空间 
    using Lanetnet.AspnetCore.APILog;

* 在项目中的Startup.cs 的 
    public void Configure(IApplicationBuilder app, IHostingEnvironment env){
        app.UseResponseTime();
    }

