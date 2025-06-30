using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAccessApprovalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentAccessApprovalSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<AccessRequest> AccessRequests { get; set; }
        public DbSet<Decision> Decisions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<Document>().HasKey(d => d.Id);
            builder.Entity<AccessRequest>().HasKey(ar => ar.Id);

            builder.Entity<AccessRequest>()
                .HasOne(ar => ar.User)
                .WithMany()
                .HasForeignKey(ar => ar.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AccessRequest>()
                .HasOne(ar => ar.Document)
                .WithMany()
                .HasForeignKey(ar => ar.DocumentId)
                .OnDelete(DeleteBehavior.Restrict);

         

        }
    }
}
