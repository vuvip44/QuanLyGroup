using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLy.Models;

namespace QuanLy.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Group>()
                .HasIndex(g => g.Code)
                .IsUnique();

            modelBuilder.Entity<UserGroup>(x => x.HasKey(p => new { p.UserId, p.GroupId }));

            modelBuilder.Entity<UserGroup>()
                .HasOne(u => u.User)
                .WithMany(ug => ug.UserGroups)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(g => g.Group)
                .WithMany(ug => ug.UserGroups)
                .HasForeignKey(g => g.GroupId);
        }
    }
}