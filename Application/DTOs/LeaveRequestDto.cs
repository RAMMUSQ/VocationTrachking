namespace Application.DTOs
{
    public class LeaveRequestDto
    {
        
        //[ForeignKey("User")] public int UserId { get; set; } // User tablosundaki Id ile eşleşmeli
        public int UserId { get; set; }
        public string username { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        
      // public virtual User User { get; set; }
    }
}