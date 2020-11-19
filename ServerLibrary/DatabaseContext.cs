using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using ServerLibrary.Models;


namespace ServerLibrary
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("Default")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
            Database.SetInitializer<DatabaseContext>(null);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
        
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.CreateTables();

            this.OnModelCreatingTable(modelBuilder.Entity<User>());
            this.OnModelCreatingTable(modelBuilder.Entity<Channel>());
            this.OnModelCreatingTable(modelBuilder.Entity<Message>());

            this.OnModelCreatingTableRelations(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void CreateTables()
        {
            const string sqlTextCreateTables = @"
                CREATE TABLE IF NOT EXISTS Channel (
	                Id	INTEGER NOT NULL UNIQUE,
	                Name	TEXT NOT NULL UNIQUE,
	                
	                PRIMARY KEY(Id AUTOINCREMENT)
                );

                CREATE TABLE IF NOT EXISTS Message (
	                Id	INTEGER NOT NULL UNIQUE,
	                ChannelId	INTEGER NOT NULL,
	                UserId	INTEGER NOT NULL,
	                Text	INTEGER NOT NULL,

	                FOREIGN KEY(ChannelId) REFERENCES Channel(Id) ON DELETE CASCADE,
	                FOREIGN KEY(UserId) REFERENCES User(Id) ON DELETE CASCADE,
	                PRIMARY KEY(Id AUTOINCREMENT)
                );

                CREATE TABLE IF NOT EXISTS User (
	                Id	INTEGER NOT NULL UNIQUE,
	                Username	TEXT NOT NULL UNIQUE,
	                Password	TEXT NOT NULL,
	                Admin	INTEGER NOT NULL DEFAULT 0,

	                PRIMARY KEY(Id AUTOINCREMENT)
                );

                CREATE TABLE IF NOT EXISTS UserChannel (
	                UserId	INTEGER NOT NULL,
	                ChannelId	INTEGER NOT NULL,

	                FOREIGN KEY(ChannelId) REFERENCES Channel(Id) ON DELETE CASCADE,
	                FOREIGN KEY(UserId) REFERENCES User(Id) ON DELETE CASCADE,
                    PRIMARY KEY(UserId, ChannelId)
                );
            ";

            var connectionString = this.Database.Connection.ConnectionString;
            using (var dbConnection = new System.Data.SQLite.SQLiteConnection(connectionString))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = sqlTextCreateTables;
                    dbCommand.ExecuteNonQuery();
                }
            }
        }

        private void OnModelCreatingTable(EntityTypeConfiguration<User> users)
        {
            users.ToTable("User").HasKey(user => user.Id);
            users.Property(user => user.Username).IsRequired();
            users.Property(user => user.Password).IsRequired();
            users.Property(user => user.Admin).IsRequired();
        }

        private void OnModelCreatingTable(EntityTypeConfiguration<Channel> channels)
        {
            channels.ToTable("Channel").HasKey(channel => channel.Id);
            channels.Property(channel => channel.Name).IsRequired();
        }

        private void OnModelCreatingTable(EntityTypeConfiguration<Message> messages)
        {
            messages.ToTable("Message").HasKey(message => message.Id);
            messages.Property(message => message.Text).IsRequired();

            messages.HasRequired(message => message.User)
                .WithMany(user => user.Messages)
                .HasForeignKey(message => message.UserId);

            messages.HasRequired(message => message.Channel)
                .WithMany(channel => channel.Messages)
                .HasForeignKey(message => message.ChannelId);
        }

        private void OnModelCreatingTableRelations(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(user => user.Channels)
                .WithMany(channel => channel.Users)
                .Map(manyToMany =>
                {
                    manyToMany.MapLeftKey("UserId");
                    manyToMany.MapRightKey("ChannelId");
                    manyToMany.ToTable("UserChannel");
                });
        }
    }
}
