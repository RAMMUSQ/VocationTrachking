using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Core
{
    

    public class LeaveRequest
    {
        [Key] public int Id { get; set; } // İZİN İD

        [ForeignKey("User")] public int UserId { get; set; } // User tablosundaki Id ile eşleşmeli

        public string Username { get; set; }

        public Leavetype  LeaveType { get; set; }// public LeaveType LeaveType { get; set; } OLARAK DEĞİŞECEK
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool? Approved { get; set; } // null: pending, true: approved, false: rejected

        // Navigation property
        // public virtual User User { get; set; }
    }
}