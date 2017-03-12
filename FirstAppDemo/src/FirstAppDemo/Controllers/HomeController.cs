using Microsoft.AspNetCore.Mvc;
using FirstAppDemo.Models;

namespace FirstAppDemo.Controllers
{
    public class HomeController : Controller
    {

        //public ContentResult Index()
        //{
        //    return Content("Hello, World! this message is from Home Controller using the Action Result");
        //}

        // 第15课例子。
        //// 回复的对象会被序列化为XML或是JSON格式或是其他格式，
        //// 取决于你在startup 时给MVC的配置，
        //// 如果什么都没配置，默认返回JSON 格式。
        //public ObjectResult Index()
        //{
        //    var employee = new Employee { ID = 1, Name = "Li Jian" };
        //    return new ObjectResult(employee);
        //}

        // 16课：Views 例子。
        public ViewResult Index()
        {
            var employee = new Employee { ID = 1, Name = "李健" };
            return View(employee);
        }
    }
}
