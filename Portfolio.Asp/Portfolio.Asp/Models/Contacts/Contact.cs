using Portfolio.Asp.Models.User;

namespace Portfolio.Asp.Models.Contacts;

public class Contact
{
    public int id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    
    // One-To-Many კავშირი 
    public int profileId { get; set; }
    public Profile Profile { get; set; }
}