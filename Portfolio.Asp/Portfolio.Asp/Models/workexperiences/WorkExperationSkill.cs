namespace Portfolio.Asp.Models.WorkExperationSkill;

using Portfolio.Asp.Models.WorkExperience;

public class WorkExperationSkill
{
    public int id { get; set; }
    public string name { get; set; }

    
    // One-To-Many კავშირი 
    public int WorkExperationId { get; set; }
    public WorkExperience  WorkExperience { get; set; }
}