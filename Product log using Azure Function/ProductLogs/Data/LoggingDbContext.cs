using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using ProductLogs.Models;

public class LoggingDbContext : DbContext
{
    public LoggingDbContext(
        DbContextOptions<LoggingDbContext> options)
        : base(options)
    {
    }

    public DbSet<ProductLog> ProductLog => 
        Set<ProductLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductLog>()
        .ToTable("ProductLog");
    
    }

}