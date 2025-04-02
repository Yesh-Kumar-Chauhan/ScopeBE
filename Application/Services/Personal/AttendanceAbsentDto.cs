namespace Application.Services
{
    public class AttendanceAbsentDto
    {
        public long AttendanceID { get; set; }
        public long? STAFF_ID { get; set; }
        public DateTime? DATE { get; set; }
        public string? REASON { get; set; }
        public bool? PAID { get; set; }
        public bool? CHARGED { get; set; }
        public string? FRACTION { get; set; }
        public long? SITENUM { get; set; }
        public string? SITENAM { get; set; }
        public long? ReasonID { get; set; }
        public string? Paycode { get; set; }
        public long? SITEID { get; set; }
    }
}