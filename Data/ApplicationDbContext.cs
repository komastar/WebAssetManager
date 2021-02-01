using AssetWebManager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetWebManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AssetBundleModel> AssetBundleModel { get; set; }
        public DbSet<GameRoomModel> GameRoom { get; set; }
        public DbSet<ContentLockModel> ContentLock { get; set; }
        public DbSet<GameUserModel> GameUser { get; set; }
        public DbSet<DiceModel> Dice { get; set; }
        public DbSet<AssetWebManager.Models.TestModel> TestModel { get; set; }
    }
}
