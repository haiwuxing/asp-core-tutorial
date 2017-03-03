学习 ASP.NET Core 教程：https://www.tutorialspoint.com/asp.net_core/index.htm.

1. 注意：ASP.NET Core 类库从1.0 升级到1.1 后，需要安装ASP.NET Core 运行时“dotnet-win-x64.1.1.0.exe”。
2. 类库升级到1.1 后出现“Can not find runtime target for framework '.NETCoreApp,Version=v1.0' compatible with one of the target runtimes: 'win10-x64, win81-x64, win8-x64, win7-x64'. Possible causes:”错误的解决办法：修改project.json, 将"Microsoft.NETCore.App": "1.1.0" 改为：
    "Microsoft.NETCore.App": {
      "version": "1.1.0",
      "type": "platform"
    }
3. 