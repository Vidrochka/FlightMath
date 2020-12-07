using FlightMath.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FlightMath.DB
{
    public class FlightDbContext : DbContext
    {
        private readonly Func<FlightDbContext, IEnumerable<MainData>> _getDataWithAirports = EF.CompileQuery(
            (FlightDbContext context) => context.MainData
                .AsNoTracking()
                .Include(d => d.Airports)
                .AsNoTracking());

        public IEnumerable<MainData> GetDataWithAirports => _getDataWithAirports(this);

        public DbSet<MainData> MainData { get; set; }
        public DbSet<Airport> Airports { get; set; }

        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MainData>()
                .HasMany(d => d.Airports)
                .WithMany(a => a.MainData)
                .UsingEntity(e => e.ToTable("MainDataAirportLinks", "dbo"));

            modelBuilder.Entity<Airport>(entity =>
            {
                entity.HasKey(e => e.Sequence);

                entity.ToTable("AIRPORT");

                entity.Property(e => e.Sequence)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AirportName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasColumnName("Airport_Name");

                entity.Property(e => e.Altitude).HasColumnType("numeric(8, 0)");

                entity.Property(e => e.BaseId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Base_Id");

                entity.Property(e => e.City).HasMaxLength(70);

                entity.Property(e => e.CountryId).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Dst)
                    .HasMaxLength(1)
                    .HasColumnName("DST")
                    .IsFixedLength(true);

                entity.Property(e => e.IataCode)
                    .HasMaxLength(4)
                    .HasColumnName("IATA_Code");

                entity.Property(e => e.IcaoCode)
                    .HasMaxLength(4)
                    .HasColumnName("ICAO_Code");

                entity.Property(e => e.Latitude).HasColumnType("numeric(10, 6)");

                entity.Property(e => e.Longitude).HasColumnType("numeric(10, 6)");

                entity.Property(e => e.UtcOffset)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("UTC_Offset");
            });

            modelBuilder.Entity<MainData>(entity =>
            {
                entity.HasKey(e => e.Sequence);

                entity.Property(e => e.Sequence)
                    .HasColumnType("numeric(9, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ActualKGs)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AWBSeq)
                    .HasColumnType("numeric(9, 0)");

                entity.Property(e => e.Carrier)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dates)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Dest)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Flights)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Origin)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PCWeight)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("PC_Weight");

                entity.Property(e => e.Prefix)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Serial)
                    .HasMaxLength(12)
                    .IsUnicode(false);
            });
        }
    }
}