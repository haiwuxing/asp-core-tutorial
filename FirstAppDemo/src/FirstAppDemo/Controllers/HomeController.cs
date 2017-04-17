using Microsoft.AspNetCore.Mvc;
using FirstAppDemo.Models;
using System.Collections.Generic; // for IEnumerable interface.
using System.Linq; // for FirstOrDefault
using System.ComponentModel.DataAnnotations; // for [Required, MaxLength(80)]

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

        public IActionResult Details(int id)
        {
            SQLEmployeeData sqlData = new SQLEmployeeData(_context);
            var model = sqlData.Get(id);

            if(model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // 给表单传递内容。
        [HttpGet]
        public IActionResult Edit(int id)
        {
            SQLEmployeeData sqlData = new SQLEmployeeData(_context);
            var model = sqlData.Get(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // 表单处理。
        [HttpPost]
        public IActionResult Edit(int id, EmployeeEditViewModel input)
        {
            SQLEmployeeData sqlData = new SQLEmployeeData(_context);
            var employee = sqlData.Get(id);

            if (employee != null && ModelState.IsValid)
            {
                employee.Name = input.Name;
                _context.SaveChanges();
                return RedirectToAction("Details", new { id = employee.Id });
            }
            return View(employee);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee();
                employee.Name = model.Name;
                var context = _context;

                SQLEmployeeData sqlData = new SQLEmployeeData(context);
                sqlData.Add(employee);
                return RedirectToAction("Details", new { id = employee.Id });
            }
            return View();
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
        public Employee Get(int Id)
        {
            return _context.Employees.FirstOrDefault(e => e.Id == Id);
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

    public class EmployeeEditViewModel
    {
        [Required, MaxLength(80)]
        public string Name { get; set; }
    }
}
