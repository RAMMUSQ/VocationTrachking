using Domain.Core;

namespace Presentation.Models;

public class AnnualLeaveRight
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int LeaveDays { get; set; }
    
    public int NonAnnualLeaveDays { get; set; } // Yeni sütun
    
    public DateTime? LastUpdated { get; set; } // Bu alanı ekleyin
    
   // public bool HasUsedBirthdayLeave { get; set; } // Yeni alan
    public User User { get; set; }
    
}