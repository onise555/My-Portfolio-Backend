using Portfolio.Asp.Models.Contacts;
using Portfolio.Asp.Models.languages;
using Portfolio.Asp.Models.SocialLinks;

namespace Portfolio.Asp.Models.User
{
    public class Profile
    {
        public int Id { get; set; }
        public string About { get; set; }


        // One-To-One კავშირი  
        public int UserId { get; set; }
        public User User { get; set; }


        // One-To-Many კავშირი 
        public List<SocialLink> links { get; set; } = new List<SocialLink>();

        // One-To-Many კავშირი 
        public List<language> languages { get; set; } = new List<language>();

        // One-To-Many კავშირი 
        public List<Contact> contacts { get; set; } = new List<Contact>();

    }
}
