using Microsoft.EntityFrameworkCore;

namespace FirstAppDemo.Models
{
    // 负责同数据库打交道。
    public class FirstAppDemoDbContext : DbContext
    {
        /* 每一个DbSet 对应于数据中的一张表。
         * 这里对应的是 Employees 表。
         */
        public DbSet<Employee> Employees { get; set; }

        public FirstAppDemoDbContext(DbContextOptions<FirstAppDemoDbContext> options)
            : base(options) { }
    }
}
