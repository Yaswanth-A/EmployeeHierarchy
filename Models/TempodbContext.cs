using Microsoft.EntityFrameworkCore;


namespace EmpHeirarchy.DAL.Models
{
    public class TempoDbContext : DbContext
    {
        public TempoDbContext(DbContextOptions<TempoDbContext> options) : base(options)
        {
        }

        public DbSet<EmployeeDetails> EmployeeDetails { get; set; }
    }


}



