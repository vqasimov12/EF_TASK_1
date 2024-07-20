using Microsoft.EntityFrameworkCore;

namespace EF_TASK_2;
public class CarContext:DbContext
{
    public DbSet<Car> Cars { get; set; }
    public string cs = "";
    public CarContext(string _cs)
    {
        cs = _cs;
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(cs);
    }
}

