using GamePlay;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlay
{
    public class GamePlayContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data source=KANINI-LTP-470\SQLSERVER2019; user id = sa; password=Admin@123; Initial Catalog =GamePlayDb");
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<WordsAssigned> WordsAssigned { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordsAssigned>().HasKey(e => new { e.Word_ID, e.UserName });
        }
    }
}