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
5. 第15课：
	1. 例子2中，参照教程会出现“HTTP 406 错误”。解决办法：将代码

			services.AddMvcCore();
改为

        	var mvcCore = services.AddMvcCore();
        	mvcCore.AddJsonFormatters();
	
	2. 在第16课中发现：如果将project.json 中的`"Microsoft.AspNetCore.Mvc.Core": "1.0.1` 改为 `"Microsoft.AspNetCore.Mvc": "1.0.1"` , Startup.cs 中的 `services.AddMvcCore();` 改为 `services.AddMvc();`, 则不需要`mvcCore.AddJsonFormatters();` 也可以返回 json字符串。
    
6. 第17课中EF引用到的两个包新版本中进行了重命名：
	1. EntityFramework.MicrosoftSqlServer -> Microsoft.EntityFrameworkCore.SqlServer
	2. EntityFramework.Commands -> "Microsoft.EntityFrameworkCore.Tools

7. 第18课中
	1. `using Microsoft.Data.Entity; `改为`using Microsoft.EntityFrameworkCore;`。
	2. 教程中使用的 dnx 命令已经被淘汰了，改为：dotnet，另外需要在project.json 的 tools 节点下加入以下内容：
			"Microsoft.EntityFrameworkCore.Tools.DotNet": "1.0.0"
	   为啥？
	3. 运行`dotnet ef migrations add v1` 会出错，提示"No project was found. Change the current working directory or use the --project option.", 需要修改tools 节点中的`"Microsoft.EntityFrameworkCore.Tools.DotNet": "1.0.0"`为：
			"Microsoft.EntityFrameworkCore.Tools.DotNet": "1.0.0-preview3-final"
	4. 命令行中运行 `dotnet ef migrations add v1`时出现 **An error occurred while calling method 'ConfigureServices' on startup class 'Startup'. Consider using IDbContextFactory to override the initialization of the DbContext at design-time. Error: The configuration file 'AppSettings.json' was not found and is not optional.** 错误。解决办法：将代码
	
	        public Startup()
	        {
	            var builder = new ConfigurationBuilder()
	                .SetBasePath(Directory.GetCurrentDirectory())
	                .AddJsonFile("AppSettings.json");
	            Configuration = builder.Build();
	        }
改为：

	        public Startup(IHostingEnvironment env)
	        {
	            var builder = new ConfigurationBuilder()
	                .SetBasePath(env.ContentRootPath)
	                .AddJsonFile("AppSettings.json");
	            Configuration = builder.Build();
        	}
	5. 执行上述修改后，运行程序或是执行`dotnet ef migrations add v1`时出现**System.InvalidOperationException: No database provider has been configured for this DbContext. A provider can be configured by overriding the DbContext.OnConfiguring method or by using AddDbContext on the application service provider. If AddDbContext is used, then also ensure that your DbContext type accepts a DbContextOptions<TContext> object in its constructor and passes it to the base constructor for DbContext.** 异常，解决办法：
		1. FirstAppDemoDbContext.cs 中添加带有`DbContextOptions` 参数的构造函数：
		
		        public FirstAppDemoDbContext(DbContextOptions<FirstAppDemoDbContext> options)
		            : base(options) { }
		2. HomeController 中将

			    public class HomeController : Controller
			    {
			        // 第18课
			        public ViewResult Index()
			        {
			            var employee = new Employee { ID = 1, Name = "李健" };
			
			            using (var context = new FirstAppDemoDbContext())
			            {
			                SQLEmployeeData sqlData = new SQLEmployeeData(context);
			                sqlData.Add(employee);
			            }
			
			            return View(employee);
			        } 
			    }
改为

			    public class HomeController : Controller
			    {
			        FirstAppDemoDbContext _context;
			
			        public HomeController(FirstAppDemoDbContext context)
			        {
			            _context = context;
			        }
			
			        // 第18课
			        public ViewResult Index()
			        {
			            var model = new HomePageViewModel();
			
			            if (_context != null)
			            {
			                SQLEmployeeData sqlData = new SQLEmployeeData(_context);
			                model.Employees = sqlData.GetAll();
			            }
			
			            return View(model);
			        }
			    }

	6. 执行上述修改后，运行程序，出现**Win32Exception: 管道的另一端上无任何进程。**异常。这是由于没有通过创建数据库所致。
	7. 执行上述操作后，可成功通过`dotnet ef migrations add v1`和`dotnet ef database update`命令创建数据库，如果通过**SQL Server Object Explorer**查看不到数据库，可尝试点刷新按钮。
	8. **SqlException: Cannot insert explicit value for identity column in table 'Employees' when IDENTITY_INSERT is set to OFF.** 异常。
	9. Index.cshtml 引起的异常**'IHtmlHelper<dynamic>' has no applicable method named 'ActionLink' but appears to have an extension method by that name. Extension methods cannot be dynamically dispatched. Consider casting the dynamic arguments or calling the extension method without the extension method syntax.**， 将代码
	
			@Html.ActionLink(employee.Id.ToString(), "Details", new { id = employee.Id })
	改为：注释掉该代码。

