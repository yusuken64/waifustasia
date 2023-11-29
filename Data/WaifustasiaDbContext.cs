using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Waifustasia.Data.Chat;
using static Waifustasia.Data.User.User;

namespace Waifustasia.Data;

public class WaifustasiaDbContext : IdentityDbContext<User.User>
{
    public DbSet<Waifu.Waifu> Waifus { get; set; }
    public DbSet<Chat.Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Memory.Memory> Memories { get; set; }
    public DbSet<UserWaifuFollow> UserWaifuFollows { get; set; }

    public WaifustasiaDbContext(DbContextOptions<WaifustasiaDbContext> options) : base(options)
	{
	}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User.User>(user =>
        {
            user.HasKey(u => u.Id);
            
            user.HasMany(u => u.LikedMemories)
                .WithOne()
                .IsRequired();

            user.HasMany(u => u.CreatedWaifus)
                .WithOne(w => w.Creator)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<User.User>()
            .HasMany(u => u.LikedMemories);

        modelBuilder.Entity<Waifu.Waifu>(waifu =>
        {
            waifu.HasKey(w => w.Id);
            // Other configurations for the Waifu entity
        });

		modelBuilder.Entity<Waifu.Waifu>()
	        .HasOne(waifu => waifu.Creator) // Waifu has one Creator (User)
	        .WithMany() // A User can create multiple Waifus
	        .HasForeignKey(waifu => waifu.UserId); // Define the foreign key

		modelBuilder.Entity<Memory.Memory>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Memory.Memory>()
            .HasOne(m => m.Waifu)
            .WithMany(w => w.Memories)
            .HasForeignKey(m => m.WaifuId);

        modelBuilder.Entity<Memory.Memory>()
            .HasOne(m => m.MemoryUser)
            .WithMany(u => u.LikedMemories)
            .HasForeignKey(m => m.MemoryUserId)
            .IsRequired();

        // Define relationships
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId);

        modelBuilder.Entity<UserWaifuFollow>()
            .HasKey(uwf => new { uwf.UserId, uwf.WaifuId });

        modelBuilder.Entity<UserWaifuFollow>()
            .HasOne(uwf => uwf.User)
            .WithMany(u => u.FollowedWaifus)
            .HasForeignKey(uwf => uwf.UserId);

        modelBuilder.Entity<UserWaifuFollow>()
            .HasOne(uwf => uwf.Waifu)
            .WithMany(w => w.Followers)
            .HasForeignKey(uwf => uwf.WaifuId);
    }
}