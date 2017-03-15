using Microsoft.AspNetCore.Mvc;
using FirstAppDemo.Models;
using System.Collections.Generic; // for IEnumerable interface.
using System.Linq; // for FirstOrDefault

namespace FirstAppDemo.Controllers
{
    public class HomeController : Controller
    {
        FirstAppDemoDbContext _context;

        public HomeController(FirstAppDemoDbContext context)
        {
            _context = context;
        }

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

        //// 16课：Views 例子。
        //public ViewResult Index()
        //{
        //    var employee = new Employee { ID = 1, Name = "李健" };
        //    return View(employee);
        //}

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

    // 操作 Employees 表。
    public class SQLEmployeeData
    {
        private FirstAppDemoDbContext _context { get; set; }
        public SQLEmployeeData(FirstAppDemoDbContext context)
        {
            _context = context;
        }
        public void Add(Employee emp)
        {
            _context.Add(emp);
            _context.SaveChanges();
        }
        public Employee Get(int ID)
        {
            return _context.Employees.FirstOrDefault(e => e.ID == ID);
        }
        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList<Employee>();
        }
    }

    public class HomePageViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
    }
}
