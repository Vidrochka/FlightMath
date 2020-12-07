using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using FlightMath.Models;

#nullable disable

namespace FlightMath
{
    [Table("Airport", Schema = "dbo")]
    public class Airport
    {
        [Key]
        [Required]
        [Column("Sequence")]
        public decimal Sequence { get; set; }

        [AllowNull]
        [Column("IATA_Code")]
        public string IataCode { get; set; }

        [AllowNull]
        [Column("ICAO_Code")]
        public string IcaoCode { get; set; }

        [Required]
        [Column("Airport_Name")]
        public string AirportName { get; set; }

        [AllowNull]
        [Column("City")]
        public string City { get; set; }

        [AllowNull]
        [Column("CountryId")]
        public decimal? CountryId { get; set; }

        [AllowNull]
        [Column("Latitude")]
        public decimal? Latitude { get; set; }

        [AllowNull]
        [Column("Longitude")]
        public decimal? Longitude { get; set; }

        [AllowNull]
        [Column("Altitude")]
        public decimal? Altitude { get; set; }

        [AllowNull]
        [Column("UTC_Offset")]
        public decimal? UtcOffset { get; set; }

        [AllowNull]
        [Column("DST")]
        public string Dst { get; set; }

        [AllowNull]
        [Column("Base_Id")]
        public decimal? BaseId { get; set; }

        public List<MainData> MainData { get; set; } = new List<MainData>();
    }
}
