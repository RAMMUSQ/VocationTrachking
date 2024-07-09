namespace Domain.Core
    
{
    public class UserGroupMember
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public DateTime JoinDate { get; set; }
    }
}