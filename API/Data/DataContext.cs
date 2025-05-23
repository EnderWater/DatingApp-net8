using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options) 
{
    // The name of the variable will be the name of the table in the database
    public DbSet<AppUser> User { get; set; }
}