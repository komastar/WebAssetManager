using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AssetWebManager.Models;
using Microsoft.AspNetCore.Identity;

namespace AssetWebManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AssetBundleModel> AssetBundleModel { get; set; }
        public DbSet<GameModel> GameModel { get; set; }
    }
}
