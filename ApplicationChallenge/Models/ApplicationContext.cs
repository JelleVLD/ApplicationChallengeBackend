using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public DbSet<Bedrijf> Bedrijven { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Maker> Makers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Opdracht> Opdrachten { get; set; }
        public DbSet<OpdrachtMaker> OpdrachtMakers { get; set; }
        public DbSet<OpdrachtTag> OpdrachtTags { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillMaker> SkillMakers { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<BedrijfTag> BedrijfTags { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Bedrijf>().ToTable("Bedrijf");
            modelBuilder.Entity<BedrijfTag>().ToTable("BedrijfTag");
            modelBuilder.Entity<Maker>().ToTable("Maker");
            modelBuilder.Entity<Opdracht>().ToTable("Opdracht");
            modelBuilder.Entity<OpdrachtMaker>().ToTable("OpdrachtMaker");
            modelBuilder.Entity<OpdrachtTag>().ToTable("OpdrachtTag");
            modelBuilder.Entity<Review>().ToTable("Review");
            modelBuilder.Entity<Skill>().ToTable("Skill");
            modelBuilder.Entity<SkillMaker>().ToTable("SkillMaker");
            modelBuilder.Entity<Tag>().ToTable("Tag");
            modelBuilder.Entity<UserType>().ToTable("UserType");
            modelBuilder.Entity<Admin>().ToTable("Admin");
            modelBuilder.Entity<Permission>().ToTable("Permission");

        }

    }
}
