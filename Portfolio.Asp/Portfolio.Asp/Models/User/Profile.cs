namespace Portfolio.Asp.Models.User
{
    public class Profile
    {
        public int Id { get; set; }
        public string About { get; set; }


        // One-To-One კავშირი  
        public int UserId { get; set; }
        public User User { get; set; }  



    }
