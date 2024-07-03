namespace Core.Entities
{
    public class UserGroups 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /* buraya Gruop admini de eklenicek*/
        public List<User> Members { get; set; }
    }
}