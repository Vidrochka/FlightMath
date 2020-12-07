using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FlightMath.Models
{
    [Table("MainData", Schema = "dbo")]
    public class MainData
    {
        [Key]
        [Required]
        [Column("Sequence")]
        public decimal Sequence { get; set; }

        [AllowNull]
        [Column("AWBSeq")]
        public decimal? AWBSeq { get; set; }

        [AllowNull]
        [Column("Prefix")]
        public string Prefix { get; set; }

        [AllowNull]
        [Column("Serial")]
        public string Serial { get; set; }

        [AllowNull]
        [Column("PC_Weight")]
        public decimal? PCWeight { get; set; }

        [AllowNull]
        [Column("Flights")]
        public string Flights { get; set; }

        [AllowNull]
        [Column("Origin")]
        public string Origin { get; set; }

        [AllowNull]
        [Column("Dates")]
        public string Dates { get; set; }

        [AllowNull]
        [Column("Dest")]
        public string Dest { get; set; }

        [AllowNull]
        [Column("Carrier")]
        public string Carrier { get; set; }

        [AllowNull]
        [Column("ActualKGs")]
        public string ActualKGs { get; set; }

        public List<Airport> Airports { get; set; } = new List<Airport>();
    }
}
