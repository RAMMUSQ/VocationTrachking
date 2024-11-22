namespace Domain.Core
{
    public class PermissionRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public UserGroups  UserGroups  { get; set; }
        public string Status { get; set; }
    }
}