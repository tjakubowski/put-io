using System.Configuration;
using System.Data.Entity;


namespace ServerLibrary
{
    public class ServerDbContext : DbContext
    {
        public ServerDbContext() : base("Default")
        {
        }

        public DbSet<User> Users { get; set; }
        
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
