using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TaskPriorities> TaskPriorities { get; set; }
        public DbSet<TaskStatuses> TaskStatuses { get; set; }
    }
}
