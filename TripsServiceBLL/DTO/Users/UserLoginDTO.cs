namespace TripsServiceBLL.DTO.Users
{
    public class UserLoginDTO
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
