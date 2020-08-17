using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using TTDataAccessLibrary.Models;

namespace TTDataAccessLibrary
{
    public class TechnicalTaskContext : DbContext
    {
        public TechnicalTaskContext(DbContextOptions options) : base(options) { }
        public TechnicalTaskContext() : base() { }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Flag> Flags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flag>().HasKey(f => new { f.BitFlag, f.Name });
            modelBuilder.Entity<Asset>().Property(p => p.TypeBitField).HasDefaultValue(0);
            modelBuilder.Entity<Asset>().Property(p => p.TimeStamp).HasDefaultValue(DateTime.MinValue);

            modelBuilder.Entity<Flag>().HasData(
                new Flag { Name = "IsFixIncome", BitFlag = 1 },
                new Flag { Name = "IsConvertible", BitFlag = 2 },
                new Flag { Name = "IsSwap", BitFlag = 4 },
                new Flag { Name = "IsCash", BitFlag = 8 },
                new Flag { Name = "IsFuture", BitFlag = 16 }
                );
        }
    }
}
