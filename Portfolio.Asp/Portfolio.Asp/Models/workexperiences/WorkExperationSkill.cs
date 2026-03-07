using Portfolio.Asp.Models.WorkExperiences;

namespace Portfolio.Asp.Models.WorkExperiencess;

public class WorkExperationSkill
{
    public int id { get; set; }
    public string name { get; set; }

    
    // One-To-Many კავშირი 
    public int WorkExperationId { get; set; }
    public WorkExperience  WorkExperience { get; set; }
}