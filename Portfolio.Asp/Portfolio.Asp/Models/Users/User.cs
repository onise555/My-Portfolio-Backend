namespace Portfolio.Asp.Models.User
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string ProfileImage { get; set; }

        public string ProfileVideo { get; set; }

        public Profile Profile {  get; set; }


    }
}
