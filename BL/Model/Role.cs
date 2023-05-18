namespace BL.Model
{
    public class Role
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public List<AuthInfo>? AuthInfos { get; set; }
        public List<UsersChats>? UsersChats { get; set; }
    }
}