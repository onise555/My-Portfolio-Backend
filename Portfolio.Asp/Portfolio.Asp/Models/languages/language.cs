using Portfolio.Asp.Enum;
using Portfolio.Asp.Models.User;

namespace Portfolio.Asp.Models.languages;

public class language
{
    public int id { get; set; }
    public string Name { get; set; }
    public LanguageLevel level { get; set; }
    
    
    
    
    // Many-To-One კავშირი  
    public int ProfileId { get; set; }
    public Profile Profile { get; set; }
}