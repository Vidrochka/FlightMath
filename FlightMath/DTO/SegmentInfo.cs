namespace FlightMath.DTO
{
    public class SegmentInfo
    {
        public decimal Sequence { get; set; }
        public decimal? AWBSeq { get; set; }
        public string Prefix { get; set; }
        public string Serial { get; set; }
        public decimal? PC_Weight { get; set; }
        public string Flights { get; set; }
        public string Origin { get; set; }
        public string Dates { get; set; }
        public string Dest { get; set; }
        public string Carrier { get; set; }
        public decimal ActualKGs { get; set; }
    }
}