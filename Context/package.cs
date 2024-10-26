using Demo2.Model;
using Microsoft.EntityFrameworkCore;

namespace Demo2.Context
{
    public class Package : DbContext
    {
        public Package(DbContextOptions<Package> options) : base(options) { }
        public DbSet<tbl_user> tbl_user { get; set; }
        public DbSet<tbl_package> tbl_package { get; set; }
        public DbSet<tbl_menu> tbl_menu { get; set; }
        public DbSet<tbl_menu_feature> tbl_menu_feature { get; set; }
        public DbSet<tbl_package_menu_condition> tbl_package_menu_condition { get; set; }
        public DbSet<tbl_user_package> tbl_user_package { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            //user_package relation
            modelBuilder.Entity<tbl_user_package>()
            .HasKey(p => new { p.user_id, p.package_id, p.menu_id });

            modelBuilder.Entity<tbl_user_package>()
            .HasOne(p => p.userId)
            .WithMany(l => l.user_package)
            .HasForeignKey(f => f.user_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<tbl_user_package>()
            .HasOne(p => p.packageId)
            .WithMany(l => l.user_package)
            .HasForeignKey(up => up.package_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<tbl_user_package>()
            .HasOne(p => p.menuId)
            .WithMany(l => l.user_package)
            .HasForeignKey(f => f.menu_id)
            .OnDelete(DeleteBehavior.Cascade);

            //package_menu_condition relation
            modelBuilder.Entity<tbl_package_menu_condition>()
            .HasKey(p => new { p.package_id, p.menu_id, p.menu_feature_id });

            modelBuilder.Entity<tbl_package_menu_condition>()
            .HasOne(p => p.packageId)
            .WithMany(p => p.package_menu_condition)
            .HasForeignKey(p => p.package_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<tbl_package_menu_condition>()
            .HasOne(p => p.menuId)
            .WithMany(m => m.package_menu_condition)
            .HasForeignKey(p => p.menu_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<tbl_package_menu_condition>()
            .HasOne(p => p.menu_features)
            .WithMany(mf => mf.package_menu_condition)
            .HasForeignKey(p => p.menu_feature_id)
            .OnDelete(DeleteBehavior.Cascade);

            //menu_feature relation
            modelBuilder.Entity<tbl_menu_feature>()
            .HasOne(mf => mf.menuId)
            .WithMany(m => m.menu_features)
            .HasForeignKey(mf => mf.menu_id)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
