namespace PromoApp.Models
{
    public class TblRole
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class TblUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public TblRole? Role { get; set; }
    }
}