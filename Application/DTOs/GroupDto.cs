namespace Application.DTOs
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public List<string> MemberNames { get; set; } // Y
    }
}