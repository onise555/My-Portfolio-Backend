using Portfolio.Asp.Models.Users;

namespace Portfolio.Asp.Models.SocialLinks
{
    public class SocialLink
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public Platforms platform { get; set; }
        public string Url { get; set; }


        // One-To-Many კავშირი
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
