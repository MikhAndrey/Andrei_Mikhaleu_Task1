namespace Andrei_Mikhaleu_Task1.Models.Entities
{
    public class User
    {
        public User() 
        {
            Trips = new();
            Comments = new();
            UserName = "";
            Password = "";
        }
        
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public List<Trip> Trips { get; set; }

        public List<Comment> Comments { get; set; }

    }
}
