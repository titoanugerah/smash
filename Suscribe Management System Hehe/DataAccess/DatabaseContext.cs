using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suscribe_Management_System_Hehe.DataAccess
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public DatabaseContext(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._loggerFactory = loggerFactory;
        }

        public DbSet<Models.Application> Application { set; get; }
        public DbSet<Models.Config> Config { set; get; }
        public DbSet<Models.Group> Group { set; get; }
        public DbSet<Models.GroupConfig> GroupConfig { set; get; }
        public DbSet<Models.Member> Member { set; get; }
        public DbSet<Models.User> User { set; get; }
        public DbSet<Models.Role> Role { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(this._loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseMySQL(_configuration["DatabaseConnection"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Models.Application>(application =>
            {
                application
                .HasIndex(col => col.Name)
                .IsUnique();
            });

            modelBuilder.Entity<Models.Config>(config =>
            {
                config
                .HasKey(col => col.ApplicationId);

                config
                .HasOne(col => col.Application)
                .WithMany(col => col.Configs)
                .HasForeignKey(col => col.ApplicationId);
            });

            modelBuilder.Entity<Models.Group>(group =>
            {
                group
                .HasOne(col => col.Application)
                .WithMany(col => col.Groups)
                .HasForeignKey(col => col.ApplicationId);

            });

            modelBuilder.Entity<Models.GroupConfig>(groupConfig =>
            {
                groupConfig
                .HasOne(col => col.Group)
                .WithMany(col => col.GroupConfigs)
                .HasForeignKey(col => col.GroupId);
            });

            modelBuilder.Entity<Models.Member>(member =>
            {
                member
                .HasOne(col => col.Group)
                .WithMany(col => col.Members)
                .HasForeignKey(col => col.GroupId);
            });

            modelBuilder.Entity<Models.User>(user =>
            {
                user
                .HasIndex(col => col.Email)
                .IsUnique();

                user
                .HasOne(col => col.Role)
                .WithMany(col => col.Users)
                .HasForeignKey(col => col.RoleId);
            });

            modelBuilder.Entity<Models.Role>(role =>
            {
                role
                .HasIndex(col => col.Name)
                .IsUnique();

            });

        }


    }
}
