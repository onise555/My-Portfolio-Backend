<<<<<<< HEAD:Portfolio.Asp/Portfolio.Asp/Models/WorkExperiences/WorkExperationSkill.cs
using Portfolio.Asp.Models.WorkExperiences;

namespace Portfolio.Asp.Models.WorkExperiencess;
=======
namespace Portfolio.Asp.Models.WorkExperationSkill;

using Portfolio.Asp.Models.WorkExperience;
>>>>>>> parent of 0e70da9 (Merge pull request #44 from onise555/develop):Portfolio.Asp/Portfolio.Asp/Models/WorkExperationSkill/WorkExperationSkill.cs

public class WorkExperationSkill
{
    public int id { get; set; }
    public string name { get; set; }

    
    // One-To-Many კავშირი 
    public int WorkExperationId { get; set; }
    public WorkExperience  WorkExperience { get; set; }
}