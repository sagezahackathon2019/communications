using Communications.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Communications.API.Data
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<MailTask> MailTasks { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region MailTask Configuration

            modelBuilder.Entity<MailTask>().HasKey(k => k.Id);
            modelBuilder.Entity<MailTask>().Property(p => p.Id).ValueGeneratedOnAdd();

            #endregion MailTask Configuration

            #region Vendor Configuration

            modelBuilder.Entity<Vendor>().HasKey(k => k.Id);
            modelBuilder.Entity<Vendor>().Property(p => p.Id).ValueGeneratedOnAdd();

            #endregion Vendor Configuration
        }
    }
}