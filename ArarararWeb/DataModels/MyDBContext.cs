using System;
using Microsoft.EntityFrameworkCore;

namespace ArarararWeb.DataModels
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) {
            var dbContextOptions = new DbContextOptionsBuilder<MyDBContext>()
    .UseSqlite("Data Source=mydatabase.db;Cache=Shared")
    .Options;
        }
        public DbSet<FileItem> Files { get; set; }
        public DbSet<Hall> Halls { get; set; }

    }
}

