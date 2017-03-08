学习 ASP.NET Core 教程：https://www.tutorialspoint.com/asp.net_core/index.htm.

1. 注意：ASP.NET Core 类库从1.0 升级到1.1 后，需要安装ASP.NET Core 运行时“dotnet-win-x64.1.1.0.exe”。
2. 类库升级到1.1 后出现“Can not find runtime target for framework '.NETCoreApp,Version=v1.0' compatible with one of the target runtimes: 'win10-x64, win81-x64, win8-x64, win7-x64'. Possible causes:”错误的解决办法：修改project.json, 将"Microsoft.NETCore.App": "1.1.0" 改为：
	    "Microsoft.NETCore.App": {
	      "version": "1.1.0",
	      "type": "platform"
	    }
3. 在ASP.NET Core 1.1 中 UseRuntimeInfoPage 被移除了，需要注意：https://github.com/aspnet/Home/issues/1632
4. 第7节 Configuration（添加配置文件AppSettings.json）
	1. 使用代码
	    	var builder = new ConfigurationBuilder()
				.AddJsonFile("AppSettings.json");
会引起“500 Internal Server Error” 错误。需要指明AppSettings.json 的路径，正确代码如下：
			var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json");
	2. 部署到服务器上还会有错误“500 Internal Server Error”。需要将 project.json 的
	
			"publishOptions": {
		    "include": [
		      "wwwroot",
		      "web.config"
		    ]
		  	},
改为：
			"publishOptions": {
		    "include": [
		      "wwwroot",
		      "AppSettings.json",
		      "web.config"
		    ]
		   },
5. 第15课例子2中，参照教程会出现“HTTP 406 错误”。解决办法：将代码
		services.AddMvcCore();
改为
        var mvcCore = services.AddMvcCore();
        mvcCore.AddJsonFormatters();         		