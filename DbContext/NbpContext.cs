using Microsoft.EntityFrameworkCore;
using NBP.Context.Models;

namespace NBP.Context
{
    public class NbpContext : DbContext
    {
        public DbSet<ExchangeRateTable> ExchangeRateTable { get; set; }
        public DbSet<ExchangeRateValue> ExchangeRateValue { get; set; }

        public NbpContext(DbContextOptions<NbpContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeRateTable>()
                .HasKey(ert => ert.Id);
            modelBuilder.Entity<ExchangeRateTable>()
                .Property(ert => ert.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ExchangeRateValue>()
                .HasOne(ert => ert.ExchangeRateTable)
                .WithMany(er => er.ExchangeRates)
                .HasForeignKey(ert => ert.ExchangeRateTableId);
        }
    }
}
