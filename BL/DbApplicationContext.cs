using BL.db.ModelConfiguration;
using BL.Model;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class DbApplicationContext : DbContext
    {
        public DbSet<AuthInfo> AuthInfos { get; set; } = null!;
        public DbSet<Chat> Chats { get; set; } = null!;
        public DbSet<Friend> Friends { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<MessageFiles> MessagesFiles { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UsersChats> UsersChats { get; set; } = null!;

        public DbApplicationContext(DbContextOptions<DbApplicationContext> options)
        : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthInfoConfiguration());
            modelBuilder.ApplyConfiguration(new ChatConfiguration());
            modelBuilder.ApplyConfiguration(new FriendConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new MessageFilesConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UsersChatsConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Title = "user" });
        }
    }
}
