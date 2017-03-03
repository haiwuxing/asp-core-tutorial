using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace FirstAppDemo.Controllers
{
    // 显式属性路由。
    //[Route("about")]
    //public class AboutController
    //{
    //    [Route("")]
    //    public string Phone()
    //    {
    //        return "+49-333-3333333";
    //    }

    //    [Route("country")]
    //    public string Country()
    //    {
    //        return "Germany";
    //    }
    //}

    // 隐式属性路由：不怕控制器重命名。
    [Route("[controller]")]
    public class AboutController
    {
        [Route("")]
        public string Phone()
        {
            return "+49-333-3333333";
        }
        
        [Route("[action]")]
        public string Country()
        {
            return "Germany";
        }
    }
}
