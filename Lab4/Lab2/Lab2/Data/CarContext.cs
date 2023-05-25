using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Lab2.Models;

namespace Lab2.Data
{
    public class CarContext : DbContext
    {
        public CarContext(DbContextOptions<CarContext> options) : base(options) { }

        public DbSet<Car> Car { get; set; }
    }
}
