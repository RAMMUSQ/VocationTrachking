using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Entities
{
    public class User
    {
        [Key] public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        //public DateTime Birthdate { get; set; }

        //public int YearsWorked { get; set; }
        
       // public string Department { get; set; }
        
       // public string Occupation { get; set; }
        
       // public int Age { get; set; }
        
       // public string Email { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        

        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }

    }
}