using Een.Model;
using Microsoft.EntityFrameworkCore;

namespace Een.Data;

public class Database : DbContext
{
    public DbSet<User> Users { get; set; }

    public string DbPath { get; }

    public Database()
    {
        Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        string path = Environment.GetFolderPath(folder);
        DbPath = Path.Join("./users.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}